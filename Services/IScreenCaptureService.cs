using System.Drawing;

namespace ForgeMacro.Services
{
    public interface IScreenCaptureService
    {
        /// <summary>
        /// Captures the entire screen
        /// </summary>
        Bitmap CaptureScreen();

        /// <summary>
        /// Captures a specific region of the screen
        /// </summary>
        Bitmap CaptureRegion(Rectangle region);

        /// <summary>
        /// Captures the game window (if found)
        /// </summary>
        Bitmap? CaptureGameWindow();

        /// <summary>
        /// Gets the screen dimensions
        /// </summary>
        (int Width, int Height) GetScreenDimensions();
    }
}
