using System;
using ElectronicShop.App.ViewModels.Base;

namespace ElectronicShop.App.Navigation
{
    public class NavigationItem : ViewModelBase
    {
        public string Title { get; }
        public Type ViewModelType { get; }

        private bool _isSelected;
        public bool IsSelected
        {
            get => _isSelected;
            set => SetProperty(ref _isSelected, value);
        }

        public NavigationItem(string title, Type viewModelType)
        {
            Title = title;
            ViewModelType = viewModelType;
        }
    }
}