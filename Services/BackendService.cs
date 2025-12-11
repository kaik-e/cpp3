using System.Net.Http.Json;
using Serilog;

namespace ForgeMacro.Services
{
    public class BackendService : IBackendService
    {
        private HttpClient? _httpClient;
        private string _baseUrl = string.Empty;

        public async Task InitializeAsync(string baseUrl)
        {
            try
            {
                _baseUrl = baseUrl;
                _httpClient = new HttpClient();
                _httpClient.BaseAddress = new Uri(baseUrl);
                _httpClient.Timeout = TimeSpan.FromSeconds(30);

                Log.Information("Backend service initialized with URL: {BaseUrl}", baseUrl);
                await Task.CompletedTask;
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Error initializing backend service");
                throw;
            }
        }

        public async Task<MacroConfig> GetConfigAsync()
        {
            try
            {
                if (_httpClient == null)
                    throw new InvalidOperationException("Backend service not initialized");

                var response = await _httpClient.GetAsync("/api/config");
                response.EnsureSuccessStatusCode();

                var config = await response.Content.ReadAsAsync<MacroConfig>();
                Log.Information("Retrieved macro config from backend");
                return config;
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Error getting config from backend");
                // Return default config on error
                return new MacroConfig();
            }
        }

        public async Task UploadStatsAsync(MacroStats stats)
        {
            try
            {
                if (_httpClient == null)
                    throw new InvalidOperationException("Backend service not initialized");

                var response = await _httpClient.PostAsJsonAsync("/api/stats", stats);
                response.EnsureSuccessStatusCode();

                Log.Information("Uploaded stats to backend");
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Error uploading stats to backend");
            }
        }

        public async Task ReportDetectionAsync(string oreType, int count)
        {
            try
            {
                if (_httpClient == null)
                    throw new InvalidOperationException("Backend service not initialized");

                var payload = new { oreType, count, timestamp = DateTime.UtcNow };
                var response = await _httpClient.PostAsJsonAsync("/api/detections", payload);
                response.EnsureSuccessStatusCode();

                Log.Information("Reported {Count} {OreType} detections to backend", count, oreType);
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Error reporting detection to backend");
            }
        }

        public async Task<bool> IsAuthorizedAsync()
        {
            try
            {
                if (_httpClient == null)
                    throw new InvalidOperationException("Backend service not initialized");

                var response = await _httpClient.GetAsync("/api/auth/check");
                return response.IsSuccessStatusCode;
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Error checking authorization");
                return false;
            }
        }

        public async Task<string> DownloadModelAsync(string modelName)
        {
            try
            {
                if (_httpClient == null)
                    throw new InvalidOperationException("Backend service not initialized");

                var response = await _httpClient.GetAsync($"/api/models/{modelName}");
                response.EnsureSuccessStatusCode();

                var modelsDir = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "models");
                Directory.CreateDirectory(modelsDir);

                var modelPath = Path.Combine(modelsDir, modelName);
                using (var fs = File.Create(modelPath))
                {
                    await response.Content.CopyToAsync(fs);
                }

                Log.Information("Downloaded model: {ModelName}", modelName);
                return modelPath;
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Error downloading model");
                throw;
            }
        }
    }
}
