using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using ElectronicShop.App.Models;
using ElectronicShop.App.ViewModels.Base;
using ElectronicShop.Core.Models;
using ElectronicShop.Core.Repositories;

namespace ElectronicShop.App.ViewModels
{
    public class BillingViewModel : ViewModelBase
    {
        private readonly IStockRepository _stockRepository;
        private readonly IBillingRepository _billingRepository;
        private readonly ICustomerRepository _customerRepository;

        public ObservableCollection<Customer> Customers { get; } = new();
        public ObservableCollection<ServiceTicket> CustomerTickets { get; } = new();
        public ObservableCollection<Product> SearchResults { get; } = new();
        public ObservableCollection<BillLineItemDraft> Cart { get; } = new();

        private Customer? _selectedCustomer;
        public Customer? SelectedCustomer
        {
            get => _selectedCustomer;
            set
            {
                if (SetProperty(ref _selectedCustomer, value))
                    _ = LoadTicketsForCustomerAsync();
            }
        }

        private ServiceTicket? _selectedTicket;
        public ServiceTicket? SelectedTicket
        {
            get => _selectedTicket;
            set => SetProperty(ref _selectedTicket, value);
        }

        private string _productSearchText = string.Empty;
        public string ProductSearchText
        {
            get => _productSearchText;
            set { SetProperty(ref _productSearchText, value); _ = SearchProductsAsync(); }
        }

        private decimal _discountAmount;
        public decimal DiscountAmount
        {
            get => _discountAmount;
            set { SetProperty(ref _discountAmount, value); RecalculateTotals(); }
        }

        private decimal _taxAmount;
        public decimal TaxAmount
        {
            get => _taxAmount;
            set { SetProperty(ref _taxAmount, value); RecalculateTotals(); }
        }

        private decimal _serviceCharge;
        public decimal ServiceCharge
        {
            get => _serviceCharge;
            set { SetProperty(ref _serviceCharge, value); RecalculateTotals(); }
        }

        public decimal Subtotal => Cart.Sum(i => i.LineTotal);
        public decimal TotalAmount => Subtotal - DiscountAmount + TaxAmount + ServiceCharge;

        private string _paymentStatus = "Paid";
        public string PaymentStatus
        {
            get => _paymentStatus;
            set => SetProperty(ref _paymentStatus, value);
        }

        private string? _errorMessage;
        public string? ErrorMessage
        {
            get => _errorMessage;
            set => SetProperty(ref _errorMessage, value);
        }

        private bool _isSaving;
        public bool IsSaving
        {
            get => _isSaving;
            set => SetProperty(ref _isSaving, value);
        }

        private string _customItemDescription = string.Empty;
        public string CustomItemDescription
        {
            get => _customItemDescription;
            set => SetProperty(ref _customItemDescription, value);
        }

        private decimal _customItemPrice;
        public decimal CustomItemPrice
        {
            get => _customItemPrice;
            set => SetProperty(ref _customItemPrice, value);
        }

        private int _customItemQuantity = 1;
        public int CustomItemQuantity
        {
            get => _customItemQuantity;
            set => SetProperty(ref _customItemQuantity, value);
        }

        public ICommand AddCustomItemCommand { get; }

        public ICommand AddProductToCartCommand { get; }
        public ICommand RemoveFromCartCommand { get; }
        public ICommand SaveBillCommand { get; }

        public BillingViewModel(
            IStockRepository stockRepository,
            IBillingRepository billingRepository,
            ICustomerRepository customerRepository)
        {
            _stockRepository = stockRepository;
            _billingRepository = billingRepository;
            _customerRepository = customerRepository;

            AddProductToCartCommand = new RelayCommand(p => AddToCart((Product)p));
            RemoveFromCartCommand = new RelayCommand(item => RemoveFromCart((BillLineItemDraft)item));
            SaveBillCommand = new RelayCommand(async _ => await SaveBillAsync(), _ => Cart.Any() && SelectedCustomer != null);
            AddCustomItemCommand = new RelayCommand(_ => AddCustomItem(), _ => !string.IsNullOrWhiteSpace(CustomItemDescription));

            Cart.CollectionChanged += (_, _) =>
            {
                OnPropertyChanged(nameof(Subtotal));
                OnPropertyChanged(nameof(TotalAmount));
                (SaveBillCommand as RelayCommand)?.RaiseCanExecuteChanged();
            };
        }

        public override void OnNavigatedTo() => _ = LoadCustomersAsync();

        private async Task LoadCustomersAsync()
        {
            var customers = await _customerRepository.GetAllAsync();
            Customers.Clear();
            foreach (var c in customers) Customers.Add(c);
        }

        private async Task LoadTicketsForCustomerAsync()
        {
            CustomerTickets.Clear();
            SelectedTicket = null;
            (SaveBillCommand as RelayCommand)?.RaiseCanExecuteChanged();

            if (SelectedCustomer == null) return;

            var tickets = await _customerRepository.GetTicketsByCustomerAsync(SelectedCustomer.Id);
            foreach (var t in tickets.Where(t => t.Status != "Resolved"))
                CustomerTickets.Add(t);
        }

        private async Task SearchProductsAsync()
        {
            SearchResults.Clear();
            if (string.IsNullOrWhiteSpace(ProductSearchText)) return;

            try
            {
                var all = await _stockRepository.GetAllAsync();
                var matches = all.Where(p =>
                    p.Name.Contains(ProductSearchText, StringComparison.OrdinalIgnoreCase) ||
                    p.Sku.Contains(ProductSearchText, StringComparison.OrdinalIgnoreCase))
                    .Take(8);

                foreach (var p in matches) SearchResults.Add(p);
            }
            catch (Exception ex)
            {
                ErrorMessage = $"Product search failed: {ex.Message}";
            }
        }

        private void AddToCart(Product product)
        {
            var existing = Cart.FirstOrDefault(c => c.ProductId == product.Id);
            if (existing != null)
            {
                existing.Quantity += 1;
            }
            else
            {
                Cart.Add(new BillLineItemDraft
                {
                    ProductId = product.Id,
                    Description = product.Name,
                    UnitPrice = product.UnitPrice,
                    AvailableStock = product.Quantity,
                    Quantity = 1
                });
            }

            ProductSearchText = string.Empty;
            SearchResults.Clear();
            RecalculateTotals();
        }

        private void RemoveFromCart(BillLineItemDraft item)
        {
            Cart.Remove(item);
            RecalculateTotals();
        }

        private void AddCustomItem()
        {
            if (string.IsNullOrWhiteSpace(CustomItemDescription)) return;

            Cart.Add(new BillLineItemDraft
            {
                ProductId = null,                 // not in inventory — nothing to deduct
                Description = CustomItemDescription.Trim(),
                UnitPrice = CustomItemPrice,
                Quantity = CustomItemQuantity,
                AvailableStock = int.MaxValue     // no stock ceiling check for non-inventory items
            });

            CustomItemDescription = string.Empty;
            CustomItemPrice = 0;
            CustomItemQuantity = 1;
            RecalculateTotals();
        }

        private void RecalculateTotals()
        {
            OnPropertyChanged(nameof(Subtotal));
            OnPropertyChanged(nameof(TotalAmount));
        }

        private async Task SaveBillAsync()
        {
            ErrorMessage = null;

            var overStock = Cart.FirstOrDefault(c => c.Quantity > c.AvailableStock);
            if (overStock != null)
            {
                ErrorMessage = $"Only {overStock.AvailableStock} units of '{overStock.Description}' available.";
                return;
            }

            IsSaving = true;
            try
            {
                var bill = new Bill
                {
                    BillNumber = await _billingRepository.GenerateNextBillNumberAsync(),
                    CustomerId = SelectedCustomer!.Id,
                    ServiceTicketId = SelectedTicket?.Id,
                    BillDate = DateTime.UtcNow.ToString("o"),
                    Subtotal = Subtotal,
                    DiscountAmount = DiscountAmount,
                    TaxAmount = TaxAmount,
                    TotalAmount = TotalAmount,
                    PaymentStatus = PaymentStatus,
                    CreatedAt = DateTime.UtcNow.ToString("o")
                };

                var lineItems = Cart.Select(c => new BillLineItem
                {
                    ProductId = c.ProductId,
                    Description = c.Description,
                    Quantity = c.Quantity,
                    UnitPrice = c.UnitPrice,
                    LineTotal = c.LineTotal
                }).ToList();

                if (ServiceCharge > 0)
                {
                    lineItems.Add(new BillLineItem
                    {
                        ProductId = null,
                        Description = "Service Charge",
                        Quantity = 1,
                        UnitPrice = ServiceCharge,
                        LineTotal = ServiceCharge
                    });
                }

                var savedBill = await _billingRepository.CreateBillAsync(bill, lineItems);

                // Reset for next bill
                Cart.Clear();
                DiscountAmount = 0;
                TaxAmount = 0;
                SelectedCustomer = null;
                SelectedTicket = null;

                // TODO: navigate to a Bill Preview/Print screen using savedBill.Id
            }
            catch (Exception ex)
            {
                ErrorMessage = $"Couldn't save bill: {ex.Message}";
            }
            finally
            {
                IsSaving = false;
            }
        }
    }
}