using System.Windows;
using ForgeMacro.Views;

namespace ForgeMacro
{
    public partial class MainWindow : Window
    {
        private readonly MainWindowViewModel _viewModel;

        public MainWindow(MainWindowViewModel viewModel)
        {
            InitializeComponent();
            _viewModel = viewModel;
            DataContext = _viewModel;
        }

        private void NavigateToDashboard(object sender, RoutedEventArgs e)
        {
            PageTitle.Text = "Dashboard";
            ContentFrame.Navigate(new DashboardPage());
        }

        private void NavigateToSettings(object sender, RoutedEventArgs e)
        {
            PageTitle.Text = "Settings";
            ContentFrame.Navigate(new SettingsPage());
        }

        private void NavigateToDetection(object sender, RoutedEventArgs e)
        {
            PageTitle.Text = "Detection";
            ContentFrame.Navigate(new DetectionPage());
        }

        private void NavigateToStatistics(object sender, RoutedEventArgs e)
        {
            PageTitle.Text = "Statistics";
            ContentFrame.Navigate(new StatisticsPage());
        }

        private void NavigateToLogs(object sender, RoutedEventArgs e)
        {
            PageTitle.Text = "Logs";
            ContentFrame.Navigate(new LogsPage());
        }

        private void StartMacro(object sender, RoutedEventArgs e)
        {
            _viewModel.StartMacroCommand.Execute(null);
        }

        private void StopMacro(object sender, RoutedEventArgs e)
        {
            _viewModel.StopMacroCommand.Execute(null);
        }
    }
}
