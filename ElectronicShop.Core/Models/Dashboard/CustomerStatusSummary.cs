using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElectronicShop.Core.Models.Dashboard
{
    public class CustomerStatusSummary
    {
        public string Status { get; set; }
        public int Count { get; set; }
        public string BrushKey { get; set; }
    }
}