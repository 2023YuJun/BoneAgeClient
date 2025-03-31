namespace ConfigWindowApp
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
            tableLayoutPanel1 = new TableLayoutPanel();
            panel4 = new Panel();
            BrowserLabel = new Label();
            label8 = new Label();
            panel2 = new Panel();
            DetectionLabel = new Label();
            label4 = new Label();
            panel1 = new Panel();
            StatusLabel = new Label();
            label1 = new Label();
            panel3 = new Panel();
            ResultLabel = new Label();
            label6 = new Label();
            panel5 = new Panel();
            tableLayoutPanel2 = new TableLayoutPanel();
            SwitchConfigBtn = new Button();
            SwitchBrowserBtn = new Button();
            SwitchVersionBtn = new Button();
            ReinstallBtn = new Button();
            DetectionBtn = new Button();
            DetectNetworkBtn = new Button();
            IPSwitchBtn = new Button();
            ResetPositionBtn = new Button();
            SwitchDetectionBtn = new Button();
            RegularMatchBtn = new Button();
            BootUpBtn = new Button();
            RestartBtn = new Button();
            label9 = new Label();
            panel6 = new Panel();
            tableLayoutPanel3 = new TableLayoutPanel();
            ClickToOpenCheck = new CheckBox();
            BlackScreenOrCrashCheck = new CheckBox();
            TotalNoResultCheck = new CheckBox();
            IndividualNoResultCheck = new CheckBox();
            IdentifyNonIDCheck = new CheckBox();
            IncorrectIDCheck = new CheckBox();
            OutOfRightCheck = new CheckBox();
            StartupCheck = new CheckBox();
            label10 = new Label();
            tableLayoutPanel1.SuspendLayout();
            panel4.SuspendLayout();
            panel2.SuspendLayout();
            panel1.SuspendLayout();
            panel3.SuspendLayout();
            panel5.SuspendLayout();
            tableLayoutPanel2.SuspendLayout();
            panel6.SuspendLayout();
            tableLayoutPanel3.SuspendLayout();
            SuspendLayout();
            // 
            // tableLayoutPanel1
            // 
            tableLayoutPanel1.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            tableLayoutPanel1.AutoSize = true;
            tableLayoutPanel1.BackColor = Color.WhiteSmoke;
            tableLayoutPanel1.ColumnCount = 4;
            tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 25F));
            tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 25F));
            tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 25F));
            tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 25F));
            tableLayoutPanel1.Controls.Add(panel4, 3, 0);
            tableLayoutPanel1.Controls.Add(panel2, 1, 0);
            tableLayoutPanel1.Controls.Add(panel1, 0, 0);
            tableLayoutPanel1.Controls.Add(panel3, 2, 0);
            tableLayoutPanel1.Location = new Point(20, 20);
            tableLayoutPanel1.Margin = new Padding(0);
            tableLayoutPanel1.Name = "tableLayoutPanel1";
            tableLayoutPanel1.Padding = new Padding(2);
            tableLayoutPanel1.RowCount = 1;
            tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
            tableLayoutPanel1.Size = new Size(865, 55);
            tableLayoutPanel1.TabIndex = 0;
            // 
            // panel4
            // 
            panel4.Controls.Add(BrowserLabel);
            panel4.Controls.Add(label8);
            panel4.Dock = DockStyle.Fill;
            panel4.Font = new Font("Microsoft YaHei UI", 12F);
            panel4.Location = new Point(647, 2);
            panel4.Margin = new Padding(0);
            panel4.Name = "panel4";
            panel4.Padding = new Padding(5);
            panel4.Size = new Size(216, 51);
            panel4.TabIndex = 4;
            // 
            // BrowserLabel
            // 
            BrowserLabel.Dock = DockStyle.Fill;
            BrowserLabel.Location = new Point(105, 5);
            BrowserLabel.Name = "BrowserLabel";
            BrowserLabel.Size = new Size(106, 41);
            BrowserLabel.TabIndex = 1;
            BrowserLabel.Text = "不存在";
            BrowserLabel.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // label8
            // 
            label8.Dock = DockStyle.Left;
            label8.Location = new Point(5, 5);
            label8.Margin = new Padding(0);
            label8.Name = "label8";
            label8.Size = new Size(100, 41);
            label8.TabIndex = 0;
            label8.Text = "浏览器 :";
            label8.TextAlign = ContentAlignment.MiddleRight;
            // 
            // panel2
            // 
            panel2.Controls.Add(DetectionLabel);
            panel2.Controls.Add(label4);
            panel2.Dock = DockStyle.Fill;
            panel2.Font = new Font("Microsoft YaHei UI", 12F);
            panel2.Location = new Point(217, 2);
            panel2.Margin = new Padding(0);
            panel2.Name = "panel2";
            panel2.Padding = new Padding(5);
            panel2.Size = new Size(215, 51);
            panel2.TabIndex = 2;
            // 
            // DetectionLabel
            // 
            DetectionLabel.Dock = DockStyle.Fill;
            DetectionLabel.Location = new Point(105, 5);
            DetectionLabel.Name = "DetectionLabel";
            DetectionLabel.Size = new Size(105, 41);
            DetectionLabel.TabIndex = 1;
            DetectionLabel.Text = "无";
            DetectionLabel.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // label4
            // 
            label4.Dock = DockStyle.Left;
            label4.Location = new Point(5, 5);
            label4.Margin = new Padding(0);
            label4.Name = "label4";
            label4.Size = new Size(100, 41);
            label4.TabIndex = 0;
            label4.Text = "识别 :";
            label4.TextAlign = ContentAlignment.MiddleRight;
            // 
            // panel1
            // 
            panel1.Controls.Add(StatusLabel);
            panel1.Controls.Add(label1);
            panel1.Dock = DockStyle.Fill;
            panel1.Font = new Font("Microsoft YaHei UI", 12F);
            panel1.Location = new Point(2, 2);
            panel1.Margin = new Padding(0);
            panel1.Name = "panel1";
            panel1.Padding = new Padding(5);
            panel1.Size = new Size(215, 51);
            panel1.TabIndex = 1;
            // 
            // StatusLabel
            // 
            StatusLabel.Dock = DockStyle.Fill;
            StatusLabel.Location = new Point(105, 5);
            StatusLabel.Name = "StatusLabel";
            StatusLabel.Size = new Size(105, 41);
            StatusLabel.TabIndex = 1;
            StatusLabel.Text = "未启动";
            StatusLabel.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // label1
            // 
            label1.Dock = DockStyle.Left;
            label1.Location = new Point(5, 5);
            label1.Margin = new Padding(0);
            label1.Name = "label1";
            label1.Size = new Size(100, 41);
            label1.TabIndex = 0;
            label1.Text = "状态 :";
            label1.TextAlign = ContentAlignment.MiddleRight;
            // 
            // panel3
            // 
            panel3.Controls.Add(ResultLabel);
            panel3.Controls.Add(label6);
            panel3.Dock = DockStyle.Fill;
            panel3.Font = new Font("Microsoft YaHei UI", 12F);
            panel3.Location = new Point(432, 2);
            panel3.Margin = new Padding(0);
            panel3.Name = "panel3";
            panel3.Padding = new Padding(5);
            panel3.Size = new Size(215, 51);
            panel3.TabIndex = 3;
            // 
            // ResultLabel
            // 
            ResultLabel.Dock = DockStyle.Fill;
            ResultLabel.Location = new Point(105, 5);
            ResultLabel.Name = "ResultLabel";
            ResultLabel.Size = new Size(105, 41);
            ResultLabel.TabIndex = 1;
            ResultLabel.Text = "无";
            ResultLabel.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // label6
            // 
            label6.Dock = DockStyle.Left;
            label6.Location = new Point(5, 5);
            label6.Margin = new Padding(0);
            label6.Name = "label6";
            label6.Size = new Size(100, 41);
            label6.TabIndex = 0;
            label6.Text = "结果 :";
            label6.TextAlign = ContentAlignment.MiddleRight;
            // 
            // panel5
            // 
            panel5.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            panel5.BackColor = Color.WhiteSmoke;
            panel5.Controls.Add(tableLayoutPanel2);
            panel5.Controls.Add(label9);
            panel5.Location = new Point(20, 100);
            panel5.Margin = new Padding(0);
            panel5.Name = "panel5";
            panel5.Size = new Size(865, 227);
            panel5.TabIndex = 1;
            // 
            // tableLayoutPanel2
            // 
            tableLayoutPanel2.ColumnCount = 4;
            tableLayoutPanel2.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 25F));
            tableLayoutPanel2.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 25F));
            tableLayoutPanel2.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 25F));
            tableLayoutPanel2.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 25F));
            tableLayoutPanel2.Controls.Add(SwitchConfigBtn, 3, 2);
            tableLayoutPanel2.Controls.Add(SwitchBrowserBtn, 2, 2);
            tableLayoutPanel2.Controls.Add(SwitchVersionBtn, 1, 2);
            tableLayoutPanel2.Controls.Add(ReinstallBtn, 0, 2);
            tableLayoutPanel2.Controls.Add(DetectionBtn, 3, 1);
            tableLayoutPanel2.Controls.Add(DetectNetworkBtn, 2, 1);
            tableLayoutPanel2.Controls.Add(IPSwitchBtn, 1, 1);
            tableLayoutPanel2.Controls.Add(ResetPositionBtn, 0, 1);
            tableLayoutPanel2.Controls.Add(SwitchDetectionBtn, 3, 0);
            tableLayoutPanel2.Controls.Add(RegularMatchBtn, 2, 0);
            tableLayoutPanel2.Controls.Add(BootUpBtn, 1, 0);
            tableLayoutPanel2.Controls.Add(RestartBtn, 0, 0);
            tableLayoutPanel2.Location = new Point(150, 40);
            tableLayoutPanel2.Margin = new Padding(0);
            tableLayoutPanel2.Name = "tableLayoutPanel2";
            tableLayoutPanel2.RowCount = 3;
            tableLayoutPanel2.RowStyles.Add(new RowStyle(SizeType.Percent, 33.33F));
            tableLayoutPanel2.RowStyles.Add(new RowStyle(SizeType.Percent, 33.33F));
            tableLayoutPanel2.RowStyles.Add(new RowStyle(SizeType.Percent, 33.34F));
            tableLayoutPanel2.Size = new Size(670, 150);
            tableLayoutPanel2.TabIndex = 2;
            // 
            // SwitchConfigBtn
            // 
            SwitchConfigBtn.Anchor = AnchorStyles.None;
            SwitchConfigBtn.BackColor = Color.Black;
            SwitchConfigBtn.FlatStyle = FlatStyle.System;
            SwitchConfigBtn.Font = new Font("Microsoft YaHei UI", 10F);
            SwitchConfigBtn.Location = new Point(525, 106);
            SwitchConfigBtn.Margin = new Padding(0);
            SwitchConfigBtn.Name = "SwitchConfigBtn";
            SwitchConfigBtn.Size = new Size(120, 35);
            SwitchConfigBtn.TabIndex = 11;
            SwitchConfigBtn.TabStop = false;
            SwitchConfigBtn.Text = "更换配置";
            SwitchConfigBtn.UseVisualStyleBackColor = false;
            // 
            // SwitchBrowserBtn
            // 
            SwitchBrowserBtn.Anchor = AnchorStyles.None;
            SwitchBrowserBtn.BackColor = Color.Black;
            SwitchBrowserBtn.FlatStyle = FlatStyle.System;
            SwitchBrowserBtn.Font = new Font("Microsoft YaHei UI", 10F);
            SwitchBrowserBtn.Location = new Point(357, 106);
            SwitchBrowserBtn.Margin = new Padding(0);
            SwitchBrowserBtn.Name = "SwitchBrowserBtn";
            SwitchBrowserBtn.Size = new Size(120, 35);
            SwitchBrowserBtn.TabIndex = 10;
            SwitchBrowserBtn.TabStop = false;
            SwitchBrowserBtn.Text = "切换自带";
            SwitchBrowserBtn.UseVisualStyleBackColor = false;
            // 
            // SwitchVersionBtn
            // 
            SwitchVersionBtn.Anchor = AnchorStyles.None;
            SwitchVersionBtn.BackColor = Color.Black;
            SwitchVersionBtn.FlatStyle = FlatStyle.System;
            SwitchVersionBtn.Font = new Font("Microsoft YaHei UI", 10F);
            SwitchVersionBtn.Location = new Point(190, 106);
            SwitchVersionBtn.Margin = new Padding(0);
            SwitchVersionBtn.Name = "SwitchVersionBtn";
            SwitchVersionBtn.Size = new Size(120, 35);
            SwitchVersionBtn.TabIndex = 9;
            SwitchVersionBtn.TabStop = false;
            SwitchVersionBtn.Text = "版本切换";
            SwitchVersionBtn.UseVisualStyleBackColor = false;
            // 
            // ReinstallBtn
            // 
            ReinstallBtn.Anchor = AnchorStyles.None;
            ReinstallBtn.BackColor = Color.Black;
            ReinstallBtn.FlatStyle = FlatStyle.System;
            ReinstallBtn.Font = new Font("Microsoft YaHei UI", 10F);
            ReinstallBtn.Location = new Point(23, 106);
            ReinstallBtn.Margin = new Padding(0);
            ReinstallBtn.Name = "ReinstallBtn";
            ReinstallBtn.Size = new Size(120, 35);
            ReinstallBtn.TabIndex = 8;
            ReinstallBtn.TabStop = false;
            ReinstallBtn.Text = "重新安装";
            ReinstallBtn.UseVisualStyleBackColor = false;
            // 
            // DetectionBtn
            // 
            DetectionBtn.Anchor = AnchorStyles.None;
            DetectionBtn.BackColor = Color.Black;
            DetectionBtn.FlatStyle = FlatStyle.System;
            DetectionBtn.Font = new Font("Microsoft YaHei UI", 10F);
            DetectionBtn.Location = new Point(525, 56);
            DetectionBtn.Margin = new Padding(0);
            DetectionBtn.Name = "DetectionBtn";
            DetectionBtn.Size = new Size(120, 35);
            DetectionBtn.TabIndex = 7;
            DetectionBtn.TabStop = false;
            DetectionBtn.Text = "识别区域";
            DetectionBtn.UseVisualStyleBackColor = false;
            DetectionBtn.Click += DetectionBtn_Click;
            // 
            // DetectNetworkBtn
            // 
            DetectNetworkBtn.Anchor = AnchorStyles.None;
            DetectNetworkBtn.BackColor = Color.Black;
            DetectNetworkBtn.FlatStyle = FlatStyle.System;
            DetectNetworkBtn.Font = new Font("Microsoft YaHei UI", 10F);
            DetectNetworkBtn.Location = new Point(357, 56);
            DetectNetworkBtn.Margin = new Padding(0);
            DetectNetworkBtn.Name = "DetectNetworkBtn";
            DetectNetworkBtn.Size = new Size(120, 35);
            DetectNetworkBtn.TabIndex = 6;
            DetectNetworkBtn.TabStop = false;
            DetectNetworkBtn.Text = "网络检测";
            DetectNetworkBtn.UseVisualStyleBackColor = false;
            // 
            // IPSwitchBtn
            // 
            IPSwitchBtn.Anchor = AnchorStyles.None;
            IPSwitchBtn.BackColor = Color.Black;
            IPSwitchBtn.FlatStyle = FlatStyle.System;
            IPSwitchBtn.Font = new Font("Microsoft YaHei UI", 10F);
            IPSwitchBtn.Location = new Point(190, 56);
            IPSwitchBtn.Margin = new Padding(0);
            IPSwitchBtn.Name = "IPSwitchBtn";
            IPSwitchBtn.Size = new Size(120, 35);
            IPSwitchBtn.TabIndex = 5;
            IPSwitchBtn.TabStop = false;
            IPSwitchBtn.Text = "IP变更";
            IPSwitchBtn.UseVisualStyleBackColor = false;
            // 
            // ResetPositionBtn
            // 
            ResetPositionBtn.Anchor = AnchorStyles.None;
            ResetPositionBtn.BackColor = Color.Black;
            ResetPositionBtn.FlatStyle = FlatStyle.System;
            ResetPositionBtn.Font = new Font("Microsoft YaHei UI", 10F);
            ResetPositionBtn.Location = new Point(23, 56);
            ResetPositionBtn.Margin = new Padding(0);
            ResetPositionBtn.Name = "ResetPositionBtn";
            ResetPositionBtn.Size = new Size(120, 35);
            ResetPositionBtn.TabIndex = 4;
            ResetPositionBtn.TabStop = false;
            ResetPositionBtn.Text = "重设位置";
            ResetPositionBtn.UseVisualStyleBackColor = false;
            // 
            // SwitchDetectionBtn
            // 
            SwitchDetectionBtn.Anchor = AnchorStyles.None;
            SwitchDetectionBtn.BackColor = Color.Black;
            SwitchDetectionBtn.FlatStyle = FlatStyle.System;
            SwitchDetectionBtn.Font = new Font("Microsoft YaHei UI", 10F);
            SwitchDetectionBtn.Location = new Point(525, 7);
            SwitchDetectionBtn.Margin = new Padding(0);
            SwitchDetectionBtn.Name = "SwitchDetectionBtn";
            SwitchDetectionBtn.Size = new Size(120, 35);
            SwitchDetectionBtn.TabIndex = 3;
            SwitchDetectionBtn.TabStop = false;
            SwitchDetectionBtn.Text = "切换识别";
            SwitchDetectionBtn.UseVisualStyleBackColor = false;
            // 
            // RegularMatchBtn
            // 
            RegularMatchBtn.Anchor = AnchorStyles.None;
            RegularMatchBtn.BackColor = Color.Black;
            RegularMatchBtn.FlatStyle = FlatStyle.System;
            RegularMatchBtn.Font = new Font("Microsoft YaHei UI", 10F);
            RegularMatchBtn.Location = new Point(357, 7);
            RegularMatchBtn.Margin = new Padding(0);
            RegularMatchBtn.Name = "RegularMatchBtn";
            RegularMatchBtn.Size = new Size(120, 35);
            RegularMatchBtn.TabIndex = 2;
            RegularMatchBtn.TabStop = false;
            RegularMatchBtn.Text = "正则匹配";
            RegularMatchBtn.UseVisualStyleBackColor = false;
            // 
            // BootUpBtn
            // 
            BootUpBtn.Anchor = AnchorStyles.None;
            BootUpBtn.BackColor = Color.Black;
            BootUpBtn.FlatStyle = FlatStyle.System;
            BootUpBtn.Font = new Font("Microsoft YaHei UI", 10F);
            BootUpBtn.Location = new Point(190, 7);
            BootUpBtn.Margin = new Padding(0);
            BootUpBtn.Name = "BootUpBtn";
            BootUpBtn.Size = new Size(120, 35);
            BootUpBtn.TabIndex = 1;
            BootUpBtn.TabStop = false;
            BootUpBtn.Text = "开机启动";
            BootUpBtn.UseVisualStyleBackColor = false;
            // 
            // RestartBtn
            // 
            RestartBtn.Anchor = AnchorStyles.None;
            RestartBtn.BackColor = Color.Black;
            RestartBtn.FlatStyle = FlatStyle.System;
            RestartBtn.Font = new Font("Microsoft YaHei UI", 10F);
            RestartBtn.Location = new Point(23, 7);
            RestartBtn.Margin = new Padding(0);
            RestartBtn.Name = "RestartBtn";
            RestartBtn.Size = new Size(120, 35);
            RestartBtn.TabIndex = 0;
            RestartBtn.TabStop = false;
            RestartBtn.Text = "重新启动";
            RestartBtn.UseVisualStyleBackColor = false;
            // 
            // label9
            // 
            label9.BackColor = Color.WhiteSmoke;
            label9.Font = new Font("Microsoft YaHei UI", 12F, FontStyle.Bold);
            label9.ForeColor = Color.DeepSkyBlue;
            label9.Location = new Point(5, 5);
            label9.Name = "label9";
            label9.Size = new Size(102, 25);
            label9.TabIndex = 0;
            label9.Text = "可选操作";
            label9.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // panel6
            // 
            panel6.Anchor = AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            panel6.BackColor = Color.WhiteSmoke;
            panel6.Controls.Add(tableLayoutPanel3);
            panel6.Controls.Add(label10);
            panel6.Location = new Point(20, 357);
            panel6.Margin = new Padding(0);
            panel6.Name = "panel6";
            panel6.Size = new Size(865, 167);
            panel6.TabIndex = 2;
            // 
            // tableLayoutPanel3
            // 
            tableLayoutPanel3.ColumnCount = 4;
            tableLayoutPanel3.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 25F));
            tableLayoutPanel3.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 25F));
            tableLayoutPanel3.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 25F));
            tableLayoutPanel3.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 25F));
            tableLayoutPanel3.Controls.Add(ClickToOpenCheck, 3, 1);
            tableLayoutPanel3.Controls.Add(BlackScreenOrCrashCheck, 2, 1);
            tableLayoutPanel3.Controls.Add(TotalNoResultCheck, 1, 1);
            tableLayoutPanel3.Controls.Add(IndividualNoResultCheck, 0, 1);
            tableLayoutPanel3.Controls.Add(IdentifyNonIDCheck, 3, 0);
            tableLayoutPanel3.Controls.Add(IncorrectIDCheck, 2, 0);
            tableLayoutPanel3.Controls.Add(OutOfRightCheck, 1, 0);
            tableLayoutPanel3.Controls.Add(StartupCheck, 0, 0);
            tableLayoutPanel3.Location = new Point(150, 40);
            tableLayoutPanel3.Margin = new Padding(0);
            tableLayoutPanel3.Name = "tableLayoutPanel3";
            tableLayoutPanel3.RowCount = 2;
            tableLayoutPanel3.RowStyles.Add(new RowStyle(SizeType.Percent, 50F));
            tableLayoutPanel3.RowStyles.Add(new RowStyle(SizeType.Percent, 50F));
            tableLayoutPanel3.Size = new Size(670, 100);
            tableLayoutPanel3.TabIndex = 2;
            // 
            // ClickToOpenCheck
            // 
            ClickToOpenCheck.Anchor = AnchorStyles.None;
            ClickToOpenCheck.Font = new Font("Microsoft YaHei UI", 10F);
            ClickToOpenCheck.Location = new Point(525, 57);
            ClickToOpenCheck.Margin = new Padding(0);
            ClickToOpenCheck.Name = "ClickToOpenCheck";
            ClickToOpenCheck.Size = new Size(120, 35);
            ClickToOpenCheck.TabIndex = 10;
            ClickToOpenCheck.Text = "点击打不开";
            ClickToOpenCheck.TextAlign = ContentAlignment.MiddleCenter;
            ClickToOpenCheck.UseVisualStyleBackColor = true;
            // 
            // BlackScreenOrCrashCheck
            // 
            BlackScreenOrCrashCheck.Anchor = AnchorStyles.None;
            BlackScreenOrCrashCheck.Font = new Font("Microsoft YaHei UI", 10F);
            BlackScreenOrCrashCheck.Location = new Point(357, 57);
            BlackScreenOrCrashCheck.Margin = new Padding(0);
            BlackScreenOrCrashCheck.Name = "BlackScreenOrCrashCheck";
            BlackScreenOrCrashCheck.Size = new Size(120, 35);
            BlackScreenOrCrashCheck.TabIndex = 9;
            BlackScreenOrCrashCheck.Text = "黑屏或崩溃";
            BlackScreenOrCrashCheck.TextAlign = ContentAlignment.MiddleCenter;
            BlackScreenOrCrashCheck.UseVisualStyleBackColor = true;
            // 
            // TotalNoResultCheck
            // 
            TotalNoResultCheck.Anchor = AnchorStyles.None;
            TotalNoResultCheck.Font = new Font("Microsoft YaHei UI", 10F);
            TotalNoResultCheck.Location = new Point(190, 57);
            TotalNoResultCheck.Margin = new Padding(0);
            TotalNoResultCheck.Name = "TotalNoResultCheck";
            TotalNoResultCheck.Size = new Size(120, 35);
            TotalNoResultCheck.TabIndex = 8;
            TotalNoResultCheck.Text = "全部无结果";
            TotalNoResultCheck.TextAlign = ContentAlignment.MiddleCenter;
            TotalNoResultCheck.UseVisualStyleBackColor = true;
            // 
            // IndividualNoResultCheck
            // 
            IndividualNoResultCheck.Anchor = AnchorStyles.None;
            IndividualNoResultCheck.Font = new Font("Microsoft YaHei UI", 10F);
            IndividualNoResultCheck.Location = new Point(23, 57);
            IndividualNoResultCheck.Margin = new Padding(0);
            IndividualNoResultCheck.Name = "IndividualNoResultCheck";
            IndividualNoResultCheck.Size = new Size(120, 35);
            IndividualNoResultCheck.TabIndex = 7;
            IndividualNoResultCheck.Text = "个别无结果";
            IndividualNoResultCheck.TextAlign = ContentAlignment.MiddleCenter;
            IndividualNoResultCheck.UseVisualStyleBackColor = true;
            // 
            // IdentifyNonIDCheck
            // 
            IdentifyNonIDCheck.Anchor = AnchorStyles.None;
            IdentifyNonIDCheck.Font = new Font("Microsoft YaHei UI", 10F);
            IdentifyNonIDCheck.Location = new Point(525, 7);
            IdentifyNonIDCheck.Margin = new Padding(0);
            IdentifyNonIDCheck.Name = "IdentifyNonIDCheck";
            IdentifyNonIDCheck.Size = new Size(120, 35);
            IdentifyNonIDCheck.TabIndex = 6;
            IdentifyNonIDCheck.Text = "识别非编号";
            IdentifyNonIDCheck.TextAlign = ContentAlignment.MiddleCenter;
            IdentifyNonIDCheck.UseVisualStyleBackColor = true;
            // 
            // IncorrectIDCheck
            // 
            IncorrectIDCheck.Anchor = AnchorStyles.None;
            IncorrectIDCheck.Font = new Font("Microsoft YaHei UI", 10F);
            IncorrectIDCheck.Location = new Point(357, 7);
            IncorrectIDCheck.Margin = new Padding(0);
            IncorrectIDCheck.Name = "IncorrectIDCheck";
            IncorrectIDCheck.Size = new Size(120, 35);
            IncorrectIDCheck.TabIndex = 5;
            IncorrectIDCheck.Text = "编号不准确";
            IncorrectIDCheck.TextAlign = ContentAlignment.MiddleCenter;
            IncorrectIDCheck.UseVisualStyleBackColor = true;
            // 
            // OutOfRightCheck
            // 
            OutOfRightCheck.Anchor = AnchorStyles.None;
            OutOfRightCheck.Font = new Font("Microsoft YaHei UI", 10F);
            OutOfRightCheck.Location = new Point(190, 7);
            OutOfRightCheck.Margin = new Padding(0);
            OutOfRightCheck.Name = "OutOfRightCheck";
            OutOfRightCheck.Size = new Size(120, 35);
            OutOfRightCheck.TabIndex = 4;
            OutOfRightCheck.Text = "看不到程序";
            OutOfRightCheck.TextAlign = ContentAlignment.MiddleCenter;
            OutOfRightCheck.UseVisualStyleBackColor = true;
            // 
            // StartupCheck
            // 
            StartupCheck.Anchor = AnchorStyles.None;
            StartupCheck.Font = new Font("Microsoft YaHei UI", 10F);
            StartupCheck.Location = new Point(23, 7);
            StartupCheck.Margin = new Padding(0);
            StartupCheck.Name = "StartupCheck";
            StartupCheck.Size = new Size(120, 35);
            StartupCheck.TabIndex = 3;
            StartupCheck.Text = "启动报错了";
            StartupCheck.TextAlign = ContentAlignment.MiddleCenter;
            StartupCheck.UseVisualStyleBackColor = true;
            // 
            // label10
            // 
            label10.BackColor = Color.WhiteSmoke;
            label10.Font = new Font("Microsoft YaHei UI", 12F, FontStyle.Bold);
            label10.ForeColor = Color.DeepSkyBlue;
            label10.Location = new Point(5, 5);
            label10.Name = "label10";
            label10.Size = new Size(102, 25);
            label10.TabIndex = 1;
            label10.Text = "其他描述";
            label10.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // MainForm
            // 
            AutoScaleDimensions = new SizeF(9F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.White;
            ClientSize = new Size(906, 533);
            Controls.Add(panel6);
            Controls.Add(panel5);
            Controls.Add(tableLayoutPanel1);
            Icon = (Icon)resources.GetObject("$this.Icon");
            Name = "MainForm";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "修复工具";
            Load += MainForm_Load;
            tableLayoutPanel1.ResumeLayout(false);
            panel4.ResumeLayout(false);
            panel2.ResumeLayout(false);
            panel1.ResumeLayout(false);
            panel3.ResumeLayout(false);
            panel5.ResumeLayout(false);
            tableLayoutPanel2.ResumeLayout(false);
            panel6.ResumeLayout(false);
            tableLayoutPanel3.ResumeLayout(false);
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private TableLayoutPanel tableLayoutPanel1;
        private Panel panel1;
        private Label StatusLabel;
        private Label label1;
        private Panel panel4;
        private Label BrowserLabel;
        private Label label8;
        private Panel panel2;
        private Label DetectionLabel;
        private Label label4;
        private Panel panel3;
        private Label ResultLabel;
        private Label label6;
        private Panel panel5;
        private Panel panel6;
        private Label label9;
        private Button RestartBtn;
        private TableLayoutPanel tableLayoutPanel2;
        private Button SwitchConfigBtn;
        private Button SwitchBrowserBtn;
        private Button SwitchVersionBtn;
        private Button ReinstallBtn;
        private Button DetectionBtn;
        private Button DetectNetworkBtn;
        private Button IPSwitchBtn;
        private Button ResetPositionBtn;
        private Button SwitchDetectionBtn;
        private Button RegularMatchBtn;
        private Button BootUpBtn;
        private Label label10;
        private CheckBox StartupCheck;
        private TableLayoutPanel tableLayoutPanel3;
        private CheckBox ClickToOpenCheck;
        private CheckBox BlackScreenOrCrashCheck;
        private CheckBox TotalNoResultCheck;
        private CheckBox IndividualNoResultCheck;
        private CheckBox IdentifyNonIDCheck;
        private CheckBox IncorrectIDCheck;
        private CheckBox OutOfRightCheck;
    }
}
