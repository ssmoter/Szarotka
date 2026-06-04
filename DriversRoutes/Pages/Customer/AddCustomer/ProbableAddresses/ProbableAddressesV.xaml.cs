using CommunityToolkit.Maui.Extensions;
using CommunityToolkit.Maui.Views;

using DataBase.Model.EntitiesRoutes;

namespace DriversRoutes.Pages.Customer.AddCustomer.ProbableAddresses;

public partial class ProbableAddressesV : Popup
{
    public ProbableAddressesV(ResidentialAddress[] residentialAddresses)
    {
        InitializeComponent();
        ProbableAddressesVM vm = new();
        vm.Close += OnVmClose;
        vm.ProbableAddressesM.ResidentialAddresses =
            [.. residentialAddresses];
        BindingContext = vm;
    }

    private Task OnVmClose(object result, CancellationToken token)
    {
        return Shell.Current.ClosePopupAsync(result, token);
        //return CloseAsync(result);
    }
}