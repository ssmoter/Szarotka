using CommunityToolkit.Maui.Alerts;
using CommunityToolkit.Maui.Core;

using DataBase.Data;
using DataBase.Model.EntitiesInventory;

using Inventory.Service;

using System.Collections.ObjectModel;

namespace Inventory.Data
{
    public class SaveDayService(AccessDataBase db) : ISaveDayService
    {
        readonly AccessDataBase _db = db;

        public async Task<Day> SaveDayAsync(Day value)
        {
            try
            {
                if (value is null)
                {
                    return value;
                }


                value.CanUpadte = false;
                if (await CheckDriver(value))
                {
                    return value;
                }

                if (value.DriverGuid == Guid.Empty)
                {
                    value.DriverGuid = new Guid(Helper.SelectedDriver.Id);
                }

                if (value.Id != Guid.Empty)
                {
                    value.Updated = DateTime.Now;
                    await _db.DataBaseAsync.UpdateAsync(value);
                }
                else
                {
                    value.Id = Guid.NewGuid();
                    value.Created = DateTime.Now;
                    value.Updated = DateTime.Now;
                    await _db.DataBaseAsync.InsertAsync(value);
                }


                var product = SaveProductsAsync(value.Products, value.Id.ToByteArray());
                var cake = SaveCakesAsync(value.Cakes, value.Id.ToByteArray());

                await Task.WhenAll(product, cake);

                value.Products = product.Result;
                value.Cakes = cake.Result;

                await Toast.Make("Zapisano", ToastDuration.Short).Show();
                value.CanUpadte = true;

                for (int i = 0; i < value.Products.Count; i++)
                {
                    value.Products[i].CanUpadte = true;
                }

            }
            catch (Exception ex)
            {
                _db.SaveLog(ex);
            }
            return value;
        }

        static async Task<bool> CheckDriver(Day day)
        {
            if (day.DriverGuid == Guid.Empty)
            {
                day.DriverGuid = new Guid(Helper.SelectedDriver.Id);
            }
            if (day.DriverGuid == Guid.Empty)
            {
                await Shell.Current.DisplayAlert("Kierowca", $"Kierowca nie został wybrany{Environment.NewLine}Wybierz kierowcę w celu zapisania", "Ok");
                return true;
            }
            return false;
        }


        private async Task<ObservableCollection<Product>> SaveProductsAsync(ObservableCollection<Product> product, byte[] id)
        {
            var tasks = new Task[product.Count];

            for (int i = 0; i < product.Count; i++)
            {
                product[i].DayId = new(id);
                product[i].Updated = DateTime.Now;

                if (product[i].Id != Guid.Empty)
                {
                    tasks[i] = _db.DataBaseAsync.UpdateAsync(product[i]);
                }
                else
                {
                    product[i].Id = Guid.NewGuid();
                    product[i].Created = DateTime.Now;

                    tasks[i] = _db.DataBaseAsync.InsertAsync(product[i]);
                }
            }
            await Task.WhenAll(tasks);

            return product;
        }

        private async Task<ObservableCollection<Cake>> SaveCakesAsync(ObservableCollection<Cake> cake, byte[] id)
        {
            var tasks = new Task[cake.Count];

            for (int i = 0; i < cake.Count; i++)
            {
                cake[i].DayId = new(id);
                if (cake[i].Id != Guid.Empty)
                {
                    cake[i].Updated = DateTime.Now;
                    tasks[i] = _db.DataBaseAsync.UpdateAsync(cake[i]);
                }
                else
                {
                    cake[i].Id = Guid.NewGuid();
                    if (cake[i].Created == DateTime.UnixEpoch)
                    {
                        cake[i].Created = DateTime.Now;
                    }
                    cake[i].Updated = DateTime.Now;
                    tasks[i] = _db.DataBaseAsync.InsertAsync(cake[i]);
                }
            }
            await Task.WhenAll(tasks);

            return cake;
        }
    }
}
