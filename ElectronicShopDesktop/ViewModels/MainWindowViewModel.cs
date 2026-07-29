using ElectronicShop.App.Navigation;
using ElectronicShop.App.ViewModels.Base;
using System.Collections.ObjectModel;
using System.Windows.Input;

namespace ElectronicShop.App.ViewModels
{
    public class MainWindowViewModel : ViewModelBase
    {
        private readonly INavigationService _navigation;

        public ObservableCollection<NavigationItem> NavigationItems { get; }

        public ViewModelBase CurrentViewModel => _navigation.CurrentViewModel;

        public string CurrentPageTitle =>
            NavigationItems.FirstOrDefault(n => n.IsSelected)?.Title ?? string.Empty;

        public ICommand NavigateCommand { get; }
        public ICommand ToggleThemeCommand { get; }

        public MainWindowViewModel(INavigationService navigation)
        {
            _navigation = navigation;
            _navigation.CurrentViewModelChanged += OnCurrentViewModelChanged;

            NavigationItems = new ObservableCollection<NavigationItem>
            {
                new(Localisation.Strings.ES_LABEL_DASHBOARD, typeof(DashboardViewModel)),
                new(Localisation.Strings.ES_LABEL_INVENTORY, typeof(InventoryViewModel)),
                new(Localisation.Strings.ES_LABEL_BILLING, typeof(BillingViewModel)),
                new(Localisation.Strings.ES_LABEL_CUSTOMERS, typeof(CustomersViewModel)),
                new(Localisation.Strings.ES_LABEL_SETTINGS, typeof(SettingsViewModel)),
            };

            NavigateCommand = new RelayCommand(item => Navigate((NavigationItem)item));
            ToggleThemeCommand = new RelayCommand(_ => ThemeManager.Toggle());

            Navigate(NavigationItems[0]);
        }

        private void Navigate(NavigationItem item)
        {
            foreach (var navItem in NavigationItems)
                navItem.IsSelected = navItem == item;

            _navigation.NavigateTo(item.ViewModelType);
            OnPropertyChanged(nameof(CurrentPageTitle));
        }

        private void OnCurrentViewModelChanged()
            => OnPropertyChanged(nameof(CurrentViewModel));
    }
}