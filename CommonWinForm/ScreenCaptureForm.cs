using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CommonWinForm
{
    public partial class ScreenCaptureForm : Form
    {
        // 记录用户最终是否点击确定
        public bool IsConfirmed { get; private set; } = false;
        // 记录用户选择的区域（屏幕坐标）
        public Rectangle SelectedRegion { get; private set; }
        public ScreenCaptureForm()
        {
            InitializeComponent();
        }
        private void ResetSelection(object sender, EventArgs e)
        {
            selectionRect = Rectangle.Empty;
            controlPanel.Visible = false;
            isSelecting = false;
            Invalidate();
        }

        private void CancelSelection(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
            Close();
        }

        private void ConfirmSelection(object sender, EventArgs e)
        {
            // 点击确定后记录区域并关闭窗体
            IsConfirmed = true;
            SelectedRegion = selectionRect;
            DialogResult = DialogResult.OK;
            Close();
        }

        private void ScreenCaptureForm_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                startPoint = e.Location;
                selectionRect = new Rectangle(e.Location, new Size(0, 0));
                isSelecting = true;
                controlPanel.Visible = false;
            }
        }

        private void ScreenCaptureForm_MouseMove(object sender, MouseEventArgs e)
        {
            if (isSelecting)
            {
                int x = Math.Min(e.X, startPoint.X);
                int y = Math.Min(e.Y, startPoint.Y);
                int width = Math.Abs(e.X - startPoint.X);
                int height = Math.Abs(e.Y - startPoint.Y);
                selectionRect = new Rectangle(x, y, width, height);
                Invalidate();
            }
        }

        private void ScreenCaptureForm_MouseUp(object sender, MouseEventArgs e)
        {
            if (isSelecting)
            {
                isSelecting = false;
                if (selectionRect.Width > 10 && selectionRect.Height > 10)
                {
                    // 将控制面板放在框选区域右下角
                    int panelX = selectionRect.Right - controlPanel.Width;
                    int panelY = selectionRect.Bottom;
                    // 获取屏幕工作区（不包括任务栏）
                    Rectangle screenBounds = Screen.PrimaryScreen.WorkingArea;
                    if (panelY + controlPanel.Height > screenBounds.Bottom)
                        panelY = selectionRect.Bottom - controlPanel.Height;
                    controlPanel.Location = new Point(panelX, panelY);
                    controlPanel.Visible = true;
                }
                else
                {
                    selectionRect = Rectangle.Empty;
                }
                Invalidate();
            }
        }

        private void ScreenCaptureForm_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape)
            {
                this.DialogResult = DialogResult.Cancel;
                this.Close();
            }
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);
            if (!selectionRect.IsEmpty)
            {
                // 绘制选中区域的矩形框
                using (Pen pen = new Pen(Color.LightGray, 2))
                {
                    e.Graphics.DrawRectangle(pen, selectionRect);
                }
            }
        }
        /// <summary>
        /// 根据用户选择的区域截取屏幕图片
        /// </summary>
        /// <returns>截取的图片</returns>
        public Bitmap CaptureSelectedRegion()
        {
            if (selectionRect.IsEmpty)
                return null;
            Bitmap bmp = new Bitmap(selectionRect.Width, selectionRect.Height, PixelFormat.Format32bppArgb);
            using (Graphics g = Graphics.FromImage(bmp))
            {
                // 注意：屏幕坐标和窗体坐标一致（全屏覆盖时）
                g.CopyFromScreen(selectionRect.Location, Point.Empty, selectionRect.Size);
            }
            return bmp;
        }
    }
}
