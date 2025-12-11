namespace ForgeMacro.Services
{
    public interface IInputSimulationService
    {
        /// <summary>
        /// Moves the mouse to a specific position
        /// </summary>
        void MoveMouse(int x, int y);

        /// <summary>
        /// Clicks at the current mouse position
        /// </summary>
        void Click();

        /// <summary>
        /// Right-clicks at the current mouse position
        /// </summary>
        void RightClick();

        /// <summary>
        /// Double-clicks at the current mouse position
        /// </summary>
        void DoubleClick();

        /// <summary>
        /// Presses a key
        /// </summary>
        void PressKey(string key);

        /// <summary>
        /// Types text
        /// </summary>
        void TypeText(string text);

        /// <summary>
        /// Gets current mouse position
        /// </summary>
        (int X, int Y) GetMousePosition();

        /// <summary>
        /// Adds a delay between actions
        /// </summary>
        void Delay(int milliseconds);
    }
}
