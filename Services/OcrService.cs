using System.Drawing;
using Tesseract;
using Serilog;

namespace ForgeMacro.Services
{
    public class OcrService : IOcrService
    {
        private TesseractEngine? _engine;
        private readonly string _tessDataPath;

        public OcrService()
        {
            _tessDataPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "tessdata");
        }

        public async Task InitializeAsync()
        {
            try
            {
                // Ensure tessdata directory exists
                if (!Directory.Exists(_tessDataPath))
                {
                    Log.Warning("Tessdata directory not found at {Path}", _tessDataPath);
                    Directory.CreateDirectory(_tessDataPath);
                }

                _engine = new TesseractEngine(_tessDataPath, "eng", EngineMode.Default);
                Log.Information("OCR engine initialized");
                await Task.CompletedTask;
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Error initializing OCR engine");
                throw;
            }
        }

        public string ExtractText(Bitmap image)
        {
            try
            {
                if (_engine == null)
                    throw new InvalidOperationException("OCR engine not initialized");

                using (var page = _engine.Process(image))
                {
                    return page.GetText();
                }
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Error extracting text");
                return string.Empty;
            }
        }

        public string ExtractTextFromRegion(Bitmap image, Rectangle region)
        {
            try
            {
                using (var croppedImage = new Bitmap(region.Width, region.Height))
                {
                    using (var g = Graphics.FromImage(croppedImage))
                    {
                        g.DrawImage(image, 0, 0, region, GraphicsUnit.Pixel);
                    }
                    return ExtractText(croppedImage);
                }
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Error extracting text from region");
                return string.Empty;
            }
        }

        public Dictionary<string, double> DetectOreTypes(Bitmap image)
        {
            try
            {
                var text = ExtractText(image);
                var oreTypes = new Dictionary<string, double>();

                // Common ore names to detect
                var oreNames = new[] { "iron", "gold", "diamond", "emerald", "coal", "copper", "tin", "silver" };

                foreach (var ore in oreNames)
                {
                    if (text.Contains(ore, StringComparison.OrdinalIgnoreCase))
                    {
                        oreTypes[ore] = 1.0;
                    }
                }

                return oreTypes;
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Error detecting ore types");
                return new Dictionary<string, double>();
            }
        }

        public void Dispose()
        {
            _engine?.Dispose();
            Log.Information("OCR service disposed");
        }
    }
}
