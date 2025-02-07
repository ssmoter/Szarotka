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
            if (driverId == Guid.Empty.ToByteArray())
            {
                throw new ArgumentOutOfRangeException(nameof(driverId), nameof(SaveCustomerRoutes));
            }
            if (customerRoutes.UserCreatedId == Guid.Empty)
            {
                customerRoutes.UserCreatedId = new Guid(driverId);
            }
            customerRoutes.UserUpdatedId = new Guid(driverId);

            var sql = CustomerRoutesQuery.SaveOrUpdate(customerRoutes);
            try
            {
                _ = await _db.DataBaseAsync.ExecuteAsync(sql);
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
            if (driverId == Guid.Empty.ToByteArray())
            {
                throw new ArgumentOutOfRangeException(nameof(driverId), nameof(SaveResidentialAddress));
            }
            if (residentialAddress.UserCreatedId == Guid.Empty)
            {
                residentialAddress.UserCreatedId = new Guid(driverId);
            }
            residentialAddress.UserUpdatedId = new Guid(driverId);

            var sql = ResidentialAddressQuery.SaveOrUpdate(residentialAddress);
            try
            {
                _ = await _db.DataBaseAsync.ExecuteAsync(sql);
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
            if (driverId == Guid.Empty.ToByteArray())
            {
                throw new ArgumentOutOfRangeException(nameof(driverId), nameof(SaveRoutes));
            }
            if (routes.UserCreatedId == Guid.Empty)
            {
                routes.UserCreatedId = new Guid(driverId);
            }
            routes.UserUpdatedId = new Guid(driverId);

            var sql = RoutesQuery.SaveOrUpdate(routes);
            try
            {
                _ = await _db.DataBaseAsync.ExecuteAsync(sql);
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
            if (driverId == Guid.Empty.ToByteArray())
            {
                throw new ArgumentOutOfRangeException(nameof(driverId), nameof(SaveSelectedDayOfWeekRoutes));
            }
            if (selectedDayOfWeek.UserCreatedId == Guid.Empty)
            {
                selectedDayOfWeek.UserCreatedId = new Guid(driverId);
            }
            selectedDayOfWeek.UserUpdatedId = new Guid(driverId);

            var sql = SelectedDayOfWeekRoutesQuery.SaveOrUpdate(selectedDayOfWeek);
            try
            {
                _ = await _db.DataBaseAsync.ExecuteAsync(sql);
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
