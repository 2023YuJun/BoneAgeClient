using System;
using System.Drawing;
using System.Windows.Forms;

namespace FloatingWindowApp
{
    partial class MainForm
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
            components = new System.ComponentModel.Container();
            textBox = new TextBox();
            contextMenuStrip = new ContextMenuStrip(components);
            Appearance = new ToolStripMenuItem();
            SelectColor = new ToolStripMenuItem();
            AutoOpen = new ToolStripMenuItem();
            ResultResident = new ToolStripMenuItem();
            BootUp = new ToolStripMenuItem();
            Detection = new ToolStripMenuItem();
            UsageGuide = new ToolStripMenuItem();
            Feedback = new ToolStripMenuItem();
            Exit = new ToolStripMenuItem();
            contextMenuStrip.SuspendLayout();
            SuspendLayout();
            // 
            // textBox
            // 
            textBox.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            textBox.Cursor = Cursors.IBeam;
            textBox.Font = new Font("Microsoft YaHei UI", 10F);
            textBox.Location = new Point(45, 5);
            textBox.Margin = new Padding(0);
            textBox.MinimumSize = new Size(180, 30);
            textBox.Name = "textBox";
            textBox.Size = new Size(180, 30);
            textBox.TabIndex = 0;
            textBox.KeyPress += textBox_KeyPress;
            // 
            // contextMenuStrip
            // 
            contextMenuStrip.Font = new Font("Microsoft YaHei UI", 12F);
            contextMenuStrip.ImageScalingSize = new Size(25, 25);
            contextMenuStrip.Items.AddRange(new ToolStripItem[] { Appearance, Detection, UsageGuide, Feedback, Exit });
            contextMenuStrip.Name = "contextMenuStrip";
            contextMenuStrip.Size = new Size(174, 164);
            // 
            // Appearance
            // 
            Appearance.DropDownItems.AddRange(new ToolStripItem[] { SelectColor, AutoOpen, ResultResident, BootUp });
            Appearance.Image = Properties.Resources.外观设置;
            Appearance.Name = "Appearance";
            Appearance.Size = new Size(173, 32);
            Appearance.Text = "外观设置";
            // 
            // SelectColor
            // 
            SelectColor.Name = "SelectColor";
            SelectColor.Size = new Size(224, 32);
            SelectColor.Text = "配色选择";
            SelectColor.Click += SelectColor_Click;
            // 
            // AutoOpen
            // 
            AutoOpen.CheckOnClick = true;
            AutoOpen.Name = "AutoOpen";
            AutoOpen.Size = new Size(224, 32);
            AutoOpen.Text = "自动打开";
            AutoOpen.Click += AutoOpen_Click;
            // 
            // ResultResident
            // 
            ResultResident.CheckOnClick = true;
            ResultResident.Name = "ResultResident";
            ResultResident.Size = new Size(224, 32);
            ResultResident.Text = "结果常驻";
            ResultResident.Click += ResultResident_Click;
            // 
            // BootUp
            // 
            BootUp.CheckOnClick = true;
            BootUp.Name = "BootUp";
            BootUp.Size = new Size(224, 32);
            BootUp.Text = "开机启动";
            BootUp.Click += BootUp_Click;
            // 
            // Detection
            // 
            Detection.Image = Properties.Resources.识别区域;
            Detection.Name = "Detection";
            Detection.Size = new Size(173, 32);
            Detection.Text = "识别区域";
            Detection.Click += Detection_Click;
            // 
            // UsageGuide
            // 
            UsageGuide.Image = Properties.Resources.使用指南;
            UsageGuide.Name = "UsageGuide";
            UsageGuide.Size = new Size(173, 32);
            UsageGuide.Text = "使用指南";
            UsageGuide.Click += UsageGuide_Click;
            // 
            // Feedback
            // 
            Feedback.Image = Properties.Resources.爬虫;
            Feedback.Name = "Feedback";
            Feedback.Size = new Size(173, 32);
            Feedback.Text = "故障反馈";
            Feedback.Click += Feedback_Click;
            // 
            // Exit
            // 
            Exit.Image = Properties.Resources.退出程序;
            Exit.Name = "Exit";
            Exit.Size = new Size(173, 32);
            Exit.Text = "退出程序";
            Exit.Click += Exit_Click;
            // 
            // MainForm
            // 
            AutoScaleDimensions = new SizeF(9F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.FromArgb(219, 219, 219);
            ClientSize = new Size(230, 40);
            ContextMenuStrip = contextMenuStrip;
            Controls.Add(textBox);
            FormBorderStyle = FormBorderStyle.None;
            Location = new Point(800, 50);
            MaximumSize = new Size(230, 40);
            MinimumSize = new Size(230, 40);
            Name = "MainForm";
            StartPosition = FormStartPosition.Manual;
            TopMost = true;
            FormClosing += MainForm_FormClosing;
            Load += MainForm_Load;
            MouseDown += MainForm_MouseDown;
            MouseMove += MainForm_MouseMove;
            MouseUp += MainForm_MouseUp;
            contextMenuStrip.ResumeLayout(false);
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private TextBox textBox;
        private ContextMenuStrip contextMenuStrip;
        private ToolStripMenuItem UsageGuide;
        private ToolStripMenuItem Feedback;
        private ToolStripMenuItem Exit;
        private ToolStripMenuItem Appearance;
        private ToolStripMenuItem Detection;
        private ToolStripMenuItem SelectColor;
        private ToolStripMenuItem AutoOpen;
        private ToolStripMenuItem ResultResident;
        private ToolStripMenuItem BootUp;
    }
}
