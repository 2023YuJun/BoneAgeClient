using Common.Config;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Common.Services
{
    public class HttpClientService : IDisposable
    {
        private readonly HttpClient _client;
        private readonly string _baseUrl;

        public HttpClientService()
        {
            _baseUrl = ConfigProvider.Settings.GetConfig().ServiceIP + "/winform/";
            _client = new HttpClient
            {
                Timeout = TimeSpan.FromSeconds(30)
            };
            // 可以添加默认请求头等其他初始化逻辑
        }

        // POST 请求
        public async Task<HttpResponseMessage> PostAsync(string endpoint, object data)
        {
            try
            {
                var json = JsonSerializer.Serialize(data);
                var content = new StringContent(json, Encoding.UTF8, "application/json");
                return await _client.PostAsync($"{_baseUrl}{endpoint}", content);
            }
            catch (Exception ex)
            {
                // 记录异常信息（可以使用日志框架）
                Console.WriteLine($"POST请求出错：{ex.Message}");
                throw; // 重新抛出异常，供调用者处理
            }
        }

        // GET 请求
        public async Task<HttpResponseMessage> GetAsync(string endpoint)
        {
            try
            {
                return await _client.GetAsync($"{_baseUrl}{endpoint}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"GET请求出错：{ex.Message}");
                throw;
            }
        }
        // GET请求（带查询参数）
        public async Task<HttpResponseMessage> GetAsync(string endpoint, Dictionary<string, string> queryParams = null)
        {
            try
            {
                // 构建带查询参数的URL
                var url = $"{_baseUrl}{endpoint}";
                if (queryParams != null && queryParams.Count > 0)
                {
                    var queryString = string.Join("&", queryParams.Select(kvp => $"{Uri.EscapeDataString(kvp.Key)}={Uri.EscapeDataString(kvp.Value)}"));
                    url += $"?{queryString}";
                }
                return await _client.GetAsync(url);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"GET请求出错：{ex.Message}");
                throw;
            }
        }

        // PUT 请求
        public async Task<HttpResponseMessage> PutAsync(string endpoint, object data)
        {
            try
            {
                var json = JsonSerializer.Serialize(data);
                var content = new StringContent(json, Encoding.UTF8, "application/json");
                return await _client.PutAsync($"{_baseUrl}{endpoint}", content);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"PUT请求出错：{ex.Message}");
                throw;
            }
        }

        // DELETE 请求
        public async Task<HttpResponseMessage> DeleteAsync(string endpoint)
        {
            try
            {
                return await _client.DeleteAsync($"{_baseUrl}{endpoint}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"DELETE请求出错：{ex.Message}");
                throw;
            }
        }

        // 实现IDisposable接口，确保资源被正确释放
        public void Dispose()
        {
            _client?.Dispose();
        }
    }
}
