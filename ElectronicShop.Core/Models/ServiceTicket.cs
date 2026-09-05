using System.ComponentModel.DataAnnotations;

namespace ElectronicShop.Core.Models
{
    public class ServiceTicket
    {
        public int Id { get; set; }

        public int CustomerId { get; set; }

        [Required]
        public string DeviceDescription { get; set; } = string.Empty;

        [Required]
        [MaxLength(50)]
        public string Status { get; set; } = "InProgress"; // e.g. "InProgress", "Resolved", "WaitingForStock"

        public string ReceivedDate { get; set; } = DateTime.UtcNow.ToString("o");

        public string? ResolvedDate { get; set; }

        public string? Notes { get; set; }

        // Navigation
        public Customer Customer { get; set; } = null!;
        public ICollection<Bill> Bills { get; set; } = new List<Bill>();
    }
}