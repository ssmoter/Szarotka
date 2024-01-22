using DataBase.Data;

using DriversRoutes.Model;
using DriversRoutes.Service;

namespace DriversRoutes.Data
{
    public class SaveRoutes : ISaveRoutes
    {
        readonly DataBase.Data.AccessDataBase _db;

        public SaveRoutes(AccessDataBase db)
        {
            _db = db;
        }

        public async Task SaveCustomer(CustomerRoutes customer, byte[] idRoute)
        {
            byte[] customerId = Guid.NewGuid().ToByteArray();
            if (customer.Id == Guid.Empty)
            {
                customer.Id = new Guid(customerId);
                customer.RoutesId = new Guid(idRoute);
                await _db.DataBaseAsync.InsertAsync(customer);
            }
            else
            {
                await _db.DataBaseAsync.UpdateAsync(customer);
            }

            if (customer.DayOfWeek.Id == Guid.Empty)
            {
                customer.DayOfWeek.Id = Guid.NewGuid();
                customer.DayOfWeek.CustomerId = new Guid(customerId);
                await _db.DataBaseAsync.InsertAsync(customer.DayOfWeek);
            }
            else
            {
                await _db.DataBaseAsync.UpdateAsync(customer.DayOfWeek);
            }

            if (customer.ResidentialAddress.Id == Guid.Empty)
            {
                customer.ResidentialAddress.Id = Guid.NewGuid();
                customer.ResidentialAddress.CustomerId = new Guid(customerId);
                await _db.DataBaseAsync.InsertAsync(customer.ResidentialAddress);
            }
            else
            {
                await _db.DataBaseAsync.UpdateAsync(customer.ResidentialAddress);
            }


        }


    }
}
