using ElectronicShop.App.ViewModels.Base;
using CoreModels = ElectronicShop.Core.Models;

namespace ElectronicShop.App.ViewModels
{
    public class CustomerViewModel : ViewModelBase
    {
        private readonly CoreModels.Customer _model;

        public CustomerViewModel(CoreModels.Customer model) => _model = model;

        public int Id => _model.Id;
        public string Name => _model.Name;
        public string Phone => _model.Phone;
        public string? Email => _model.Email;
        public string? Address => _model.Address;

        public CoreModels.Customer ToModel() => _model;
    }
}