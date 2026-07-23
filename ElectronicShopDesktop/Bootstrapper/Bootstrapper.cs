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

            builder.RegisterType<MainWindowViewModel>();
            builder.RegisterType<DashboardViewModel>();
            builder.RegisterType<InventoryViewModel>();
            builder.RegisterType<BillingViewModel>();
            builder.RegisterType<CustomersViewModel>();
            builder.RegisterType<SettingsViewModel>();

            //builder.Register<Func<Type, ViewModelBase>>(ctx =>
            //{
            //    var c = ctx.Resolve<IComponentContext>();
            //    return t => (ViewModelBase)c.Resolve(t);
            //});
            //builder.RegisterType<NavigationService>().As<INavigationService>().SingleInstance();

            builder.RegisterType<MainWindow>();

            return builder.Build();
        }
    }
}