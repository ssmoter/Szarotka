using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

using DataBase.Data;

using Shared.Service;

namespace Shared.Pages.UpdateDataBase
{
    public partial class UpdateDataBaseVM : ObservableObject
    {

        [ObservableProperty]
        UpdateDataBaseM updateDataBaseM;


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
                var resutl = await _createdDataBase.UpdateDataBase(UppdateDataBase, UppdateInventory, UppdateDriverRoutes);

                if (resutl)
                {
                    UpdateDataBaseM.BackIsVisible = true;
                }
            }
            catch (Exception ex)
            {
                _db.SaveLog(ex);
            }
        }

        private void UppdateDataBase(double progressBar, int version)
        {
            UpdateDataBaseM.DataBaseProgresBar = progressBar;
            UpdateDataBaseM.UppdateVersion.DataBase = version;
        }
        private void UppdateInventory(double progressBar, int version)
        {
            UpdateDataBaseM.InventioryProgresBar = progressBar;
            UpdateDataBaseM.UppdateVersion.Inventory = version;
        }
        private void UppdateDriverRoutes(double progressBar, int version)
        {
            UpdateDataBaseM.DriverRoutesProgresBar = progressBar;
            UpdateDataBaseM.UppdateVersion.DriversRoutes = version;
        }

        [RelayCommand]
        async Task Back()
        {
            await Shell.Current.GoToAsync("..");
        }


    }
}
