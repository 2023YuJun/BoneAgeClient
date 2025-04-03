namespace ConfigWindowApp
{
    partial class InputForm
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
            OKBtn = new Button();
            textBox1 = new TextBox();
            label = new Label();
            SuspendLayout();
            // 
            // OKBtn
            // 
            OKBtn.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
            OKBtn.BackColor = Color.Black;
            OKBtn.FlatStyle = FlatStyle.System;
            OKBtn.Font = new Font("Microsoft YaHei UI", 10F);
            OKBtn.Location = new Point(560, 50);
            OKBtn.Margin = new Padding(0);
            OKBtn.Name = "OKBtn";
            OKBtn.Size = new Size(120, 35);
            OKBtn.TabIndex = 1;
            OKBtn.TabStop = false;
            OKBtn.Text = "确定";
            OKBtn.UseVisualStyleBackColor = false;
            OKBtn.Click += OKBtn_Click;
            // 
            // textBox1
            // 
            textBox1.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
            textBox1.Font = new Font("Microsoft YaHei UI", 12F);
            textBox1.Location = new Point(10, 50);
            textBox1.Multiline = true;
            textBox1.Name = "textBox1";
            textBox1.Size = new Size(530, 35);
            textBox1.TabIndex = 2;
            textBox1.KeyUp += textBox1_KeyUp;
            // 
            // label
            // 
            label.AutoSize = true;
            label.Location = new Point(5, 10);
            label.Name = "label";
            label.Size = new Size(57, 27);
            label.TabIndex = 3;
            label.Text = "label";
            label.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // InputForm
            // 
            FormBorderStyle = FormBorderStyle.FixedSingle;
            MaximizeBox = false;
            MinimizeBox = false;
            AutoScaleDimensions = new SizeF(12F, 27F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(692, 103);
            Controls.Add(label);
            Controls.Add(textBox1);
            Controls.Add(OKBtn);
            Font = new Font("Microsoft YaHei UI", 12F);
            Margin = new Padding(4);
            MinimumSize = new Size(710, 150);
            Name = "InputForm";
            ShowIcon = false;
            StartPosition = FormStartPosition.CenterScreen;
            Text = "InputForm";
            Load += InputForm_Load;
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Button OKBtn;
        private TextBox textBox1;
        private Label label;
    }
}