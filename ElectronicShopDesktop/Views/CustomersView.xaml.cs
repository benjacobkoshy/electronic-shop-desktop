using ElectronicShop.App.ViewModels;
using System.Windows.Controls;
namespace ElectronicShop.App.Views
{
    /// <summary>
    /// Interaction logic for CustomersView.xaml
    /// </summary>
    public partial class CustomersView : UserControl
    {
        public CustomersView()
        {
            InitializeComponent();
        }

        private void StatusComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (sender is not ComboBox { SelectedItem: string newStatus } combo) return;
            if (combo.DataContext is not ServiceTicketViewModel ticket) return;
            if (DataContext is not CustomersViewModel vm) return;

            if (ticket.Status != newStatus)
                vm.UpdateTicketStatusCommand.Execute(new object[] { ticket, newStatus });
        }
    }
}