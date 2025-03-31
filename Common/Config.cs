using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;
using Microsoft.Win32;
using static System.Net.Mime.MediaTypeNames;

namespace Common
{
    public static class Config
    {
        private const string RegistryKeyPath = @"SOFTWARE\Microsoft\Windows\CurrentVersion\Run";

        public static void SettingBootUp(string appPath, bool enable)
        {
            try
            {
                string appName;
                ConfigHelper.GetSetting("AppName", out appName);
                if (string.IsNullOrWhiteSpace(appName))
                {
                    throw new InvalidOperationException("配置文件中未定义应用程序名称");
                }
                SetBootUp(appName, appPath, enable);
                IsBootUpEnabled(appName);
                ConfigHelper.SetSetting("BootUp", enable.ToString());
            }
            catch (Exception ex)
            {   
                throw new InvalidOperationException("设置开机自启动失败。", ex);
            }
        }

        /// <summary>
        /// 设置应用程序的开机自启动状态。
        /// </summary>
        /// <param name="appName">应用程序名称。</param>
        /// <param name="appPath">应用程序可执行文件的完整路径。</param>
        /// <param name="enable">true 表示设置为开机自启动，false 表示取消自启动。</param>
        public static void SetBootUp(string appName, string appPath, bool enable)
        {
            if (string.IsNullOrEmpty(appName))
                throw new ArgumentException("应用程序名称不能为空。", nameof(appName));

            if (string.IsNullOrEmpty(appPath))
                throw new ArgumentException("应用程序路径不能为空。", nameof(appPath));

            if (!File.Exists(appPath))
                throw new FileNotFoundException("指定的应用程序路径不存在。", appPath);

            try
            {
                using (RegistryKey key = Registry.CurrentUser.OpenSubKey(RegistryKeyPath, true))
                {
                    if (enable)
                    {
                        key.SetValue(appName, appPath);
                    }
                    else
                    {
                        key.DeleteValue(appName, false);
                    }
                }
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException("设置开机自启动失败。", ex);
            }
        }

        /// <summary>
        /// 检查应用程序是否设置为开机自启动。
        /// </summary>
        /// <param name="appName">应用程序名称。</param>
        /// <returns>如果已设置为开机自启动，则返回 true；否则返回 false。</returns>
        public static bool IsBootUpEnabled(string appName)
        {
            if (string.IsNullOrEmpty(appName))
                throw new ArgumentException("应用程序名称不能为空。", nameof(appName));

            using (RegistryKey key = Registry.CurrentUser.OpenSubKey(RegistryKeyPath, false))
            {
                return key?.GetValue(appName) != null;
            }
        }
    }
}
