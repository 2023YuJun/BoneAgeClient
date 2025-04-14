using Common.Helpers;
using Common.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Config
{
    public static class ConfigProvider
    {
        private static readonly string solutionPath = Directory.GetParent(Directory.GetCurrentDirectory()).Parent.Parent.Parent.FullName;
        private static readonly string ConfigPath = Path.Combine(solutionPath,"Common", "Config", "Settings.json");

        // 全局单例配置助手
        public static readonly SettingJsonHelper<AppSettings> Settings =
            new SettingJsonHelper<AppSettings>(ConfigPath, enableFileWatcher: true);
    }
}
