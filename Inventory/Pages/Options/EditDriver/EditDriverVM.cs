using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

using DataBase.Data;
using DataBase.Model.EntitiesInventory;

namespace Inventory.Pages.Options.EditDriver
{
    [QueryProperty(nameof(Driver), nameof(Driver))]
    public partial class EditDriverVM : ObservableObject
    {
        [ObservableProperty]
        Driver driver;

        readonly AccessDataBase _db;
        public EditDriverVM(AccessDataBase db)
        {
            Driver = new();
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

                var entities = Driver;
                entities.Updated = DateTime.Now;

                await _db.DataBaseAsync.UpdateAsync(entities);
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
