using Microsoft.EntityFrameworkCore;
using ElectronicShop.Core.Models;
using ElectronicShop.Core.Repositories;
using ElectronicShop.Infrastructure.Data;

namespace ElectronicShop.Infrastructure.Repositories
{
    public class BillingRepository : IBillingRepository
    {
        private readonly ShopDbContext _db;
        private readonly IStockRepository _stockRepository;

        public BillingRepository(ShopDbContext db, IStockRepository stockRepository)
        {
            _db = db;
            _stockRepository = stockRepository;
        }

        public async Task<List<Bill>> GetRecentAsync(int count)
            => await _db.Bills
                .Include(b => b.Customer)
                .OrderByDescending(b => b.BillDate)
                .Take(count)
                .ToListAsync();

        public async Task<Bill?> GetByIdAsync(int id)
            => await _db.Bills
                .Include(b => b.Customer)
                .Include(b => b.ServiceTicket)
                .Include(b => b.LineItems).ThenInclude(li => li.Product)
                .FirstOrDefaultAsync(b => b.Id == id);

        public async Task<string> GenerateNextBillNumberAsync()
        {
            var lastBill = await _db.Bills.OrderByDescending(b => b.Id).FirstOrDefaultAsync();
            var nextNumber = (lastBill == null ? 1000 : int.Parse(lastBill.BillNumber.Split('-')[1])) + 1;
            return $"INV-{nextNumber}";
        }

        public async Task<Bill> CreateBillAsync(Bill bill, List<BillLineItem> lineItems)
        {
            using var transaction = await _db.Database.BeginTransactionAsync();

            _db.Bills.Add(bill);
            await _db.SaveChangesAsync(); // Bill.Id gets populated here

            foreach (var item in lineItems)
            {
                item.BillId = bill.Id;
                _db.BillLineItems.Add(item);

                if (item.ProductId.HasValue)
                {
                    await _stockRepository.AdjustStockAsync(
                        item.ProductId.Value,
                        -item.Quantity,
                        "Sale",
                        referenceBillId: bill.Id);
                }
            }

            await _db.SaveChangesAsync();
            await transaction.CommitAsync();

            return bill;
        }
    }
}