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


        readonly ICreatedDataBase _createdDataBase;
        readonly AccessDataBase _db;
        public UpdateDataBaseVM(ICreatedDataBase createdDataBase, AccessDataBase db)
        {
            _createdDataBase = createdDataBase;
            UpdateDataBaseM ??= new();
            _db = db;
        }

        public async Task Update()
        {
            try
            {
                UpdateDataBaseM.FromVersion = _createdDataBase.GetCurrentVersion();
                UpdateDataBaseM.UppdateVersion = UpdateDataBaseM.FromVersion;
                var result = await _createdDataBase.UpdateDataBase(UpdateDataBase, UpdateInventory, UpdateDriverRoutes);

                if (result)
                {
                    UpdateDataBaseM.BackIsVisible = true;
                }
            }
            catch (Exception ex)
            {
                _db.SaveLogExtension(ex);
            }
        }

        private void UpdateDataBase(double progressBar, int version)
        {
            UpdateDataBaseM.DataBaseProgresBar = progressBar;
            UpdateDataBaseM.UppdateVersion.DataBase = version;
        }
        private void UpdateInventory(double progressBar, int version)
        {
            UpdateDataBaseM.InventioryProgresBar = progressBar;
            UpdateDataBaseM.UppdateVersion.Inventory = version;
        }
        private void UpdateDriverRoutes(double progressBar, int version)
        {
            UpdateDataBaseM.DriverRoutesProgresBar = progressBar;
            UpdateDataBaseM.UppdateVersion.DriversRoutes = version;
        }

        [RelayCommand]
        async static Task Back()
        {
            await Shell.Current.GoToAsync("..");
        }


    }
}
