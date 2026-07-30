using ElectronicShop.App.Models.Inventory;
using ElectronicShop.App.ViewModels.Base;

namespace ElectronicShop.App.ViewModels
{
    public class StockItemViewModel : ViewModelBase
    {
        private readonly StockItem _model;

        public StockItemViewModel(StockItem model) => _model = model;

        public string Id => _model.Id;
        public string Name => _model.Name;
        public string Category => _model.Category;
        public string Sku => _model.Sku;
        public decimal UnitPrice => _model.UnitPrice;
        public int LowStockThreshold => _model.LowStockThreshold;

        public int Quantity
        {
            get => _model.Quantity;
            set
            {
                if (_model.Quantity == value) return;
                _model.Quantity = value;
                OnPropertyChanged();
                OnPropertyChanged(nameof(StockStatus));
            }
        }

        public string StockStatus =>
            Quantity == 0 ? "Out of Stock" :
            Quantity <= LowStockThreshold ? "Low Stock" : "In Stock";

        // Exposes the underlying domain model for save operations
        public StockItem ToModel() => _model;
    }
}