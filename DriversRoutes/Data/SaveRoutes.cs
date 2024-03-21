using DataBase.Data;

using DataBase.Model.EntitiesRoutes;

using DriversRoutes.Service;

namespace DriversRoutes.Data
{
    public class SaveRoutes(AccessDataBase db) : ISaveRoutes
    {
        readonly DataBase.Data.AccessDataBase _db = db;

        public async Task SaveCustomer(CustomerRoutes customer, byte[] idRoute)
        {
            byte[] customerId = Guid.NewGuid().ToByteArray();
            if (customer.Id == Guid.Empty)
            {
                customer.Id = new Guid(customerId);
                customer.RoutesId = new Guid(idRoute);
                customer.Created = DateTime.Now;
                customer.Updated = DateTime.Now;
                await _db.DataBaseAsync.InsertAsync(customer);
            }
            else
            {
                customer.Updated = DateTime.Now;
                await _db.DataBaseAsync.InsertOrReplaceAsync(customer);
            }

            if (customer.DayOfWeek.Id == Guid.Empty)
            {
                customer.DayOfWeek.Id = Guid.NewGuid();
                customer.DayOfWeek.CustomerId = new Guid(customerId);
                customer.DayOfWeek.Created = DateTime.Now;
                customer.DayOfWeek.Updated = DateTime.Now;
                await _db.DataBaseAsync.InsertAsync(customer.DayOfWeek);
            }
            else
            {
                customer.DayOfWeek.Updated = DateTime.Now;
                await _db.DataBaseAsync.InsertOrReplaceAsync(customer.DayOfWeek);
            }

            if (customer.ResidentialAddress.Id == Guid.Empty)
            {
                customer.ResidentialAddress.Id = Guid.NewGuid();
                customer.ResidentialAddress.CustomerId = new Guid(customerId);
                customer.ResidentialAddress.Created = DateTime.Now;
                customer.ResidentialAddress.Updated = DateTime.Now;
                await _db.DataBaseAsync.InsertAsync(customer.ResidentialAddress);
            }
            else
            {
                customer.ResidentialAddress.Updated = DateTime.Now;
                await _db.DataBaseAsync.InsertOrReplaceAsync(customer.ResidentialAddress);
            }
        }


    }
}
