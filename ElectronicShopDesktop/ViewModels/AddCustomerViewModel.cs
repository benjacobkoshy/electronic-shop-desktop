using System;
using System.Threading.Tasks;
using System.Windows.Input;
using ElectronicShop.App.ViewModels.Base;
using ElectronicShop.Core.Models;
using ElectronicShop.Core.Repositories;

namespace ElectronicShop.App.ViewModels
{
    public class AddCustomerViewModel : ViewModelBase
    {
        private readonly ICustomerRepository _customerRepository;

        private string _name = string.Empty;
        public string Name { get => _name; set { SetProperty(ref _name, value); RaiseCanSave(); } }

        private string _phone = string.Empty;
        public string Phone { get => _phone; set { SetProperty(ref _phone, value); RaiseCanSave(); } }

        private string _email = string.Empty;
        public string Email { get => _email; set => SetProperty(ref _email, value); }

        private string _address = string.Empty;
        public string Address { get => _address; set => SetProperty(ref _address, value); }

        private string? _errorMessage;
        public string? ErrorMessage { get => _errorMessage; set => SetProperty(ref _errorMessage, value); }

        public bool? DialogResult { get; private set; }
        public event Action? RequestClose;

        public ICommand SaveCommand { get; }
        public ICommand CancelCommand { get; }

        public AddCustomerViewModel(ICustomerRepository customerRepository)
        {
            _customerRepository = customerRepository;
            SaveCommand = new RelayCommand(async _ => await SaveAsync(), _ => CanSave());
            CancelCommand = new RelayCommand(_ => { DialogResult = false; RequestClose?.Invoke(); });
        }

        private bool CanSave() => !string.IsNullOrWhiteSpace(Name) && !string.IsNullOrWhiteSpace(Phone);
        private void RaiseCanSave() => (SaveCommand as RelayCommand)?.RaiseCanExecuteChanged();

        private async Task SaveAsync()
        {
            ErrorMessage = null;
            try
            {
                await _customerRepository.AddAsync(new Customer
                {
                    Name = Name.Trim(),
                    Phone = Phone.Trim(),
                    Email = string.IsNullOrWhiteSpace(Email) ? null : Email.Trim(),
                    Address = string.IsNullOrWhiteSpace(Address) ? null : Address.Trim()
                });

                DialogResult = true;
                RequestClose?.Invoke();
            }
            catch (Exception ex)
            {
                ErrorMessage = $"Couldn't save customer: {ex.Message}";
            }
        }
    }
}