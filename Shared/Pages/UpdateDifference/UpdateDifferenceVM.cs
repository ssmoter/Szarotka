using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

using DataBase.Model.EntitiesServer;

using Shared.Data;

namespace Shared.Pages.UpdateDifference
{
    public partial class UpdateDifferenceVM : ObservableObject, IQueryAttributable
    {
        public void ApplyQueryAttributes(IDictionary<string, object> query)
        {
            if (query.TryGetValue(nameof(UpdateDifferences), out object value))
            {
                if (value is UpdateDifferences update)
                {
                    UpdateDifferences = update;
                }
            }
        }

        private UpdateDifferences updateDifferences;
        public UpdateDifferences UpdateDifferences
        {
            get => updateDifferences;
            set
            {
                if (SetProperty(ref updateDifferences, value, nameof(UpdateDifferences))) { }
            }
        }
        private bool selectAllValue;
        public bool SelectAllValue
        {
            get => selectAllValue;
            set
            {
                if (SetProperty(ref selectAllValue, value, nameof(SelectAllValue)))
                {
                    DriverRoutesUpdate.OnSelectedChange(value);
                }
            }
        }


        private readonly DataBase.Data.Save.ISaveDriverRoutesAoT _saveRoutes;
        private readonly DataBase.Data.IAccessDataBase _db;
        private readonly ToolbarItem ToolbarItem;
        public UpdateDifferenceVM(DataBase.Data.Save.ISaveDriverRoutesAoT saveRoutes, DataBase.Data.IAccessDataBase db)
        {
            _saveRoutes = saveRoutes;

            _db = db;
            ToolbarItem = new ToolbarItem()
            {
                Text = "Synchronizacja danych",
                Command = new AsyncRelayCommand(async () =>
                {
                    if (!Shell.Current.Navigation.ModalStack.Any(page => page is UpdateDifferenceV))
                    {
                        await Shell.Current.GoToAsync(nameof(UpdateDifferenceV));
                    }
                })
            };
            Shared.Pages.FlyoutHeader.FlyoutHeaderVM.SetToolbarItem(ToolbarItem);
        }

        [RelayCommand]
        async Task Back()
        {
            var leave = await Shell.Current.DisplayAlert("Cofnij", "Podczas cofania wybrane rekordy nie zostaną zsynchronizowane. W prawym górnym rogu znajduje się opcja powrotu do tej strony.", "Tak", "Nie");
            if (leave)
            {
                if (Shell.Current.Navigation.NavigationStack.Count > 1)
                {
                    await Shell.Current.GoToAsync("..");
                }
                else
                {
                    await Shell.Current.GoToAsync("//MainPage");
                }
            }
        }

        [RelayCommand]
        async Task Save()
        {
            try
            {
                var customers = DriverRoutesUpdate.OnReturnCustomerRoutes();
                var result = await Shell.Current.DisplayAlert("Zapis", "Czy chcesz zapisać wybrane rekordy", "Tak", "Nie");

                var user = Shared.Helper.UserAfterLogin.User.Id;
                if (result)
                {
                    foreach (var item in customers)
                    {
                        //await _saveRoutes.SaveCustomerRoutes(item, user.ToByteArray());
                        //await _saveRoutes.SaveResidentialAddress(item.ResidentialAddress, user.ToByteArray());
                        //await _saveRoutes.SaveSelectedDayOfWeekRoutes(item.DayOfWeek, user.ToByteArray());
                    }
                    UpdateDifferences.UpdateDifferencesDriverRoutes?.Clear();
                    UpdateDifferences.UpdateDifferencesInventory?.Clear();
                    UpdateDifferences = null;
                    if (Shell.Current.Navigation.NavigationStack.Count > 1)
                    {
                        await Shell.Current.GoToAsync("..");
                    }
                    else
                    {
                        await Shell.Current.GoToAsync("//MainPage");
                    }
                    Shared.Pages.FlyoutHeader.FlyoutHeaderVM.RemoveToolbarItem(ToolbarItem);
                }
            }
            catch (Exception ex)
            {
                _db.SaveLogExtension(ex);
            }
        }

    }
}
