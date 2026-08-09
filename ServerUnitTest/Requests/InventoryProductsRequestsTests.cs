using DataBase.Data;
using DataBase.Data.Get;
using DataBase.Data.Save;
using DataBase.Model.EntitiesInventory;
using DataBase.Model.EntitiesServer;

using Microsoft.AspNetCore.Http.HttpResults;

using Moq;

using Server.Requests;

namespace ServerUnitTest.Requests
{
    public class InventoryProductsRequestsTests
    {
        private readonly Mock<IGetInventoryAoT> _mockGet;
        private readonly Mock<ISaveInventoryAoT> _mockSave;
        private readonly Mock<IAccessDataBaseAoT> _mockDb;
        private readonly InventoryProductsRequests _inventoryProductsRequests;

        public InventoryProductsRequestsTests()
        {
            _mockDb = new Mock<IAccessDataBaseAoT>();
            _mockGet = new Mock<IGetInventoryAoT>();
            _mockSave = new Mock<ISaveInventoryAoT>();
            _inventoryProductsRequests = new InventoryProductsRequests(_mockDb.Object, _mockGet.Object, _mockSave.Object);
        }


        [Fact]
        public async Task EmptyProducts_ShouldGet()
        {
            var product = Array.Empty<(ProductName name, IList<ProductPrice> prices)>();

            _mockGet.Setup(x => x.EmptyProductsNameAndPrices(false)).ReturnsAsync(product);

            var result = await _inventoryProductsRequests.GetEmptyProducts();

            Assert.IsType<Ok<EmptyProducts>>(result);
        }

        [Fact]
        public async Task EmptyProducts_Cancel()
        {
            var product = Array.Empty<(ProductName name, IList<ProductPrice> prices)>();

            _mockGet.Setup(x => x.EmptyProductsNameAndPrices(false)).ReturnsAsync(product);
            CancellationTokenSource token = new();
            token.Cancel();

            await Assert.ThrowsAsync<OperationCanceledException>(() => _inventoryProductsRequests.GetEmptyProducts(false, token.Token));
        }

        [Fact]
        public async Task UpdateEmptyProduct_ShouldGet()
        {
            var product = new EmptyProduct()
            {
                Name = new ProductName()
                {
                    UserUpdatedId = Guid.CreateVersion7(),
                    Updated = DateTime.UtcNow
                },
                Prices =
                [
                    new  ProductPrice()
                    {
                        Id =Guid.CreateVersion7(),
                        Updated = DateTime.UtcNow,
                    }
                ]
            };

            _mockGet.Setup(x => x.GetProductName(It.IsAny<Guid>()));
            _mockGet.Setup(x => x.GetProductPrice(It.IsAny<Guid>()));

            var result = await _inventoryProductsRequests.Update(product, false, default);

            Assert.IsType<Ok>(result);

        }
        [Fact]
        public async Task UpdateEmptyProduct_OnConflict()
        {
            var product = new EmptyProduct()
            {
                Name = new ProductName()
                {
                    UserUpdatedId = Guid.CreateVersion7(),
                    Updated = DateTime.UtcNow
                },
                Prices =
                [
                    new  ProductPrice()
                    {
                        Id =Guid.CreateVersion7(),
                        Updated = DateTime.UtcNow,
                    }
                ]
            };

            _mockGet.Setup(x => x.GetProductName(It.IsAny<Guid>())).ReturnsAsync(product.Name);
            _mockGet.Setup(x => x.GetProductPrice(It.IsAny<Guid>()));

            var result = await _inventoryProductsRequests.Update(product, false, default);

            Assert.IsType<Conflict<ProductName>>(result);

        }

        [Fact]
        public async Task UpdateEmptyProducts_ShouldGet()
        {
            var products = new EmptyProducts()
            {
                Products =
                 [
                    new EmptyProduct()
                    {
                            Name = new ProductName()
                        {
                            UserUpdatedId = Guid.CreateVersion7(),
                            Updated = DateTime.UtcNow
                        },
                        Prices =
                        [
                            new  ProductPrice()
                            {
                                Id =Guid.CreateVersion7(),
                                Updated = DateTime.UtcNow,
                            }
                        ]
                    },
                    new EmptyProduct()
                    {
                            Name = new ProductName()
                        {
                            UserUpdatedId = Guid.CreateVersion7(),
                            Updated = DateTime.UtcNow
                        },
                        Prices =
                        [
                            new  ProductPrice()
                            {
                                Id =Guid.CreateVersion7(),
                                Updated = DateTime.UtcNow,
                            }
                        ]
                    },
               ]
            };

            _mockGet.Setup(x => x.GetProductName(It.IsAny<Guid>()));
            _mockGet.Setup(x => x.GetProductPrice(It.IsAny<Guid>()));

            var result = await _inventoryProductsRequests.Updates(products, false, default);

            Assert.IsType<Ok>(result);

        }
        [Fact]
        public async Task UpdateEmptyProducts_OnConflict()
        {
            EmptyProducts products = new()
            {
                Products =
                 [
                    new EmptyProduct()
                    {
                            Name = new ProductName()
                        {
                            UserUpdatedId = Guid.CreateVersion7(),
                            Updated = DateTime.UtcNow
                        },
                        Prices =
                        [
                            new  ProductPrice()
                            {
                                Id =Guid.CreateVersion7(),
                                Updated = DateTime.UtcNow,
                            }
                        ]
                    },
                    new EmptyProduct()
                    {
                            Name = new ProductName()
                        {
                            UserUpdatedId = Guid.CreateVersion7(),
                            Updated = DateTime.UtcNow
                        },
                        Prices =
                        [
                            new  ProductPrice()
                            {
                                Id =Guid.CreateVersion7(),
                                Updated = DateTime.UtcNow,
                            }
                        ]
                    },
               ]
            };

            _mockGet.Setup(x => x.GetProductName(It.IsAny<Guid>())).ReturnsAsync(products.Products[0].Name);
            _mockGet.Setup(x => x.GetProductPrice(It.IsAny<Guid>()));

            var result = await _inventoryProductsRequests.Updates(products, false, default);

            Assert.IsType<Conflict<IList<UpdateDifference>>>(result);

        }
    }
}
