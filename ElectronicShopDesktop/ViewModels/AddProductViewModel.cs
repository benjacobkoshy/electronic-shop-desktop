using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using ElectronicShop.App.ViewModels.Base;
using ElectronicShop.Core.Models;
using ElectronicShop.Core.Repositories;

namespace ElectronicShop.App.ViewModels
{
    public class AddProductViewModel : ViewModelBase
    {
        private readonly IStockRepository _stockRepository;

        private string _newCategoryName = string.Empty;
        public string NewCategoryName
        {
            get => _newCategoryName;
            set { SetProperty(ref _newCategoryName, value); (AddCategoryCommand as RelayCommand)?.RaiseCanExecuteChanged(); }
        }
        public ObservableCollection<Category> Categories { get; } = new();

        private string _name = string.Empty;
        public string Name { get => _name; set { SetProperty(ref _name, value); RaiseCanSave(); } }

        private string _sku = string.Empty;
        public string Sku { get => _sku; set { SetProperty(ref _sku, value); RaiseCanSave(); } }

        private Category? _selectedCategory;
        public Category? SelectedCategory { get => _selectedCategory; set => SetProperty(ref _selectedCategory, value); }


        private decimal _unitPrice;
        public decimal UnitPrice { get => _unitPrice; set { SetProperty(ref _unitPrice, value); RaiseCanSave(); } }

        private int _quantity;
        public int Quantity { get => _quantity; set => SetProperty(ref _quantity, value); }

        private int _lowStockThreshold = 5;
        public int LowStockThreshold { get => _lowStockThreshold; set => SetProperty(ref _lowStockThreshold, value); }

        private string? _errorMessage;
        public string? ErrorMessage { get => _errorMessage; set => SetProperty(ref _errorMessage, value); }

        public bool? DialogResult { get; private set; }

        public ICommand SaveCommand { get; }
        public ICommand CancelCommand { get; }
        public ICommand AddCategoryCommand { get; }
        public event Action? RequestClose;

        public AddProductViewModel(IStockRepository stockRepository)
        {
            _stockRepository = stockRepository;
            SaveCommand = new RelayCommand(async _ => await SaveAsync(), _ => CanSave());
            CancelCommand = new RelayCommand(_ => { DialogResult = false; RequestClose?.Invoke(); });
            AddCategoryCommand = new RelayCommand(async _ => await AddCategoryAsync(), _ => !string.IsNullOrWhiteSpace(NewCategoryName));

            _ = LoadCategoriesAsync();
        }

        private async Task LoadCategoriesAsync()
        {
            var categories = await _stockRepository.GetCategoriesAsync();
            Categories.Clear();
            foreach (var c in categories) Categories.Add(c);
        }

        private bool CanSave() =>
            !string.IsNullOrWhiteSpace(Name) &&
            !string.IsNullOrWhiteSpace(Sku) &&
            UnitPrice >= 0;

        private void RaiseCanSave() => (SaveCommand as RelayCommand)?.RaiseCanExecuteChanged();

        private async Task SaveAsync()
        {
            ErrorMessage = null;
            try
            {
                var product = new Product
                {
                    Name = Name.Trim(),
                    Sku = Sku.Trim(),
                    CategoryId = SelectedCategory?.Id,
                    UnitPrice = UnitPrice,
                    Quantity = Quantity,
                    LowStockThreshold = LowStockThreshold
                };

                await _stockRepository.AddAsync(product);

                DialogResult = true;
                RequestClose?.Invoke();
            }
            catch (Exception ex)
            {
                ErrorMessage = $"Couldn't save product: {ex.Message}";
            }
        }

        private async Task AddCategoryAsync()
        {
            if (string.IsNullOrWhiteSpace(NewCategoryName)) return;

            var category = await _stockRepository.AddCategoryAsync(NewCategoryName.Trim());

            if (!Categories.Any(c => c.Id == category.Id))
                Categories.Add(category);

            SelectedCategory = category; // auto-select the newly created one
            NewCategoryName = string.Empty;
        }
    }
}