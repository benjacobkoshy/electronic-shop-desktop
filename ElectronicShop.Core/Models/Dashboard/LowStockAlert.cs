using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElectronicShop.Core.Models.Dashboard
{
    public class LowStockAlert
    {
        public string ProductName { get; set; }
        public int RemainingQty { get; set; }
        public int ThresholdQty { get; set; }
    }
}
