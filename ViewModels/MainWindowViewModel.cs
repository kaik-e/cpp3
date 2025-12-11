using System.Windows.Input;
using System.ComponentModel;
using ForgeMacro.Services;
using Serilog;

namespace ForgeMacro
{
    public class MainWindowViewModel
    {
        private readonly IMacroEngineService _macroEngine;
        private bool _isRunning;

        public ICommand StartMacroCommand { get; }
        public ICommand StopMacroCommand { get; }

        public bool IsRunning
        {
            get => _isRunning;
            set
            {
                if (_isRunning != value)
                {
                    _isRunning = value;
                    OnPropertyChanged(nameof(IsRunning));
                }
            }
        }

        public MainWindowViewModel(IMacroEngineService macroEngine)
        {
            _macroEngine = macroEngine;
            StartMacroCommand = new RelayCommand(StartMacro, () => !IsRunning);
            StopMacroCommand = new RelayCommand(StopMacro, () => IsRunning);
        }

        private void StartMacro()
        {
            try
            {
                Log.Information("Starting macro...");
                _macroEngine.Start();
                IsRunning = true;
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Error starting macro");
            }
        }

        private void StopMacro()
        {
            try
            {
                Log.Information("Stopping macro...");
                _macroEngine.Stop();
                IsRunning = false;
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Error stopping macro");
            }
        }

        public event PropertyChangedEventHandler? PropertyChanged;

        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }

    public class RelayCommand : ICommand
    {
        private readonly Action _execute;
        private readonly Func<bool>? _canExecute;

        public RelayCommand(Action execute, Func<bool>? canExecute = null)
        {
            _execute = execute;
            _canExecute = canExecute;
        }

        public event EventHandler? CanExecuteChanged
        {
            add { CommandManager.RequerySuggested += value; }
            remove { CommandManager.RequerySuggested -= value; }
        }

        public bool CanExecute(object? parameter) => _canExecute?.Invoke() ?? true;

        public void Execute(object? parameter) => _execute();
    }
}
