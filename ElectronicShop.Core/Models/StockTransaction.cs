using System.ComponentModel.DataAnnotations;

namespace ElectronicShop.Core.Models
{
    public class StockTransaction
    {
        public int Id { get; set; }

        public int ProductId { get; set; }

        public int ChangeQuantity { get; set; }

        [Required]
        [MaxLength(30)]
        public string TransactionType { get; set; } = string.Empty;

        public int? ReferenceBillId { get; set; }

        public string Timestamp { get; set; } = DateTime.UtcNow.ToString("o");

        public string? Notes { get; set; }

        // Navigation
        public Product Product { get; set; } = null!;
        public Bill? ReferenceBill { get; set; }
    }
}