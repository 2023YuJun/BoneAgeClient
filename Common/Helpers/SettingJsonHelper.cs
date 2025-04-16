using System.Diagnostics;
using Common.Models;
using Newtonsoft.Json;

namespace Common.Helpers
{
    public class SettingJsonHelper<T> where T : IConfig, new()
    {
        private static readonly ReaderWriterLockSlim _lock = new ReaderWriterLockSlim();
        private readonly string _configFilePath;
        private T _configData;
        private FileSystemWatcher _fileWatcher;
        private bool _disposed = false;
        private bool isSelfChange = false;

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
            return _configData;
        }

        /// <summary>
        /// 更新配置并保存
        /// </summary>
        public void UpdateConfig(Action<T> updateAction)
        {
            _lock.EnterWriteLock();
            try
            {
                updateAction(_configData);
                SaveConfig();
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
            try
            {
                string json = File.ReadAllText(_configFilePath);
                T newConfig = JsonConvert.DeserializeObject<T>(json) ?? new T();

                if (_lock.TryEnterWriteLock(100))
                {
                    try
                    {
                        _configData = newConfig;
                        CheckAndTriggerEvents(newConfig);
                    }
                    finally
                    {
                        _lock.ExitWriteLock();
                    }
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"加载配置文件失败: {ex.Message}");
                throw;
            }
        }

        /// <summary>
        /// 保存配置文件
        /// </summary>
        public void SaveConfig()
        {
            try
            {
                string json = JsonConvert.SerializeObject(_configData, Formatting.Indented);
                isSelfChange = true;
                File.WriteAllText(_configFilePath, json);
                Task.Delay(100).ContinueWith(_ => isSelfChange = false);
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"保存配置失败: {ex.Message}");
                throw;
            }
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

            var reloadDebouncer = new Timer(_ => LoadConfig(), null, Timeout.Infinite, Timeout.Infinite);
            isSelfChange = false;

            _fileWatcher.Changed += (sender, e) =>
            {
                if (isSelfChange)
                {
                    isSelfChange = false;
                    return;
                }
                reloadDebouncer.Change(500, Timeout.Infinite);
            };

            _fileWatcher.EnableRaisingEvents = true;
        }

        // 定义独立事件
        public event Action ConfigChanged;

        // 记录上一次的字段值
        private int _lastFormLocationX;
        private int _lastFormLocationY;
        private bool BootUp;

        private void CheckAndTriggerEvents(T newConfig)
        {
            bool isChanged = false;

            // 检查所有需要监听的字段
            if (_lastFormLocationX != newConfig.FormLocationX)
            {
                _lastFormLocationX = newConfig.FormLocationX;
                isChanged = true;
            }

            if (_lastFormLocationY != newConfig.FormLocationY)
            {
                _lastFormLocationY = newConfig.FormLocationY;
                isChanged = true;
            }

            if (BootUp != newConfig.BootUp)
            {
                BootUp = newConfig.BootUp;
                isChanged = true;
            }

            // 若任意字段变化，触发事件
            if (isChanged)
            {
                ConfigChanged?.Invoke();
            }
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
