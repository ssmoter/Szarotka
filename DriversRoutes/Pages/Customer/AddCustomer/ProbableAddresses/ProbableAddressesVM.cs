using CommunityToolkit.Maui;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

using DataBase.Model.EntitiesRoutes;


namespace DriversRoutes.Pages.Customer.AddCustomer.ProbableAddresses
{
    public partial class ProbableAddressesVM : ObservableObject, IQueryAttributable
    {
        public void ApplyQueryAttributes(IDictionary<string, object> query)
        {
            if (query.TryGetValue(nameof(ResidentialAddress), out object address))
            {
                if (address is ResidentialAddress[] _address)
                {
                    ProbableAddressesM.ResidentialAddresses = [.._address];
                }
            }
        }

        private ProbableAddressesM probableAddressesM;
        public ProbableAddressesM ProbableAddressesM
        {
            get => probableAddressesM;
            set
            {
                if (SetProperty(ref probableAddressesM, value, nameof(ProbableAddressesM))) { }
            }
        }

        private readonly IPopupService _popupService;
        public ProbableAddressesVM(IPopupService popupService)
        {
            ProbableAddressesM ??= new ProbableAddressesM();
            _popupService = popupService;
        }



        [RelayCommand]
        async Task SaveAndReturn(ResidentialAddress address)
        {
            await _popupService.ClosePopupAsync<ResidentialAddress>(page: Shell.Current, result: address);

        }
        [RelayCommand]
        async Task CancelAndReturn()
        {
            await _popupService.ClosePopupAsync(page: Shell.Current);
        }


    }
}
