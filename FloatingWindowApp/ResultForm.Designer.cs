namespace FloatingWindowApp
{
    partial class ResultForm
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
            resultLabel = new Label();
            SuspendLayout();
            // 
            // resultLabel
            // 
            resultLabel.Dock = DockStyle.Fill;
            resultLabel.Location = new Point(5, 5);
            resultLabel.Margin = new Padding(0);
            resultLabel.Name = "resultLabel";
            resultLabel.Size = new Size(160, 20);
            resultLabel.TabIndex = 0;
            resultLabel.TextAlign = ContentAlignment.MiddleLeft;
            resultLabel.TextChanged += resultLabel_TextChanged;
            resultLabel.Click += resultLabel_Click;
            // 
            // ResultForm
            // 
            AutoScaleDimensions = new SizeF(9F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.Black;
            ClientSize = new Size(170, 30);
            Controls.Add(resultLabel);
            ForeColor = Color.White;
            FormBorderStyle = FormBorderStyle.None;
            MaximumSize = new Size(300, 30);
            MinimumSize = new Size(160, 30);
            Name = "ResultForm";
            Padding = new Padding(5);
            ShowIcon = false;
            ShowInTaskbar = false;
            Text = "ResultForm";
            ResumeLayout(false);
        }

        #endregion

        private Label resultLabel;
    }
}