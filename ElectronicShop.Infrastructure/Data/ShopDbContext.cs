using Microsoft.EntityFrameworkCore;
using ElectronicShop.Core.Models;

namespace ElectronicShop.Infrastructure.Data
{
    public class ShopDbContext : DbContext
    {
        public DbSet<Category> Categories => Set<Category>();
        public DbSet<Product> Products => Set<Product>();
        public DbSet<Customer> Customers => Set<Customer>();
        public DbSet<ServiceTicket> ServiceTickets => Set<ServiceTicket>();
        public DbSet<Bill> Bills => Set<Bill>();
        public DbSet<BillLineItem> BillLineItems => Set<BillLineItem>();
        public DbSet<StockTransaction> StockTransactions => Set<StockTransaction>();
        public DbSet<ShopSettings> ShopSettings => Set<ShopSettings>();

        public ShopDbContext(DbContextOptions<ShopDbContext> options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Product>()
                .HasOne(p => p.Category)
                .WithMany(c => c.Products)
                .HasForeignKey(p => p.CategoryId)
                .OnDelete(DeleteBehavior.SetNull);

            modelBuilder.Entity<Bill>()
                .HasOne(b => b.ServiceTicket)
                .WithMany(t => t.Bills)
                .HasForeignKey(b => b.ServiceTicketId)
                .OnDelete(DeleteBehavior.SetNull);

            modelBuilder.Entity<Bill>()
                .HasOne(b => b.ServiceTicket)
                .WithMany()
                .HasForeignKey(b => b.ServiceTicketId)
                .OnDelete(DeleteBehavior.SetNull);

            modelBuilder.Entity<BillLineItem>()
                .HasOne(li => li.Product)
                .WithMany()
                .HasForeignKey(li => li.ProductId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Product>().HasIndex(p => p.Sku).IsUnique();
            modelBuilder.Entity<Bill>().HasIndex(b => b.BillNumber).IsUnique();
        }
    }
}