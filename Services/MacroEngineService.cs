using System.Diagnostics;
using Serilog;

namespace ForgeMacro.Services
{
    public class MacroEngineService : IMacroEngineService
    {
        private readonly IScreenCaptureService _screenCapture;
        private readonly IOcrService _ocr;
        private readonly IObjectDetectionService _detection;
        private readonly IInputSimulationService _input;
        private readonly IBackendService _backend;

        private bool _isRunning;
        private bool _isPaused;
        private CancellationTokenSource? _cancellationTokenSource;
        private Task? _engineTask;

        private int _oresMined;
        private int _rocksDetected;
        private Stopwatch _stopwatch = new();
        private long _totalDetectionTime;
        private int _detectionCount;

        public bool IsRunning => _isRunning;

        public event EventHandler<string>? OreDetected;
        public event EventHandler<string>? RockDetected;
        public event EventHandler<string>? ErrorOccurred;

        public MacroEngineService(
            IScreenCaptureService screenCapture,
            IOcrService ocr,
            IObjectDetectionService detection,
            IInputSimulationService input,
            IBackendService backend)
        {
            _screenCapture = screenCapture;
            _ocr = ocr;
            _detection = detection;
            _input = input;
            _backend = backend;
        }

        public void Start()
        {
            if (_isRunning)
            {
                Log.Warning("Macro already running");
                return;
            }

            try
            {
                _isRunning = true;
                _isPaused = false;
                _cancellationTokenSource = new CancellationTokenSource();
                _stopwatch.Restart();
                _oresMined = 0;
                _rocksDetected = 0;
                _totalDetectionTime = 0;
                _detectionCount = 0;

                _engineTask = RunMacroAsync(_cancellationTokenSource.Token);
                Log.Information("Macro engine started");
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Error starting macro engine");
                _isRunning = false;
                ErrorOccurred?.Invoke(this, ex.Message);
            }
        }

        public void Stop()
        {
            if (!_isRunning)
            {
                Log.Warning("Macro not running");
                return;
            }

            try
            {
                _isRunning = false;
                _cancellationTokenSource?.Cancel();
                _engineTask?.Wait(TimeSpan.FromSeconds(5));
                _stopwatch.Stop();

                Log.Information("Macro engine stopped");
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Error stopping macro engine");
                ErrorOccurred?.Invoke(this, ex.Message);
            }
        }

        public void Pause()
        {
            _isPaused = true;
            Log.Information("Macro paused");
        }

        public void Resume()
        {
            _isPaused = false;
            Log.Information("Macro resumed");
        }

        public string GetStatus()
        {
            if (!_isRunning)
                return "Idle";
            if (_isPaused)
                return "Paused";
            return "Running";
        }

        public MacroStats GetStats()
        {
            return new MacroStats
            {
                OresMined = _oresMined,
                RocksDetected = _rocksDetected,
                RunningTime = _stopwatch.Elapsed,
                AverageDetectionTime = _detectionCount > 0 ? _totalDetectionTime / (double)_detectionCount : 0
            };
        }

        private async Task RunMacroAsync(CancellationToken cancellationToken)
        {
            try
            {
                while (!cancellationToken.IsCancellationRequested && _isRunning)
                {
                    if (_isPaused)
                    {
                        await Task.Delay(100, cancellationToken);
                        continue;
                    }

                    var sw = Stopwatch.StartNew();

                    try
                    {
                        // Capture screen
                        var screenshot = _screenCapture.CaptureScreen();

                        // Detect ores
                        var ores = _detection.DetectOres(screenshot);
                        if (ores.Count > 0)
                        {
                            foreach (var ore in ores)
                            {
                                _oresMined++;
                                OreDetected?.Invoke(this, ore.Label);
                                Log.Information("Ore detected: {OreLabel} (confidence: {Confidence})", ore.Label, ore.Confidence);

                                // Move to ore and click
                                var centerX = ore.BoundingBox.X + ore.BoundingBox.Width / 2;
                                var centerY = ore.BoundingBox.Y + ore.BoundingBox.Height / 2;
                                _input.MoveMouse(centerX, centerY);
                                _input.Delay(50);
                                _input.Click();
                                _input.Delay(100);
                            }
                        }

                        // Detect rocks
                        var rocks = _detection.DetectRocks(screenshot);
                        if (rocks.Count > 0)
                        {
                            _rocksDetected += rocks.Count;
                            foreach (var rock in rocks)
                            {
                                RockDetected?.Invoke(this, rock.Label);
                                Log.Information("Rock detected: {RockLabel}", rock.Label);
                            }
                        }

                        screenshot.Dispose();
                    }
                    catch (Exception ex)
                    {
                        Log.Error(ex, "Error in macro loop iteration");
                        ErrorOccurred?.Invoke(this, ex.Message);
                    }

                    sw.Stop();
                    _totalDetectionTime += sw.ElapsedMilliseconds;
                    _detectionCount++;

                    // Wait before next iteration (configurable)
                    await Task.Delay(100, cancellationToken);
                }
            }
            catch (OperationCanceledException)
            {
                Log.Information("Macro engine cancelled");
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Fatal error in macro engine");
                ErrorOccurred?.Invoke(this, ex.Message);
            }
            finally
            {
                _isRunning = false;
            }
        }
    }
}
