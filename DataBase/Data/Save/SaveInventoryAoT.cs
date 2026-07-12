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

    public class SaveInventoryAoT(IAccessDataBase db) : ISaveInventoryAoT
    {
        private readonly IAccessDataBase _db = db;

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
                _ = await _db.DataBaseAsync.ExecuteAsync(sql,
                                                         day.Id,
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
            await Task.Run(() =>
             {
                 _db.DataBase.RunInTransaction(() =>
                 {
                     foreach (Day day in days)
                     {
                         SetDay(day,
                               driverId,
                               isServer,
                               out DateTime lastUpdate,
                               out byte[] userUpdateId,
                               out string sql);
                         _ = _db.DataBase.Execute(sql,
                                         day.Id,
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
                         SaveProductsNonTransaction(day.Products, driverId, isServer);
                         SaveCakesNonTransaction(day.Cakes, driverId, isServer);
                     }
                 });
             });
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
                _ = await _db.DataBaseAsync.ExecuteAsync(sql,
                                                         product.Id,
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
        private void SaveProductsNonTransaction(IList<Product> products, byte[] driverId, bool isServer = false)
        {
            foreach (Product product in products)
            {
                SetProduct(product,
                           driverId,
                           isServer,
                           out DateTime lastUpdate,
                           out byte[] userUpdateId,
                           out string sql);
                try
                {
                    _ = _db.DataBase.Execute(sql,
                                             product.Id,
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
                catch (Exception)
                {
                    product.Updated = lastUpdate;
                    product.UserUpdatedId = new Guid(userUpdateId);
                    throw;
                }
            }
        }
        public async Task SaveProductsTransaction(IList<Product> products, byte[] driverId, bool isServer = false)
        {
            await Task.Run(() =>
            {
                _db.DataBase.RunInTransaction(() =>
                {
                    SaveProductsNonTransaction(products, driverId, isServer);
                });
            });
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
                _ = await _db.DataBaseAsync.ExecuteAsync(sql,
                                                         cake.Id,
                                                         cake.DayId,
                                                         cake.IsSell,
                                                         cake.Price,
                                                         cake.CreatedTicks,
                                                         cake.UpdatedTicks,
                                                         cake.IsDelete,
                                                         cake.UserCreatedId,
                                                         cake.UserUpdatedId);
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
        public void SaveCakesNonTransaction(IList<Cake> cakes, byte[] driverId, bool isServer = false)
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
                    _ = _db.DataBase.Execute(sql,
                                            cake.Id,
                                            cake.DayId,
                                            cake.IsSell,
                                            cake.Price,
                                            cake.CreatedTicks,
                                            cake.UpdatedTicks,
                                            cake.IsDelete,
                                            cake.UserCreatedId,
                                            cake.UserUpdatedId);
                }
                catch (Exception)
                {
                    cake.Updated = lastUpdate;
                    cake.UserUpdatedId = new Guid(userUpdateId);
                    throw;
                }
            }

        }
        public async Task SaveCakesTransaction(IList<Cake> cakes, byte[] driverId, bool isServer = false)
        {
            await Task.Run(() =>
            {
                _db.DataBase.RunInTransaction(() =>
                {
                    SaveCakesNonTransaction(cakes, driverId, isServer);
                });
            });
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
                _ = await _db.DataBaseAsync.ExecuteAsync(sql,
                                                         productName.Id,
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
        public void SaveProductNamesNonTransaction(IList<ProductName> productNames, byte[] driverId, bool isServer = false)
        {
            foreach (ProductName productName in productNames)
            {
                SetProductName(productName,
                              driverId,
                              isServer,
                              out DateTime lastUpdate,
                              out byte[] userUpdateId,
                              out string sql);
                try
                {
                    _ = _db.DataBase.Execute(sql,
                                            productName.Id,
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
                catch (Exception)
                {
                    productName.Updated = lastUpdate;
                    productName.UserUpdatedId = new Guid(userUpdateId);
                    throw;
                }
            }

        }
        public async Task SaveProductNamesTransaction(IList<ProductName> productNames, byte[] driverId, bool isServer = false)
        {
            await Task.Run(() =>
            {
                _db.DataBase.RunInTransaction(() =>
                {
                    SaveProductNamesNonTransaction(productNames, driverId, isServer);
                });
            });
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
                _ = await _db.DataBaseAsync.ExecuteAsync(sql,
                                                         productPrice.Id,
                                                         productPrice.Price,
                                                         productPrice.CreatedTicks,
                                                         productPrice.UpdatedTicks,
                                                         productPrice.UserCreatedId,
                                                         productPrice.UserUpdatedId,
                                                         productPrice.ProductNameId,
                                                         productPrice.IsDelete);
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
        public void SaveProductPricesNonTransaction(IList<ProductPrice> productPrices, byte[] driverId, bool isServer = false)
        {
            foreach (ProductPrice productPrice in productPrices)
            {
                SetProductPrice(productPrice,
                              driverId,
                              isServer,
                              out DateTime lastUpdate,
                              out byte[] userUpdateId,
                              out string sql);
                try
                {
                    _ = _db.DataBase.Execute(sql,
                                            productPrice.Id,
                                            productPrice.Price,
                                            productPrice.CreatedTicks,
                                            productPrice.UpdatedTicks,
                                            productPrice.UserCreatedId,
                                            productPrice.UserUpdatedId,
                                            productPrice.ProductNameId,
                                            productPrice.IsDelete);
                }
                catch (Exception)
                {
                    productPrice.Updated = lastUpdate;
                    productPrice.UserUpdatedId = new Guid(userUpdateId);
                    throw;
                }
            }

        }
        public async Task SaveProductPricesTransaction(IList<ProductPrice> productPrices, byte[] driverId, bool isServer = false)
        {
            await Task.Run(() =>
            {
                _db.DataBase.RunInTransaction(() =>
                {
                    SaveProductPricesNonTransaction(productPrices, driverId, isServer);
                });
            });
        }

    }
}
