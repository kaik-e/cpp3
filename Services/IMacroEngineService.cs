namespace ForgeMacro.Services
{
    public interface IMacroEngineService
    {
        /// <summary>
        /// Starts the macro automation
        /// </summary>
        void Start();

        /// <summary>
        /// Stops the macro automation
        /// </summary>
        void Stop();

        /// <summary>
        /// Pauses the macro
        /// </summary>
        void Pause();

        /// <summary>
        /// Resumes the macro
        /// </summary>
        void Resume();

        /// <summary>
        /// Gets current macro status
        /// </summary>
        string GetStatus();

        /// <summary>
        /// Gets current statistics
        /// </summary>
        MacroStats GetStats();

        /// <summary>
        /// Checks if macro is running
        /// </summary>
        bool IsRunning { get; }

        /// <summary>
        /// Event fired when ore is detected
        /// </summary>
        event EventHandler<string>? OreDetected;

        /// <summary>
        /// Event fired when rock is detected
        /// </summary>
        event EventHandler<string>? RockDetected;

        /// <summary>
        /// Event fired when error occurs
        /// </summary>
        event EventHandler<string>? ErrorOccurred;
    }
}
