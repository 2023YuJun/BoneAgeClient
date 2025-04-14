using Common.Config;
using Common.Helpers;
using CommonWinForm;
using System.Drawing.Imaging;

namespace ConfigWindowApp
{
    public partial class MainForm : Form
    {
        public MainForm()
        {
            InitializeComponent();
        }

        private void MainForm_Load(object sender, EventArgs e)
        {

        }
        private void RestartBtn_Click(object sender, EventArgs e)
        {

        }

        private void DetectionBtn_Click(object sender, EventArgs e)
        {
            // 最小化主窗体，确保截图时主窗体不出现在屏幕上
            this.WindowState = FormWindowState.Minimized;
            Application.DoEvents();
            System.Threading.Thread.Sleep(200);
            using (ScreenCaptureForm scf = new ScreenCaptureForm())
            {
                // 显示覆盖窗体，让用户框选区域
                if (scf.ShowDialog() == DialogResult.OK && scf.IsConfirmed)
                {
                    Bitmap capturedImage = scf.CaptureSelectedRegion();
                    if (capturedImage != null)
                    {
                        // 保存截图到文件
                        try
                        {
                            string currentDirectory = Directory.GetCurrentDirectory();
                            string projectPath = Directory.GetParent(currentDirectory).Parent.Parent.FullName;
                            string targetFolder = Path.Combine(projectPath, "DetectionImage");

                            // 如果文件夹不存在，则创建
                            if (!Directory.Exists(targetFolder))
                            {
                                Directory.CreateDirectory(targetFolder);
                            }
                            string randomFileName = Utils.GenerateRandomFileName(8);
                            string filePath = Path.Combine(targetFolder, randomFileName + ".jpeg");

                            capturedImage.Save(filePath, ImageFormat.Jpeg);
                            MessageBox.Show("截图已保存：" + filePath);
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show($"保存内容时发生错误: {ex.Message}");
                        }
                    }
                }
            }
            this.WindowState = FormWindowState.Normal;
        }

        private void BootUpBtn_Click(object sender, EventArgs e)
        {

        }

        private void RegularMatchBtn_Click(object sender, EventArgs e)
        {
            using (InputForm inputDialog = new InputForm(RegularMatchBtn.Text, "修改正则表达式"))
            {
                if (inputDialog.ShowDialog() == DialogResult.OK)
                {
                    try
                    {
                        ConfigProvider.Settings.UpdateConfig(s =>
                        {
                            s.RE = inputDialog.InputText;
                        });
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message);
                    }
                }
            }
        }

        private void SwitchDetectionBtn_Click(object sender, EventArgs e)
        {
            using (InputForm inputDialog = new InputForm(SwitchDetectionBtn.Text, "修改识别方式"))
            {
                if (inputDialog.ShowDialog() == DialogResult.OK)
                {
                    string inputText = inputDialog.InputText;
                    string[] inputTexts = inputText.Split(',');
                    try
                    {
                        ConfigProvider.Settings.UpdateConfig(s =>
                        {
                            s.IFZ = inputText;
                            s.MinArea = int.Parse(inputTexts[0]);
                            s.MaxArea = int.Parse(inputTexts[1]);
                            s.MaxAspectRatio = int.Parse(inputTexts[2]);
                            s.ColumnToTable = int.Parse(inputTexts[3]);
                        });
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message);
                    }
                }
            }
        }

        private void ResetPositionBtn_Click(object sender, EventArgs e)
        {
            ConfigProvider.Settings.UpdateConfig(s =>
            {
                s.FormLocationX = 800;
                s.FormLocationY = 50;
            });
        }

        private void IPSwitchBtn_Click(object sender, EventArgs e)
        {
            using (InputForm inputDialog = new InputForm(IPSwitchBtn.Text, "修改服务器IP"))
            {
                if (inputDialog.ShowDialog() == DialogResult.OK)
                {
                    string inputText = inputDialog.InputText;
                    try
                    {
                        ConfigProvider.Settings.UpdateConfig(s => { s.ServiceIP = inputText; });
                    }
                    catch (Exception ex) { 
                        MessageBox.Show(ex.Message);
                    }
                }
            }
        }

        private void DetectNetworkBtn_Click(object sender, EventArgs e)
        {

        }

        private void ReinstallBtn_Click(object sender, EventArgs e)
        {

        }

        private void SwitchVersionBtn_Click(object sender, EventArgs e)
        {

        }

        private void SwitchBrowserBtn_Click(object sender, EventArgs e)
        {

        }

        private void SwitchConfigBtn_Click(object sender, EventArgs e)
        {

        }
    }
}
