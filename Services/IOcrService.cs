using System.Drawing;

namespace ForgeMacro.Services
{
    public interface IOcrService
    {
        /// <summary>
        /// Extracts text from an image
        /// </summary>
        string ExtractText(Bitmap image);

        /// <summary>
        /// Extracts text from a specific region
        /// </summary>
        string ExtractTextFromRegion(Bitmap image, Rectangle region);

        /// <summary>
        /// Detects ore names and types from image
        /// </summary>
        Dictionary<string, double> DetectOreTypes(Bitmap image);

        /// <summary>
        /// Initializes OCR engine
        /// </summary>
        Task InitializeAsync();

        /// <summary>
        /// Disposes OCR resources
        /// </summary>
        void Dispose();
    }
}
