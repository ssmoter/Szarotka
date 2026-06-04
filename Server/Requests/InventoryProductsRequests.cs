using DataBase.Data;
using DataBase.Data.CheckUpdateDifferences;
using DataBase.Data.Get;
using DataBase.Data.Save;
using DataBase.Model.EntitiesInventory;
using DataBase.Model.EntitiesServer;

namespace Server.Requests
{
    public interface IInventoryProductsRequests
    {
        Task<IResult> GetEmptyProducts(bool isDelete = false, CancellationToken token = default);
        Task<IResult?> Update(EmptyProduct product, bool forceUpdate = false, CancellationToken token = default);
        Task<IResult?> Updates(EmptyProducts products, bool forceUpdate = false, CancellationToken token = default);
    }

    public class InventoryProductsRequests(IAccessDataBase db,
                                     IGetInventoryAoT getInventoryAoT,
                                     ISaveInventoryAoT saveInventoryAoT) : IInventoryProductsRequests
    {
        private readonly IAccessDataBase _db = db;
        private readonly IGetInventoryAoT _getInventoryAoT = getInventoryAoT;
        private readonly ISaveInventoryAoT _saveInventoryAoT = saveInventoryAoT;

        public async Task<IResult> GetEmptyProducts(bool isDelete = false, CancellationToken token = default)
        {
            try
            {
                token.ThrowIfCancellationRequested();

                IList<(ProductName, IList<ProductPrice>)> product = await _getInventoryAoT.EmptyProductsNameAndPrices(isDelete);

                var emptyProducts = new EmptyProducts(product);

                return Results.Ok(emptyProducts);
            }
            catch (OperationCanceledException ex)
            {
                Console.WriteLine(ex.Message);
                throw;
            }
            catch (Exception ex)
            {
                _db.SaveLog(ex);
                throw;
            }
        }

        public async Task<IResult?> Update(EmptyProduct product, bool forceUpdate = false, CancellationToken token = default)
        {

            try
            {
                token.ThrowIfCancellationRequested();

                ArgumentNullException.ThrowIfNull(product);

                (bool, ProductName?) name = await ModelsDifferences.Check(_getInventoryAoT, product.Name, forceUpdate);
                if (name.Item1)
                {
                    await _saveInventoryAoT.SaveProductName(product.Name, product.Name.UserUpdatedId.ToByteArray(), true);
                }
                for (int i = 0; i < product.Prices.Count; i++)
                {
                    (bool, ProductPrice?) price = await ModelsDifferences.Check(_getInventoryAoT, product.Prices[i], forceUpdate);
                    if (price.Item1)
                    {
                        await _saveInventoryAoT.SaveProductPrice(product.Prices[i], product.Prices[i].UserUpdatedId.ToByteArray(), forceUpdate);
                    }
                }
                if (name.Item2 is not null)
                {
                    return Results.Conflict(name.Item2);
                }

                return Results.Ok();
            }
            catch (OperationCanceledException ex)
            {
                Console.WriteLine(ex.Message);
                throw;
            }
            catch (Exception ex)
            {
                _db.SaveLog(ex);
                throw;
            }
        }

        public async Task<IResult?> Updates(EmptyProducts products, bool forceUpdate = false, CancellationToken token = default)
        {
            try
            {
                token.ThrowIfCancellationRequested();

                ArgumentNullException.ThrowIfNull(products);

                IList<UpdateDifference> differences = [];
                foreach (var product in products.Products)
                {
                    (bool, ProductName?) name = await ModelsDifferences.Check(_getInventoryAoT, product.Name, forceUpdate);
                    if (name.Item1)
                    {
                        await _saveInventoryAoT.SaveProductName(product.Name, product.Name.UserUpdatedId.ToByteArray(), true);
                    }
                    for (int i = 0; i < product.Prices.Count; i++)
                    {
                        (bool, ProductPrice?) price = await ModelsDifferences.Check(_getInventoryAoT, product.Prices[i], forceUpdate);
                        if (price.Item1)
                        {
                            await _saveInventoryAoT.SaveProductPrice(product.Prices[i], product.Prices[i].UserUpdatedId.ToByteArray(), forceUpdate);
                        }
                    }
                    if (name.Item2 is not null)
                    {
                        differences.Add(new UpdateDifference()
                        {
                            Server = name.Item2,
                            Update = product.Name
                        });
                    }
                }
                if (differences.Count > 0)
                {
                    return Results.Conflict(differences);
                }
                return Results.Ok();
            }
            catch (OperationCanceledException ex)
            {
                Console.WriteLine(ex.Message);
                throw;
            }
            catch (Exception ex)
            {
                _db.SaveLog(ex);
                throw;
            }
        }
    }
}
