using System.Drawing;
using System.Runtime.InteropServices;
using Serilog;

namespace ForgeMacro.Services
{
    public class ScreenCaptureService : IScreenCaptureService
    {
        [DllImport("user32.dll")]
        private static extern IntPtr GetDesktopWindow();

        [DllImport("user32.dll")]
        private static extern IntPtr GetWindowDC(IntPtr hWnd);

        [DllImport("user32.dll")]
        private static extern int ReleaseDC(IntPtr hWnd, IntPtr hDC);

        [DllImport("gdi32.dll")]
        private static extern IntPtr CreateCompatibleDC(IntPtr hdc);

        [DllImport("gdi32.dll")]
        private static extern IntPtr CreateCompatibleBitmap(IntPtr hdc, int nWidth, int nHeight);

        [DllImport("gdi32.dll")]
        private static extern IntPtr SelectObject(IntPtr hdc, IntPtr hgdiobj);

        [DllImport("gdi32.dll")]
        private static extern bool BitBlt(IntPtr hdcDest, int nXDest, int nYDest, int nWidth, int nHeight,
            IntPtr hdcSrc, int nXSrc, int nYSrc, uint dwRop);

        [DllImport("gdi32.dll")]
        private static extern bool DeleteObject(IntPtr hObject);

        [DllImport("gdi32.dll")]
        private static extern bool DeleteDC(IntPtr hdc);

        private const uint SRCCOPY = 0x00CC0020;

        public Bitmap CaptureScreen()
        {
            try
            {
                var (width, height) = GetScreenDimensions();
                return CaptureRegion(new Rectangle(0, 0, width, height));
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Error capturing screen");
                throw;
            }
        }

        public Bitmap CaptureRegion(Rectangle region)
        {
            IntPtr hDesktop = GetDesktopWindow();
            IntPtr hDC = GetWindowDC(hDesktop);
            IntPtr hMemDC = CreateCompatibleDC(hDC);
            IntPtr hBitmap = CreateCompatibleBitmap(hDC, region.Width, region.Height);
            IntPtr hOld = SelectObject(hMemDC, hBitmap);

            BitBlt(hMemDC, 0, 0, region.Width, region.Height, hDC, region.X, region.Y, SRCCOPY);

            SelectObject(hMemDC, hOld);
            DeleteDC(hMemDC);
            ReleaseDC(hDesktop, hDC);

            Bitmap bitmap = Image.FromHbitmap(hBitmap);
            DeleteObject(hBitmap);

            return bitmap;
        }

        public Bitmap? CaptureGameWindow()
        {
            // TODO: Implement game window detection and capture
            // This would search for Minecraft/Forge window
            Log.Warning("Game window capture not yet implemented");
            return null;
        }

        public (int Width, int Height) GetScreenDimensions()
        {
            return ((int)System.Windows.SystemParameters.PrimaryScreenWidth,
                    (int)System.Windows.SystemParameters.PrimaryScreenHeight);
        }
    }
}
