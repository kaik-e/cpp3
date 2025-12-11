using System.Drawing;
using Microsoft.ML.OnnxRuntime;
using Microsoft.ML.OnnxRuntime.Tensors;
using OpenCvSharp;
using Serilog;

namespace ForgeMacro.Services
{
    public class ObjectDetectionService : IObjectDetectionService
    {
        private InferenceSession? _session;
        private string _modelPath = string.Empty;
        private int _inputWidth = 640;
        private int _inputHeight = 640;

        public async Task LoadModelAsync(string modelPath)
        {
            try
            {
                if (!File.Exists(modelPath))
                    throw new FileNotFoundException($"Model file not found: {modelPath}");

                _modelPath = modelPath;

                // Load ONNX model
                var sessionOptions = new SessionOptions();
                sessionOptions.GraphOptimizationLevel = GraphOptimizationLevel.ORT_ENABLE_ALL;
                
                _session = new InferenceSession(modelPath, sessionOptions);
                
                Log.Information("Object detection model loaded: {ModelPath}", modelPath);
                await Task.CompletedTask;
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Error loading object detection model");
                throw;
            }
        }

        public List<DetectionResult> Detect(Bitmap image, float confidenceThreshold = 0.5f)
        {
            try
            {
                if (_session == null)
                    throw new InvalidOperationException("Model not loaded");

                var results = new List<DetectionResult>();

                // Convert bitmap to Mat
                using (var mat = OpenCvSharp.Extensions.BitmapConverter.ToMat(image))
                {
                    // Preprocess image
                    var inputTensor = PreprocessImage(mat);

                    // Run inference
                    var inputs = new List<NamedOnnxValue>
                    {
                        NamedOnnxValue.CreateFromTensor("images", inputTensor)
                    };

                    using (var outputs = _session.Run(inputs))
                    {
                        // Parse outputs
                        results = ParseDetectionOutput(outputs, confidenceThreshold);
                    }
                }

                return results;
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Error during object detection");
                return new List<DetectionResult>();
            }
        }

        public List<DetectionResult> DetectOres(Bitmap image, float confidenceThreshold = 0.6f)
        {
            var allDetections = Detect(image, confidenceThreshold);
            
            // Filter for ore-related classes
            var oreClasses = new[] { "iron_ore", "gold_ore", "diamond_ore", "emerald_ore", "coal_ore", "copper_ore" };
            
            return allDetections
                .Where(d => oreClasses.Any(c => d.Label.Contains(c, StringComparison.OrdinalIgnoreCase)))
                .ToList();
        }

        public List<DetectionResult> DetectRocks(Bitmap image, float confidenceThreshold = 0.6f)
        {
            var allDetections = Detect(image, confidenceThreshold);
            
            // Filter for rock/stone-related classes
            var rockClasses = new[] { "rock", "stone", "ore_block", "mining_spot" };
            
            return allDetections
                .Where(d => rockClasses.Any(c => d.Label.Contains(c, StringComparison.OrdinalIgnoreCase)))
                .ToList();
        }

        public (string ModelPath, int InputWidth, int InputHeight) GetModelInfo()
        {
            return (_modelPath, _inputWidth, _inputHeight);
        }

        public void Dispose()
        {
            _session?.Dispose();
            Log.Information("Object detection service disposed");
        }

        private DenseTensor<float> PreprocessImage(Mat image)
        {
            // Resize image to model input size
            var resized = new Mat();
            OpenCvSharp.Cv2.Resize(image, resized, new OpenCvSharp.Size(_inputWidth, _inputHeight));

            // Normalize and convert to tensor
            var tensor = new DenseTensor<float>(new[] { 1, 3, _inputHeight, _inputWidth });
            
            // Convert BGR to RGB and normalize
            for (int y = 0; y < _inputHeight; y++)
            {
                for (int x = 0; x < _inputWidth; x++)
                {
                    var pixel = resized.At<Vec3b>(y, x);
                    tensor[0, 0, y, x] = pixel.Item2 / 255f; // R
                    tensor[0, 1, y, x] = pixel.Item1 / 255f; // G
                    tensor[0, 2, y, x] = pixel.Item0 / 255f; // B
                }
            }

            resized.Dispose();
            return tensor;
        }

        private List<DetectionResult> ParseDetectionOutput(IDisposableReadOnlyCollection<DisposableNamedOnnxValue> outputs, float confidenceThreshold)
        {
            var results = new List<DetectionResult>();

            // YOLO output format: [batch, num_detections, 85] (x, y, w, h, confidence, class_probs...)
            var output = outputs.First().AsEnumerable<float>().ToArray();

            // TODO: Implement proper YOLO output parsing
            // This is a placeholder - actual implementation depends on your model's output format

            return results;
        }
    }
}
