using Microsoft.EntityFrameworkCore;
using ElectronicShop.Core.Models;
using ElectronicShop.Core.Repositories;
using ElectronicShop.Infrastructure.Data;

namespace ElectronicShop.Infrastructure.Repositories
{
    public class CustomerRepository : ICustomerRepository
    {
        private readonly ShopDbContext _db;

        public CustomerRepository(ShopDbContext db) => _db = db;

        public async Task<List<Customer>> GetAllAsync()
            => await _db.Customers.ToListAsync();

        public async Task<Customer?> GetByIdAsync(int id)
            => await _db.Customers
                .Include(c => c.ServiceTickets)
                .Include(c => c.Bills)
                .FirstOrDefaultAsync(c => c.Id == id);

        public async Task AddAsync(Customer customer)
        {
            _db.Customers.Add(customer);
            await _db.SaveChangesAsync();
        }

        public async Task UpdateAsync(Customer customer)
        {
            _db.Customers.Update(customer);
            await _db.SaveChangesAsync();
        }

        public async Task<List<ServiceTicket>> GetTicketsByCustomerAsync(int customerId)
            => await _db.ServiceTickets
                .Where(t => t.CustomerId == customerId)
                .OrderByDescending(t => t.ReceivedDate)
                .ToListAsync();

        public async Task AddServiceTicketAsync(ServiceTicket ticket)
        {
            _db.ServiceTickets.Add(ticket);
            await _db.SaveChangesAsync();
        }

        public async Task UpdateTicketStatusAsync(int ticketId, string status)
        {
            var ticket = await _db.ServiceTickets.FindAsync(ticketId)
                ?? throw new InvalidOperationException($"Ticket {ticketId} not found.");

            ticket.Status = status;
            if (status == "Resolved")
                ticket.ResolvedDate = DateTime.UtcNow.ToString("o");

            await _db.SaveChangesAsync();
        }

        public async Task<List<ServiceTicket>> GetAllTicketsAsync()
    => await _db.ServiceTickets.ToListAsync();
    }
}