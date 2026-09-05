using System;
using System.Windows;
using ElectronicShop.App.ViewModels;
using ElectronicShop.App.Views;

namespace ElectronicShop.App.Navigation
{
    public class DialogService : IDialogService
    {
        public bool? ShowDialog<TViewModel>(TViewModel viewModel) where TViewModel : class
        {
            Window window = viewModel switch
            {
                AddProductViewModel => new AddProductWindow { DataContext = viewModel },
                AddCustomerViewModel => new AddCustomerWindow { DataContext = viewModel },
                AddServiceTicketViewModel => new AddServiceTicketWindow { DataContext = viewModel },
                _ => throw new InvalidOperationException($"No dialog registered for {typeof(TViewModel).Name}")
            };

            window.Owner = Application.Current.MainWindow;
            return window.ShowDialog();
        }
    }
}