using DataBase.Data;
using DataBase.Data.Get;
using DataBase.Model.EntitiesInventory;

using Microsoft.AspNetCore.Http.HttpResults;

using Moq;

using Server.Requests;

namespace ServerUnitTest.Requests
{
    public class InventoryProductsRequestsTests
    {
        private readonly Mock<IGetInventoryAoT> _mockGet;
        private readonly Mock<IAccessDataBase> _mockDb;
        private readonly InventoryProductsRequests _inventoryProductsRequests;

        public InventoryProductsRequestsTests()
        {
            _mockDb = new Mock<IAccessDataBase>();
            _mockGet = new Mock<IGetInventoryAoT>();
            _inventoryProductsRequests = new InventoryProductsRequests(_mockDb.Object, _mockGet.Object);
        }


        [Fact]
        public async Task EmptyProducts_ShouldGet()
        {
            var product = Array.Empty<(ProductName name, IList<ProductPrice> prices)>();

            _mockGet.Setup(x => x.EmptyProducts()).ReturnsAsync(product);

            var result = await _inventoryProductsRequests.GetEmptyProducts();

            Assert.IsType<Ok<EmptyProducts>>(result);
        }

        [Fact]
        public async Task EmptyProducts_Cancel()
        {
            var product = Array.Empty<(ProductName name, IList<ProductPrice> prices)>();

            _mockGet.Setup(x => x.EmptyProducts()).ReturnsAsync(product);
            CancellationTokenSource token = new CancellationTokenSource();
            token.Cancel();

            await Assert.ThrowsAsync<OperationCanceledException>(() => _inventoryProductsRequests.GetEmptyProducts(token.Token));
        }
    }
}
