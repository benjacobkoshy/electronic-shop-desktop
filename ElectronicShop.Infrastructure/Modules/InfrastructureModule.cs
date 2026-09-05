using Autofac;
using ElectronicShop.Core.Repositories;
using ElectronicShop.Infrastructure.Data;
using ElectronicShop.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;

namespace ElectronicShop.Infrastructure.Modules
{
    public class InfrastructureModule : Autofac.Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            var dbFolder = Path.Combine(
                Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData),
                "ElectronicShopManager");
            Directory.CreateDirectory(dbFolder);
            var dbPath = Path.Combine(dbFolder, "shop.db");

            builder.Register(ctx =>
            {
                var options = new DbContextOptionsBuilder<ShopDbContext>()
                    .UseSqlite($"Data Source={dbPath}")
                    .Options;
                return new ShopDbContext(options);
            }).InstancePerLifetimeScope();

            builder.RegisterType<StockRepository>().As<IStockRepository>();
            builder.RegisterType<BillingRepository>().As<IBillingRepository>();
            builder.RegisterType<CustomerRepository>().As<ICustomerRepository>();
        }
    }
}