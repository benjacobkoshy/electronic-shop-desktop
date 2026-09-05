using ElectronicShop.Core.Models;

namespace ElectronicShop.Core.Repositories
{
    public interface ICustomerRepository
    {
        Task<List<Customer>> GetAllAsync();
        Task<Customer?> GetByIdAsync(int id);
        Task AddAsync(Customer customer);
        Task UpdateAsync(Customer customer);

        Task<List<ServiceTicket>> GetTicketsByCustomerAsync(int customerId);
        Task AddServiceTicketAsync(ServiceTicket ticket);
        Task UpdateTicketStatusAsync(int ticketId, string status);
        Task<List<ServiceTicket>> GetAllTicketsAsync();
    }
}