using ElectronicShop.App.ViewModels.Base;

namespace ElectronicShop.App.Models
{
    public class BillLineItemDraft : ViewModelBase
    {
        public int? ProductId { get; set; }
        public string Description { get; set; } = string.Empty;

        private int _quantity = 1;
        public int Quantity
        {
            get => _quantity;
            set { SetProperty(ref _quantity, value); OnPropertyChanged(nameof(LineTotal)); }
        }

        public decimal UnitPrice { get; set; }
        public int AvailableStock { get; set; } // for validation, not persisted

        public decimal LineTotal => Quantity * UnitPrice;
    }
}