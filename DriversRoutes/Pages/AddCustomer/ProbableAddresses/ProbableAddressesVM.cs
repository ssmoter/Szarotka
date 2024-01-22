using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

using DriversRoutes.Model;



namespace DriversRoutes.Pages.AddCustomer.ProbableAddresses
{
    public partial class ProbableAddressesVM : ObservableObject
    {
        [ObservableProperty]
        ProbableAddressesM probableAddressesM;

        public Func<object, Task> Close;
        public Task OnClose(object result = null)
        {
            return Close?.Invoke(result);
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
