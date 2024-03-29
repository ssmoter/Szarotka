using CommunityToolkit.Maui.Views;

using DataBase.Model.EntitiesRoutes;

namespace DriversRoutes.Pages.Customer.AddCustomer.ProbableAddresses;

public partial class ProbableAddressesV : Popup
{
    public ProbableAddressesV(ResidentialAddress[] residentialAddresses)
    {
        InitializeComponent();
        ProbableAddressesVM vm = new();
        vm.Close += CloseAsync;
        vm.ProbableAddressesM.ResidentialAddresses =
            new System.Collections.ObjectModel.ObservableCollection<ResidentialAddress>(residentialAddresses);
        BindingContext = vm;
    }
}