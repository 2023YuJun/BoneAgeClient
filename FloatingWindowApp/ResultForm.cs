using Common.Config;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace FloatingWindowApp
{
    public partial class ResultForm : Form
    {
        protected override bool ShowWithoutActivation => true;
        public ResultForm()
        {
            InitializeComponent();
        }
        public string ResultLabelText
        {
            get => resultLabel.Text;
            set
            {
                if (resultLabel.Text != value)
                {
                    resultLabel.Text = value;
                }
            }
        }

        private void resultLabel_TextChanged(object sender, EventArgs e)
        {
            // 计算文本所需的宽度
            int textWidth = TextRenderer.MeasureText(resultLabel.Text, resultLabel.Font, new Size(resultLabel.Width, 0), TextFormatFlags.SingleLine).Width;
            // 如果文本宽度大于窗体可用宽度，则调整窗体宽度
            if (textWidth > this.ClientSize.Width - this.Padding.Horizontal)
            {
                this.Width = textWidth + this.Padding.Horizontal;
            }
        }

        private void resultLabel_Click(object sender, EventArgs e)
        {
            try
            {
                var settings = ConfigProvider.Settings.GetConfig();
                string resultUrl = settings.ResultUrl;
                string currentBrowser = settings.CurrentBrowser;
                string defaultBrowserPath = settings.DefaultBrowserPath;
                string ourBrowserPath = settings.OurBrowserPath;
                if (string.IsNullOrEmpty(resultUrl))
                {
                    return;
                }
                if (string.IsNullOrEmpty(currentBrowser))
                {
                    MessageBox.Show("请先使用配置工具选择浏览器！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }
                if (currentBrowser == "default" && !string.IsNullOrEmpty(defaultBrowserPath))
                {
                    System.Diagnostics.Process.Start(defaultBrowserPath, resultUrl);
                }
                else if (currentBrowser == "our" && !string.IsNullOrEmpty(ourBrowserPath))
                {
                    System.Diagnostics.Process.Start(ourBrowserPath, resultUrl);
                }
                else return;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"打开浏览器失败：{ex.Message}", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                throw;
            }
        }
    }
}
