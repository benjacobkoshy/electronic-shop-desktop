using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ElectronicShop.Core.Models
{
    public class Bill
    {
        public int Id { get; set; }

        [Required]
        [MaxLength(50)]
        public string BillNumber { get; set; } = string.Empty;

        public int CustomerId { get; set; }

        public int? ServiceTicketId { get; set; }

        public string BillDate { get; set; } = DateTime.UtcNow.ToString("o");

        [Column(TypeName = "decimal(18,2)")]
        public decimal Subtotal { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public decimal DiscountAmount { get; set; } = 0m;

        [Column(TypeName = "decimal(18,2)")]
        public decimal TaxAmount { get; set; } = 0m;

        [Column(TypeName = "decimal(18,2)")]
        public decimal TotalAmount { get; set; }

        [Required]
        [MaxLength(20)]
        public string PaymentStatus { get; set; } = "Pending";

        public string CreatedAt { get; set; } = DateTime.UtcNow.ToString("o");

        // Navigation
        public Customer Customer { get; set; } = null!;
        public ServiceTicket? ServiceTicket { get; set; }
        public ICollection<BillLineItem> LineItems { get; set; } = new List<BillLineItem>();
        public ICollection<StockTransaction> StockTransactions { get; set; } = new List<StockTransaction>();
    }
}