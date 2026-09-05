using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using ElectronicShop.Core.Models.Dashboard;
using ElectronicShop.App.ViewModels.Base;
using ElectronicShop.Core.Repositories;

namespace ElectronicShop.App.ViewModels
{
    public class DashboardViewModel : ViewModelBase
    {
        private readonly IStockRepository _stockRepository;
        private readonly IBillingRepository _billingRepository;
        private readonly ICustomerRepository _customerRepository;

        public string WelcomeMessage => "Welcome back — here's what's happening today";
        public string TodayDate => DateTime.Now.ToString("dddd, dd MMMM yyyy");

        public ObservableCollection<DashboardStat> Stats { get; } = new();
        public ObservableCollection<RecentBillSummary> RecentBills { get; } = new();
        public ObservableCollection<LowStockAlert> LowStockAlerts { get; } = new();
        public ObservableCollection<CustomerStatusSummary> CustomerStatusBreakdown { get; } = new();

        private bool _isLoading;
        public bool IsLoading
        {
            get => _isLoading;
            set => SetProperty(ref _isLoading, value);
        }

        public DashboardViewModel(
            IStockRepository stockRepository,
            IBillingRepository billingRepository,
            ICustomerRepository customerRepository)
        {
            _stockRepository = stockRepository;
            _billingRepository = billingRepository;
            _customerRepository = customerRepository;
        }

        public override void OnNavigatedTo() => _ = LoadAsync();

        private async Task LoadAsync()
        {
            IsLoading = true;
            try
            {
                var products = await _stockRepository.GetAllAsync();
                var lowStock = await _stockRepository.GetLowStockAsync();
                var recentBills = await _billingRepository.GetRecentAsync(4);
                var tickets = await _customerRepository.GetAllTicketsAsync();

                var todaysBills = recentBills.Where(b => DateTime.Parse(b.BillDate).Date == DateTime.Today).ToList();
                var todaysSales = todaysBills.Sum(b => b.TotalAmount);
                var activeCustomerCount = tickets.Count(t => t.Status != "Resolved");

                Stats.Clear();
                Stats.Add(new DashboardStat { Label = "Today's Sales", Value = $"₹ {todaysSales:N0}", Icon = "💰", AccentBrushKey = "PrimaryBrush" });
                Stats.Add(new DashboardStat { Label = "Bills Today", Value = todaysBills.Count.ToString(), Icon = "🧾", AccentBrushKey = "AccentBrush" });
                Stats.Add(new DashboardStat { Label = "Low Stock Items", Value = lowStock.Count.ToString(), Icon = "⚠️", AccentBrushKey = "DangerBrush" });
                Stats.Add(new DashboardStat { Label = "Active Customers", Value = activeCustomerCount.ToString(), Icon = "👥", AccentBrushKey = "PrimaryBrush" });

                RecentBills.Clear();
                foreach (var b in recentBills)
                {
                    RecentBills.Add(new RecentBillSummary
                    {
                        BillNumber = b.BillNumber,
                        CustomerName = b.Customer?.Name ?? "Walk-in",
                        Amount = b.TotalAmount,
                        Date = DateTime.Parse(b.BillDate),
                        Status = b.PaymentStatus
                    });
                }

                LowStockAlerts.Clear();
                foreach (var p in lowStock)
                {
                    LowStockAlerts.Add(new LowStockAlert
                    {
                        ProductName = p.Name,
                        RemainingQty = p.Quantity,
                        ThresholdQty = p.LowStockThreshold
                    });
                }

                CustomerStatusBreakdown.Clear();
                foreach (var group in tickets.GroupBy(t => t.Status))
                {
                    CustomerStatusBreakdown.Add(new CustomerStatusSummary
                    {
                        Status = group.Key,
                        Count = group.Count(),
                        BrushKey = group.Key switch
                        {
                            "Resolved" => "AccentBrush",
                            "WaitingForStock" => "DangerBrush",
                            _ => "PrimaryBrush"
                        }
                    });
                }
            }
            finally
            {
                IsLoading = false;
            }
        }
    }
}