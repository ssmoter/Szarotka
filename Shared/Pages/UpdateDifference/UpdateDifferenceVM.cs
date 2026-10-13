using CommunityToolkit.Maui.Alerts;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

using DataBase.Model.EntitiesInventory;
using DataBase.Model.EntitiesRoutes;

using Shared.Data;
using Shared.Pages.UpdateDifference.Templates;

using System.Collections;

namespace Shared.Pages.UpdateDifference
{
    public partial class UpdateDifferenceVM : ObservableObject, IQueryAttributable
    {
        public void ApplyQueryAttributes(IDictionary<string, object> query)
        {
            if (query.TryGetValue(nameof(UpdateDifference), out object value))
            {
                if (value is IList<DataBase.Model.EntitiesServer.UpdateDifference> update)
                {
                    UpdateDifferences = update;
                    SetUpdateSelect();
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




        private IList<DataBase.Model.EntitiesServer.UpdateDifference> updateDifferences;
        public IList<DataBase.Model.EntitiesServer.UpdateDifference> UpdateDifferences
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
                    foreach (DataBase.Model.EntitiesServer.UpdateDifference item in UpdateDifferences)
                    {
                        if (item.UpdateSelect is CustomerRoutesUpdate routes)
                        {
                            DriverRoutesUpdate.SetUpdateBool(routes, value);
                        }
                        else if (item.UpdateSelect is BoolPNameUpdate name)
                        {
                            ProductNameUpdate.SetUpdateBool(name, value);
                        }
                        else if (item.UpdateSelect is BoolDayUpdate day)
                        {
                            DayUpdate.SetUpdateBool(day, value);
                        }
                    }
                }
            }
        }




        private Action<IEnumerable> _saveAction;
        private readonly DataBase.Data.IAccessDataBaseAoT _db;
        private readonly ToolbarItem ToolbarItem;
        public UpdateDifferenceVM(DataBase.Data.IAccessDataBaseAoT db)
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

        private void SetUpdateSelect()
        {
            int n = 1;
            foreach (var item in UpdateDifferences)
            {
                item.Index = n;
                switch (item.Server)
                {
                    case CustomerRoutes:
                        item.UpdateSelect = new CustomerRoutesUpdate();
                        break;
                    case ProductName:
                        item.UpdateSelect = new BoolPNameUpdate();
                        break;
                    case Day:
                        item.UpdateSelect = new BoolDayUpdate();
                        break;
                }

                n++;
            }
        }


        [RelayCommand]
        static async Task Back()
        {
            var leave = await Shell.Current.DisplayAlertAsync("Cofnij", "Podczas cofania wybrane rekordy nie zostaną zsynchronizowane. W prawym górnym rogu znajduje się opcja powrotu do tej strony.", "Tak", "Nie");
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
                var result = await Shell.Current.DisplayAlertAsync("Zapis", "Czy chcesz zapisać wybrane rekordy", "Tak", "Nie");

                if (!result)
                {
                    return;
                }

                IList<CustomerRoutes> customers = [];
                IList<ProductName> pNames = [];
                IList<Day> days = [];


                foreach (DataBase.Model.EntitiesServer.UpdateDifference item in UpdateDifferences)
                {
                    if (item.UpdateSelect is CustomerRoutesUpdate)
                    {
                        customers.Add((DriverRoutesUpdate.ApplyUpdateBoolTo(
                                 (CustomerRoutes)item.Server,
                                 (CustomerRoutes)item.Update,
                                 item.UpdateSelect)));
                    }
                    else if (item.UpdateSelect is BoolPNameUpdate)
                    {
                        pNames.Add(ProductNameUpdate.ApplyUpdateBoolTo(
                             (ProductName)item.Server,
                             (ProductName)item.Update,
                             item.UpdateSelect));
                    }
                    else if (item.UpdateSelect is BoolDayUpdate)
                    {
                        days.Add(DayUpdate.ApplyUpdateBoolTo(
                            (Day)item.Server,
                            (Day)item.Update,
                            item.UpdateSelect));
                    }
                }

                if (customers.Any())
                {
                    _saveAction?.Invoke(customers);
                }
                if (pNames.Any())
                {
                    _saveAction?.Invoke(pNames);
                }
                if (days.Any())
                {
                    _saveAction?.Invoke(days);
                }



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

