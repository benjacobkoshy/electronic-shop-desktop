using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ElectronicShop.Core.Models
{
    public class Product
    {
        public int Id { get; set; }

        [Required]
        [MaxLength(100)]
        public string Name { get; set; } = string.Empty;

        [Required]
        [MaxLength(50)]
        public string Sku { get; set; } = string.Empty;

        public int? CategoryId { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public decimal UnitPrice { get; set; }

        public int Quantity { get; set; } = 0;

        public int LowStockThreshold { get; set; } = 5;

        public string CreatedAt { get; set; } = DateTime.UtcNow.ToString("o");

        public string UpdatedAt { get; set; } = DateTime.UtcNow.ToString("o");

        // Navigation
        public Category? Category { get; set; }
        public ICollection<BillLineItem> BillLineItems { get; set; } = new List<BillLineItem>();
        public ICollection<StockTransaction> StockTransactions { get; set; } = new List<StockTransaction>();
    }
}