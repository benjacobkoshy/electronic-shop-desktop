using Microsoft.EntityFrameworkCore;
using ElectronicShop.Core.Models;
using ElectronicShop.Core.Repositories;
using ElectronicShop.Infrastructure.Data;

namespace ElectronicShop.Infrastructure.Repositories
{
    public class StockRepository : IStockRepository
    {
        private readonly ShopDbContext _db;

        public StockRepository(ShopDbContext db) => _db = db;

        public async Task<List<Product>> GetAllAsync()
            => await _db.Products.Include(p => p.Category).ToListAsync();

        public async Task<Product?> GetByIdAsync(int id)
            => await _db.Products.Include(p => p.Category).FirstOrDefaultAsync(p => p.Id == id);

        public async Task<List<Category>> GetCategoriesAsync()
            => await _db.Categories.OrderBy(c => c.Name).ToListAsync();

        public async Task<List<Product>> GetLowStockAsync()
            => await _db.Products.Where(p => p.Quantity <= p.LowStockThreshold).ToListAsync();

        public async Task AddAsync(Product product)
        {
            _db.Products.Add(product);
            await _db.SaveChangesAsync();
        }

        public async Task UpdateAsync(Product product)
        {
            product.UpdatedAt = DateTime.UtcNow.ToString("o");
            _db.Products.Update(product);
            await _db.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var product = await _db.Products.FindAsync(id);
            if (product == null) return;
            _db.Products.Remove(product);
            await _db.SaveChangesAsync();
        }

        public async Task AdjustStockAsync(int productId, int changeQuantity, string transactionType, int? referenceBillId = null, string? notes = null)
        {
            using var transaction = await _db.Database.BeginTransactionAsync();

            var product = await _db.Products.FindAsync(productId)
                ?? throw new InvalidOperationException($"Product {productId} not found.");

            product.Quantity += changeQuantity;
            if (product.Quantity < 0)
                throw new InvalidOperationException($"Insufficient stock for '{product.Name}'.");

            product.UpdatedAt = DateTime.UtcNow.ToString("o");

            _db.StockTransactions.Add(new StockTransaction
            {
                ProductId = productId,
                ChangeQuantity = changeQuantity,
                TransactionType = transactionType,
                ReferenceBillId = referenceBillId,
                Notes = notes
            });

            await _db.SaveChangesAsync();
            await transaction.CommitAsync();
        }

        public async Task<Category> AddCategoryAsync(string name)
        {
            var existing = await _db.Categories.FirstOrDefaultAsync(c => c.Name == name);
            if (existing != null) return existing; // avoid duplicate categories with the same name

            var category = new Category { Name = name };
            _db.Categories.Add(category);
            await _db.SaveChangesAsync();
            return category;
        }
    }
}