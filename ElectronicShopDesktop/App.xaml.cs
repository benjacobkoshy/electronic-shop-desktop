using Autofac;
using ElectronicShop.App.ViewModels.Base;
using System.Configuration;
using System.Data;
using System.Windows;
using static ElectronicShop.App.ViewModels.Base.ThemeManager;

namespace ElectronicShop.App
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {

        private IContainer _container;

        /// <summary>
        /// Initializes the application and registers global exception handlers.
        /// </summary>
        public App()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Handles the application startup event by configuring dependency injection and showing the login window.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">Startup arguments for the application.</param>
        private void ApplicationStartup(object sender, StartupEventArgs e)
        {
            var bootstrapper = new Bootstrapper.Bootstrapper();
            _container = bootstrapper.Bootstrap();

            ThemeManager.ApplyTheme(AppTheme.Light);

            var mainWindow = _container.Resolve<MainWindow>();
            mainWindow.Show();
        }

        protected override void OnExit(ExitEventArgs e)
        {
            _container?.Dispose();
            base.OnExit(e);
        }

    }
}