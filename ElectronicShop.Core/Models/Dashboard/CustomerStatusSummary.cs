

namespace ElectronicShop.Core.Models.Dashboard
{
    public class CustomerStatusSummary
    {
        public string Status { get; set; } = string.Empty;
        public int Count { get; set; }
        public string BrushKey { get; set; } = string.Empty;
    }
}