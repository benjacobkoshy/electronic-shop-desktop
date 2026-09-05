using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ElectronicShop.Core.Models
{
    public class BillLineItem
    {
        public int Id { get; set; }

        public int BillId { get; set; }

        public int? ProductId { get; set; }

        [Required]
        public string Description { get; set; } = string.Empty;

        public int Quantity { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public decimal UnitPrice { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public decimal LineTotal { get; set; }

        // Navigation
        public Bill Bill { get; set; } = null!;
        public Product? Product { get; set; }
    }
}