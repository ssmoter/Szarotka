using DataBase.Data.SqlQuery;
using DataBase.Model.EntitiesRoutes;

using System.Text.Json;

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
        Task SaveCustomerRoutesTransaction(IList<CustomerRoutes> customerRoutes, byte[] driverId, bool isServer = false);

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
        Task SaveSelectedDayOfWeekRoutesTransaction(IList<SelectedDayOfWeekRoutes> selectedDayOfWeekRoutes, byte[] driverId, bool isServer = false);
    }

    public class SaveDriverRoutesAoT(IAccessDataBaseAoT db) : ISaveDriverRoutesAoT
    {
        private readonly IAccessDataBaseAoT _db = db;

        public async Task SaveCustomerRoutes(CustomerRoutes customerRoutes, byte[] driverId, bool isServer = false)
        {
            SetCustomerRoutes(customerRoutes, driverId, isServer, out var lastUpdate, out var userUpdateId, out var sql);
            try
            {
                _ = await _db.DbAsyncAoT.ExecuteAsync(sql,
                                                         new
                                                         {
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
                                                             customerRoutes.UserUpdatedId
                                                         });
            }
            catch (Exception)
            {
                customerRoutes.Updated = lastUpdate;
                customerRoutes.UserUpdatedId = new Guid(userUpdateId);
                throw;
            }
        }
        public async Task SaveCustomerRoutesTransaction(IList<CustomerRoutes> customerRoutes, byte[] driverId, bool isServer = false)
        {
            byte[] copy = JsonSerializer.SerializeToUtf8Bytes(customerRoutes, DataBase.Model.SourceGenerator.SzarotkaJsonSerializerContext.Default.IListCustomerRoutes);

            await using var tx = await _db.DbAsyncAoT.BeginTransactionAsync();

            try
            {
                foreach (var item in customerRoutes)
                {
                    SetCustomerRoutes(item,
                                      driverId,
                                      isServer,
                                      out var lastUpdate,
                                      out var userUpdateId,
                                      out var sql);
                    _ = await tx.ExecuteAsync(sql,
                                         new
                                         {
                                             item.Id,
                                             item.RoutesId,
                                             item.Name,
                                             item.Description,
                                             item.PhoneNumber,
                                             item.Longitude,
                                             item.Latitude,
                                             item.CreatedTicks,
                                             item.UpdatedTicks,
                                             item.IsDelete,
                                             item.UserCreatedId,
                                             item.UserUpdatedId
                                         });
                    await SaveResidentialAddressTransaction(item.ResidentialAddress, driverId, isServer);
                    await SaveSelectedDayOfWeekRoutesTransaction(item.DayOfWeek, driverId, isServer);
                }
                await tx.CommitAsync();
            }
            catch (Exception)
            {
                await tx.RollbackAsync();
                var fromCopy = JsonSerializer.Deserialize(copy, DataBase.Model.SourceGenerator.SzarotkaJsonSerializerContext.Default.IListCustomerRoutes)!;
                customerRoutes = fromCopy;
                throw;
            }
            async Task SaveResidentialAddressTransaction(ResidentialAddress residentialAddress, byte[] driverId, bool isServer = false)
            {
                SetResidentialAddress(residentialAddress,
                  driverId,
                  isServer,
                  out var lastUpdate,
                  out var userUpdateId,
                  out var sql);
                try
                {
                    _ = await tx.ExecuteAsync(sql,
                                     new
                                     {
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
                                         residentialAddress.UserUpdatedId
                                     });
                }
                catch (Exception)
                {
                    throw;
                }

            }
            async Task SaveSelectedDayOfWeekRoutesTransaction(SelectedDayOfWeekRoutes selectedDayOfWeek, byte[] driverId, bool isServer = false)
            {
                SetSelectedDayOfWeekRoutes(selectedDayOfWeek,
                           driverId,
                           isServer,
                           out var lastUpdate,
                           out var userUpdateId,
                           out var sql);
                try
                {
                    _ = await tx.ExecuteAsync(sql,
                                                new
                                                {
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
                                                    selectedDayOfWeek.UserUpdatedId
                                                });
                }
                catch (Exception)
                {
                    throw;
                }
            }
        }
        public async Task SaveResidentialAddress(ResidentialAddress residentialAddress, byte[] driverId, bool isServer = false)
        {
            SetResidentialAddress(residentialAddress,
                                  driverId,
                                  isServer,
                                  out var lastUpdate,
                                  out var userUpdateId,
                                  out var sql);
            try
            {
                _ = await _db.DbAsyncAoT.ExecuteAsync(sql,
                                                         new
                                                         {
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
                                                             residentialAddress.UserUpdatedId
                                                         });
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
            SetRoutes(routes, driverId, isServer, out var lastUpdate, out var userUpdateId, out var sql);
            try
            {
                _ = await _db.DbAsyncAoT.ExecuteAsync(sql,
                                               new
                                               {
                                                   routes.Id,
                                                   routes.Name,
                                                   routes.CreatedTicks,
                                                   routes.UpdatedTicks,
                                                   routes.IsDelete,
                                                   routes.UserCreatedId,
                                                   routes.UserUpdatedId
                                               });
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
            SetSelectedDayOfWeekRoutes(selectedDayOfWeek,
                                       driverId,
                                       isServer,
                                       out var lastUpdate,
                                       out var userUpdateId,
                                       out var sql);
            try
            {
                _ = await _db.DbAsyncAoT.ExecuteAsync(sql,
                                                         new
                                                         {
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
                                                             selectedDayOfWeek.UserUpdatedId
                                                         });
            }
            catch (Exception)
            {
                selectedDayOfWeek.Updated = lastUpdate;
                selectedDayOfWeek.UserUpdatedId = new Guid(userUpdateId);
                throw;
            }
        }
        public async Task SaveSelectedDayOfWeekRoutesTransaction(IList<SelectedDayOfWeekRoutes> selectedDayOfWeekRoutes, byte[] driverId, bool isServer = false)
        {
            byte[] copy = JsonSerializer.SerializeToUtf8Bytes(selectedDayOfWeekRoutes, DataBase.Model.SourceGenerator.SzarotkaJsonSerializerContext.Default.IListSelectedDayOfWeekRoutes);

            await using var tx = await _db.DbAsyncAoT.BeginTransactionAsync();

            try
            {
                foreach (var item in selectedDayOfWeekRoutes)
                {
                    SetSelectedDayOfWeekRoutes(item,
                            driverId,
                            isServer,
                            out var lastUpdate,
                            out var userUpdateId,
                            out var sql);

                    _ = await tx.ExecuteAsync(sql,
                                                new
                                                {
                                                    item.Id,
                                                    item.CustomerId,
                                                    item.Sunday,
                                                    item.SundayTicks,
                                                    item.Monday,
                                                    item.MondayTicks,
                                                    item.Tuesday,
                                                    item.TuesdayTicks,
                                                    item.Wednesday,
                                                    item.WednesdayTicks,
                                                    item.Thursday,
                                                    item.ThursdayTicks,
                                                    item.Friday,
                                                    item.FridayTicks,
                                                    item.Saturday,
                                                    item.SaturdayTicks,
                                                    item.Optional,
                                                    item.CreatedTicks,
                                                    item.UpdatedTicks,
                                                    item.IsDelete,
                                                    item.UserCreatedId,
                                                    item.UserUpdatedId
                                                });

                }
                await tx.CommitAsync();
            }
            catch (Exception)
            {
                await tx.RollbackAsync();
                var fromCopy = JsonSerializer.Deserialize(copy, DataBase.Model.SourceGenerator.SzarotkaJsonSerializerContext.Default.IListSelectedDayOfWeekRoutes)!;
                selectedDayOfWeekRoutes = fromCopy;
                throw;
            }
        }


        private static void SetCustomerRoutes(CustomerRoutes customerRoutes, byte[] driverId, bool isServer, out DateTime lastUpdate, out byte[] userUpdateId, out string sql)
        {
            lastUpdate = customerRoutes.Updated;
            userUpdateId = customerRoutes.UserUpdatedId.ToByteArray();
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

            sql = CustomerRoutesQuery.SaveOrUpdate(customerRoutes.Id,
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
        private static void SetResidentialAddress(ResidentialAddress residentialAddress, byte[] driverId, bool isServer, out DateTime lastUpdate, out byte[] userUpdateId, out string sql)
        {
            lastUpdate = residentialAddress.Updated;
            userUpdateId = residentialAddress.UserUpdatedId.ToByteArray();
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

            sql = ResidentialAddressQuery.SaveOrUpdate(residentialAddress.Id,
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
        private static void SetRoutes(Routes routes, byte[] driverId, bool isServer, out DateTime lastUpdate, out byte[] userUpdateId, out string sql)
        {
            lastUpdate = routes.Updated;
            userUpdateId = routes.UserUpdatedId.ToByteArray();
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

            sql = RoutesQuery.SaveOrUpdate(routes.Id,
                                               routes.Name,
                                               routes.CreatedTicks,
                                               routes.UpdatedTicks,
                                               routes.IsDelete,
                                               routes.UserCreatedId,
                                               routes.UserUpdatedId);
        }
        private static void SetSelectedDayOfWeekRoutes(SelectedDayOfWeekRoutes selectedDayOfWeek, byte[] driverId, bool isServer, out DateTime lastUpdate, out byte[] userUpdateId, out string sql)
        {
            lastUpdate = selectedDayOfWeek.Updated;
            userUpdateId = selectedDayOfWeek.UserUpdatedId.ToByteArray();
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

            sql = SelectedDayOfWeekRoutesQuery.SaveOrUpdate(selectedDayOfWeek.Id,
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


    }
}
