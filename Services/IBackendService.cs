namespace ForgeMacro.Services
{
    public class MacroConfig
    {
        public string ModelPath { get; set; } = string.Empty;
        public float OreConfidenceThreshold { get; set; } = 0.6f;
        public float RockConfidenceThreshold { get; set; } = 0.6f;
        public int ScreenCaptureInterval { get; set; } = 100; // ms
        public bool EnableOcr { get; set; } = true;
        public bool EnableAutoMine { get; set; } = true;
    }

    public class MacroStats
    {
        public int OresMined { get; set; }
        public int RocksDetected { get; set; }
        public TimeSpan RunningTime { get; set; }
        public double AverageDetectionTime { get; set; }
    }

    public interface IBackendService
    {
        /// <summary>
        /// Initializes connection to backend
        /// </summary>
        Task InitializeAsync(string baseUrl);

        /// <summary>
        /// Gets macro configuration from backend
        /// </summary>
        Task<MacroConfig> GetConfigAsync();

        /// <summary>
        /// Uploads macro statistics to backend
        /// </summary>
        Task UploadStatsAsync(MacroStats stats);

        /// <summary>
        /// Reports detected ores to backend
        /// </summary>
        Task ReportDetectionAsync(string oreType, int count);

        /// <summary>
        /// Checks if macro is authorized
        /// </summary>
        Task<bool> IsAuthorizedAsync();

        /// <summary>
        /// Gets latest model from backend
        /// </summary>
        Task<string> DownloadModelAsync(string modelName);
    }
}
