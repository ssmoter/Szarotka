using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

using Inventory.Helper.Parse;

namespace Inventory.Pages.Options.EditDriver
{
    [QueryProperty(nameof(Driver), nameof(Model.MVVM.DriverM))]
    public partial class EditDriverVM : ObservableObject
    {
        [ObservableProperty]
        Inventory.Model.MVVM.DriverM driver;

        readonly DataBase.Data.AccessDataBase _db;
        public EditDriverVM(DataBase.Data.AccessDataBase db)
        {
            Driver = new Model.MVVM.DriverM();
            _db = db;
        }



        #region Command

        [RelayCommand]
        async Task Save()
        {
            try
            {
                if (Driver is null)
                {
                    return;
                }

                await _db.DataBaseAsync.UpdateAsync(Driver.PareseAsDriver());
            }
            catch (Exception ex)
            {
                _db.SaveLog(ex);
            }

        }

        [RelayCommand]
        static async Task Back()
        {
            await Shell.Current.GoToAsync("..");
        }

        #endregion
    }
}
