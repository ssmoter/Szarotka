using DataBase.Data;
using DataBase.Data.Get;
using DataBase.Model.EntitiesInventory;

namespace Server.Requests
{
    public interface IInventoryProductsRequests
    {
        Task<IResult> GetEmptyProducts(CancellationToken token = default);
    }

    public class InventoryProductsRequests : IInventoryProductsRequests
    {
        private readonly IAccessDataBase _db;
        private readonly IGetInventoryAoT _getInventoryAoT;
        public InventoryProductsRequests(IAccessDataBase db, IGetInventoryAoT getInventoryAoT)
        {
            _db = db;
            _getInventoryAoT = getInventoryAoT;
        }


        public async Task<IResult> GetEmptyProducts(CancellationToken token = default)
        {
            try
            {
                token.ThrowIfCancellationRequested();

                IList<(ProductName, IList<ProductPrice>)> product = await _getInventoryAoT.EmptyProducts();

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






    }
}
