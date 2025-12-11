using System.Runtime.InteropServices;
using Serilog;

namespace ForgeMacro.Services
{
    public class InputSimulationService : IInputSimulationService
    {
        // Native Windows API imports
        [DllImport("user32.dll")]
        private static extern bool SetCursorPos(int x, int y);

        [DllImport("user32.dll")]
        private static extern void mouse_event(uint dwFlags, int dx, int dy, uint dwData, int dwExtraInfo);

        [DllImport("user32.dll")]
        private static extern bool GetCursorPos(out POINT lpPoint);

        [DllImport("user32.dll")]
        private static extern void keybd_event(byte bVk, byte bScan, uint dwFlags, int dwExtraInfo);

        [StructLayout(LayoutKind.Sequential)]
        private struct POINT
        {
            public int X;
            public int Y;
        }

        // Mouse event constants
        private const uint MOUSEEVENTF_LEFTDOWN = 0x0002;
        private const uint MOUSEEVENTF_LEFTUP = 0x0004;
        private const uint MOUSEEVENTF_RIGHTDOWN = 0x0008;
        private const uint MOUSEEVENTF_RIGHTUP = 0x0010;

        // Keyboard event constants
        private const uint KEYEVENTF_KEYUP = 0x0002;

        public InputSimulationService()
        {
        }

        public void MoveMouse(int x, int y)
        {
            try
            {
                SetCursorPos(x, y);
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Error moving mouse to ({X}, {Y})", x, y);
            }
        }

        public void Click()
        {
            try
            {
                mouse_event(MOUSEEVENTF_LEFTDOWN, 0, 0, 0, 0);
                mouse_event(MOUSEEVENTF_LEFTUP, 0, 0, 0, 0);
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Error clicking mouse");
            }
        }

        public void RightClick()
        {
            try
            {
                mouse_event(MOUSEEVENTF_RIGHTDOWN, 0, 0, 0, 0);
                mouse_event(MOUSEEVENTF_RIGHTUP, 0, 0, 0, 0);
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Error right-clicking mouse");
            }
        }

        public void DoubleClick()
        {
            try
            {
                Click();
                Thread.Sleep(50);
                Click();
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Error double-clicking mouse");
            }
        }

        public void PressKey(string key)
        {
            try
            {
                // Convert key string to virtual key code
                byte vkCode = GetVirtualKeyCode(key);
                if (vkCode != 0)
                {
                    keybd_event(vkCode, 0, 0, 0);
                    keybd_event(vkCode, 0, KEYEVENTF_KEYUP, 0);
                }
                else
                {
                    Log.Warning("Unknown key: {Key}", key);
                }
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Error pressing key {Key}", key);
            }
        }

        public void TypeText(string text)
        {
            try
            {
                foreach (char c in text)
                {
                    // Simple implementation - send each character
                    byte vkCode = (byte)char.ToUpper(c);
                    keybd_event(vkCode, 0, 0, 0);
                    keybd_event(vkCode, 0, KEYEVENTF_KEYUP, 0);
                    Thread.Sleep(10);
                }
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Error typing text");
            }
        }

        public (int X, int Y) GetMousePosition()
        {
            try
            {
                if (GetCursorPos(out POINT point))
                {
                    return (point.X, point.Y);
                }
                return (0, 0);
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Error getting mouse position");
                return (0, 0);
            }
        }

        public void Delay(int milliseconds)
        {
            Thread.Sleep(milliseconds);
        }

        private byte GetVirtualKeyCode(string key)
        {
            return key.ToUpper() switch
            {
                "A" => 0x41, "B" => 0x42, "C" => 0x43, "D" => 0x44, "E" => 0x45,
                "F" => 0x46, "G" => 0x47, "H" => 0x48, "I" => 0x49, "J" => 0x4A,
                "K" => 0x4B, "L" => 0x4C, "M" => 0x4D, "N" => 0x4E, "O" => 0x4F,
                "P" => 0x50, "Q" => 0x51, "R" => 0x52, "S" => 0x53, "T" => 0x54,
                "U" => 0x55, "V" => 0x56, "W" => 0x57, "X" => 0x58, "Y" => 0x59,
                "Z" => 0x5A,
                "0" => 0x30, "1" => 0x31, "2" => 0x32, "3" => 0x33, "4" => 0x34,
                "5" => 0x35, "6" => 0x36, "7" => 0x37, "8" => 0x38, "9" => 0x39,
                "SPACE" => 0x20, "ENTER" => 0x0D, "TAB" => 0x09, "ESCAPE" => 0x1B,
                "BACKSPACE" => 0x08, "DELETE" => 0x2E,
                "LEFT" => 0x25, "UP" => 0x26, "RIGHT" => 0x27, "DOWN" => 0x28,
                "SHIFT" => 0x10, "CTRL" => 0x11, "ALT" => 0x12,
                "F1" => 0x70, "F2" => 0x71, "F3" => 0x72, "F4" => 0x73,
                "F5" => 0x74, "F6" => 0x75, "F7" => 0x76, "F8" => 0x77,
                "F9" => 0x78, "F10" => 0x79, "F11" => 0x7A, "F12" => 0x7B,
                _ => 0
            };
        }
    }
}
