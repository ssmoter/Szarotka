using DataBase.Data.SqlQuery;
using DataBase.Model.EntitiesInventory;

namespace DataBase.Data.Save
{
    public interface ISaveInventoryAoT
    {
        /// <summary>
        /// Zapisanie rekordu wykorzustując zapytanie sql.Tworzy nowy jeżeli nie istnieje
        /// </summary>
        /// <param name="day">rekord do zapisanie / aktualizacji</param>
        /// <param name="driverId">
        /// Przy tworzenu jest to id DriverGuid, UserCreatedId,UserUpdatedId.
        /// A przy edycji tylko UserUpdatedId jest zmieniany
        /// </param>
        /// <param name="isServer">Zaktualizować date edycji czy zostawić bez zmian, na serwerze powinna nie być zmieniana</param>
        /// <returns></returns>
        /// <exception cref="ArgumentOutOfRangeException"></exception>
        Task SaveDay(Day day, byte[] driverId, bool isServer = false);

        /// <summary>
        /// Zapisanie rekordu wykorzustując zapytanie sql.Tworzy nowy jeżeli nie istnieje
        /// </summary>
        /// <param name="product">rekord który jest zapisywany lub aktualizowany jeżeli istnieje</param>
        /// <param name="driverId">Id użytkownika który ostatni edytował rekord</param>
        /// <param name="isServer">Zaktualizować date edycji czy zostawić bez zmian, na serwerze powinna nie być zmieniana</param>
        /// <returns></returns>
        /// <exception cref="ArgumentOutOfRangeException"></exception>
        Task SaveProduct(Product product, byte[] driverId, bool isServer = false);
        /// <summary>
        /// Zapisanie rekordu wykorzustując zapytanie sql.Tworzy nowy jeżeli nie istnieje
        /// </summary>
        /// <param name="cake">rekord do zapisanie / aktualizacji</param>
        /// <param name="driverId">
        /// Przy tworzenu jest to id DriverGuid, UserCreatedId,UserUpdatedId.
        /// A przy edycji tylko UserUpdatedId jest zmieniany
        /// </param>
        /// <param name="isServer">Zaktualizować date edycji czy zostawić bez zmian, na serwerze powinna nie być zmieniana</param>
        /// <returns></returns>
        /// <exception cref="ArgumentOutOfRangeException"></exception>
        Task SaveCake(Cake cake, byte[] driverId, bool isServer = false);
        /// <summary>
        /// Zapisanie rekordu wykorzustując zapytanie sql.Tworzy nowy jeżeli nie istnieje
        /// </summary>
        /// <param name="productName">rekord do zapisanie / aktualizacji</param>
        /// <param name="driverId">
        /// Przy tworzenu jest to id DriverGuid, UserCreatedId,UserUpdatedId.
        /// A przy edycji tylko UserUpdatedId jest zmieniany
        /// </param>
        /// <param name="isServer">Zaktualizować date edycji czy zostawić bez zmian, na serwerze powinna nie być zmieniana</param>
        /// <returns></returns>
        /// <exception cref="ArgumentOutOfRangeException"></exception>
        Task SaveProductName(ProductName productName, byte[] driverId, bool isServer = false);
        /// <summary>
        /// Zapisanie rekordu wykorzustując zapytanie sql.Tworzy nowy jeżeli nie istnieje
        /// </summary>
        /// <param name="productPrice">rekord do zapisanie / aktualizacji</param>
        /// <param name="driverId">
        /// Przy tworzenu jest to id DriverGuid, UserCreatedId,UserUpdatedId.
        /// A przy edycji tylko UserUpdatedId jest zmieniany
        /// </param>
        /// <param name="isServer">Zaktualizować date edycji czy zostawić bez zmian, na serwerze powinna nie być zmieniana</param>
        /// <returns></returns>
        /// <exception cref="ArgumentOutOfRangeException"></exception>
        Task SaveProductPrice(ProductPrice productPrice, byte[] driverId, bool isServer = false);
    }

    public class SaveInventoryAoT : ISaveInventoryAoT
    {
        private readonly IAccessDataBase _db;

        public SaveInventoryAoT(IAccessDataBase db)
        {
            _db = db;
        }

        public async Task SaveDay(Day day, byte[] driverId, bool isServer = false)
        {
            var now = DateTime.UtcNow;
            if (day.Created == DateTime.MinValue)
            {
                day.Created = now;
            }
            var lastUpdate = day.Updated;
            if (!isServer)
            {
                day.Updated = now;
            }

            if (day.Id == Guid.Empty)
            {
                day.Id = Guid.CreateVersion7();
            }
            ArgumentNullException.ThrowIfNull(driverId, $"{nameof(driverId)} in {nameof(SaveDay)}");
            if (driverId == Guid.Empty.ToByteArray())
            {
                throw new ArgumentOutOfRangeException(nameof(driverId), nameof(SaveDay));
            }
            if (day.DriverGuid == Guid.Empty)
            {
                day.DriverGuid = new Guid(driverId);
                day.UserCreatedId = new Guid(driverId);
            }
            var userUpdateId = day.UserUpdatedId.ToByteArray();
            day.UserUpdatedId = new Guid(driverId);

            var sql = DayQuery.SaveOrUpdate(day);
            try
            {
                _ = await _db.DataBaseAsync.ExecuteAsync(sql);
            }
            catch (Exception)
            {
                day.Updated = lastUpdate;
                day.UserUpdatedId = new Guid(userUpdateId);
                throw;
            }
        }
        public async Task SaveProduct(Product product, byte[] driverId, bool isServer = false)
        {
            var now = DateTime.UtcNow;
            if (product.Created == DateTime.MinValue)
            {
                product.Created = now;
            }
            var lastUpdate = product.Updated;
            if (!isServer)
            {
                product.Updated = now;
            }

            if (product.Id == Guid.Empty)
            {
                product.Id = Guid.CreateVersion7();
            }
            ArgumentNullException.ThrowIfNull(driverId, $"{nameof(driverId)} in {nameof(SaveProduct)}");

            if (driverId == Guid.Empty.ToByteArray())
            {
                throw new ArgumentOutOfRangeException(nameof(driverId), nameof(SaveProduct));
            }
            if (product.UserCreatedId == Guid.Empty)
            {
                product.UserCreatedId = new Guid(driverId);
            }
            var userUpdateId = product.UserUpdatedId.ToByteArray();
            product.UserUpdatedId = new Guid(driverId);

            var sql = ProductQuery.SaveOrUpdate(product);
            try
            {
                _ = await _db.DataBaseAsync.ExecuteAsync(sql);
            }
            catch (Exception)
            {
                product.Updated = lastUpdate;
                product.UserUpdatedId = new Guid(userUpdateId);
                throw;
            }
        }
        public async Task SaveCake(Cake cake, byte[] driverId, bool isServer = false)
        {
            var now = DateTime.UtcNow;
            if (cake.Created == DateTime.MinValue)
            {
                cake.Created = now;
            }
            var lastUpdate = cake.Updated;
            if (!isServer)
            {
                cake.Updated = now;
            }

            if (cake.Id == Guid.Empty)
            {
                cake.Id = Guid.CreateVersion7();
            }
            ArgumentNullException.ThrowIfNull(driverId, $"{nameof(driverId)} in {nameof(SaveProduct)}");

            if (driverId == Guid.Empty.ToByteArray())
            {
                throw new ArgumentOutOfRangeException(nameof(driverId), nameof(SaveProduct));
            }
            if (cake.UserCreatedId == Guid.Empty)
            {
                cake.UserCreatedId = new Guid(driverId);
            }
            var userUpdateId = cake.UserUpdatedId.ToByteArray();
            cake.UserUpdatedId = new Guid(driverId);

            var sql = CakeQuery.SaveOrUpdate(cake);
            try
            {
                _ = await _db.DataBaseAsync.ExecuteAsync(sql);
            }
            catch (Exception)
            {
                cake.Updated = lastUpdate;
                cake.UserUpdatedId = new Guid(userUpdateId);
                throw;
            }
        }
        public async Task SaveProductName(ProductName productName, byte[] driverId, bool isServer = false)
        {
            var now = DateTime.UtcNow;
            if (productName.Created == DateTime.MinValue)
            {
                productName.Created = now;
            }
            var lastUpdate = productName.Updated;
            if (!isServer)
            {
                productName.Updated = now;
            }

            if (productName.Id == Guid.Empty)
            {
                productName.Id = Guid.CreateVersion7();
            }
            ArgumentNullException.ThrowIfNull(driverId, $"{nameof(driverId)} in {nameof(SaveProductName)}");

            if (driverId == Guid.Empty.ToByteArray())
            {
                throw new ArgumentOutOfRangeException(nameof(driverId), nameof(SaveProductName));
            }
            if (productName.UserCreatedId == Guid.Empty)
            {
                productName.UserCreatedId = new Guid(driverId);
            }
            var userUpdateId = productName.UserUpdatedId.ToByteArray();
            productName.UserUpdatedId = new Guid(driverId);

            var sql = ProductNameQuery.SaveOrUpdate(productName);
            try
            {
                _ = await _db.DataBaseAsync.ExecuteAsync(sql);
            }
            catch (Exception)
            {
                productName.Updated = lastUpdate;
                productName.UserUpdatedId = new Guid(userUpdateId);
                throw;
            }
        }
        public async Task SaveProductPrice(ProductPrice productPrice, byte[] driverId, bool isServer = false)
        {
            var now = DateTime.UtcNow;
            if (productPrice.Created == DateTime.MinValue)
            {
                productPrice.Created = now;
            }
            var lastUpdate = productPrice.Updated;
            if (!isServer)
            {
                productPrice.Updated = now;
            }

            if (productPrice.Id == Guid.Empty)
            {
                productPrice.Id = Guid.CreateVersion7();
            }
            ArgumentNullException.ThrowIfNull(driverId, $"{nameof(driverId)} in {nameof(SaveProductPrice)}");

            if (driverId == Guid.Empty.ToByteArray())
            {
                throw new ArgumentOutOfRangeException(nameof(driverId), nameof(SaveProductPrice));
            }
            if (productPrice.UserCreatedId == Guid.Empty)
            {
                productPrice.UserCreatedId = new Guid(driverId);
            }
            var userUpdateId = productPrice.UserUpdatedId.ToByteArray();
            productPrice.UserUpdatedId = new Guid(driverId);

            var sql = ProductPriceQuery.SaveOrUpdate(productPrice);
            try
            {
                _ = await _db.DataBaseAsync.ExecuteAsync(sql);
            }
            catch (Exception)
            {
                productPrice.Updated = lastUpdate;
                productPrice.UserUpdatedId = new Guid(userUpdateId);
                throw;
            }
        }

    }
}
