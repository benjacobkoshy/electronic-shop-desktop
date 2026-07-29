using ElectronicShop.App.ViewModels.Base;
using ElectronicShop.Core.Models.Dashboard;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElectronicShop.App.ViewModels
{
    public class DashboardViewModel : ViewModelBase
    {
        public string WelcomeMessage => $"Welcome back — here's what's happening today";
        public string TodayDate => DateTime.Now.ToString("dddd, dd MMMM yyyy");

        public ObservableCollection<DashboardStat> Stats { get; }
        public ObservableCollection<RecentBillSummary> RecentBills { get; }
        public ObservableCollection<LowStockAlert> LowStockAlerts { get; }
        public ObservableCollection<CustomerStatusSummary> CustomerStatusBreakdown { get; }

        public DashboardViewModel()
        {
            Stats = new ObservableCollection<DashboardStat>
            {
                new() { Label = "Today's Sales", Value = "₹ 12,450", Icon = "💰", AccentBrushKey = "PrimaryBrush" },
                new() { Label = "Bills Today", Value = "18", Icon = "🧾", AccentBrushKey = "AccentBrush" },
                new() { Label = "Low Stock Items", Value = "5", Icon = "⚠️", AccentBrushKey = "DangerBrush" },
                new() { Label = "Active Customers", Value = "9", Icon = "👥", AccentBrushKey = "PrimaryBrush" },
            };

            RecentBills = new ObservableCollection<RecentBillSummary>
            {
                new() { BillNumber = "INV-1042", CustomerName = "Ravi Kumar", Amount = 1250, Date = DateTime.Today, Status = "Paid" },
                new() { BillNumber = "INV-1041", CustomerName = "Anjali Menon", Amount = 3400, Date = DateTime.Today, Status = "Pending" },
                new() { BillNumber = "INV-1040", CustomerName = "Suresh Nair", Amount = 800, Date = DateTime.Today.AddDays(-1), Status = "Paid" },
                new() { BillNumber = "INV-1039", CustomerName = "Priya Das", Amount = 2100, Date = DateTime.Today.AddDays(-1), Status = "Paid" },
            };

            LowStockAlerts = new ObservableCollection<LowStockAlert>
            {
                new() { ProductName = "Capacitor 470µF", RemainingQty = 4, ThresholdQty = 10 },
                new() { ProductName = "IC LM7805", RemainingQty = 2, ThresholdQty = 15 },
                new() { ProductName = "USB-C Connector", RemainingQty = 6, ThresholdQty = 20 },
            };

            CustomerStatusBreakdown = new ObservableCollection<CustomerStatusSummary>
            {
                new() { Status = "In Progress", Count = 6, BrushKey = "PrimaryBrush" },
                new() { Status = "Waiting for Stock", Count = 3, BrushKey = "DangerBrush" },
                new() { Status = "Resolved", Count = 14, BrushKey = "AccentBrush" },
            };
        }

        public override void OnNavigatedTo()
        {
            //base.OnNavigatedTo();
        }
    }
}