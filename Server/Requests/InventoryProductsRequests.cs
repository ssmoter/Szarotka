using DataBase.Data;
using DataBase.Data.CheckUpdateDifferences;
using DataBase.Data.Get;
using DataBase.Data.Save;
using DataBase.Model.EntitiesInventory;
using DataBase.Model.EntitiesServer;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;

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
                                     ISaveInventoryAoT saveInventoryAoT,
                                     ILogger<InventoryProductsRequests>? logger = null) : IInventoryProductsRequests
    {
        private readonly IAccessDataBase _db = db;
        private readonly IGetInventoryAoT _getInventoryAoT = getInventoryAoT;
        private readonly ISaveInventoryAoT _saveInventoryAoT = saveInventoryAoT;
        private readonly ILogger<InventoryProductsRequests> _logger = logger ?? NullLogger<InventoryProductsRequests>.Instance;

        public async Task<IResult> GetEmptyProducts(bool isDelete = false, CancellationToken token = default)
        {
            try
            {
                _logger.LogInformation("GetEmptyProducts started isDelete={IsDelete}", isDelete);
                token.ThrowIfCancellationRequested();

                IList<(ProductName, IList<ProductPrice>)> product = await _getInventoryAoT.EmptyProductsNameAndPrices(isDelete);

                var emptyProducts = new EmptyProducts(product);

                _logger.LogInformation("GetEmptyProducts: returning {Count} product groups", emptyProducts.Products.Count);
                return Results.Ok(emptyProducts);
            }
            catch (OperationCanceledException ex)
            {
                _logger.LogWarning(ex, "Operation cancelled in GetEmptyProducts");
                throw;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unexpected error in GetEmptyProducts");
                _db.SaveLog(ex);
                throw;
            }
        }

        public async Task<IResult?> Update(EmptyProduct product, bool forceUpdate = false, CancellationToken token = default)
        {

            try
            {
                _logger.LogInformation("Update(EmptyProduct) started for productNameId={NameId}", product?.Name?.Id);
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
                    _logger.LogInformation("Update(EmptyProduct): conflict for productNameId={NameId}", product.Name.Id);
                    return Results.Conflict(name.Item2);
                }

                _logger.LogInformation("Update(EmptyProduct): updated productNameId={NameId}", product.Name.Id);
                return Results.Ok();
            }
            catch (OperationCanceledException ex)
            {
                _logger.LogWarning(ex, "Operation cancelled in Update(EmptyProduct)");
                throw;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unexpected error in Update(EmptyProduct)");
                _db.SaveLog(ex);
                throw;
            }
        }

        public async Task<IResult?> Updates(EmptyProducts products, bool forceUpdate = false, CancellationToken token = default)
        {
            try
            {
                _logger.LogInformation("Updates(EmptyProducts) started for {Count} products", products?.Products.Count ?? 0);
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
                    _logger.LogInformation("Updates(EmptyProducts): {Count} conflicts found", differences.Count);
                    return Results.Conflict(differences);
                }
                _logger.LogInformation("Updates(EmptyProducts): all products updated");
                return Results.Ok();
            }
            catch (OperationCanceledException ex)
            {
                _logger.LogWarning(ex, "Operation cancelled in Updates(EmptyProducts)");
                throw;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unexpected error in Updates(EmptyProducts)");
                _db.SaveLog(ex);
                throw;
            }
        }
    }
}
