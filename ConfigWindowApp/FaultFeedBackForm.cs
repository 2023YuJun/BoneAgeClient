using Common.Config;
using Common.Helpers;
using Common.Services;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Net.Mime.MediaTypeNames;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace ConfigWindowApp
{
    public partial class FaultFeedBackForm : Form
    {
        private readonly HttpClientService _client;
        private string BackendUrl = ConfigProvider.Settings.GetConfig().ServiceIP;

        public FaultFeedBackForm()
        {
            InitializeComponent();
            _client = new HttpClientService();
        }

        private void UserBrowserBtn_Click(object sender, EventArgs e)
        {
            string defaultBrowserPath = BrowserService.GetDefaultBrowserPath();
            if (BrowserService.ValidateBrowserPath(defaultBrowserPath))
            {
                ConfigProvider.Settings.UpdateConfig(s =>
                {
                    s.DefaultBrowserPath = defaultBrowserPath;
                    s.CurrentBrowser = "default";
                });
                MessageBox.Show("默认浏览器路径验证成功！\n已使用默认浏览器" );
            }
            else
            {
                MessageBox.Show("默认浏览器路径验证失败！\n" +
                    "请检查默认浏览器设置。");
            }
        }
        private async void UpdateBrowserBtn_Click(object sender, EventArgs e)
        {
            // 检查网络连接
            if (!BrowserService.IsNetworkAvailable())
            {
                MessageBox.Show("当前无法连接到网络，请检查您的网络设置。", "网络错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            var settings = ConfigProvider.Settings.GetConfig();
            string downloadUrl = settings.OurBrowserUrl;

            // 创建文件保存对话框
            SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.Filter = "可执行文件|*.exe";
            saveFileDialog.FileName = "chrome_installer.exe";

            if (saveFileDialog.ShowDialog() == DialogResult.OK)
            {
                string savePath = saveFileDialog.FileName;

                // 下载文件
                await BrowserService.DownloadFile(downloadUrl, savePath);

                // 检查是否已安装Google浏览器
                string installedPath = BrowserService.CheckIfChromeInstalled();
                if (!string.IsNullOrEmpty(installedPath))
                {
                    // 如果已安装，先卸载
                    BrowserService.UninstallChrome(installedPath);
                }

                // 运行安装程序
                BrowserService.RunInstaller(savePath);

                // 弹出文件对话框，让用户选择安装后的浏览器路径
                FolderBrowserDialog folderBrowserDialog = new FolderBrowserDialog();
                folderBrowserDialog.Description = "选择Google浏览器的安装路径";
                folderBrowserDialog.RootFolder = Environment.SpecialFolder.ProgramFiles;

                if (folderBrowserDialog.ShowDialog() == DialogResult.OK)
                {
                    string newInstalledPath = Path.Combine(folderBrowserDialog.SelectedPath, "chrome.exe");

                    // 记录安装路径
                    ConfigProvider.Settings.UpdateConfig(s =>
                    {
                        s.OurBrowserPath = newInstalledPath;
                        s.CurrentBrowser = "Our";
                    });

                    MessageBox.Show($"Google浏览器已成功安装！安装路径已记录。", "安装成功", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
        }

        private void OurBrowserBtn_Click(object sender, EventArgs e)
        {
            ConfigProvider.Settings.UpdateConfig(s => {
                s.CurrentBrowser = "Our";
            });
        }

        private void ReinstallBrowserBtn_Click(object sender, EventArgs e)
        {
            // 获取项目目录中的安装程序路径
            string installerPath = Path.Combine(ConfigProvider.solutionRoot, System.Windows.Forms.Application.ProductName, "Google Chrome", "ChromeSetup.exe");
            if (!File.Exists(installerPath))
            {
                MessageBox.Show("安装程序未找到，请确保文件存在于项目目录中。", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            // 检查是否已安装Google浏览器
            string installedPath = BrowserService.CheckIfChromeInstalled();
            if (!string.IsNullOrEmpty(installedPath))
            {
                // 如果已安装，先卸载
                BrowserService.UninstallChrome(installedPath);
            }

            // 运行安装程序
            BrowserService.RunInstaller(installerPath);

            // 弹出文件对话框，让用户选择安装后的浏览器路径
            FolderBrowserDialog folderBrowserDialog = new FolderBrowserDialog();
            folderBrowserDialog.Description = "选择Google浏览器的安装路径";
            folderBrowserDialog.RootFolder = Environment.SpecialFolder.ProgramFiles;

            if (folderBrowserDialog.ShowDialog() == DialogResult.OK)
            {
                string newInstalledPath = Path.Combine(folderBrowserDialog.SelectedPath, "chrome.exe");

                ConfigProvider.Settings.UpdateConfig(s =>
                {
                    s.OurBrowserPath = newInstalledPath;
                    s.CurrentBrowser = "Our";
                });

                MessageBox.Show($"Google浏览器已成功安装！安装路径已记录。", "安装成功", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void SwitchVersionBtn_Click(object sender, EventArgs e)
        {
            MessageBox.Show("该按钮功能目的是为了实现Tesseract-OCR模型版本切换，\n" +
                "但由于编写时间有限，暂时不实现该功能。");
        }

        private async void SeedFaultBtn_Click(object sender, EventArgs e)
        {
            string faultMessage = ConfigProvider.Settings.GetConfig().FaultFeedBack;
            string deviceIP = Utils.GetNonLocalhostIPsAsString();
            if (string.IsNullOrEmpty(faultMessage))
            {
                MessageBox.Show("当前没有故障信息可供反馈。");
                return;
            }
            else
            {
                LogHelper.Log(LogHelper.LogLevel.Error, faultMessage);
                var faultInfo = new
                {
                    content = faultMessage,  
                    deviceIP = deviceIP  
                };
                var response = await _client.PostAsync("/winform/faultinfo", faultInfo);
                MessageBox.Show($"故障信息已反馈。响应状态：{response}");

            }
        }
    }
}
