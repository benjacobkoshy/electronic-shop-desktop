using Autofac;
using ElectronicShop.App.Navigation;
using ElectronicShop.App.ViewModels;
using ElectronicShop.App.ViewModels.Base;

namespace ElectronicShop.App.Bootstrapper
{
    public class Bootstrapper
    {
        public IContainer Bootstrap()
        {
            var builder = new ContainerBuilder();

            builder.RegisterType<MainWindowViewModel>().AsSelf();
            builder.RegisterType<DashboardViewModel>().AsSelf();
            builder.RegisterType<InventoryViewModel>().AsSelf();
            builder.RegisterType<BillingViewModel>().AsSelf();
            builder.RegisterType<CustomersViewModel>().AsSelf();
            builder.RegisterType<SettingsViewModel>().AsSelf();

            builder.Register<Func<Type, ViewModelBase>>(ctx =>
            {
                var c = ctx.Resolve<IComponentContext>();
                return t => (ViewModelBase)c.Resolve(t);
            });
            builder.RegisterType<NavigationService>().As<INavigationService>().SingleInstance();

            builder.RegisterType<MainWindow>();

            return builder.Build();
        }
    }
}