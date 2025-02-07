using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

using DataBase.Data;

using Shared.Data;
using Shared.Service;

namespace Shared.Pages.UpdateDataBase
{
    public partial class UpdateDataBaseVM : ObservableObject
    {
        UpdateDataBaseM updateDataBaseM;
        public UpdateDataBaseM UpdateDataBaseM
        {
            get => updateDataBaseM;
            set
            {
                if (SetProperty(ref updateDataBaseM, value))
                {
                    OnPropertyChanged(nameof(UpdateDataBaseM));
                }
            }
        }


        private readonly ICreatedDataBase _createdDataBase;
        public readonly IAccessDataBase _db;
        public UpdateDataBaseVM(ICreatedDataBase createdDataBase, IAccessDataBase db)
        {
            _createdDataBase = createdDataBase;
            UpdateDataBaseM ??= new();
            _db = db;
        }

        public async Task UpdateAsync()
        {
            try
            {
                UpdateDataBaseM.FromVersion = _createdDataBase.GetCurrentVersion();
                UpdateDataBaseM.UpdateVersion = UpdateDataBaseM.FromVersion;
                var result = await _createdDataBase.UpdateDataBase(UpdateDataBase, UpdateInventory, UpdateDriverRoutes);

                if (result)
                {
                    UpdateDataBaseM.BackIsVisible = true;
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        private void UpdateDataBase(double progressBar, int version)
        {
            UpdateDataBaseM.DataBaseProgressBar = progressBar;
            UpdateDataBaseM.UpdateVersion.DataBase = version;
        }
        private void UpdateInventory(double progressBar, int version)
        {
            UpdateDataBaseM.InventoryProgressBar = progressBar;
            UpdateDataBaseM.UpdateVersion.Inventory = version;
        }
        private void UpdateDriverRoutes(double progressBar, int version)
        {
            UpdateDataBaseM.DriverRoutesProgressBar = progressBar;
            UpdateDataBaseM.UpdateVersion.DriversRoutes = version;
        }

        [RelayCommand]
        async Task Update()
        {
            try
            {
                await UpdateAsync();
            }
            catch (Exception ex)
            {
                _db.SaveLogExtension(ex);
            }
        }

        [RelayCommand]
        async static Task Back()
        {
            await Shell.Current.GoToAsync("..");
        }


    }
}
