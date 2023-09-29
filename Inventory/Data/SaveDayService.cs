using DataBase.Data;

using Inventory.Helper;
using Inventory.Helper.Parse;
using Inventory.Model;
using Inventory.Model.MVVM;

namespace Inventory.Service
{
    public class SaveDayService : ISaveDayService
    {
        readonly DataBase.Data.AccessDataBase _db;

        public SaveDayService(AccessDataBase db)
        {
            _db = db;
        }
        public async Task<DayM> SaveDayMAsync(DayM dayM)
        {
            try
            {
                if (dayM is not null)
                {
                    Service.ProductUpdatePriceService.EnableUpdate = false;

                    if (await SaveDayService.CheckDriver(dayM))
                    {
                        return dayM;
                    }

                    if (dayM.DriverGuid == Guid.Empty)
                    {
                        dayM.DriverGuid = Helper.SelectedDriver.Id;
                    }
                    var day = dayM.ParseAsDay();

                    if (day.Id != Guid.Empty)
                    {
                        await _db.DataBaseAsync.UpdateAsync(day);
                    }
                    else
                    {
                        day.Id = Guid.NewGuid();
                        await _db.DataBaseAsync.InsertAsync(day);
                    }

                    await SaveProductsAsync(day);

                    await SaveCakesAsync(day);
                    day.ParseAsDayM(dayM);

                    day = null;
                    await SnackbarAsToats.OnShow("Zapisano");
                    Service.ProductUpdatePriceService.EnableUpdate = true;
                }
            }
            catch (Exception ex)
            {
                _db.SaveLog(ex);
            }
            return dayM;
        }

        static async Task<bool> CheckDriver(DayM dayM)
        {
            if (dayM.DriverGuid == Guid.Empty)
            {
                dayM.DriverGuid = Helper.SelectedDriver.Id;
            }
            if (dayM.DriverGuid == Guid.Empty)
            {
                await Shell.Current.DisplayAlert("Kierowca", $"Kierowca nie został wybrany{Environment.NewLine}Wybierz kierowcę w celu zapisania", "Ok");
                return true;
            }
            return false;
        }


        private async Task SaveProductsAsync(Day day)
        {
            for (int i = 0; i < day.Products.Count; i++)
            {
                day.Products[i].DayId = day.Id;
                if (day.Products[i].Id != Guid.Empty)
                {
                    await _db.DataBaseAsync.UpdateAsync(day.Products[i]);
                }
                else
                {
                    day.Products[i].Id = Guid.NewGuid();
                    await _db.DataBaseAsync.InsertAsync(day.Products[i]);
                }
            }
        }

        private async Task SaveCakesAsync(Day day)
        {
            for (int i = 0; i < day.Cakes.Count; i++)
            {
                day.Cakes[i].DayId = day.Id;
                if (day.Cakes[i].Id != Guid.Empty)
                {
                    await _db.DataBaseAsync.UpdateAsync(day.Cakes[i]);
                }
                else
                {
                    day.Cakes[i].Id = Guid.NewGuid();
                    await _db.DataBaseAsync.InsertAsync(day.Cakes[i]);
                }
            }
        }
    }
}
