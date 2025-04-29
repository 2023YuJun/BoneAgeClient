using Common.Config;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Common.Services
{
    public class HttpClientService
    {
        private readonly HttpClient _client;
        private readonly string _baseUrl;

        public HttpClientService()
        {
            _baseUrl = ConfigProvider.Settings.GetConfig().ServiceIP;
            _client = new HttpClient
            {
                Timeout = TimeSpan.FromSeconds(30)
            };
        }

        public async Task<HttpResponseMessage> PostAsync<T>(string endpoint, T data)
        {
            var json = JsonSerializer.Serialize(data);
            var content = new StringContent(json, Encoding.UTF8, "application/json");
            return await _client.PostAsync($"{_baseUrl}{endpoint}", content);
        }
    }
}
