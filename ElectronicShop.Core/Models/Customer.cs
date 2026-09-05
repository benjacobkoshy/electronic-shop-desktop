

using System.ComponentModel.DataAnnotations;

namespace ElectronicShop.Core.Models
{
    public class Customer
    {
        public int Id { get; set; }

        [Required]
        [MaxLength(100)]
        public string Name { get; set; } = string.Empty;

        [MaxLength(10)]
        public string Phone { get; set; } = string.Empty;

        [MaxLength(100)]
        public string? Email { get; set; }

        public string? Address { get; set; }

        public string CreatedAt { get; set; } = DateTime.UtcNow.ToString("o");

        // Navigation
        public ICollection<ServiceTicket> ServiceTickets { get; set; } = new List<ServiceTicket>();
        public ICollection<Bill> Bills { get; set; } = new List<Bill>();
    }
}