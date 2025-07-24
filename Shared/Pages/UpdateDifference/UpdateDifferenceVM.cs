using CommunityToolkit.Maui.Alerts;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

using DataBase.Model.EntitiesRoutes;
using DataBase.Model.EntitiesServer;

using Shared.Data;

using System.Collections;

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
                    for (int i = 0; i < UpdateDifferences?.UpdateDifferencesInventory?.Count; i++)
                    {
                        UpdateDifferences.UpdateDifferencesInventory[i].Index = i + 1;
                    }
                    for (int i = 0; i < UpdateDifferences?.UpdateDifferencesDriverRoutes?.Count; i++)
                    {
                        UpdateDifferences.UpdateDifferencesDriverRoutes[i].Index = i + 1;
                        UpdateDifferences.UpdateDifferencesDriverRoutes[i].UpdateSelect = new CustomerRoutesUpdate();
                    }
                }
            }
            if (query.TryGetValue(nameof(Action), out object actionValue))
            {
                if (actionValue is Action<IEnumerable> saveAction)
                {
                    _saveAction = saveAction;
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
                    foreach (var item in UpdateDifferences.UpdateDifferencesDriverRoutes)
                    {
                        DriverRoutesUpdate.SetUpdateBool(item.UpdateSelect, value);
                    }
                }
            }
        }




        private Action<IEnumerable> _saveAction;
        private readonly DataBase.Data.IAccessDataBase _db;
        private readonly ToolbarItem ToolbarItem;
        public UpdateDifferenceVM(DataBase.Data.IAccessDataBase db)
        {
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
        static async Task Back()
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
                var result = await Shell.Current.DisplayAlert("Zapis", "Czy chcesz zapisać wybrane rekordy", "Tak", "Nie");

                if (!result)
                {
                    return;
                }

                if (UpdateDifferences.UpdateDifferencesDriverRoutes is not null)
                {
                    var customersCount = UpdateDifferences.UpdateDifferencesDriverRoutes.Count;
                    CustomerRoutes[] customers = new CustomerRoutes[customersCount];
                    for (int i = 0; i < customersCount; i++)
                    {
                        customers[i] = DriverRoutesUpdate.ApplyUpdateBoolToCustomerRoutes(
                            UpdateDifferences.UpdateDifferencesDriverRoutes[i].Server,
                            UpdateDifferences.UpdateDifferencesDriverRoutes[i].Update,
                            UpdateDifferences.UpdateDifferencesDriverRoutes[i].UpdateSelect);
                    }
                    if (customers is not null)
                    {
                        _saveAction?.Invoke(customers);
                    }
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
                await Toast.Make("Zapisywanie zakończone").Show();
            }
            catch (Exception ex)
            {
                _db.SaveLogExtension(ex);
            }
        }


    }
}
