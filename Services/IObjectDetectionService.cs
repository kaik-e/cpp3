using System.Drawing;

namespace ForgeMacro.Services
{
    public class DetectionResult
    {
        public string Label { get; set; } = string.Empty;
        public float Confidence { get; set; }
        public Rectangle BoundingBox { get; set; }
    }

    public interface IObjectDetectionService
    {
        /// <summary>
        /// Loads a YOLO model from .pt or .onnx file
        /// </summary>
        Task LoadModelAsync(string modelPath);

        /// <summary>
        /// Detects objects in an image
        /// </summary>
        List<DetectionResult> Detect(Bitmap image, float confidenceThreshold = 0.5f);

        /// <summary>
        /// Detects ores specifically
        /// </summary>
        List<DetectionResult> DetectOres(Bitmap image, float confidenceThreshold = 0.6f);

        /// <summary>
        /// Detects rocks/mining spots
        /// </summary>
        List<DetectionResult> DetectRocks(Bitmap image, float confidenceThreshold = 0.6f);

        /// <summary>
        /// Gets the current model info
        /// </summary>
        (string ModelPath, int InputWidth, int InputHeight) GetModelInfo();

        /// <summary>
        /// Disposes model resources
        /// </summary>
        void Dispose();
    }
}
