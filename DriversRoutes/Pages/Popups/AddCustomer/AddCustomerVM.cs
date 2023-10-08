using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace DriversRoutes.Pages.Popups.AddCustomer
{
    public partial class AddCustomerVM : ObservableObject, IDisposable
    {
        #region Variable


        [ObservableProperty]
        AddCustomerM addCustomer;


        bool setAll = false;
        #endregion

        public AddCustomerVM(Model.Customer customer)
        {
            AddCustomer = new AddCustomerM();
        }


        #region Command


        #endregion



        public void Dispose()
        {
            AddCustomer.Dispose();
        }
    }
}
