using ElectronicShop.App.ViewModels.Base;
using CoreModels = ElectronicShop.Core.Models;

namespace ElectronicShop.App.ViewModels
{
    public class ServiceTicketViewModel : ViewModelBase
    {
        private readonly CoreModels.ServiceTicket _model;

        public ServiceTicketViewModel(CoreModels.ServiceTicket model) => _model = model;

        public int Id => _model.Id;
        public string DeviceDescription => _model.DeviceDescription;
        public string ReceivedDate => _model.ReceivedDate;
        public string? Notes => _model.Notes;

        private string _status = null!;
        public string Status
        {
            get => _status;
            set => SetProperty(ref _status, value);
        }

        public void SyncFromModel() => Status = _model.Status;

        public CoreModels.ServiceTicket ToModel() => _model;
    }
}