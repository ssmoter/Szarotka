using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

using DataBase.Service;

namespace DataBase.Pages.UpdateDataBase
{
    public partial class UpdateDataBaseVM : ObservableObject
    {

        [ObservableProperty]
        UpdateDataBaseM updateDataBaseM;


        readonly ICreatedDataBase _createdDataBase;
        public UpdateDataBaseVM(ICreatedDataBase createdDataBase)
        {
            _createdDataBase = createdDataBase;
            UpdateDataBaseM ??= new();
        }

        public async Task Update()
        {
            UpdateDataBaseM.FromVersion = _createdDataBase.GetCurrentVersion();
            var resutl = await _createdDataBase.UpdateDataBase(UppdateDataBase, UppdateInventory, UppdateDriverRoutes);

            if (resutl)
            {
                UpdateDataBaseM.BackIsVisible= true;
            }
        }

        private void UppdateDataBase(double progressBar, int version)
        {
            UpdateDataBaseM.DataBaseProgresBar = progressBar;
            UpdateDataBaseM.FromVersion.DataBase = version;
        }
        private void UppdateInventory(double progressBar, int version)
        {
            UpdateDataBaseM.InventioryProgresBar = progressBar;
            UpdateDataBaseM.FromVersion.Inventory = version;
        }
        private void UppdateDriverRoutes(double progressBar, int version)
        {
            UpdateDataBaseM.DriverRoutesProgresBar = progressBar;
            UpdateDataBaseM.FromVersion.DriversRoutes = version;
        }

        [RelayCommand]
        async Task Back()
        {
            await Shell.Current.GoToAsync("..");
        }


    }
}
