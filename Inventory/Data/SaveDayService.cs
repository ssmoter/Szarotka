using CommunityToolkit.Maui.Alerts;
using CommunityToolkit.Maui.Core;

using DataBase.Data;
using DataBase.Model.EntitiesInventory;

using Inventory.Helper.Parse;
using Inventory.Model.MVVM;
using Inventory.Service;

namespace Inventory.Data
{
    public class SaveDayService(AccessDataBase db) : ISaveDayService
    {
        readonly AccessDataBase _db = db;

        public async Task<DayM> SaveDayMAsync(DayM dayM)
        {
            try
            {
                if (dayM is not null)
                {
                    dayM.CanUpadte = false;

                    if (await CheckDriver(dayM))
                    {
                        return dayM;
                    }

                    if (dayM.DriverGuid == Guid.Empty)
                    {
                        dayM.DriverGuid = new Guid(Helper.SelectedDriver.Id);
                    }
                    var day = dayM.ParseAsDay();

                    if (day.Id != Guid.Empty)
                    {
                        day.Updated = DateTime.Now;
                        await _db.DataBaseAsync.UpdateAsync(day);
                    }
                    else
                    {
                        day.Id = Guid.NewGuid();
                        day.SelectedDate = day.Created;
                        day.Created = DateTime.Now;
                        day.Updated = DateTime.Now;
                        await _db.DataBaseAsync.InsertAsync(day);
                    }

                    day = await SaveProductsAsync(day);

                    day = await SaveCakesAsync(day);

                    day.ParseAsDayM(dayM);

                    day = null;
                    await Toast.Make("Zapisano", ToastDuration.Short).Show();
                    dayM.CanUpadte = true;

                    for (int i = 0; i < dayM.Products.Count; i++)
                    {
                        dayM.Products[i].CanUpadte = true;
                    }
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
                dayM.DriverGuid = new Guid(Helper.SelectedDriver.Id);
            }
            if (dayM.DriverGuid == Guid.Empty)
            {
                await Shell.Current.DisplayAlert("Kierowca", $"Kierowca nie został wybrany{Environment.NewLine}Wybierz kierowcę w celu zapisania", "Ok");
                return true;
            }
            return false;
        }


        private async Task<Day> SaveProductsAsync(Day day)
        {
            for (int i = 0; i < day.Products.Count; i++)
            {
                day.Products[i].DayId = day.Id;
                if (day.Products[i].Id != Guid.Empty)
                {
                    day.Products[i].Updated = DateTime.Now;
                    await _db.DataBaseAsync.UpdateAsync(day.Products[i]);
                }
                else
                {
                    day.Products[i].Id = Guid.NewGuid();
                    day.Products[i].Created = DateTime.Now;
                    day.Products[i].Updated = DateTime.Now;

                    await _db.DataBaseAsync.InsertAsync(day.Products[i]);
                }
            }
            return day;
        }

        private async Task<Day> SaveCakesAsync(Day day)
        {
            for (int i = 0; i < day.Cakes.Count; i++)
            {
                day.Cakes[i].DayId = day.Id;
                if (day.Cakes[i].Id != Guid.Empty)
                {
                    day.Cakes[i].Updated = DateTime.Now;
                    await _db.DataBaseAsync.UpdateAsync(day.Cakes[i]);
                }
                else
                {
                    day.Cakes[i].Id = Guid.NewGuid();
                    day.Cakes[i].Created = DateTime.Now;
                    day.Cakes[i].Updated = DateTime.Now;
                    await _db.DataBaseAsync.InsertAsync(day.Cakes[i]);
                }
            }
            return day;
        }
    }
}
