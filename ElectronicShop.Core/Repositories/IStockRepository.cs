using ElectronicShop.Core.Models;

namespace ElectronicShop.Core.Repositories
{
    public interface IStockRepository
    {
        Task<List<Product>> GetAllAsync();
        Task<Product?> GetByIdAsync(int id);
        Task<List<Category>> GetCategoriesAsync();
        Task<List<Product>> GetLowStockAsync();

        Task AddAsync(Product product);
        Task UpdateAsync(Product product);
        Task DeleteAsync(int id);

        // Adjusts stock and writes a StockTransaction row in the same operation
        Task AdjustStockAsync(int productId, int changeQuantity, string transactionType, int? referenceBillId = null, string? notes = null);
        Task<Category> AddCategoryAsync(string name);
    }
}