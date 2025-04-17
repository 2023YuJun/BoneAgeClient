using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.NetworkInformation;
using System.Runtime.InteropServices;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace ConfigWindowApp
{
    public class BrowserService
    {
        // 检查网络连接
        public static bool IsNetworkAvailable()
        {
            return NetworkInterface.GetIsNetworkAvailable();
        }
        public static bool ValidateBrowserPath(string path)
        {
            try
            {
                return File.Exists(path) &&
                       Path.GetExtension(path).Equals(".exe", StringComparison.OrdinalIgnoreCase);
            }
            catch
            {
                return false;
            }
        }

        public static string GetDefaultBrowserPath()
        {
            try
            {
                // 方法一：通过注册表精确解析
                using (var key = Registry.ClassesRoot.OpenSubKey(@"http\shell\open\command"))
                {
                    if (key != null)
                    {
                        // 示例值："C:\Program Files (x86)\Google\Chrome\Application\chrome.exe" -- "%1"
                        var rawValue = key.GetValue("").ToString();

                        // 增强版正则表达式（处理带参数和引号的情况）
                        var match = Regex.Match(rawValue,
                            @"(?i)^\s*""?(?<path>[^""]+?\.exe)""?",
                            RegexOptions.ExplicitCapture);

                        if (match.Success)
                        {
                            var path = match.Groups["path"].Value;
                            return Path.GetFullPath(path); // 标准化路径
                        }
                    }
                }

                // 方法二：通过系统关联查询（更可靠的方式）
                return GetDefaultBrowserByAssociation();
            }
            catch
            {
                // 方法三：最后尝试的保底方式
                return GetDefaultBrowserFallback();
            }
        }

        // 使用Windows API获取关联程序
        [DllImport("shlwapi.dll", CharSet = CharSet.Unicode, SetLastError = true)]
        private static extern int AssocQueryString(
            AssocF flags,
            AssocStr str,
            string pszAssoc,
            string pszExtra,
            [Out] StringBuilder pszOut,
            [In][Out] ref uint pcchOut);

        private static string GetDefaultBrowserByAssociation()
        {
            const int S_OK = 0;
            const int S_FALSE = 1;

            uint length = 0;
            int ret = AssocQueryString(
                AssocF.None,
                AssocStr.Executable,
                "http",
                null,
                null,
                ref length);

            if (ret != S_FALSE)
                return null;

            var sb = new StringBuilder((int)length);
            ret = AssocQueryString(
                AssocF.None,
                AssocStr.Executable,
                "http",
                null,
                sb,
                ref length);

            return ret == S_OK ? sb.ToString() : null;
        }

        // 保底方法：检查常见浏览器默认路径
        private static string GetDefaultBrowserFallback()
        {
            var candidates = new[]
            {
                @"C:\Program Files\Google\Chrome\Application\chrome.exe",
                @"C:\Program Files (x86)\Google\Chrome\Application\chrome.exe",
                @"C:\Program Files\Mozilla Firefox\firefox.exe",
                @"C:\Program Files (x86)\Mozilla Firefox\firefox.exe",
                @"C:\Program Files\Microsoft\Edge\Application\msedge.exe",
                @"C:\Program Files (x86)\Microsoft\Edge\Application\msedge.exe",
                Environment.ExpandEnvironmentVariables(@"%ProgramFiles%\Internet Explorer\iexplore.exe")
            };

            return candidates.FirstOrDefault(File.Exists);
        }

        // API调用所需的枚举
        private enum AssocF
        {
            None = 0
        }

        private enum AssocStr
        {
            Executable = 2
        }

        // 下载文件
        public static async Task DownloadFile(string downloadUrl, string savePath)
        {
            try
            {
                using (HttpClient client = new HttpClient())
                {
                    byte[] fileBytes = await client.GetByteArrayAsync(downloadUrl);
                    File.WriteAllBytes(savePath, fileBytes);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"下载失败：{ex.Message}", "下载错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                throw;
            }
        }

        // 运行安装程序
        public static void RunInstaller(string installerPath)
        {
            try
            {
                ProcessStartInfo startInfo = new ProcessStartInfo(installerPath)
                {
                    UseShellExecute = true,
                    Verb = "runas" // 以管理员权限运行
                };

                Process installProcess = new Process { StartInfo = startInfo };
                installProcess.Start();

                // 等待安装程序退出
                installProcess.WaitForExit();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"运行安装程序失败：{ex.Message}", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                throw;
            }
        }

        // 检查是否已安装Google浏览器
        public static string CheckIfChromeInstalled()
        {
            string chromePath = null;
            string[] chromePaths = {
                @"SOFTWARE\Microsoft\Windows\CurrentVersion\Uninstall\Google Chrome",
                @"SOFTWARE\WOW6432Node\Microsoft\Windows\CurrentVersion\Uninstall\Google Chrome"
            };

            foreach (string path in chromePaths)
            {
                using (RegistryKey key = Registry.LocalMachine.OpenSubKey(path))
                {
                    if (key != null)
                    {
                        chromePath = key.GetValue("InstallLocation")?.ToString();
                        break;
                    }
                }
            }

            return chromePath;
        }

        // 卸载Google浏览器
        public static void UninstallChrome(string installPath)
        {
            try
            {
                string uninstallPath = Path.Combine(installPath, "Installer", "setup.exe");

                if (File.Exists(uninstallPath))
                {
                    ProcessStartInfo startInfo = new ProcessStartInfo(uninstallPath)
                    {
                        UseShellExecute = true,
                        Verb = "runas", // 以管理员权限运行
                        Arguments = "--uninstall --force --multi-install --system-level"
                    };

                    Process uninstallProcess = new Process { StartInfo = startInfo };
                    uninstallProcess.Start();

                    // 等待卸载程序退出
                    uninstallProcess.WaitForExit();

                    MessageBox.Show("Google浏览器已成功卸载。", "卸载成功", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    MessageBox.Show("无法找到卸载程序。", "卸载错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"卸载失败：{ex.Message}", "卸载错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                throw;
            }
        }

    
    }
    
}
