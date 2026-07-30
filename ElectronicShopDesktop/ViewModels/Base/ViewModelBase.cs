using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace ElectronicShop.App.ViewModels.Base
{
    public abstract class ViewModelBase : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler? PropertyChanged;

        protected void OnPropertyChanged([CallerMemberName] string? propertyName = null)
            => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));

        protected bool SetProperty<T>(ref T field, T value, [CallerMemberName] string? propertyName = null)
        {
            if (Equals(field, value)) return false;
            field = value;
            OnPropertyChanged(propertyName);
            return true;
        }

        // Called by NavigationService right after this ViewModel becomes the active page.
        // Override in page ViewModels to load data (e.g. InventoryViewModel loads stock list here).
        public virtual void OnNavigatedTo() { }
    }
}