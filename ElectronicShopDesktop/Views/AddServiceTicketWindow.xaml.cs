using ElectronicShop.App.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace ElectronicShop.App.Views
{
    /// <summary>
    /// Interaction logic for AddServiceTicketWindow.xaml
    /// </summary>
    public partial class AddServiceTicketWindow : Window
    {
        public AddServiceTicketWindow()
        {
            InitializeComponent();
            Loaded += (_, _) =>
            {
                if (DataContext is AddServiceTicketViewModel vm)
                    vm.RequestClose += () => DialogResult = vm.DialogResult;
            };
        }
    }
}
