using System;
using System.Threading.Tasks;
using System.Windows.Input;
using ElectronicShop.App.ViewModels.Base;
using ElectronicShop.Core.Models;
using ElectronicShop.Core.Repositories;

namespace ElectronicShop.App.ViewModels
{
    public class AddServiceTicketViewModel : ViewModelBase
    {
        private readonly ICustomerRepository _customerRepository;
        private readonly int _customerId;

        private string _deviceDescription = string.Empty;
        public string DeviceDescription { get => _deviceDescription; set { SetProperty(ref _deviceDescription, value); RaiseCanSave(); } }

        private string _notes = string.Empty;
        public string Notes { get => _notes; set => SetProperty(ref _notes, value); }

        private string? _errorMessage;
        public string? ErrorMessage { get => _errorMessage; set => SetProperty(ref _errorMessage, value); }

        public bool? DialogResult { get; private set; }
        public event Action? RequestClose;

        public ICommand SaveCommand { get; }
        public ICommand CancelCommand { get; }

        public AddServiceTicketViewModel(ICustomerRepository customerRepository, int customerId)
        {
            _customerRepository = customerRepository;
            _customerId = customerId;
            SaveCommand = new RelayCommand(async _ => await SaveAsync(), _ => CanSave());
            CancelCommand = new RelayCommand(_ => { DialogResult = false; RequestClose?.Invoke(); });
        }

        private bool CanSave() => !string.IsNullOrWhiteSpace(DeviceDescription);
        private void RaiseCanSave() => (SaveCommand as RelayCommand)?.RaiseCanExecuteChanged();

        private async Task SaveAsync()
        {
            ErrorMessage = null;
            try
            {
                await _customerRepository.AddServiceTicketAsync(new ServiceTicket
                {
                    CustomerId = _customerId,
                    DeviceDescription = DeviceDescription.Trim(),
                    Status = "InProgress",
                    ReceivedDate = DateTime.UtcNow.ToString("o"),
                    Notes = string.IsNullOrWhiteSpace(Notes) ? null : Notes.Trim()
                });

                DialogResult = true;
                RequestClose?.Invoke();
            }
            catch (Exception ex)
            {
                ErrorMessage = $"Couldn't save ticket: {ex.Message}";
            }
        }
    }
}