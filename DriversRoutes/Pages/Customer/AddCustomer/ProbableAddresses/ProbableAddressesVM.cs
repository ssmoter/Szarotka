using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

using DataBase.Model.EntitiesRoutes;


namespace DriversRoutes.Pages.Customer.AddCustomer.ProbableAddresses
{
    public partial class ProbableAddressesVM : ObservableObject
    {
        [ObservableProperty]
        ProbableAddressesM probableAddressesM;

        public Func<object, CancellationToken, Task> Close;
        public Task OnClose(object result = null, CancellationToken token = default(CancellationToken))
        {
            return Close?.Invoke(result, token);
        }
        public ProbableAddressesVM()
        {
            ProbableAddressesM ??= new ProbableAddressesM();
        }

        #region Command

        [RelayCommand]
        async Task SaveAndReturn(ResidentialAddress address)
        {
            await OnClose(address);
        }
        [RelayCommand]
        async Task CancelAndRetur()
        {
            await OnClose(null);
        }

        #endregion

    }
}
