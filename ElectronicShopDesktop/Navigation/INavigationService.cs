using System;
using ElectronicShop.App.ViewModels.Base;

namespace ElectronicShop.App.Navigation
{
    public interface INavigationService
    {
        ViewModelBase CurrentViewModel { get; }
        event Action CurrentViewModelChanged;
        void NavigateTo(Type viewModelType);
        void NavigateTo<T>() where T : ViewModelBase;
    }
}