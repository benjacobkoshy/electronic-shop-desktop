using Autofac;
using ElectronicShop.App.ViewModels.Base;
using ElectronicShop.Core.Models;
using ElectronicShop.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
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

            try
            {
                using (var scope = _container.BeginLifetimeScope())
                {
                    var db = scope.Resolve<ShopDbContext>();
                    db.Database.Migrate();

                    if (!db.ShopSettings.Any())
                    {
                        db.ShopSettings.Add(new ShopSettings { ShopName = "My Electronics Shop", CurrencySymbol = "₹" });
                        db.SaveChanges();
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    $"Failed to initialize the application database:\n{ex.Message}",
                    "Startup Error", MessageBoxButton.OK, MessageBoxImage.Error);
                Shutdown();
                return;
            }

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