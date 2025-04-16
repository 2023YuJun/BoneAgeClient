using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Common.Helpers
{
    public class LogHelper
    {
        private static readonly string _solutionRoot = Path.GetFullPath(Path.Combine(
            System.AppDomain.CurrentDomain.BaseDirectory,
            "..", "..", "..", ".."
        ));
        private static readonly string _logConfigPath = Path.Combine(_solutionRoot, "Common", "Config", "LogConfig.json");
        
        private static readonly string _logBaseDir = Path.Combine(_solutionRoot, "logs");
        private static readonly string _projectName = Assembly.GetEntryAssembly()?.GetName().Name ?? "Unknown";
        private static LogLevel _minLogLevel = LogLevel.Debug;
        private static long _maxLogSize = 10 * 1024 * 1024; // 10MB
        private static int _retainDays = 7;

        // 日志队列（线程安全）
        private static readonly BlockingCollection<LogMessage> _logQueue = new BlockingCollection<LogMessage>();
        private static readonly CancellationTokenSource _cts = new CancellationTokenSource();

        // 日志级别枚举
        public enum LogLevel { Debug, Info, Warn, Error }

        private static void LoadConfig()
        {
            try
            {
                if (File.Exists(_logConfigPath))
                {
                    var json = File.ReadAllText(_logConfigPath);
                    var config = JsonSerializer.Deserialize<LogConfig>(json);

                    _minLogLevel = config.Logging.MinLogLevel switch
                    {
                        "Debug" => LogLevel.Debug,
                        "Info" => LogLevel.Info,
                        "Warn" => LogLevel.Warn,
                        "Error" => LogLevel.Error,
                        _ => LogLevel.Debug
                    };

                    _maxLogSize = config.Logging.MaxLogSizeMB * 1024 * 1024;
                    _retainDays = config.Logging.RetainDays;
                }
            }
            catch (Exception ex)
            {
                // 记录配置加载错误（使用默认值）
                _logQueue.Add(new LogMessage
                {
                    Timestamp = DateTime.Now,
                    Level = LogLevel.Error,
                    Message = "日志配置加载失败",
                    Exception = ex
                });
            }
        }

        // 初始化时加载配置
        static LogHelper()
        {
            LoadConfig();

            // 启动后台日志写入线程
            Task.Run(() => ProcessLogQueue(_cts.Token));

            // 初始化日志清理任务
            Task.Run(CleanOldLogs);
        }
        // 日志记录方法（异步）
        public static void Log(LogLevel level, string message, Exception ex = null)
        {
            if (level < _minLogLevel) return; // 自动过滤低级别日志

            _logQueue.Add(new LogMessage
            {
                Timestamp = DateTime.Now,
                Level = level,
                Project = _projectName,
                Message = message,
                Exception = ex
            });
        }

        // 后台处理日志队列
        private static async Task ProcessLogQueue(CancellationToken token)
        {
            string currentLogPath = null;
            StreamWriter writer = null;

            try
            {
                foreach (var log in _logQueue.GetConsumingEnumerable(token))
                {
                    // 检查是否需要滚动日志
                    if (NeedRolling(currentLogPath))
                    {
                        writer?.Dispose();
                        currentLogPath = GetNewLogPath(log.Timestamp);
                        writer = new StreamWriter(currentLogPath, true);
                    }

                    // 写入日志内容
                    var logEntry = $"[{log.Timestamp:yyyy-MM-dd HH:mm:ss.fffff}] {log.Level.ToString().ToUpper()}: {log.Message}";
                    if (log.Exception != null)
                        logEntry += $"\nException: {log.Exception}\n";

                    await writer.WriteLineAsync(logEntry);
                    await writer.FlushAsync();
                }
            }
            finally
            {
                writer?.Dispose();
            }
        }

        // 检查是否需要滚动日志
        private static bool NeedRolling(string currentPath)
        {
            if (currentPath == null) return true;
            var fileInfo = new FileInfo(currentPath);
            return fileInfo.Length > _maxLogSize ||
                   fileInfo.CreationTime.Date < DateTime.Today;
        }

        // 生成新的日志路径
        private static string GetNewLogPath(DateTime timestamp)
        {
            var dateDir = timestamp.ToString("yyyyMMdd");
            var fullDir = Path.Combine(_logBaseDir, _projectName, dateDir);
            Directory.CreateDirectory(fullDir);
            return Path.Combine(fullDir, $"app_{timestamp:HHmmssfffff}.log");
        }

        // 清理旧日志
        private static async Task CleanOldLogs()
        {
            while (!_cts.IsCancellationRequested)
            {
                var cutoff = DateTime.Now.AddDays(-_retainDays);
                foreach (var dir in Directory.GetDirectories(_logBaseDir))
                {
                    if (DateTime.ParseExact(Path.GetFileName(dir), "yyyyMMdd", null) < cutoff.Date)
                    {
                        Directory.Delete(dir, true);
                    }
                }
                await Task.Delay(TimeSpan.FromHours(6)); // 每6小时检查一次
            }
        }

        // 日志消息结构体
        private class LogMessage
        {
            public DateTime Timestamp { get; set; }
            public LogLevel Level { get; set; }
            public string Project { get; set; }
            public string Message { get; set; }
            public Exception Exception { get; set; }
        }

        // 配置类
        private class LogConfig
        {
            public LoggingConfig Logging { get; set; }

            public class LoggingConfig
            {
                public int MaxLogSizeMB { get; set; }
                public int RetainDays { get; set; }
                public string MinLogLevel { get; set; }
            }
        }
    }
}
