using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

using DataBase.Data;
using DataBase.Model.EntitiesInventory;

using Inventory.Service;

using Shared.Data;


namespace Inventory.Pages.Main
{
    public partial class MainVM : ObservableObject, IQueryAttributable
    {
        public void ApplyQueryAttributes(IDictionary<string, object> query)
        {
            if (query.TryGetValue(nameof(DataBase.Model.EntitiesInventory.Day), out object day))
            {
                if (day is DataBase.Model.EntitiesInventory.Day _day)
                {
                    Day = _day;
                }
            }
        }

        private string name;
        public string Name
        {
            get => name;
            set
            {
                if (SetProperty(ref name, value, nameof(Name))) { }
            }
        }

        private MainM mainM;
        public MainM MainM
        {
            get => mainM;
            set
            {
                if (SetProperty(ref mainM, value, nameof(MainM))) { }
            }
        }


        public Day Day { get; set; }

        readonly AccessDataBase _db;
        readonly Service.ISelectDayService _selectDayService;

        public MainVM(AccessDataBase db, ISelectDayService selectDay)
        {
            _db = db;
            _selectDayService = selectDay;
            MainM = new MainM();
            Name = "wybierz kierowcę";
            LookingForSelectedDriver();
            Service.DriverNameUpdateService.Update += SetName;

        }

        #region Method


        public void LookingForSelectedDriver()
        {
            try
            {
                var tableInfo = _db.DataBase.GetTableInfo(nameof(SelectedDriver));
                bool exist = tableInfo.Count > 0;
                if (exist)
                {
                    var selectedDriver = _db.DataBase.Table<SelectedDriver>().FirstOrDefault();
                    if (selectedDriver is not null)
                    {
                        var driver = _db.DataBase.Table<Driver>().FirstOrDefault(x => x.Id == selectedDriver.SelectedGuid);
                        Helper.SelectedDriver.Id = driver.Id.ToString();
                        Helper.SelectedDriver.Name = driver.Name;
                        Helper.SelectedDriver.Description = driver.Description;
                        SetName();
                    }
                }
            }
            catch (Exception ex)
            {
                _db.SaveLogExtension(ex);
            }
        }
        void SetName()
        {
            Name = Shared.Helper.UserAfterLogin.User.Name;
        }

        #endregion

        #region Command

        [RelayCommand]
        async Task NavigationToSingleDay()
        {
            try
            {
                if (Day is not null)
                {
                    Day.Products = new(Day.Products.OrderBy(x => x.Name.Arrangement));
                }
                if (Day is null)
                {
                    Day = await _selectDayService.GetDayProcedure(DateTime.Now);
                }
                else if (Day.Created.ToShortDateString() != DateTime.Now.ToShortDateString())
                {
                    Day = await _selectDayService.GetDayProcedure(DateTime.Now);
                }
                else if (Day.DriverGuid != new Guid(Helper.SelectedDriver.Id))
                {
                    Day = await _selectDayService.GetDayProcedure(DateTime.Now);
                }


                Day.CanUpdate = true;
                for (int i = 0; i < Day.Products.Count; i++)
                {
                    Day.Products[i].CanUpdate = true;
                }


                for (int i = 0; i < 10; i++)
                {
                    Day.Cakes.Add(new Cake()
                    {
                        PriceDecimal = i * 10,
                        Created = DateTime.Now,
                        Updated = DateTime.Now,
                        IsSell = i % 2 == 0,
                        Index = i,
                    });
                }

                if (Day.Id == Guid.Empty)
                {
                    await Shell.Current.GoToAsync($"{nameof(Inventory.Pages.SingleDay.SingleDayV)}?",
                        new Dictionary<string, object>()
                        {
                            [nameof(DataBase.Model.EntitiesInventory.Day)] = Day,

                        });
                }
                else
                {
                    await Shell.Current.GoToAsync($"{nameof(Inventory.Pages.SingleDayPreview.SingleDayPreviewPage.SingleDayPreviewPageV)}?",
                        new Dictionary<string, object>()
                        {
                            [nameof(DataBase.Model.EntitiesInventory.Day)] = Day,
                            [nameof(DataBase.Model.EntitiesInventory.Driver)] = Helper.SelectedDriver.Name,

                        });
                }
            }
            catch (Exception ex)
            {
                _db.SaveLogExtension(ex);
            }
        }

        [RelayCommand]
        async Task NavigationToRange()
        {
            await Shell.Current.GoToAsync(nameof(Inventory.Pages.RangeDay.RangeDayV));
        }

        [RelayCommand]
        async Task NavigationToEdit()
        {
            await Shell.Current.GoToAsync(nameof(Inventory.Pages.Products.ListProduct.ListProductV));
        }

        [RelayCommand]
        async Task NavigationToSingleDayWitchSelectedDay()
        {
            try
            {
                if (string.IsNullOrWhiteSpace(MainM.DisplayDate))
                    return;


                var days = await _selectDayService.GetDayProcedure(MainM.Date);
                days.CanUpdate = true;
                for (int i = 0; i < days.Products.Count; i++)
                {
                    days.Products[i].CanUpdate = true;
                }


                if (days.Id == Guid.Empty)
                {
                    await Shell.Current.GoToAsync($"{nameof(Inventory.Pages.SingleDay.SingleDayV)}?",
                        new Dictionary<string, object>()
                        {
                            [nameof(DataBase.Model.EntitiesInventory.Day)] = days,

                        });
                }
                else
                {
                    await Shell.Current.GoToAsync($"{nameof(Inventory.Pages.SingleDayPreview.SingleDayPreviewPage.SingleDayPreviewPageV)}?",
                        new Dictionary<string, object>()
                        {
                            [nameof(DataBase.Model.EntitiesInventory.Day)] = days,
                            [nameof(DataBase.Model.EntitiesInventory.Driver)] = Helper.SelectedDriver.Name,
                        });
                }
            }
            catch (Exception ex)
            {
                _db.SaveLogExtension(ex);
            }
        }


        #endregion

    }
}
