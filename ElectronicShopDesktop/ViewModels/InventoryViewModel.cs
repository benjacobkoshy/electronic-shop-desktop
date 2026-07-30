using ElectronicShop.App.Models;
using ElectronicShop.App.Models.Inventory;
using ElectronicShop.App.ViewModels.Base;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows.Data;
using System.Windows.Input;

namespace ElectronicShop.App.ViewModels
{
    public class InventoryViewModel : ViewModelBase
    {
        public ObservableCollection<StockItemViewModel> AllItems { get; }
        public ICollectionView ItemsView { get; }

        public ObservableCollection<string> Categories { get; }

        private string _searchText = string.Empty;
        public string SearchText
        {
            get => _searchText;
            set { SetProperty(ref _searchText, value); ItemsView.Refresh(); }
        }

        private string _selectedCategory = "All";
        public string SelectedCategory
        {
            get => _selectedCategory;
            set { SetProperty(ref _selectedCategory, value); ItemsView.Refresh(); }
        }

        public int TotalItems => AllItems.Count;
        public int LowStockCount => AllItems.Count(i => i.StockStatus == "Low Stock" || i.StockStatus == "Out of Stock");
        public decimal TotalStockValue => AllItems.Sum(i => i.Quantity * i.UnitPrice);

        public ICommand IncreaseStockCommand { get; }
        public ICommand DecreaseStockCommand { get; }
        public ICommand AddProductCommand { get; }
        public ICommand DeleteProductCommand { get; }

        public ICommand SelectCategoryCommand { get; }

        // in constructor, alongside the other command assignments:

        public InventoryViewModel()
        {
            // TODO: replace with IStockRepository calls once Infrastructure is wired
            var seedData = new[]
            {
                new StockItem { Id = "1", Name = "Capacitor 470µF", Category = "Passive", Sku = "CAP-470", Quantity = 4, LowStockThreshold = 10, UnitPrice = 8 },
                new StockItem { Id = "2", Name = "IC LM7805", Category = "IC", Sku = "IC-7805", Quantity = 2, LowStockThreshold = 15, UnitPrice = 25 },
                new StockItem { Id = "3", Name = "USB-C Connector", Category = "Connector", Sku = "CON-USBC", Quantity = 40, LowStockThreshold = 20, UnitPrice = 15 },
                new StockItem { Id = "4", Name = "Resistor 1kΩ", Category = "Passive", Sku = "RES-1K", Quantity = 200, LowStockThreshold = 50, UnitPrice = 1 },
                new StockItem { Id = "5", Name = "Soldering Iron Tip", Category = "Tools", Sku = "TL-TIP01", Quantity = 0, LowStockThreshold = 5, UnitPrice = 60 },
                new StockItem { Id = "6", Name = "Arduino Nano", Category = "Board", Sku = "BRD-NANO", Quantity = 12, LowStockThreshold = 5, UnitPrice = 450 },
            };

            AllItems = new ObservableCollection<StockItemViewModel>(
                seedData.Select(m => new StockItemViewModel(m)));

            Categories = new ObservableCollection<string>(
                new[] { "All" }.Concat(AllItems.Select(i => i.Category).Distinct().OrderBy(c => c)));

            ItemsView = CollectionViewSource.GetDefaultView(AllItems);
            ItemsView.Filter = FilterItem;

            IncreaseStockCommand = new RelayCommand(item => Adjust((StockItem)item, +1));
            DecreaseStockCommand = new RelayCommand(item => Adjust((StockItem)item, -1), item => ((StockItem)item).Quantity > 0);
            AddProductCommand = new RelayCommand(_ => { /* open Add Product dialog — Phase: dialogs */ });
            DeleteProductCommand = new RelayCommand(item => AllItems.Remove((StockItemViewModel)item));
            SelectCategoryCommand = new RelayCommand(cat => SelectedCategory = (string)cat);

        }

        private bool FilterItem(object obj)
        {
            if (obj is not StockItem item) return false;

            var matchesCategory = SelectedCategory == "All" || item.Category == SelectedCategory;
            var matchesSearch = string.IsNullOrWhiteSpace(SearchText)
                || item.Name.Contains(SearchText, System.StringComparison.OrdinalIgnoreCase)
                || item.Sku.Contains(SearchText, System.StringComparison.OrdinalIgnoreCase);

            return matchesCategory && matchesSearch;
        }

        private void Adjust(StockItem item, int delta)
        {
            item.Quantity = System.Math.Max(0, item.Quantity + delta);
            OnPropertyChanged(nameof(LowStockCount));
            OnPropertyChanged(nameof(TotalStockValue));
        }

        public override void OnNavigatedTo()
        {
            OnPropertyChanged(nameof(TotalItems));
            OnPropertyChanged(nameof(LowStockCount));
            OnPropertyChanged(nameof(TotalStockValue));
        }
    }
}
