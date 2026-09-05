using ElectronicShop.App.Navigation;
using ElectronicShop.App.ViewModels.Base;
using ElectronicShop.Core.Repositories;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows.Data;
using System.Windows.Input;


namespace ElectronicShop.App.ViewModels
{
    public class InventoryViewModel : ViewModelBase
    {
        private readonly IStockRepository _stockRepository;

        public ObservableCollection<StockItemViewModel> AllItems { get; } = new();
        public ICollectionView ItemsView { get; }
        public ObservableCollection<string> Categories { get; } = new() { "All" };

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

        private bool _isLoading;
        public bool IsLoading
        {
            get => _isLoading;
            set => SetProperty(ref _isLoading, value);
        }

        public int TotalItems => AllItems.Count;
        public int LowStockCount => AllItems.Count(i => i.StockStatus is "Low Stock" or "Out of Stock");
        public decimal TotalStockValue => AllItems.Sum(i => i.Quantity * i.UnitPrice);

        private readonly IDialogService _dialogService;

        public ICommand IncreaseStockCommand { get; }
        public ICommand DecreaseStockCommand { get; }
        public ICommand AddProductCommand { get; }
        public ICommand DeleteProductCommand { get; }
        public ICommand SelectCategoryCommand { get; }

        public InventoryViewModel(IStockRepository stockRepository, IDialogService dialogService)
        {
            _stockRepository = stockRepository;
            _dialogService = dialogService;

            ItemsView = CollectionViewSource.GetDefaultView(AllItems);
            ItemsView.Filter = FilterItem;

            IncreaseStockCommand = new RelayCommand(async item => await Adjust((StockItemViewModel)item, +1));
            DecreaseStockCommand = new RelayCommand(async item => await Adjust((StockItemViewModel)item, -1),
                item => ((StockItemViewModel)item)?.Quantity > 0);
            AddProductCommand = new RelayCommand(_ => OpenAddProductDialog());
            DeleteProductCommand = new RelayCommand(async item => await DeleteProduct((StockItemViewModel)item));
            SelectCategoryCommand = new RelayCommand(cat => SelectedCategory = (string)cat);
        }

        public override void OnNavigatedTo() => _ = LoadAsync();

        private async Task LoadAsync()
        {
            IsLoading = true;
            try
            {
                var products = await _stockRepository.GetAllAsync();

                AllItems.Clear();
                foreach (var p in products)
                    AllItems.Add(new StockItemViewModel(p));

                Categories.Clear();
                Categories.Add("All");
                foreach (var cat in AllItems.Select(i => i.Category).Distinct().OrderBy(c => c))
                    Categories.Add(cat);

                RefreshStats();
            }
            finally
            {
                IsLoading = false;
            }
        }

        private bool FilterItem(object obj)
        {
            if (obj is not StockItemViewModel item) return false;

            var matchesCategory = SelectedCategory == "All" || item.Category == SelectedCategory;
            var matchesSearch = string.IsNullOrWhiteSpace(SearchText)
                || item.Name.Contains(SearchText, System.StringComparison.OrdinalIgnoreCase)
                || item.Sku.Contains(SearchText, System.StringComparison.OrdinalIgnoreCase);

            return matchesCategory && matchesSearch;
        }

        private async Task Adjust(StockItemViewModel item, int delta)
        {
            await _stockRepository.AdjustStockAsync(item.Id, delta, "ManualAdjustment");
            item.Quantity += delta; // reflect immediately in the UI without a full reload
            RefreshStats();
        }

        private async Task DeleteProduct(StockItemViewModel item)
        {
            await _stockRepository.DeleteAsync(item.Id);
            AllItems.Remove(item);
            RefreshStats();
        }

        private void RefreshStats()
        {
            OnPropertyChanged(nameof(TotalItems));
            OnPropertyChanged(nameof(LowStockCount));
            OnPropertyChanged(nameof(TotalStockValue));
        }

        private void OpenAddProductDialog()
        {
            var dialogVm = new AddProductViewModel(_stockRepository);
            var result = _dialogService.ShowDialog(dialogVm);

            if (result == true)
                _ = LoadAsync(); // refresh the grid with the newly added product
        }
    }
}