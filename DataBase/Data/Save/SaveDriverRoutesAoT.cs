using DataBase.Data.SqlQuery;
using DataBase.Model.EntitiesRoutes;

namespace DataBase.Data.Save
{
    public interface ISaveDriverRoutesAoT
    {
        /// <summary>
        /// Zapisanie rekordu wykorzustując zapytanie sql.Tworzy nowy jeżeli nie istnieje
        /// </summary>
        /// <param name="customerRoutes">rekord do zapisanie / aktualizacji</param>
        /// <param name="driverId">
        /// Przy tworzenu jest to id DriverGuid, UserCreatedId,UserUpdatedId.
        /// A przy edycji tylko UserUpdatedId jest zmieniany
        /// </param>
        /// <param name="isServer">Zaktualizować date edycji czy zostawić bez zmian, na serwerze powinna nie być zmieniana</param>
        /// <returns></returns>
        /// <exception cref="ArgumentOutOfRangeException"></exception>
        Task SaveCustomerRoutes(CustomerRoutes customerRoutes, byte[] driverId, bool isServer = false);
        /// <summary>
        /// Zapisanie rekordu wykorzustując zapytanie sql.Tworzy nowy jeżeli nie istnieje
        /// </summary>
        /// <param name="residentialAddress">rekord do zapisanie / aktualizacji</param>
        /// <param name="driverId">
        /// Przy tworzenu jest to id DriverGuid, UserCreatedId,UserUpdatedId.
        /// A przy edycji tylko UserUpdatedId jest zmieniany
        /// </param>
        /// <param name="isServer">Zaktualizować date edycji czy zostawić bez zmian, na serwerze powinna nie być zmieniana</param>
        /// <returns></returns>
        /// <exception cref="ArgumentOutOfRangeException"></exception>
        Task SaveResidentialAddress(ResidentialAddress residentialAddress, byte[] driverId, bool isServer = false);
        /// <summary>
        /// Zapisanie rekordu wykorzustując zapytanie sql.Tworzy nowy jeżeli nie istnieje
        /// </summary>
        /// <param name="routes">rekord do zapisanie / aktualizacji</param>
        /// <param name="driverId">
        /// Przy tworzenu jest to id DriverGuid, UserCreatedId,UserUpdatedId.
        /// A przy edycji tylko UserUpdatedId jest zmieniany
        /// </param>
        /// <param name="isServer">Zaktualizować date edycji czy zostawić bez zmian, na serwerze powinna nie być zmieniana</param>
        /// <returns></returns>
        /// <exception cref="ArgumentOutOfRangeException"></exception>
        Task SaveRoutes(Routes routes, byte[] driverId, bool isServer = false);
        /// <summary>
        /// Zapisanie rekordu wykorzustując zapytanie sql.Tworzy nowy jeżeli nie istnieje
        /// </summary>
        /// <param name="selectedDayOfWeek">rekord do zapisanie / aktualizacji</param>
        /// <param name="driverId">
        /// Przy tworzenu jest to id DriverGuid, UserCreatedId,UserUpdatedId.
        /// A przy edycji tylko UserUpdatedId jest zmieniany
        /// </param>
        /// <param name="isServer">Zaktualizować date edycji czy zostawić bez zmian, na serwerze powinna nie być zmieniana</param>
        /// <returns></returns>
        /// <exception cref="ArgumentOutOfRangeException"></exception>
        Task SaveSelectedDayOfWeekRoutes(SelectedDayOfWeekRoutes selectedDayOfWeek, byte[] driverId, bool isServer = false);
    }

    public class SaveDriverRoutesAoT : ISaveDriverRoutesAoT
    {
        private readonly IAccessDataBase _db;

        public SaveDriverRoutesAoT(IAccessDataBase db)
        {
            _db = db;
        }

        public async Task SaveCustomerRoutes(CustomerRoutes customerRoutes, byte[] driverId, bool isServer = false)
        {
            var lastUpdate = customerRoutes.Updated;
            var userUpdateId = customerRoutes.UserUpdatedId.ToByteArray();
            var now = DateTime.UtcNow;
            if (customerRoutes.Id == Guid.Empty)
            {
                customerRoutes.Id = Guid.CreateVersion7();
            }
            if (customerRoutes.Created == DateTime.MinValue)
            {
                customerRoutes.Created = now;
            }
            if (!isServer)
            {
                customerRoutes.Updated = now;
            }
            ArgumentNullException.ThrowIfNull(driverId, $"{nameof(driverId)} in {nameof(SaveCustomerRoutes)}");

            if (new Guid(driverId) == Guid.Empty)
            {
                throw new ArgumentNullException(nameof(driverId), nameof(SaveCustomerRoutes));
            }
            if (customerRoutes.RoutesId == Guid.Empty)
            {
                throw new ArgumentNullException(nameof(customerRoutes), nameof(customerRoutes.RoutesId));
            }

            if (customerRoutes.UserCreatedId == Guid.Empty)
            {
                customerRoutes.UserCreatedId = new Guid(driverId);
            }

            customerRoutes.UserUpdatedId = new Guid(driverId);

            var sql = CustomerRoutesQuery.SaveOrUpdate(customerRoutes.Id,
                                                       customerRoutes.RoutesId,
                                                       customerRoutes.Name,
                                                       customerRoutes.Description,
                                                       customerRoutes.PhoneNumber,
                                                       customerRoutes.Longitude,
                                                       customerRoutes.Latitude,
                                                       customerRoutes.CreatedTicks,
                                                       customerRoutes.UpdatedTicks,
                                                       customerRoutes.IsDelete,
                                                       customerRoutes.UserCreatedId,
                                                       customerRoutes.UserUpdatedId);
            try
            {
                _ = await _db.DataBaseAsync.ExecuteAsync(sql,
                                                         customerRoutes.Id,
                                                         customerRoutes.RoutesId,
                                                         customerRoutes.Name,
                                                         customerRoutes.Description,
                                                         customerRoutes.PhoneNumber,
                                                         customerRoutes.Longitude,
                                                         customerRoutes.Latitude,
                                                         customerRoutes.CreatedTicks,
                                                         customerRoutes.UpdatedTicks,
                                                         customerRoutes.IsDelete,
                                                         customerRoutes.UserCreatedId,
                                                         customerRoutes.UserUpdatedId);
            }
            catch (Exception)
            {
                customerRoutes.Updated = lastUpdate;
                customerRoutes.UserUpdatedId = new Guid(userUpdateId);
                throw;
            }
        }
        public async Task SaveResidentialAddress(ResidentialAddress residentialAddress, byte[] driverId, bool isServer = false)
        {
            var lastUpdate = residentialAddress.Updated;
            var userUpdateId = residentialAddress.UserUpdatedId.ToByteArray();
            var now = DateTime.UtcNow;
            if (residentialAddress.Id == Guid.Empty)
            {
                residentialAddress.Id = Guid.CreateVersion7();
            }
            if (residentialAddress.Created == DateTime.MinValue)
            {
                residentialAddress.Created = now;
            }
            if (!isServer)
            {
                residentialAddress.Updated = now;
            }
            ArgumentNullException.ThrowIfNull(driverId, $"{nameof(driverId)} in {nameof(SaveResidentialAddress)}");
            if (new Guid(driverId) == Guid.Empty)
            {
                throw new ArgumentNullException(nameof(driverId), nameof(SaveResidentialAddress));
            }
            if (residentialAddress.CustomerId == Guid.Empty)
            {
                throw new ArgumentNullException(nameof(residentialAddress), nameof(residentialAddress.CustomerId));
            }
            if (residentialAddress.UserCreatedId == Guid.Empty)
            {
                residentialAddress.UserCreatedId = new Guid(driverId);
            }
            residentialAddress.UserUpdatedId = new Guid(driverId);

            var sql = ResidentialAddressQuery.SaveOrUpdate(residentialAddress.Id,
                                                           residentialAddress.CustomerId,
                                                           residentialAddress.Name,
                                                           residentialAddress.Surname,
                                                           residentialAddress.Street,
                                                           residentialAddress.HouseNumber,
                                                           residentialAddress.ApartmentNumber,
                                                           residentialAddress.PostalCode,
                                                           residentialAddress.City,
                                                           residentialAddress.Country,
                                                           residentialAddress.CreatedTicks,
                                                           residentialAddress.UpdatedTicks,
                                                           residentialAddress.IsDelete,
                                                           residentialAddress.UserCreatedId,
                                                           residentialAddress.UserUpdatedId);
            try
            {
                _ = await _db.DataBaseAsync.ExecuteAsync(sql,
                                                         residentialAddress.Id,
                                                         residentialAddress.CustomerId,
                                                         residentialAddress.Name,
                                                         residentialAddress.Surname,
                                                         residentialAddress.Street,
                                                         residentialAddress.HouseNumber,
                                                         residentialAddress.ApartmentNumber,
                                                         residentialAddress.PostalCode,
                                                         residentialAddress.City,
                                                         residentialAddress.Country,
                                                         residentialAddress.CreatedTicks,
                                                         residentialAddress.UpdatedTicks,
                                                         residentialAddress.IsDelete,
                                                         residentialAddress.UserCreatedId,
                                                         residentialAddress.UserUpdatedId);
            }
            catch (Exception)
            {
                residentialAddress.Updated = lastUpdate;
                residentialAddress.UserUpdatedId = new Guid(userUpdateId);
                throw;
            }
        }
        public async Task SaveRoutes(Routes routes, byte[] driverId, bool isServer = false)
        {
            var lastUpdate = routes.Updated;
            var userUpdateId = routes.UserUpdatedId.ToByteArray();
            var now = DateTime.UtcNow;
            if (routes.Id == Guid.Empty)
            {
                routes.Id = Guid.CreateVersion7();
            }
            if (routes.Created == DateTime.MinValue)
            {
                routes.Created = now;
            }
            if (!isServer)
            {
                routes.Updated = now;
            }
            ArgumentNullException.ThrowIfNull(driverId, $"{nameof(driverId)} in {nameof(SaveRoutes)}");
            if (new Guid(driverId) == Guid.Empty)
            {
                throw new ArgumentNullException(nameof(driverId), nameof(SaveRoutes));
            }
            if (routes.UserCreatedId == Guid.Empty)
            {
                routes.UserCreatedId = new Guid(driverId);
            }
            routes.UserUpdatedId = new Guid(driverId);

            var sql = RoutesQuery.SaveOrUpdate(routes.Id,
                                               routes.Name,
                                               routes.CreatedTicks,
                                               routes.UpdatedTicks,
                                               routes.IsDelete,
                                               routes.UserCreatedId,
                                               routes.UserUpdatedId);
            try
            {
                _ = await _db.DataBaseAsync.ExecuteAsync(sql,
                                               routes.Id,
                                               routes.Name,
                                               routes.CreatedTicks,
                                               routes.UpdatedTicks,
                                               routes.IsDelete,
                                               routes.UserCreatedId,
                                               routes.UserUpdatedId);
            }
            catch (Exception)
            {
                routes.Updated = lastUpdate;
                routes.UserUpdatedId = new Guid(userUpdateId);
                throw;
            }
        }
        public async Task SaveSelectedDayOfWeekRoutes(SelectedDayOfWeekRoutes selectedDayOfWeek, byte[] driverId, bool isServer = false)
        {
            var lastUpdate = selectedDayOfWeek.Updated;
            var userUpdateId = selectedDayOfWeek.UserUpdatedId.ToByteArray();
            var now = DateTime.UtcNow;
            if (selectedDayOfWeek.Id == Guid.Empty)
            {
                selectedDayOfWeek.Id = Guid.CreateVersion7();
            }
            if (selectedDayOfWeek.Created == DateTime.MinValue)
            {
                selectedDayOfWeek.Created = now;
            }
            if (!isServer)
            {
                selectedDayOfWeek.Updated = now;
            }
            ArgumentNullException.ThrowIfNull(driverId, $"{nameof(driverId)} in {nameof(SaveSelectedDayOfWeekRoutes)}");
            if (new Guid(driverId) == Guid.Empty)
            {
                throw new ArgumentNullException(nameof(driverId), nameof(SaveSelectedDayOfWeekRoutes));
            }
            if (selectedDayOfWeek.CustomerId == Guid.Empty)
            {
                throw new ArgumentNullException(nameof(selectedDayOfWeek), nameof(selectedDayOfWeek.CustomerId));
            }

            if (selectedDayOfWeek.UserCreatedId == Guid.Empty)
            {
                selectedDayOfWeek.UserCreatedId = new Guid(driverId);
            }
            selectedDayOfWeek.UserUpdatedId = new Guid(driverId);

            var sql = SelectedDayOfWeekRoutesQuery.SaveOrUpdate(selectedDayOfWeek.Id,
                                                                selectedDayOfWeek.CustomerId,
                                                                selectedDayOfWeek.Sunday,
                                                                selectedDayOfWeek.SundayTicks,
                                                                selectedDayOfWeek.Monday,
                                                                selectedDayOfWeek.MondayTicks,
                                                                selectedDayOfWeek.Tuesday,
                                                                selectedDayOfWeek.TuesdayTicks,
                                                                selectedDayOfWeek.Wednesday,
                                                                selectedDayOfWeek.WednesdayTicks,
                                                                selectedDayOfWeek.Thursday,
                                                                selectedDayOfWeek.ThursdayTicks,
                                                                selectedDayOfWeek.Friday,
                                                                selectedDayOfWeek.FridayTicks,
                                                                selectedDayOfWeek.Saturday,
                                                                selectedDayOfWeek.SaturdayTicks,
                                                                selectedDayOfWeek.Optional,
                                                                selectedDayOfWeek.CreatedTicks,
                                                                selectedDayOfWeek.UpdatedTicks,
                                                                selectedDayOfWeek.IsDelete,
                                                                selectedDayOfWeek.UserCreatedId,
                                                                selectedDayOfWeek.UserUpdatedId);
            try
            {
                _ = await _db.DataBaseAsync.ExecuteAsync(sql,
                                                         selectedDayOfWeek.Id,
                                                         selectedDayOfWeek.CustomerId,
                                                         selectedDayOfWeek.Sunday,
                                                         selectedDayOfWeek.SundayTicks,
                                                         selectedDayOfWeek.Monday,
                                                         selectedDayOfWeek.MondayTicks,
                                                         selectedDayOfWeek.Tuesday,
                                                         selectedDayOfWeek.TuesdayTicks,
                                                         selectedDayOfWeek.Wednesday,
                                                         selectedDayOfWeek.WednesdayTicks,
                                                         selectedDayOfWeek.Thursday,
                                                         selectedDayOfWeek.ThursdayTicks,
                                                         selectedDayOfWeek.Friday,
                                                         selectedDayOfWeek.FridayTicks,
                                                         selectedDayOfWeek.Saturday,
                                                         selectedDayOfWeek.SaturdayTicks,
                                                         selectedDayOfWeek.Optional,
                                                         selectedDayOfWeek.CreatedTicks,
                                                         selectedDayOfWeek.UpdatedTicks,
                                                         selectedDayOfWeek.IsDelete,
                                                         selectedDayOfWeek.UserCreatedId,
                                                         selectedDayOfWeek.UserUpdatedId);
            }
            catch (Exception)
            {
                selectedDayOfWeek.Updated = lastUpdate;
                selectedDayOfWeek.UserUpdatedId = new Guid(userUpdateId);
                throw;
            }
        }



    }
}
