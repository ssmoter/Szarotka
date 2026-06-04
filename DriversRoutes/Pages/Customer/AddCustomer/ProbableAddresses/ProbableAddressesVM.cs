using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

using DataBase.Model.EntitiesRoutes;


namespace DriversRoutes.Pages.Customer.AddCustomer.ProbableAddresses
{
    public partial class ProbableAddressesVM : ObservableObject
    {
        private ProbableAddressesM probableAddressesM;
        public ProbableAddressesM ProbableAddressesM
        {
            get => probableAddressesM;
            set
            {
                if (SetProperty(ref probableAddressesM, value, nameof(ProbableAddressesM))) { }
            }
        }

        public Func<object, CancellationToken, Task> Close;
        public Task OnClose(object result = null, CancellationToken token = default)
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
        async Task CancelAndReturn()
        {
            await OnClose(null);
        }

        #endregion

    }
}
