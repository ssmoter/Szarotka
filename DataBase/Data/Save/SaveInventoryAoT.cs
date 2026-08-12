using DataBase.Data.SqlQuery;
using DataBase.Model.EntitiesInventory;

using System.Text.Json;

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

    public class SaveInventoryAoT(IAccessDataBaseAoT db) : ISaveInventoryAoT
    {
        private readonly IAccessDataBaseAoT _db = db;

        public async Task SaveDay(Day day, byte[] driverId, bool isServer = false)
        {
            SetDay(day,
                   driverId,
                   isServer,
                   out DateTime lastUpdate,
                   out byte[] userUpdateId,
                   out string sql);
            try
            {
                _ = await _db.DbAsyncAoT.ExecuteAsync(sql,
                                                        new()
                                                        {
                                                            [nameof(Day.Id)] = day.Id,
                                                            [nameof(Day.Description)] = day.Description,
                                                            [nameof(Day.DriverGuid)] = day.DriverGuid,
                                                            [nameof(Day.SelectedDateString)] = day.SelectedDateString,
                                                            [nameof(Day.SelectedDateTicks)] = day.SelectedDateTicks,
                                                            [nameof(Day.TotalPriceProducts)] = day.TotalPriceProducts,
                                                            [nameof(Day.TotalPriceCake)] = day.TotalPriceCake,
                                                            [nameof(Day.TotalPrice)] = day.TotalPrice,
                                                            [nameof(Day.TotalPriceCorrect)] = day.TotalPriceCorrect,
                                                            [nameof(Day.TotalPriceAfterCorrect)] = day.TotalPriceAfterCorrect,
                                                            [nameof(Day.TotalPriceMoney)] = day.TotalPriceMoney,
                                                            [nameof(Day.TotalPriceDifference)] = day.TotalPriceDifference,
                                                            [nameof(Day.CreatedTicks)] = day.CreatedTicks,
                                                            [nameof(Day.UpdatedTicks)] = day.UpdatedTicks,
                                                            [nameof(Day.IsDelete)] = day.IsDelete,
                                                            [nameof(Day.UserCreatedId)] = day.UserCreatedId,
                                                            [nameof(Day.UserUpdatedId)] = day.UserUpdatedId
                                                        });
            }
            catch (Exception)
            {
                day.Updated = lastUpdate;
                day.UserUpdatedId = new Guid(userUpdateId);
                throw;
            }
        }
        private static void SetDay(Day day, byte[] driverId, bool isServer, out DateTime lastUpdate, out byte[] userUpdateId, out string sql)
        {
            var now = DateTime.UtcNow;
            if (day.Created == DateTime.MinValue)
            {
                day.Created = now;
            }
            lastUpdate = day.Updated;
            if (!isServer)
            {
                day.Updated = now;
            }

            if (day.Id == Guid.Empty)
            {
                day.Id = Guid.CreateVersion7();
            }
            ArgumentNullException.ThrowIfNull(driverId, $"{nameof(driverId)} in {nameof(SaveDay)} is null");
            if (new Guid(driverId) == Guid.Empty)
            {
                throw new ArgumentNullException(nameof(driverId), $"{nameof(SaveDay)} is not valid as guid");
            }
            if (day.DriverGuid == Guid.Empty)
            {
                day.DriverGuid = new Guid(driverId);
                day.UserCreatedId = new Guid(driverId);
            }
            userUpdateId = day.UserUpdatedId.ToByteArray();
            day.UserUpdatedId = new Guid(driverId);

            sql = DayQuery.SaveOrUpdate(day.Id,
                                            day.Description,
                                            day.DriverGuid,
                                            day.SelectedDateString,
                                            day.SelectedDateTicks,
                                            day.TotalPriceProducts,
                                            day.TotalPriceCake,
                                            day.TotalPrice,
                                            day.TotalPriceCorrect,
                                            day.TotalPriceAfterCorrect,
                                            day.TotalPriceMoney,
                                            day.TotalPriceDifference,
                                            day.CreatedTicks,
                                            day.UpdatedTicks,
                                            day.IsDelete,
                                            day.UserCreatedId,
                                            day.UserUpdatedId);
        }
        public async Task SaveDaysTransaction(IList<Day> days, byte[] driverId, bool isServer = false)
        {
            byte[] copy = JsonSerializer.SerializeToUtf8Bytes(days, DataBase.Model.SourceGenerator.SzarotkaJsonSerializerContext.Default.IListDay);

            await using var tx = await _db.DbAsyncAoT.BeginTransactionAsync();
            try
            {
                foreach (var day in days)
                {
                    SetDay(day,
                           driverId,
                           isServer,
                           out DateTime lastUpdate,
                           out byte[] userUpdateId,
                           out string sql);

                    await tx.ExecuteAsync(sql,
                                             new()
                                             {
                                                 [nameof(Day.Id)] = day.Id,
                                                 [nameof(Day.Description)] = day.Description,
                                                 [nameof(Day.DriverGuid)] = day.DriverGuid,
                                                 [nameof(Day.SelectedDateString)] = day.SelectedDateString,
                                                 [nameof(Day.SelectedDateTicks)] = day.SelectedDateTicks,
                                                 [nameof(Day.TotalPriceProducts)] = day.TotalPriceProducts,
                                                 [nameof(Day.TotalPriceCake)] = day.TotalPriceCake,
                                                 [nameof(Day.TotalPrice)] = day.TotalPrice,
                                                 [nameof(Day.TotalPriceCorrect)] = day.TotalPriceCorrect,
                                                 [nameof(Day.TotalPriceAfterCorrect)] = day.TotalPriceAfterCorrect,
                                                 [nameof(Day.TotalPriceMoney)] = day.TotalPriceMoney,
                                                 [nameof(Day.TotalPriceDifference)] = day.TotalPriceDifference,
                                                 [nameof(Day.CreatedTicks)] = day.CreatedTicks,
                                                 [nameof(Day.UpdatedTicks)] = day.UpdatedTicks,
                                                 [nameof(Day.IsDelete)] = day.IsDelete,
                                                 [nameof(Day.UserCreatedId)] = day.UserCreatedId,
                                                 [nameof(Day.UserUpdatedId)] = day.UserUpdatedId
                                             });

                    await SaveProductsTransaction(day.Products, driverId, isServer);
                    await SaveCakesTransaction(day.Cakes, driverId, isServer);

                }


                await tx.CommitAsync();
            }
            catch (Exception)
            {
                await tx.RollbackAsync();
                var fromCopy = JsonSerializer.Deserialize(copy, DataBase.Model.SourceGenerator.SzarotkaJsonSerializerContext.Default.IListDay)!;
                days = fromCopy;
                throw;
            }

            async Task SaveProductsTransaction(IList<Product> products, byte[] driverId, bool isServer = false)
            {
                foreach (var product in products)
                {
                    SetProduct(product,
                             driverId,
                             isServer,
                             out DateTime lastUpdate,
                             out byte[] userUpdateId,
                             out string sql);
                    try
                    {
                        await tx.ExecuteAsync(sql,
                                                new()
                                                {
                                                    [nameof(Product.Id)] = product.Id,
                                                    [nameof(Product.DayId)] = product.DayId,
                                                    [nameof(Product.ProductNameId)] = product.ProductNameId,
                                                    [nameof(Product.ProductPriceId)] = product.ProductPriceId,
                                                    [nameof(Product.Description)] = product.Description,
                                                    [nameof(Product.PriceTotal)] = product.PriceTotal,
                                                    [nameof(Product.PriceTotalCorrect)] = product.PriceTotalCorrect,
                                                    [nameof(Product.PriceTotalAfterCorrect)] = product.PriceTotalAfterCorrect,
                                                    [nameof(Product.Number)] = product.Number,
                                                    [nameof(Product.NumberEdit)] = product.NumberEdit,
                                                    [nameof(Product.NumberReturn)] = product.NumberReturn,
                                                    [nameof(Product.CreatedTicks)] = product.CreatedTicks,
                                                    [nameof(Product.UpdatedTicks)] = product.UpdatedTicks,
                                                    [nameof(Product.IsDelete)] = product.IsDelete,
                                                    [nameof(Product.UserCreatedId)] = product.UserCreatedId,
                                                    [nameof(Product.UserUpdatedId)] = product.UserUpdatedId
                                                });
                    }
                    catch (Exception)
                    {
                        throw;
                    }
                }
            }
            async Task SaveCakesTransaction(IList<Cake> cakes, byte[] driverId, bool isServer = false)
            {
                foreach (Cake cake in cakes)
                {
                    SetCake(cake,
                            driverId,
                            isServer,
                            out DateTime lastUpdate,
                            out byte[] userUpdateId,
                            out string sql);

                    try
                    {
                        await tx.ExecuteAsync(sql,
                                                new()
                                                {
                                                    [nameof(Cake.Id)] = cake.Id,
                                                    [nameof(Cake.DayId)] = cake.DayId,
                                                    [nameof(Cake.IsSell)] = cake.IsSell,
                                                    [nameof(Cake.Price)] = cake.Price,
                                                    [nameof(Cake.CreatedTicks)] = cake.CreatedTicks,
                                                    [nameof(Cake.UpdatedTicks)] = cake.UpdatedTicks,
                                                    [nameof(Cake.IsDelete)] = cake.IsDelete,
                                                    [nameof(Cake.UserCreatedId)] = cake.UserCreatedId,
                                                    [nameof(Cake.UserUpdatedId)] =  cake.UserUpdatedId
                                                });
                    }
                    catch (Exception)
                    {
                        throw;
                    }
                }
            }
        }


        public async Task SaveProduct(Product product, byte[] driverId, bool isServer = false)
        {
            SetProduct(product,
                       driverId,
                       isServer,
                       out DateTime lastUpdate,
                       out byte[] userUpdateId,
                       out string sql);
            try
            {
                _ = await _db.DbAsyncAoT.ExecuteAsync(sql,
                                                  new()
                                                  {
                                                      [nameof(Product.Id)] = product.Id,
                                                      [nameof(Product.DayId)] = product.DayId,
                                                      [nameof(Product.ProductNameId)] = product.ProductNameId,
                                                      [nameof(Product.ProductPriceId)] = product.ProductPriceId,
                                                      [nameof(Product.Description)] = product.Description,
                                                      [nameof(Product.PriceTotal)] = product.PriceTotal,
                                                      [nameof(Product.PriceTotalCorrect)] = product.PriceTotalCorrect,
                                                      [nameof(Product.PriceTotalAfterCorrect)] = product.PriceTotalAfterCorrect,
                                                      [nameof(Product.Number)] = product.Number,
                                                      [nameof(Product.NumberEdit)] = product.NumberEdit,
                                                      [nameof(Product.NumberReturn)] = product.NumberReturn,
                                                      [nameof(Product.CreatedTicks)] = product.CreatedTicks,
                                                      [nameof(Product.UpdatedTicks)] = product.UpdatedTicks,
                                                      [nameof(Product.IsDelete)] = product.IsDelete,
                                                      [nameof(Product.UserCreatedId)] = product.UserCreatedId,
                                                      [nameof(Product.UserUpdatedId)] = product.UserUpdatedId
                                                  });
            }
            catch (Exception)
            {
                product.Updated = lastUpdate;
                product.UserUpdatedId = new Guid(userUpdateId);
                throw;
            }
        }
        private static void SetProduct(Product product, byte[] driverId, bool isServer, out DateTime lastUpdate, out byte[] userUpdateId, out string sql)
        {
            var now = DateTime.UtcNow;
            if (product.Created == DateTime.MinValue)
            {
                product.Created = now;
            }
            lastUpdate = product.Updated;
            if (!isServer)
            {
                product.Updated = now;
            }

            if (product.Id == Guid.Empty)
            {
                product.Id = Guid.CreateVersion7();
            }
            ArgumentNullException.ThrowIfNull(driverId, $"{nameof(driverId)} in {nameof(SaveProduct)} is null");

            if (new Guid(driverId) == Guid.Empty)
            {
                throw new ArgumentNullException(nameof(driverId), $"{nameof(SaveProduct)} is not valid as guid");
            }
            if (product.DayId == Guid.Empty)
            {
                throw new ArgumentNullException(nameof(product), $"{nameof(product.DayId)} is not valid as guid");
            }
            if (product.ProductNameId == Guid.Empty)
            {
                throw new ArgumentNullException(nameof(product), $"{nameof(product.ProductNameId)} is not valid as guid");
            }
            if (product.ProductPriceId == Guid.Empty)
            {
                throw new ArgumentNullException(nameof(product), $"{nameof(product.ProductPriceId)} is not valid as guid");
            }

            if (product.UserCreatedId == Guid.Empty)
            {
                product.UserCreatedId = new Guid(driverId);
            }

            userUpdateId = product.UserUpdatedId.ToByteArray();
            product.UserUpdatedId = new Guid(driverId);

            sql = ProductQuery.SaveOrUpdate(product.Id,
                                                product.DayId,
                                                product.ProductNameId,
                                                product.ProductPriceId,
                                                product.Description,
                                                product.PriceTotal,
                                                product.PriceTotalCorrect,
                                                product.PriceTotalAfterCorrect,
                                                product.Number,
                                                product.NumberEdit,
                                                product.NumberReturn,
                                                product.CreatedTicks,
                                                product.UpdatedTicks,
                                                product.IsDelete,
                                                product.UserCreatedId,
                                                product.UserUpdatedId);
        }

        public async Task SaveCake(Cake cake, byte[] driverId, bool isServer = false)
        {
            SetCake(cake,
                    driverId,
                    isServer,
                    out DateTime lastUpdate,
                    out byte[] userUpdateId,
                    out string sql);
            try
            {
                _ = await _db.DbAsyncAoT.ExecuteAsync(sql,
                                                       new()
                                                       {
                                                           [nameof(Cake.Id)] = cake.Id,
                                                           [nameof(Cake.DayId)] = cake.DayId,
                                                           [nameof(Cake.IsSell)] = cake.IsSell,
                                                           [nameof(Cake.Price)] = cake.Price,
                                                           [nameof(Cake.CreatedTicks)] = cake.CreatedTicks,
                                                           [nameof(Cake.UpdatedTicks)] = cake.UpdatedTicks,
                                                           [nameof(Cake.IsDelete)] = cake.IsDelete,
                                                           [nameof(Cake.UserCreatedId)] = cake.UserCreatedId,
                                                           [nameof(Cake.UserUpdatedId)] = cake.UserUpdatedId
                                                       });
            }
            catch (Exception)
            {
                cake.Updated = lastUpdate;
                cake.UserUpdatedId = new Guid(userUpdateId);
                throw;
            }
        }
        private static void SetCake(Cake cake, byte[] driverId, bool isServer, out DateTime lastUpdate, out byte[] userUpdateId, out string sql)
        {
            var now = DateTime.UtcNow;
            if (cake.Created == DateTime.MinValue)
            {
                cake.Created = now;
            }
            lastUpdate = cake.Updated;
            if (!isServer)
            {
                cake.Updated = now;
            }

            if (cake.Id == Guid.Empty)
            {
                cake.Id = Guid.CreateVersion7();
            }
            ArgumentNullException.ThrowIfNull(driverId, $"{nameof(driverId)} in {nameof(SaveProduct)}");

            if (new Guid(driverId) == Guid.Empty)
            {
                throw new ArgumentNullException(nameof(driverId), $"{nameof(SaveProduct)} is not valid as guid");
            }
            if (cake.DayId == Guid.Empty)
            {
                throw new ArgumentNullException(nameof(cake), $"{nameof(cake.DayId)} is not valid as guid");
            }
            if (cake.UserCreatedId == Guid.Empty)
            {
                cake.UserCreatedId = new Guid(driverId);
            }
            userUpdateId = cake.UserUpdatedId.ToByteArray();
            cake.UserUpdatedId = new Guid(driverId);

            sql = CakeQuery.SaveOrUpdate(cake.Id,
                                             cake.DayId,
                                             cake.IsSell,
                                             cake.Price,
                                             cake.CreatedTicks,
                                             cake.UpdatedTicks,
                                             cake.IsDelete,
                                             cake.UserCreatedId,
                                             cake.UserUpdatedId);
        }

        public async Task SaveProductName(ProductName productName, byte[] driverId, bool isServer = false)
        {
            SetProductName(productName,
                           driverId,
                           isServer,
                           out DateTime lastUpdate,
                           out byte[] userUpdateId,
                           out string sql);
            try
            {
                _ = await _db.DbAsyncAoT.ExecuteAsync(sql,
                                                         new()
                                                         {
                                                             [nameof(ProductName.Id)] = productName.Id,
                                                             [nameof(ProductName.Arrangement)] = productName.Arrangement,
                                                             [nameof(ProductName.Name)] = productName.Name,
                                                             [nameof(ProductName.Description)] = productName.Description,
                                                             [nameof(ProductName.Img)] = productName.Img,
                                                             [nameof(ProductName.IsVisible)] = productName.IsVisible,
                                                             [nameof(ProductName.CreatedTicks)] = productName.CreatedTicks,
                                                             [nameof(ProductName.UpdatedTicks)] = productName.UpdatedTicks,
                                                             [nameof(ProductName.UserCreatedId)] = productName.UserCreatedId,
                                                             [nameof(ProductName.UserUpdatedId)] = productName.UserUpdatedId,
                                                             [nameof(ProductName.IsDelete)] = productName.IsDelete
                                                         });
            }
            catch (Exception)
            {
                productName.Updated = lastUpdate;
                productName.UserUpdatedId = new Guid(userUpdateId);
                throw;
            }
        }
        private static void SetProductName(ProductName productName, byte[] driverId, bool isServer, out DateTime lastUpdate, out byte[] userUpdateId, out string sql)
        {
            var now = DateTime.UtcNow;
            if (productName.Created == DateTime.MinValue)
            {
                productName.Created = now;
            }
            lastUpdate = productName.Updated;
            if (!isServer)
            {
                productName.Updated = now;
            }

            if (productName.Id == Guid.Empty)
            {
                productName.Id = Guid.CreateVersion7();
            }
            ArgumentNullException.ThrowIfNull(driverId, $"{nameof(driverId)} in {nameof(SaveProductName)} is null");

            if (new Guid(driverId) == Guid.Empty)
            {
                throw new ArgumentNullException(nameof(driverId), $"{nameof(driverId)} in {nameof(SaveProductName)} is not valid guid");
            }
            if (productName.UserCreatedId == Guid.Empty)
            {
                productName.UserCreatedId = new Guid(driverId);
            }
            userUpdateId = productName.UserUpdatedId.ToByteArray();
            productName.UserUpdatedId = new Guid(driverId);

            sql = ProductNameQuery.SaveOrUpdate(productName.Id,
                                                    productName.Arrangement,
                                                    productName.Name,
                                                    productName.Description,
                                                    productName.Img,
                                                    productName.IsVisible,
                                                    productName.CreatedTicks,
                                                    productName.UpdatedTicks,
                                                    productName.UserCreatedId,
                                                    productName.UserUpdatedId,
                                                    productName.IsDelete);
        }

        public async Task SaveProductPrice(ProductPrice productPrice, byte[] driverId, bool isServer = false)
        {
            SetProductPrice(productPrice,
                          driverId,
                          isServer,
                          out DateTime lastUpdate,
                          out byte[] userUpdateId,
                          out string sql);
            try
            {
                _ = await _db.DbAsyncAoT.ExecuteAsync(sql,
                                                     new()
                                                     {
                                                         [nameof(ProductPrice.Id)] = productPrice.Id,
                                                         [nameof(ProductPrice.Price)] = productPrice.Price,
                                                         [nameof(ProductPrice.CreatedTicks)] = productPrice.CreatedTicks,
                                                         [nameof(ProductPrice.UpdatedTicks)] = productPrice.UpdatedTicks,
                                                         [nameof(ProductPrice.UserCreatedId)] = productPrice.UserCreatedId,
                                                         [nameof(ProductPrice.UserUpdatedId)] = productPrice.UserUpdatedId,
                                                         [nameof(ProductPrice.ProductNameId)] = productPrice.ProductNameId,
                                                         [nameof(ProductPrice.IsDelete)] = productPrice.IsDelete
                                                     });
            }
            catch (Exception)
            {
                productPrice.Updated = lastUpdate;
                productPrice.UserUpdatedId = new Guid(userUpdateId);
                throw;
            }
        }
        private static void SetProductPrice(ProductPrice productPrice, byte[] driverId, bool isServer, out DateTime lastUpdate, out byte[] userUpdateId, out string sql)
        {
            var now = DateTime.UtcNow;
            if (productPrice.Created == DateTime.MinValue)
            {
                productPrice.Created = now;
            }
            lastUpdate = productPrice.Updated;
            if (!isServer)
            {
                productPrice.Updated = now;
            }

            if (productPrice.Id == Guid.Empty)
            {
                productPrice.Id = Guid.CreateVersion7();
            }
            ArgumentNullException.ThrowIfNull(driverId, $"{nameof(driverId)} in {nameof(SaveProductPrice)} is null");

            if (new Guid(driverId) == Guid.Empty)
            {
                throw new ArgumentNullException(nameof(driverId), $"{nameof(SaveProductPrice)} is not valid as guid");
            }
            if (productPrice.UserCreatedId == Guid.Empty)
            {
                productPrice.UserCreatedId = new Guid(driverId);
            }
            userUpdateId = productPrice.UserUpdatedId.ToByteArray();
            productPrice.UserUpdatedId = new Guid(driverId);

            sql = ProductPriceQuery.SaveOrUpdate(productPrice.Id,
                                                     productPrice.Price,
                                                     productPrice.CreatedTicks,
                                                     productPrice.UpdatedTicks,
                                                     productPrice.UserCreatedId,
                                                     productPrice.UserUpdatedId,
                                                     productPrice.ProductNameId,
                                                     productPrice.IsDelete);
        }

    }
}
