using Common;
using Common.Config;
using Common.Helpers;
using CommonWinForm;
using System.Diagnostics;
using System.Drawing.Imaging;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;

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
            var settings = ConfigProvider.Settings.GetConfig();
            string appname = settings.AppName;
            string appPath = settings.AppPath;
            if (!System.IO.File.Exists(appPath))
            {
                MessageBox.Show("未找到主程序！");
                return;
            }
            if (Process.GetProcessesByName(appname).Length > 0)
            {
                DialogResult result = MessageBox.Show("主程序正在运行，是否重新启动？", "提示", MessageBoxButtons.YesNo);
                if (result == DialogResult.Yes)
                {
                    // 终止旧的进程
                    foreach (Process process in Process.GetProcessesByName(appname))
                    {
                        process.Kill();
                        process.WaitForExit();
                    }
                    // 启动新的进程
                    ProcessStartInfo startInfo = new ProcessStartInfo
                    {
                        FileName = appPath,
                        UseShellExecute = true
                    };
                    Process.Start(startInfo);
                }
            }
            else
            {
                ProcessStartInfo startInfo = new ProcessStartInfo
                {
                    FileName = appPath,
                    UseShellExecute = true
                };
                Process.Start(startInfo);
            }
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
                            throw;
                        }
                    }
                }
            }
            this.WindowState = FormWindowState.Normal;
        }

        private void BootUpBtn_Click(object sender, EventArgs e)
        {
            var settings = ConfigProvider.Settings.GetConfig();
            bool bootUp = settings.BootUp;
            ConfigService.SettingBootUp(!bootUp);
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
                        MessageBox.Show($"保存正则表达式时发生错误: {ex.Message}");
                        throw;
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

                    // 正则表达式验证是否为正确的 http://ip:port 格式
                    string pattern = @"^http://(?:\d{1,3}\.){3}\d{1,3}:\d+$";
                    if (Regex.IsMatch(inputText, pattern))
                    {
                        try
                        {
                            // 解析IP和端口进行进一步验证
                            Uri uriResult;
                            bool isValidUrl = Uri.TryCreate(inputText, UriKind.Absolute, out uriResult)
                                && (uriResult.Scheme == Uri.UriSchemeHttp || uriResult.Scheme == Uri.UriSchemeHttps);

                            if (isValidUrl)
                            {
                                // 确保IP地址有效
                                IPAddress ip;
                                bool isValidIp = IPAddress.TryParse(uriResult.Host, out ip);

                                if (isValidIp)
                                {
                                    // 确保端口号在有效范围内
                                    int port = uriResult.Port;
                                    if (port >= 1 && port <= 65535)
                                    {
                                        // 更新配置
                                        ConfigProvider.Settings.UpdateConfig(s => { s.ServiceIP = inputText; });
                                    }
                                    else
                                    {
                                        MessageBox.Show("端口号不在有效范围内（1-65535）");
                                    }
                                }
                                else
                                {
                                    MessageBox.Show("无效的IP地址格式");
                                }
                            }
                            else
                            {
                                MessageBox.Show("无效的URL格式");
                            }
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show($"保存服务器IP时发生错误: {ex.Message}");
                            throw;
                        }
                    }
                    else
                    {
                        MessageBox.Show("请输入正确的服务器IP格式（如：http://192.168.1.1:8080）");
                    }
                }
            }
        }

        private void DetectNetworkBtn_Click(object sender, EventArgs e)
        {

        }

        private void ReinstallBtn_Click(object sender, EventArgs e)
        {
            MessageBox.Show("该按钮功能目的是为了实现两个项目的重新安装功能");
        }

        private void SwitchVersionBtn_Click(object sender, EventArgs e)
        {
            MessageBox.Show("该按钮功能目的是为了实现Tesseract-OCR模型版本切换，\n" +
                "但由于编写时间有限，暂时不实现该功能。");
        }

        private void SwitchBrowserBtn_Click(object sender, EventArgs e)
        {
            StringBuilder selectedTexts = new StringBuilder();

            foreach (Control control in this.Controls)
            {
                GetCheckedCheckBoxes(control, selectedTexts);
            }

            ConfigProvider.Settings.UpdateConfig(s =>
            {
                s.FaultFeedBack = selectedTexts.ToString();
            });
            FaultFeedBackForm faultFeedBackForm = new FaultFeedBackForm();
            faultFeedBackForm.ShowDialog();
        }

        private void SwitchConfigBtn_Click(object sender, EventArgs e)
        {
            MessageBox.Show("该按钮功能目的是为了实现配置文件从服务器中下载，\n" +
                "但由于编写时间有限，暂时不实现该功能。");
        }

        private void GetCheckedCheckBoxes(Control parent, StringBuilder selectedTexts)
        {
            foreach (Control control in parent.Controls)
            {
                if (control is CheckBox checkBox && checkBox.Checked)
                {
                    selectedTexts.Append(checkBox.Text+" ");
                }

                if (control.HasChildren)
                {
                    GetCheckedCheckBoxes(control, selectedTexts);
                }
            }
        }
    }
}
