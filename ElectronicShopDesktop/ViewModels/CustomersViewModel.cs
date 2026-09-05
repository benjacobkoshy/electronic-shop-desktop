using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Data;
using System.Windows.Input;
using ElectronicShop.App.Navigation;
using ElectronicShop.App.ViewModels.Base;
using ElectronicShop.Core.Models;
using ElectronicShop.Core.Repositories;

namespace ElectronicShop.App.ViewModels
{
    public class CustomersViewModel : ViewModelBase
    {
        private readonly ICustomerRepository _customerRepository;
        private readonly IDialogService _dialogService;

        public ObservableCollection<CustomerViewModel> AllCustomers { get; } = new();
        public ICollectionView CustomersView { get; }

        public ObservableCollection<ServiceTicketViewModel> SelectedCustomerTickets { get; } = new();

        public static readonly string[] StatusOptions = { "InProgress", "WaitingForStock", "Resolved" };

        private string _searchText = string.Empty;
        public string SearchText
        {
            get => _searchText;
            set { SetProperty(ref _searchText, value); CustomersView.Refresh(); }
        }

        private CustomerViewModel? _selectedCustomer;
        public CustomerViewModel? SelectedCustomer
        {
            get => _selectedCustomer;
            set
            {
                if (SetProperty(ref _selectedCustomer, value))
                    _ = LoadTicketsForSelectedCustomerAsync();
            }
        }

        private bool _isLoading;
        public bool IsLoading
        {
            get => _isLoading;
            set => SetProperty(ref _isLoading, value);
        }

        public ICommand AddCustomerCommand { get; }
        public ICommand AddTicketCommand { get; }
        public ICommand UpdateTicketStatusCommand { get; }

        public CustomersViewModel(ICustomerRepository customerRepository, IDialogService dialogService)
        {
            _customerRepository = customerRepository;
            _dialogService = dialogService;

            CustomersView = CollectionViewSource.GetDefaultView(AllCustomers);
            CustomersView.Filter = FilterCustomer;

            AddCustomerCommand = new RelayCommand(_ => OpenAddCustomerDialog());
            AddTicketCommand = new RelayCommand(_ => OpenAddTicketDialog(), _ => SelectedCustomer != null);
            UpdateTicketStatusCommand = new RelayCommand(async param => await UpdateTicketStatus(param));
        }

        public override void OnNavigatedTo() => _ = LoadCustomersAsync();

        private async Task LoadCustomersAsync()
        {
            IsLoading = true;
            try
            {
                var customers = await _customerRepository.GetAllAsync();
                var previouslySelectedId = SelectedCustomer?.Id;

                AllCustomers.Clear();
                foreach (var c in customers)
                    AllCustomers.Add(new CustomerViewModel(c));

                SelectedCustomer = previouslySelectedId.HasValue
                    ? AllCustomers.FirstOrDefault(c => c.Id == previouslySelectedId.Value)
                    : AllCustomers.FirstOrDefault();
            }
            finally
            {
                IsLoading = false;
            }
        }

        private async Task LoadTicketsForSelectedCustomerAsync()
        {
            SelectedCustomerTickets.Clear();
            (AddTicketCommand as RelayCommand)?.RaiseCanExecuteChanged();

            if (SelectedCustomer == null) return;

            var tickets = await _customerRepository.GetTicketsByCustomerAsync(SelectedCustomer.Id);
            foreach (var t in tickets.OrderByDescending(t => t.ReceivedDate))
            {
                var vm = new ServiceTicketViewModel(t);
                vm.SyncFromModel();
                SelectedCustomerTickets.Add(vm);
            }
        }

        private bool FilterCustomer(object obj)
        {
            if (obj is not CustomerViewModel c) return false;
            if (string.IsNullOrWhiteSpace(SearchText)) return true;

            return c.Name.Contains(SearchText, StringComparison.OrdinalIgnoreCase)
                || c.Phone.Contains(SearchText, StringComparison.OrdinalIgnoreCase);
        }

        private void OpenAddCustomerDialog()
        {
            var dialogVm = new AddCustomerViewModel(_customerRepository);
            var result = _dialogService.ShowDialog(dialogVm);
            if (result == true) _ = LoadCustomersAsync();
        }

        private void OpenAddTicketDialog()
        {
            if (SelectedCustomer == null) return;

            var dialogVm = new AddServiceTicketViewModel(_customerRepository, SelectedCustomer.Id);
            var result = _dialogService.ShowDialog(dialogVm);
            if (result == true) _ = LoadTicketsForSelectedCustomerAsync();
        }

        private async Task UpdateTicketStatus(object? param)
        {
            if (param is not object[] args || args.Length != 2) return;
            if (args[0] is not ServiceTicketViewModel ticket || args[1] is not string newStatus) return;

            await _customerRepository.UpdateTicketStatusAsync(ticket.Id, newStatus);
            ticket.Status = newStatus;
        }
    }
}