using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ElectronicShop.Core.Models
{
    public class ShopSettings
    {
        public int Id { get; set; } = 1;

        [Required]
        [MaxLength(100)]
        public string ShopName { get; set; } = string.Empty;

        public string Address { get; set; } = string.Empty;

        [MaxLength(20)]
        public string Phone { get; set; } = string.Empty;

        [MaxLength(50)]
        public string? GSTIN { get; set; }

        [MaxLength(10)]
        public string CurrencySymbol { get; set; } = "₹";
    }
}