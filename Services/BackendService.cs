using System.Net.Http;
using System.Net.Http.Json;
using Serilog;

namespace ForgeMacro.Services
{
    public class BackendService : IBackendService
    {
        private HttpClient? _httpClient;
        private string _baseUrl = string.Empty;
        private bool _isEnabled = false;

        public async Task InitializeAsync(string baseUrl)
        {
            try
            {
                if (string.IsNullOrEmpty(baseUrl) || baseUrl == "disabled")
                {
                    Log.Information("Backend service disabled (standalone mode)");
                    _isEnabled = false;
                    await Task.CompletedTask;
                    return;
                }

                _baseUrl = baseUrl;
                _httpClient = new HttpClient();
                _httpClient.BaseAddress = new Uri(baseUrl);
                _httpClient.Timeout = TimeSpan.FromSeconds(30);
                _isEnabled = true;

                Log.Information("Backend service initialized with URL: {BaseUrl}", baseUrl);
                await Task.CompletedTask;
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Error initializing backend service - running in standalone mode");
                _isEnabled = false;
                await Task.CompletedTask;
            }
        }

        public async Task<MacroConfig> GetConfigAsync()
        {
            try
            {
                if (!_isEnabled || _httpClient == null)
                {
                    Log.Information("Using local config (backend disabled)");
                    return new MacroConfig();
                }

                var response = await _httpClient.GetAsync("/api/macro/config");
                response.EnsureSuccessStatusCode();

                var config = await response.Content.ReadAsAsync<MacroConfig>();
                Log.Information("Retrieved macro config from backend");
                return config;
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Error getting config from backend - using local config");
                return new MacroConfig();
            }
        }

        public async Task UploadStatsAsync(MacroStats stats)
        {
            try
            {
                if (!_isEnabled || _httpClient == null)
                {
                    Log.Information("Stats saved locally (backend disabled)");
                    return;
                }

                var response = await _httpClient.PostAsJsonAsync("/api/macro/stats", stats);
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
                if (!_isEnabled || _httpClient == null)
                {
                    Log.Information("Detection logged locally: {Count} {OreType}", count, oreType);
                    return;
                }

                var payload = new { oreType, count, timestamp = DateTime.UtcNow };
                var response = await _httpClient.PostAsJsonAsync("/api/macro/detections", payload);
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

                var response = await _httpClient.GetAsync("/api/macro/auth/check");
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

                var response = await _httpClient.GetAsync($"/api/macro/models/{modelName}");
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
