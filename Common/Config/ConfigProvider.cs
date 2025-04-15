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
        // 动态获取解决方案根目录（更可靠的方法）
        private static readonly string _solutionRoot = Path.GetFullPath(Path.Combine(
            System.AppDomain.CurrentDomain.BaseDirectory,
            "..", "..", "..", ".."  // 根据实际层级调整
        ));

        // 配置文件路径
        private static readonly string _configPath = Path.Combine(
            _solutionRoot, "Common", "Config", "Settings.json"
        );

        // 全局单例配置助手
        public static readonly SettingJsonHelper<AppSettings> Settings =
            new SettingJsonHelper<AppSettings>(_configPath, enableFileWatcher: true);
    }
}
