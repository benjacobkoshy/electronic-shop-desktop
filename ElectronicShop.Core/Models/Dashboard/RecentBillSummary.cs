using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElectronicShop.Core.Models.Dashboard
{
    public class RecentBillSummary
    {
        public string BillNumber { get; set; }
        public string CustomerName { get; set; }
        public decimal Amount { get; set; }
        public DateTime Date { get; set; }
        public string Status { get; set; } // "Paid", "Pending"
    }
}