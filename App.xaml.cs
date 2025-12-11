using System.Windows;
using Microsoft.Extensions.DependencyInjection;
using ForgeMacro.Services;
using Serilog;

namespace ForgeMacro
{
    public partial class App : Application
    {
        public static IServiceProvider ServiceProvider { get; private set; } = null!;

        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            try
            {
                // Configure logging
                Log.Logger = new LoggerConfiguration()
                    .MinimumLevel.Information()
                    .WriteTo.Console()
                    .WriteTo.File("logs/forgego-.txt", rollingInterval: RollingInterval.Day)
                    .CreateLogger();

                // Setup dependency injection
                var services = new ServiceCollection();
                ConfigureServices(services);
                ServiceProvider = services.BuildServiceProvider();

                Log.Information("ForgeMacro started");

                // Show main window
                var viewModel = ServiceProvider.GetRequiredService<MainWindowViewModel>();
                var mainWindow = new MainWindow(viewModel);
                mainWindow.Show();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error starting application: {ex.Message}\n\n{ex.StackTrace}", "ForgeMacro Error", MessageBoxButton.OK, MessageBoxImage.Error);
                Log.Error(ex, "Error starting application");
                Shutdown();
            }
        }

        private void ConfigureServices(IServiceCollection services)
        {
            // Register services
            services.AddSingleton<IScreenCaptureService, ScreenCaptureService>();
            services.AddSingleton<IOcrService, OcrService>();
            services.AddSingleton<IObjectDetectionService, ObjectDetectionService>();
            services.AddSingleton<IInputSimulationService, InputSimulationService>();
            // Backend service is optional - only used if configured
            services.AddSingleton<IBackendService, BackendService>();
            services.AddSingleton<IMacroEngineService, MacroEngineService>();

            // Register ViewModels
            services.AddSingleton<MainWindowViewModel>();
            services.AddSingleton<MainWindow>();
        }

        protected override void OnExit(ExitEventArgs e)
        {
            Log.Information("ForgeMacro closed");
            Log.CloseAndFlush();
            base.OnExit(e);
        }
    }
}
