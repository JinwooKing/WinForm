using System;
using System.Diagnostics;
using System.Net;
using System.Reflection;
using System.Reflection.Emit;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace WinForm;

public partial class Form1 : Form
{
    private readonly HttpClient _client = new HttpClient { MaxResponseContentBufferSize = 1_000_000 };

    private readonly IEnumerable<string> _urlList = new string[]
        {
            "https://docs.microsoft.com",
            "https://docs.microsoft.com/azure",
            "https://docs.microsoft.com/powershell",
            "https://docs.microsoft.com/dotnet",
            "https://docs.microsoft.com/aspnet/core",
            "https://docs.microsoft.com/windows",
            "https://docs.microsoft.com/office",
            "https://docs.microsoft.com/enterprise-mobility-security",
            "https://docs.microsoft.com/visualstudio",
            "https://docs.microsoft.com/microsoft-365",
            "https://docs.microsoft.com/sql",
            "https://docs.microsoft.com/dynamics365",
            "https://docs.microsoft.com/surface",
            "https://docs.microsoft.com/xamarin",
            "https://docs.microsoft.com/azure/devops",
            "https://docs.microsoft.com/system-center",
            "https://docs.microsoft.com/graph",
            "https://docs.microsoft.com/education",
            "https://docs.microsoft.com/gaming"
        };


    public Form1()
    {
        InitializeComponent();
    }

    private void Form1_Load(object sender, EventArgs e)
    {
        // 디폴트값 사용 (Maximum=100, Minimum=0, Step=10)
        //progressBar1.Style = ProgressBarStyle.Blocks;
        progressBar1.Style = ProgressBarStyle.Continuous;
        //progressBar1.Style = ProgressBarStyle.Marquee;

        // 최대,최소,간격을 임의로 조정
        // Display the ProgressBar control.
        progressBar1.Visible = true;
        progressBar1.Minimum = 0;
        progressBar1.Maximum = 15; //filenames.Length;
        progressBar1.Value = 0;
        progressBar1.Step = 1;

    }

    private void button1_Click(object sender, EventArgs e)
    {
        button1.Enabled = false;

        richTextBox1.Clear();
        progressBar1.Value = 0;

        var sw = Stopwatch.StartNew();

        foreach (var index in Enumerable.Range(1, 15))
        {
            Thread.Sleep(TimeSpan.FromMilliseconds(300));
            progressBar1.PerformStep();
            richTextBox1.AppendText($"{index,3:#,#} \n");
        }

        sw.Stop();
        richTextBox1.ScrollToCaret();
        richTextBox1.AppendText($"Elapsed time: {sw.Elapsed}");

        button1.Enabled = true;
    }

    private async void button2_Click(object sender, EventArgs e)
    {
        button2.Enabled = false;
        richTextBox1.Clear();
        progressBar1.Value = 0;

        var sw = Stopwatch.StartNew();

        await Task.Run(async () =>
        {
            foreach (var index in Enumerable.Range(1, 15))
            {
                await Task.Delay(TimeSpan.FromMilliseconds(300));

                this.BeginInvoke(new Action(() =>
                {
                    progressBar1.PerformStep();
                    richTextBox1.AppendText($"{index,3:#,#} \n");
                }));
            }
        });

        sw.Stop();
        richTextBox1.ScrollToCaret();
        richTextBox1.AppendText($"Elapsed time: {sw.Elapsed}");

        button2.Enabled = true;
    }

    private void button3_Click(object sender, EventArgs e)
    {
        button3.Enabled = false;
        richTextBox1.Clear();

        SumPageSizes();

        richTextBox1.Text += $"\nControl returned to {nameof(button3)}.";

        button3.Enabled = true;
    }

    private async void button4_Click(object sender, EventArgs e)
    {
        button4.Enabled = false;
        richTextBox1.Clear();

        await SumPageSizesAsync();

        richTextBox1.Text += $"\nControl returned to {nameof(button4)}.";
        button4.Enabled = true;
    }

    private void button5_Click(object sender, EventArgs e)
    {
        button5.Enabled = false;
        richTextBox1.Clear();
        
        Task.Run(() => StartSumPageSizesAsync());
    }

    #region button3 method
    private void SumPageSizes()
    {
        var stopwatch = Stopwatch.StartNew();

        int total = _urlList.Select(url => ProcessUrl(url)).Sum();

        stopwatch.Stop();
        richTextBox1.Text += $"\nTotal bytes returned:  {total:#,#}";
        richTextBox1.Text += $"\nElapsed time:          {stopwatch.Elapsed}\n";
    }
    private int ProcessUrl(string url)
    {
        using var memoryStream = new MemoryStream();
        var webReq = (HttpWebRequest)WebRequest.Create(url);

        using WebResponse response = webReq.GetResponse();
        using Stream responseStream = response.GetResponseStream();
        responseStream.CopyTo(memoryStream);

        byte[] content = memoryStream.ToArray();
        DisplayResults(url, content);

        return content.Length;
    }
    #endregion

    #region button4 method
    private async Task SumPageSizesAsync()
    {
        var stopwatch = Stopwatch.StartNew();

        int total = 0;
        foreach (string url in _urlList)
        {
            int contentLength = await ProcessUrlAsync(url, _client);
            total += contentLength;
        }

        stopwatch.Stop();
        richTextBox1.Text += $"\nTotal bytes returned:  {total:#,#}";
        richTextBox1.Text += $"\nElapsed time:          {stopwatch.Elapsed}\n";
    }

    private async Task<int> ProcessUrlAsync(string url, HttpClient client)
    {
        byte[] content = await client.GetByteArrayAsync(url);
        DisplayResults(url, content);

        return content.Length;
    }
    #endregion

    #region button5 method
    private async Task StartSumPageSizesAsync()
    {
        await ParallelSumPageSizesAsync();
        this.BeginInvoke(() =>
        {
            richTextBox1.Text += $"\nControl returned to {nameof(button5)}.";
            button5.Enabled = true;
        });
    }

    private async Task ParallelSumPageSizesAsync()
    {
        var stopwatch = Stopwatch.StartNew();

        IEnumerable<Task<int>> downloadTasksQuery =
            from url in _urlList
            select ParallelProcessUrlAsync(url, _client);

        Task<int>[] downloadTasks = downloadTasksQuery.ToArray();

        int[] lengths = await Task.WhenAll(downloadTasks);
        int total = lengths.Sum();

        this.BeginInvoke(() =>
        {
            stopwatch.Stop();

            richTextBox1.Text += $"\nTotal bytes returned:  {total:#,#}";
            richTextBox1.Text += $"\nElapsed time:          {stopwatch.Elapsed}\n";
        });
    }

    private async Task<int> ParallelProcessUrlAsync(string url, HttpClient client)
    {
        byte[] byteArray = await client.GetByteArrayAsync(url);
        DisplayResultsAsync(url, byteArray);

        return byteArray.Length;
    }
    #endregion

    private void DisplayResults(string url, byte[] content) =>
            richTextBox1.Text += $"{url,-60} {content.Length,10:#,#}\n";
    private void DisplayResultsAsync(string url, byte[] content) =>
        this.BeginInvoke(() =>
            richTextBox1.Text += $"{url,-60} {content.Length,10:#,#}\n");

    protected override void OnClosed(EventArgs e) => _client.Dispose();
}

