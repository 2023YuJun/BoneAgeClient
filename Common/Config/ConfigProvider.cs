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
        // 动态获取解决方案根目录
        public static readonly string solutionRoot = Path.GetFullPath(Path.Combine(
            System.AppDomain.CurrentDomain.BaseDirectory,
            "..", "..", "..", ".."  
        ));

        // 配置文件路径
        private static readonly string _configPath = Path.Combine(
            solutionRoot, "Common", "Config", "Settings.json"
        );

        // 全局单例配置助手
        public static readonly SettingJsonHelper<AppSettings> Settings =
            new SettingJsonHelper<AppSettings>(_configPath, enableFileWatcher: true);
    }
}
