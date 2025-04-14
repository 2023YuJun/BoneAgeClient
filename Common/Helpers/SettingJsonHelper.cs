using System.Diagnostics;
using Newtonsoft.Json;

namespace Common.Helpers
{
    public class SettingJsonHelper<T> where T : new()
    {
        private static readonly ReaderWriterLockSlim _lock = new ReaderWriterLockSlim();
        private readonly string _configFilePath;
        private T _configData;
        private FileSystemWatcher _fileWatcher;
        private bool _disposed = false;

        /// <summary>
        /// 初始化配置文件助手
        /// </summary>
        /// <param name="configFilePath">配置文件的物理路径</param>
        /// <param name="enableFileWatcher">是否启用文件监视</param>
        public SettingJsonHelper(string configFilePath, bool enableFileWatcher = true)
        {
            _configFilePath = configFilePath;
            EnsureConfigFileExists();
            LoadConfig();

            if (enableFileWatcher)
            {
                SetupFileWatcher();
            }
        }

        /// <summary>
        /// 获取当前配置（线程安全）
        /// </summary>
        public T GetConfig()
        {
            _lock.EnterReadLock();
            try
            {
                return _configData;
            }
            finally
            {
                _lock.ExitReadLock();
            }
        }

        /// <summary>
        /// 更新配置并保存
        /// </summary>
        public void UpdateConfig(Action<T> updateAction, bool autoSave = true)
        {
            _lock.EnterWriteLock();
            try
            {
                updateAction(_configData);
                if (autoSave)
                {
                    SaveConfig();
                }
            }
            finally
            {
                _lock.ExitWriteLock();
            }
        }

        /// <summary>
        /// 确保配置文件存在
        /// </summary>
        private void EnsureConfigFileExists()
        {
            if (File.Exists(_configFilePath))
            {
                return;
            }

            _lock.EnterWriteLock();
            try
            {
                // 再次检查，避免多线程竞争
                if (!File.Exists(_configFilePath))
                {
                    Directory.CreateDirectory(Path.GetDirectoryName(_configFilePath));
                    _configData = new T();

                    // 直接保存，不调用 SaveConfig()
                    string json = JsonConvert.SerializeObject(_configData, Formatting.Indented);
                    File.WriteAllText(_configFilePath, json);
                }
            }
            finally
            {
                _lock.ExitWriteLock();
            }
        }

        /// <summary>
        /// 加载配置文件
        /// </summary>
        private void LoadConfig()
        {
            _lock.EnterWriteLock();
            try
            {
                string json = File.ReadAllText(_configFilePath);
                _configData = JsonConvert.DeserializeObject<T>(json) ?? new T();
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"加载配置文件失败: {ex.Message}");
                _configData = new T();
            }
            finally
            {
                _lock.ExitWriteLock();
            }
        }

        /// <summary>
        /// 保存配置文件
        /// </summary>
        public void SaveConfig()
        {
            string json = JsonConvert.SerializeObject(_configData, Formatting.Indented);
            File.WriteAllText(_configFilePath, json);
        }

        /// <summary>
        /// 设置文件监视（自动重新加载）
        /// </summary>
        private void SetupFileWatcher()
        {
            _fileWatcher = new FileSystemWatcher
            {
                Path = Path.GetDirectoryName(_configFilePath),
                Filter = Path.GetFileName(_configFilePath),
                NotifyFilter = NotifyFilters.LastWrite
            };

            // 防抖处理：避免多次触发
            var reloadDebouncer = new Timer(_ => LoadConfig(), null, Timeout.Infinite, Timeout.Infinite);
            _fileWatcher.Changed += (sender, e) =>
            {
                reloadDebouncer.Change(500, Timeout.Infinite); // 500ms 内多次修改只触发一次
            };

            _fileWatcher.EnableRaisingEvents = true;
        }

        /// <summary>
        /// 释放资源
        /// </summary>
        public void Dispose()
        {
            if (!_disposed)
            {
                _fileWatcher?.Dispose();
                _lock?.Dispose();
                _disposed = true;
            }
        }
    }
}
