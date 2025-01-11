using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

using DataBase.Data;
using DataBase.Model.EntitiesInventory;

using Shared.Data;

namespace Inventory.Pages.Options.EditDriver
{
    public partial class EditDriverVM : ObservableObject, IQueryAttributable
    {
        public void ApplyQueryAttributes(IDictionary<string, object> query)
        {
            if (query.TryGetValue(nameof(Driver), out object driver))
            {
                if (driver is Driver _driver)
                {
                    Driver = _driver;
                }
            }

        }

        private Driver driver;
        public Driver Driver
        {
            get => driver;
            set
            {
                if (SetProperty(ref driver, value, nameof(Driver))) { }
            }
        }

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
                _db.SaveLogExtension(ex);
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
