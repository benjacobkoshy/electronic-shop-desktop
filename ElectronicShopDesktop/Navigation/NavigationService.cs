using System;
using ElectronicShop.App.ViewModels.Base;

namespace ElectronicShop.App.Navigation
{
    public class NavigationService : INavigationService
    {
        private readonly Func<Type, ViewModelBase> _viewModelFactory;
        private ViewModelBase _currentViewModel;

        public NavigationService(Func<Type, ViewModelBase> viewModelFactory)
        {
            _viewModelFactory = viewModelFactory;
        }

        public ViewModelBase CurrentViewModel
        {
            get => _currentViewModel;
            private set
            {
                if (_currentViewModel == value) return;
                _currentViewModel = value;
                CurrentViewModelChanged?.Invoke();
                _currentViewModel?.OnNavigatedTo();
            }
        }

        public event Action CurrentViewModelChanged;

        public void NavigateTo(Type viewModelType)
            => CurrentViewModel = _viewModelFactory(viewModelType);

        public void NavigateTo<T>() where T : ViewModelBase
            => NavigateTo(typeof(T));
    }
}