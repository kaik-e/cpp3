using InputSimulator;
using Serilog;

namespace ForgeMacro.Services
{
    public class InputSimulationService : IInputSimulationService
    {
        private readonly InputSimulator.InputSimulator _simulator;

        public InputSimulationService()
        {
            _simulator = new InputSimulator.InputSimulator();
        }

        public void MoveMouse(int x, int y)
        {
            try
            {
                _simulator.Mouse.MoveMouseTo(x, y);
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
                _simulator.Mouse.LeftButtonClick();
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
                _simulator.Mouse.RightButtonClick();
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
                _simulator.Mouse.LeftButtonDoubleClick();
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
                if (Enum.TryParse<VirtualKeyCode>(key, true, out var keyCode))
                {
                    _simulator.Keyboard.KeyPress(keyCode);
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
                _simulator.Keyboard.TextEntry(text);
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
                var pos = System.Windows.Forms.Cursor.Position;
                return (pos.X, pos.Y);
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Error getting mouse position");
                return (0, 0);
            }
        }

        public void Delay(int milliseconds)
        {
            System.Threading.Thread.Sleep(milliseconds);
        }
    }
}
