// HttpService.cs
using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;

namespace Common
{
    public class HttpService : IDisposable
    {
        private readonly HttpListener _listener;
        private readonly Dictionary<string, Func<HttpListenerRequest, Task<object>>> _routes = new();
        private CorsSettings _corsSettings = new();

        public bool IsListening => _listener?.IsListening == true;

        public HttpService(int port)
        {
            _listener = new HttpListener();
            _listener.Prefixes.Add($"http://*:{port}/");
        }

        public void ConfigureCors(Action<CorsSettings> configure)
        {
            configure(_corsSettings);
        }

        public void AddRoute(string path, Func<HttpListenerRequest, Task<object>> handler)
        {
            _routes[path] = handler;
        }

        public async Task StartAsync()
        {
            _listener.Start();
            while (IsListening)
            {
                var context = await _listener.GetContextAsync();
                _ = ProcessRequestAsync(context); // Fire and forget
            }
        }

        private async Task ProcessRequestAsync(HttpListenerContext context)
        {
            try
            {
                SetCorsHeaders(context.Response);

                if (context.Request.HttpMethod == "OPTIONS")
                {
                    HandlePreflightRequest(context.Response);
                    return;
                }

                if (_routes.TryGetValue(context.Request.Url.AbsolutePath, out var handler))
                {
                    var result = await handler(context.Request);
                    await WriteResponseAsync(context.Response, result);
                }
                else
                {
                    context.Response.StatusCode = 404;
                }
            }
            catch (Exception ex)
            {
                context.Response.StatusCode = 500;
            }
            finally
            {
                context.Response.Close();
            }
        }

        private void HandlePreflightRequest(HttpListenerResponse response)
        {
            response.StatusCode = 204;
            response.AppendHeader("Access-Control-Allow-Methods", "GET, POST, PUT, DELETE");
            response.AppendHeader("Access-Control-Allow-Headers", _corsSettings.AllowedHeaders);
        }

        private void SetCorsHeaders(HttpListenerResponse response)
        {
            response.AppendHeader("Access-Control-Allow-Origin", _corsSettings.AllowedOrigins);
            response.AppendHeader("Access-Control-Allow-Credentials", _corsSettings.AllowCredentials.ToString());
        }

        private async Task WriteResponseAsync(HttpListenerResponse response, object result)
        {
            var json = System.Text.Json.JsonSerializer.Serialize(result);
            response.ContentType = "application/json";
            await using var writer = new StreamWriter(response.OutputStream);
            await writer.WriteAsync(json);
        }

        public void Dispose()
        {
            _listener?.Stop();
            _listener?.Close();
        }
    }

    public class CorsSettings
    {
        public string AllowedOrigins { get; set; } = "*";
        public string AllowedHeaders { get; set; } = "Content-Type";
        public bool AllowCredentials { get; set; } = true;
    }
}