namespace ElectronicShop.App.Models.Inventory
{
    public class StockItem
    {
        public string Id { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public string Category { get; set; } = string.Empty;
        public string Sku { get; set; } = string.Empty;
        public int Quantity { get; set; }
        public int LowStockThreshold { get; set; }
        public decimal UnitPrice { get; set; }
    }
}