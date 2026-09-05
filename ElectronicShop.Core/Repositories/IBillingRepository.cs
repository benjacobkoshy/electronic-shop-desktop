using ElectronicShop.Core.Models;

namespace ElectronicShop.Core.Repositories
{
    public interface IBillingRepository
    {
        Task<List<Bill>> GetRecentAsync(int count);
        Task<Bill?> GetByIdAsync(int id);
        Task<string> GenerateNextBillNumberAsync();

        // Creates the bill, its line items, and deducts stock for each item — all in one transaction
        Task<Bill> CreateBillAsync(Bill bill, List<BillLineItem> lineItems);
    }
}