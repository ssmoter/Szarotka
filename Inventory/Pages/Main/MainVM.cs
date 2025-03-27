using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

using DataBase.Data;
using DataBase.Model.EntitiesInventory;

using Inventory.Service;

using Shared.Data;
using Shared.Helper;


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

        readonly IAccessDataBase _db;
        readonly Service.ISelectDayService _selectDayService;

        public MainVM(IAccessDataBase db, ISelectDayService selectDay)
        {
            _db = db;
            _selectDayService = selectDay;
            MainM = new MainM();
            Name = "Wybierz kierowcę";
            Service.DriverNameUpdateService.Update += SetName;
        }

        #region Method


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
                else if (Day.DriverGuid != new Guid(UserAfterLogin.User.Id.ToByteArray()))
                {
                    Day = await _selectDayService.GetDayProcedure(DateTime.Now);
                }

                if (Day.Id == Guid.Empty)
                {

                    var result = await Shell.Current.DisplayAlert("Czy chcesz utworzyć nowy wpis",
                        "Wraz z utworzeniem nowego dnia dane są automatycznie zapisywane",
                        "Utwórz", "Anuluj");

                    if (result)
                    {
                        await Shell.Current.GoToAsync($"{nameof(Inventory.Pages.SingleDay.SingleDayV)}?",
                        new Dictionary<string, object>()
                        {
                            [nameof(DataBase.Model.EntitiesInventory.Day)] = Day,
                        });
                    }
                    else
                    {
                        Day.Dispose();
                    }
                }
                else
                {
                    await Shell.Current.GoToAsync($"{nameof(Inventory.Pages.SingleDayPreview.SingleDayPreviewPage.SingleDayPreviewPageV)}?",
                        new Dictionary<string, object>()
                        {
                            [nameof(DataBase.Model.EntitiesInventory.Day)] = Day,
                            [nameof(DataBase.Model.EntitiesInventory.Driver)] = UserAfterLogin.User.Name,

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

                if (days.Id == Guid.Empty)
                {
                    var result = await Shell.Current.DisplayAlert("Czy chcesz utworzyć nowy wpis",
                                "Wraz z utworzeniem nowego dnia dane są automatycznie zapisywane",
                                "Utwórz", "Anuluj");

                    if (result)
                    {
                        await Shell.Current.GoToAsync($"{nameof(Inventory.Pages.SingleDay.SingleDayV)}?",
                        new Dictionary<string, object>()
                        {
                            [nameof(DataBase.Model.EntitiesInventory.Day)] = days,
                        });
                    }
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
