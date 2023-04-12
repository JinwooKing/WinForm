using System;
using System.Diagnostics;
using System.Reflection;
using System.Reflection.Emit;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace WinForm;

public partial class Form1 : Form
{
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
        progressBar1.Minimum = 1;
        progressBar1.Maximum = 15; //filenames.Length;
        progressBar1.Value = 1;
        progressBar1.Step = 1;

    }

    private void button1_Click(object sender, EventArgs e)
    {
        richTextBox1.Clear();
        progressBar1.Value = 0;
        
        var sw = Stopwatch.StartNew();

        foreach (var index in Enumerable.Range(1, 15))
        {
            Thread.Sleep(TimeSpan.FromMilliseconds(100));
            progressBar1.PerformStep();
            richTextBox1.AppendText($"{DateTime.Now.ToString("F")} : {index} \n");
        }

        sw.Stop();
        richTextBox1.ScrollToCaret();
        richTextBox1.AppendText($"{sw.ElapsedMilliseconds}");
    }

    private async void button2_Click(object sender, EventArgs e)
    {
        richTextBox1.Clear();
        progressBar1.Value = 0;

        var sw = Stopwatch.StartNew();

        await Task.Run(async () =>
        {
            foreach (var index in Enumerable.Range(1, 15))
            {
                await Task.Delay(TimeSpan.FromMilliseconds(100));
                
                this.Invoke(new Action(() =>
                {
                    progressBar1.PerformStep();
                    richTextBox1.AppendText($"{DateTime.Now.ToString("F")} : {index} \n");
                }));
            }
        });

        sw.Stop();
        richTextBox1.ScrollToCaret();
        richTextBox1.AppendText($"{sw.ElapsedMilliseconds}");
    }
}

