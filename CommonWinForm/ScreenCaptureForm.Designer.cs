namespace CommonWinForm
{
    partial class ScreenCaptureForm
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
            controlPanel = new Panel();
            btnRetry = new Button();
            btnCancel = new Button();
            btnConfirm = new Button();

            controlPanel.Size = new Size(200, 40);
            controlPanel.BackColor = Color.FromArgb(200, Color.LightGray);
            controlPanel.Visible = false;

            btnRetry.Text = "重试";
            btnRetry.Size = new Size(60, 30);
            btnRetry.Location = new Point(5, 5);
            btnRetry.Click += ResetSelection;

            btnCancel.Text = "取消";
            btnCancel.Size = new Size(60, 30);
            btnCancel.Location = new Point(70, 5);
            btnCancel.Click += CancelSelection;

            btnConfirm.Text = "确定";
            btnConfirm.Size = new Size(60, 30);
            btnConfirm.Location = new Point(135, 5);
            btnConfirm.Click += ConfirmSelection;

            controlPanel.Controls.Add(btnRetry);
            controlPanel.Controls.Add(btnCancel);
            controlPanel.Controls.Add(btnConfirm);
            this.Controls.Add(controlPanel);
            this.components = new System.ComponentModel.Container();
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Text = "ScreenCaptureForm";
            this.FormBorderStyle = FormBorderStyle.None;
            this.WindowState = FormWindowState.Maximized;
            this.BackColor = Color.Black;
            this.Opacity = 0.7;
            this.TopMost = true;
            this.DoubleBuffered = true;
            this.MouseDown += ScreenCaptureForm_MouseDown;
            this.MouseMove += ScreenCaptureForm_MouseMove;
            this.MouseUp += ScreenCaptureForm_MouseUp;
            this.KeyDown += ScreenCaptureForm_KeyDown;
        }

        #endregion

        private Point startPoint;
        private Rectangle selectionRect;
        private bool isSelecting = false;

        // 面板包含三个按钮
        private Panel controlPanel;
        private Button btnRetry;
        private Button btnCancel;
        private Button btnConfirm;
    }
}