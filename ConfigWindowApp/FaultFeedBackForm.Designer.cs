namespace ConfigWindowApp
{
    partial class FaultFeedBackForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
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
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            tableLayoutPanel2 = new TableLayoutPanel();
            SeedFaultBtn = new Button();
            ReinstallBrowserBtn = new Button();
            UpdateBrowserBtn = new Button();
            SwitchVersionBtn = new Button();
            OwnBrowserBtn = new Button();
            UserBrowserBtn = new Button();
            panel1 = new Panel();
            label1 = new Label();
            pictureBox1 = new PictureBox();
            tableLayoutPanel2.SuspendLayout();
            panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)pictureBox1).BeginInit();
            SuspendLayout();
            // 
            // tableLayoutPanel2
            // 
            tableLayoutPanel2.ColumnCount = 3;
            tableLayoutPanel2.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 25F));
            tableLayoutPanel2.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 25F));
            tableLayoutPanel2.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 25F));
            tableLayoutPanel2.Controls.Add(SeedFaultBtn, 2, 1);
            tableLayoutPanel2.Controls.Add(ReinstallBrowserBtn, 1, 1);
            tableLayoutPanel2.Controls.Add(UpdateBrowserBtn, 0, 1);
            tableLayoutPanel2.Controls.Add(SwitchVersionBtn, 2, 0);
            tableLayoutPanel2.Controls.Add(OwnBrowserBtn, 1, 0);
            tableLayoutPanel2.Controls.Add(UserBrowserBtn, 0, 0);
            tableLayoutPanel2.Location = new Point(20, 200);
            tableLayoutPanel2.Margin = new Padding(0);
            tableLayoutPanel2.Name = "tableLayoutPanel2";
            tableLayoutPanel2.RowCount = 2;
            tableLayoutPanel2.RowStyles.Add(new RowStyle(SizeType.Percent, 33.33F));
            tableLayoutPanel2.RowStyles.Add(new RowStyle(SizeType.Percent, 33.33F));
            tableLayoutPanel2.Size = new Size(604, 136);
            tableLayoutPanel2.TabIndex = 3;
            // 
            // SeedFaultBtn
            // 
            SeedFaultBtn.Anchor = AnchorStyles.None;
            SeedFaultBtn.BackColor = Color.Black;
            SeedFaultBtn.FlatStyle = FlatStyle.System;
            SeedFaultBtn.Font = new Font("Microsoft YaHei UI", 10F);
            SeedFaultBtn.Location = new Point(433, 82);
            SeedFaultBtn.Margin = new Padding(0);
            SeedFaultBtn.Name = "SeedFaultBtn";
            SeedFaultBtn.Size = new Size(140, 40);
            SeedFaultBtn.TabIndex = 6;
            SeedFaultBtn.TabStop = false;
            SeedFaultBtn.Text = "发送故障反馈";
            SeedFaultBtn.UseVisualStyleBackColor = false;
            SeedFaultBtn.Click += SeedFaultBtn_Click;
            // 
            // ReinstallBrowserBtn
            // 
            ReinstallBrowserBtn.Anchor = AnchorStyles.None;
            ReinstallBrowserBtn.BackColor = Color.Black;
            ReinstallBrowserBtn.FlatStyle = FlatStyle.System;
            ReinstallBrowserBtn.Font = new Font("Microsoft YaHei UI", 10F);
            ReinstallBrowserBtn.Location = new Point(231, 82);
            ReinstallBrowserBtn.Margin = new Padding(0);
            ReinstallBrowserBtn.Name = "ReinstallBrowserBtn";
            ReinstallBrowserBtn.Size = new Size(140, 40);
            ReinstallBrowserBtn.TabIndex = 5;
            ReinstallBrowserBtn.TabStop = false;
            ReinstallBrowserBtn.Text = "重新安装浏览器";
            ReinstallBrowserBtn.UseVisualStyleBackColor = false;
            ReinstallBrowserBtn.Click += ReinstallBrowserBtn_Click;
            // 
            // UpdateBrowserBtn
            // 
            UpdateBrowserBtn.Anchor = AnchorStyles.None;
            UpdateBrowserBtn.BackColor = Color.Black;
            UpdateBrowserBtn.FlatStyle = FlatStyle.System;
            UpdateBrowserBtn.Font = new Font("Microsoft YaHei UI", 10F);
            UpdateBrowserBtn.Location = new Point(30, 82);
            UpdateBrowserBtn.Margin = new Padding(0);
            UpdateBrowserBtn.Name = "UpdateBrowserBtn";
            UpdateBrowserBtn.Size = new Size(140, 40);
            UpdateBrowserBtn.TabIndex = 4;
            UpdateBrowserBtn.TabStop = false;
            UpdateBrowserBtn.Text = "高版本浏览器";
            UpdateBrowserBtn.UseVisualStyleBackColor = false;
            UpdateBrowserBtn.Click += UpdateBrowserBtn_Click;
            // 
            // SwitchVersionBtn
            // 
            SwitchVersionBtn.Anchor = AnchorStyles.None;
            SwitchVersionBtn.BackColor = Color.Black;
            SwitchVersionBtn.FlatStyle = FlatStyle.System;
            SwitchVersionBtn.Font = new Font("Microsoft YaHei UI", 10F);
            SwitchVersionBtn.Location = new Point(433, 14);
            SwitchVersionBtn.Margin = new Padding(0);
            SwitchVersionBtn.Name = "SwitchVersionBtn";
            SwitchVersionBtn.Size = new Size(140, 40);
            SwitchVersionBtn.TabIndex = 2;
            SwitchVersionBtn.TabStop = false;
            SwitchVersionBtn.Text = "程序版本切换";
            SwitchVersionBtn.UseVisualStyleBackColor = false;
            SwitchVersionBtn.Click += SwitchVersionBtn_Click;
            // 
            // OwnBrowserBtn
            // 
            OwnBrowserBtn.Anchor = AnchorStyles.None;
            OwnBrowserBtn.BackColor = Color.Black;
            OwnBrowserBtn.FlatStyle = FlatStyle.System;
            OwnBrowserBtn.Font = new Font("Microsoft YaHei UI", 10F);
            OwnBrowserBtn.Location = new Point(231, 14);
            OwnBrowserBtn.Margin = new Padding(0);
            OwnBrowserBtn.Name = "OwnBrowserBtn";
            OwnBrowserBtn.Size = new Size(140, 40);
            OwnBrowserBtn.TabIndex = 1;
            OwnBrowserBtn.TabStop = false;
            OwnBrowserBtn.Text = "使用安装浏览器";
            OwnBrowserBtn.UseVisualStyleBackColor = false;
            OwnBrowserBtn.Click += OurBrowserBtn_Click;
            // 
            // UserBrowserBtn
            // 
            UserBrowserBtn.Anchor = AnchorStyles.None;
            UserBrowserBtn.BackColor = Color.Black;
            UserBrowserBtn.FlatStyle = FlatStyle.System;
            UserBrowserBtn.Font = new Font("Microsoft YaHei UI", 10F);
            UserBrowserBtn.Location = new Point(30, 14);
            UserBrowserBtn.Margin = new Padding(0);
            UserBrowserBtn.Name = "UserBrowserBtn";
            UserBrowserBtn.Size = new Size(140, 40);
            UserBrowserBtn.TabIndex = 0;
            UserBrowserBtn.TabStop = false;
            UserBrowserBtn.Text = "使用默认浏览器";
            UserBrowserBtn.UseVisualStyleBackColor = false;
            UserBrowserBtn.Click += UserBrowserBtn_Click;
            // 
            // panel1
            // 
            panel1.Controls.Add(label1);
            panel1.Controls.Add(pictureBox1);
            panel1.Controls.Add(tableLayoutPanel2);
            panel1.Dock = DockStyle.Fill;
            panel1.Location = new Point(0, 0);
            panel1.Name = "panel1";
            panel1.Size = new Size(932, 353);
            panel1.TabIndex = 4;
            // 
            // label1
            // 
            label1.Location = new Point(20, 20);
            label1.Name = "label1";
            label1.Size = new Size(604, 174);
            label1.TabIndex = 5;
            label1.Text = "1. 是否可以看到程序界面，如果看不到请点击修复工具中的重设位置功能。\r\n2. 是否能识别报告编号，如果不可以请尝试使用右键菜单中的识别区域功能。\r\n3. 是否提示预测结果，如果全部未预测、其他电脑正常有可能是网络问题，可以咨询一下信息科。\r\n4. 是否可以打开浏览器?如果不可以请尝试以下方式(点击按钮)。\r\n5. 仍未解决，请扫描右侧二维码在线反馈。";
            // 
            // pictureBox1
            // 
            pictureBox1.Image = Properties.Resources.加微信;
            pictureBox1.Location = new Point(680, 20);
            pictureBox1.Name = "pictureBox1";
            pictureBox1.Size = new Size(230, 230);
            pictureBox1.SizeMode = PictureBoxSizeMode.StretchImage;
            pictureBox1.TabIndex = 4;
            pictureBox1.TabStop = false;
            // 
            // FaultFeedBackForm
            // 
            AutoScaleDimensions = new SizeF(9F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(932, 353);
            Controls.Add(panel1);
            Name = "FaultFeedBackForm";
            ShowIcon = false;
            StartPosition = FormStartPosition.CenterScreen;
            Text = "故障反馈";
            tableLayoutPanel2.ResumeLayout(false);
            panel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)pictureBox1).EndInit();
            ResumeLayout(false);
        }

        #endregion

        private TableLayoutPanel tableLayoutPanel2;
        private Button SeedFaultBtn;
        private Button ReinstallBrowserBtn;
        private Button UpdateBrowserBtn;
        private Button SwitchVersionBtn;
        private Button OwnBrowserBtn;
        private Button UserBrowserBtn;
        private Panel panel1;
        private PictureBox pictureBox1;
        private Label label1;
    }
}