﻿namespace WinForm;

partial class Form1
{
    /// <summary>
    ///  Required designer variable.
    /// </summary>
    private System.ComponentModel.IContainer components = null;

    /// <summary>
    ///  Clean up any resources being used.
    /// </summary>
    /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
    protected override void Dispose(bool disposing)
    {
        if (disposing && (components != null))
        {
            components.Dispose();
        }
        base.Dispose(disposing);
    }

    #region Windows Form Designer generated code

    /// <summary>
    ///  Required method for Designer support - do not modify
    ///  the contents of this method with the code editor.
    /// </summary>
    private void InitializeComponent()
    {
        button1 = new Button();
        richTextBox1 = new RichTextBox();
        button2 = new Button();
        progressBar1 = new ProgressBar();
        SuspendLayout();
        // 
        // button1
        // 
        button1.Location = new Point(12, 12);
        button1.Name = "button1";
        button1.Size = new Size(110, 34);
        button1.TabIndex = 0;
        button1.Text = "button1";
        button1.UseVisualStyleBackColor = true;
        button1.Click += button1_Click;
        // 
        // richTextBox1
        // 
        richTextBox1.Location = new Point(12, 52);
        richTextBox1.Name = "richTextBox1";
        richTextBox1.Size = new Size(776, 357);
        richTextBox1.TabIndex = 1;
        richTextBox1.Text = "";
        // 
        // button2
        // 
        button2.Location = new Point(128, 12);
        button2.Name = "button2";
        button2.Size = new Size(111, 34);
        button2.TabIndex = 2;
        button2.Text = "button2";
        button2.UseVisualStyleBackColor = true;
        button2.Click += button2_Click;
        // 
        // progressBar1
        // 
        progressBar1.Location = new Point(12, 415);
        progressBar1.Name = "progressBar1";
        progressBar1.Size = new Size(776, 23);
        progressBar1.TabIndex = 3;
        // 
        // Form1
        // 
        AutoScaleDimensions = new SizeF(7F, 15F);
        AutoScaleMode = AutoScaleMode.Font;
        ClientSize = new Size(800, 450);
        Controls.Add(progressBar1);
        Controls.Add(button2);
        Controls.Add(richTextBox1);
        Controls.Add(button1);
        Name = "Form1";
        Text = "Form1";
        ResumeLayout(false);
    }

    #endregion

    private Button button1;
    private RichTextBox richTextBox1;
    private Button button2;
    private ProgressBar progressBar1;
}
