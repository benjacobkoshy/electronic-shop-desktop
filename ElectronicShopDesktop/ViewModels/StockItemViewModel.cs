using ElectronicShop.App.ViewModels.Base;
using CoreModels = ElectronicShop.Core.Models;

namespace ElectronicShop.App.ViewModels
{
    public class StockItemViewModel : ViewModelBase
    {
        private readonly CoreModels.Product _model;

        public StockItemViewModel(CoreModels.Product model) => _model = model;

        public int Id => _model.Id;
        public string Name => _model.Name;
        public string Sku => _model.Sku;
        public string Category => _model.Category?.Name ?? "Uncategorized";
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
        public CoreModels.Product ToModel() => _model;
    }
}