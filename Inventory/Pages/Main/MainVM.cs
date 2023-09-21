using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

using Inventory.Helper;
using Inventory.Model.MVVM;

namespace Inventory.Pages.Main
{
    [QueryProperty(nameof(dayM), nameof(Model.MVVM.DayM))]
    public partial class MainVM : ObservableObject
    {
        [ObservableProperty]
        string name;

        Model.MVVM.DayM dayM;

        readonly DataBase.Data.AccessDataBase _db;

        public MainVM(DataBase.Data.AccessDataBase db)
        {
            _db = db;
            Name = "wybierz kierowcę";
            Service.DriverNameUpdateService.Update += SetName;
            Task.Run(async () =>
            {
                var tableInfo = await db.DataBaseAsync.GetTableInfoAsync(nameof(Model.SelectedDriver));
                bool exist = tableInfo.Count > 0;
                if (exist)
                {
                    var selected = await db.DataBaseAsync.GetAsync<Model.SelectedDriver>(1);
                    var selectedDriver = await db.DataBaseAsync.Table<Model.Driver>().FirstOrDefaultAsync(x => x.Guid == selected.SelectedGuid);
                    Helper.SelectedDriver.Id = selectedDriver.Id;
                    Helper.SelectedDriver.Name = selectedDriver.Name;
                    Helper.SelectedDriver.Description = selectedDriver.Description;
                    Helper.SelectedDriver.Guid = selectedDriver.Guid;
                    var a = Helper.SelectedDriver.Name;
                    var a2 = Helper.SelectedDriver.Guid;

                    SetName();
                }
            });
        }

        #region Method

        void SetName()
        {
            Name = Helper.SelectedDriver.Name;
        }

        async Task<DayM> GetDay()
        {
            try
            {
                await SnackbarAsToats.OnShow("Pobieranie danego dnia ");
                var DayM = new DayM();
                Service.ProductUpdatePriceService.EnableUpdate = false;
                var time = DateTime.Now.ToString("dd.MM.yyyy");
                var today = await _db.DataBaseAsync.Table<Model.Day>().Where(x => x.CreatedDate == time).FirstOrDefaultAsync();
                DayM = today.ParseAsDayM();
                await GetProductTable(DayM);
                await GetCakeTable(DayM);
                if (today is null)
                {
                    DayM.Created = DateTime.Now;
                }
                today.ParseAsDayMOnly(DayM);
                DayM.DriverGuid = Helper.SelectedDriver.Guid;
                return DayM;
            }
            catch (Exception ex)
            {
                await _db.SaveLogAsync(ex);
                throw;
            }
            finally
            {
                Service.ProductUpdatePriceService.EnableUpdate = true;
            }
        }
        async Task GetProductTable(DayM dayM)
        {
            var product = await _db.DataBaseAsync.Table<Model.Product>().Where(x => x.DayId == dayM.Id).ToArrayAsync();

            for (int i = 0; i < product.Length; i++)
            {
                int productNameId = product[i].ProductNameId;
                product[i].Name = await _db.DataBaseAsync.Table<Model.ProductName>().FirstOrDefaultAsync(x => x.Id == productNameId);
                product[i].Price = await _db.DataBaseAsync.Table<Model.ProductPrice>().OrderByDescending(x => x.Id).FirstOrDefaultAsync(x => x.ProductNameId == productNameId);

                dayM.Products.Add(product[i].ParseAsProductM());
            }

            if (dayM.Products.Count == 0)
            {
                var productName = await _db.DataBaseAsync.Table<Model.ProductName>().ToArrayAsync();
                for (int i = 0; i < productName.Length; i++)
                {
                    int nameId = productName[i].Id;
                    var price = await _db.DataBaseAsync.Table<Model.ProductPrice>().OrderByDescending(x => x.Id).FirstOrDefaultAsync(x => x.ProductNameId == nameId);
                    dayM.Products.Add(new ProductM() { Name = productName[i].PareseAsProductNameM(), ProductNameId = productName[i].Id, Price = price.PareseAsProductPriceM() });
                }
            }
        }
        async Task GetCakeTable(DayM dayM)
        {
            var cake = await _db.DataBaseAsync.Table<Model.Cake>().Where(x => x.DayId == dayM.Id).ToArrayAsync();
            for (int i = 0; i < cake.Length; i++)
            {
                dayM.Cakes.Add(cake[i].PareseAsCakeM());
            }

        }
        #endregion
        #region Command

        [RelayCommand]
        async Task NavigationToSingleDay()
        {
            if (dayM is null)
            {
                dayM = await GetDay();
            }
            else if (dayM.Created.ToShortDateString() != DateTime.Now.ToShortDateString())
            {
                dayM = await GetDay();
            }

            await Shell.Current.GoToAsync($"{nameof(Inventory.Pages.SingleDay.SingleDayV)}?",
                new Dictionary<string, object>()
                {
                    [nameof(Model.MVVM.DayM)] = dayM
                });
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

        #endregion
    }
}
