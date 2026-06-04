using DataBase.Data;
using DataBase.Data.Get;
using DataBase.Data.Save;
using DataBase.Model;
using DataBase.Model.EntitiesRoutes;
using DataBase.Service;

using Microsoft.AspNetCore.Http.HttpResults;

using Moq;

using Server.Requests;

namespace ServerUnitTest.Requests
{
    public class DriverRoutesCustomerRoutesRequestsTest
    {
        private readonly Mock<IGetDriverRoutesAoT> _mockGet;
        private readonly Mock<IAccessDataBase> _mockDb;
        private readonly Mock<ISaveDriverRoutesAoT> _mockSave;
        private readonly Mock<IUpdateLogService> _mockUpdateLogService;

        private readonly DriverRoutesCustomerRoutesRequests _driverRoutesCustomerRoutesRequests;

        public DriverRoutesCustomerRoutesRequestsTest()
        {
            _mockDb = new Mock<IAccessDataBase>();
            _mockGet = new Mock<IGetDriverRoutesAoT>();
            _mockSave = new Mock<ISaveDriverRoutesAoT>();
            _mockUpdateLogService = new Mock<IUpdateLogService>();
            _driverRoutesCustomerRoutesRequests = new DriverRoutesCustomerRoutesRequests(
                _mockDb.Object,
                _mockGet.Object,
                _mockSave.Object,
                _mockUpdateLogService.Object);
        }

        [Fact]
        public async Task CustomerRouteId_ShouldGet()
        {
            var id = Guid.CreateVersion7();
            List<CustomerRoutes> customers = [];
            customers.Add(new());
            customers.Add(new());

            _mockGet.Setup(x => x.CustomerRoutes(It.IsAny<string>(), It.IsAny<object>())).ReturnsAsync(customers);

            var result = await _driverRoutesCustomerRoutesRequests.GetCustomer(id.ToString());

            Assert.IsType<Ok<CustomerRoutes>>(result);
        }
        [Fact]
        public async Task CustomerRouteId_GuidEmpty()
        {
            var id = "";
            List<CustomerRoutes> customers = [];
            customers.Add(new());
            customers.Add(new());

            _mockGet.Setup(x => x.CustomerRoutes(It.IsAny<string>(), It.IsAny<object>())).ReturnsAsync(customers);

            var result = await _driverRoutesCustomerRoutesRequests.GetCustomer(id.ToString());

            Assert.IsType<BadRequest<string>>(result);
        }
        [Fact]
        public async Task CustomerRouteId_NoFound()
        {
            var id = Guid.CreateVersion7();
            List<CustomerRoutes> customers = [];

            _mockGet.Setup(x => x.CustomerRoutes(It.IsAny<string>(), It.IsAny<object>())).ReturnsAsync(customers);

            var result = await _driverRoutesCustomerRoutesRequests.GetCustomer(id.ToString());

            Assert.IsType<NotFound>(result);
        }




        [Fact]
        public async Task CustomerRoutes_ShouldGet_AllPointsFromRoutesId()
        {
            var id = Guid.CreateVersion7();
            List<CustomerRoutes> customers = [];
            customers.Add(new());
            customers.Add(new());

            _mockGet.Setup(x => x.CustomerRoutes(It.IsAny<string>(), It.IsAny<object[]>())).ReturnsAsync(customers);

            var result = await _driverRoutesCustomerRoutesRequests.GetCustomers(id.ToString(), []);

            Assert.IsType<Ok<IList<CustomerRoutes>>>(result);
        }
        [Fact]
        public async Task CustomerRoutes_ShouldGet_NoRoutesId()
        {
            var id = Guid.CreateVersion7();
            List<CustomerRoutes> customers = [];
            customers.Add(new());
            customers.Add(new());

            _mockGet.Setup(x => x.CustomerRoutes(It.IsAny<string>(), It.IsAny<object[]>())).ReturnsAsync(customers);

            var result = await _driverRoutesCustomerRoutesRequests.GetCustomers("", []);

            Assert.IsType<BadRequest<string>>(result);
        }
        [Fact]
        public async Task CustomerRoutes_ShouldGet_NoFound()
        {
            var id = Guid.CreateVersion7();
            List<CustomerRoutes> customers = [];

            _mockGet.Setup(x => x.CustomerRoutes(It.IsAny<string>(), It.IsAny<object[]>())).ReturnsAsync(customers);

            var result = await _driverRoutesCustomerRoutesRequests.GetCustomers(id.ToString(), []);

            Assert.IsType<NotFound>(result);
        }
        [Fact]
        public async Task CustomerRoutes_Throw_OperationCanceledException()
        {
            var id = Guid.CreateVersion7();
            List<CustomerRoutes> customers = [];

            _mockGet.Setup(x => x.CustomerRoutes(It.IsAny<string>(), It.IsAny<object[]>())).ReturnsAsync(customers);

            var token = new CancellationTokenSource();
            token.Cancel();

            await Assert.ThrowsAsync<OperationCanceledException>(() =>
                _driverRoutesCustomerRoutesRequests.GetCustomers(id.ToString(), [],false, token.Token)
            );
        }

        [Fact]
        public async Task CustomerRoutes_ShouldGet_AllPointsFromRoutesIds()
        {
            var id = Guid.CreateVersion7();
            var id1 = Guid.CreateVersion7();
            List<CustomerRoutes> customers = [];
            customers.Add(new() { Id = id });
            customers.Add(new() { Id = id1 });

            _mockGet.Setup(x => x.CustomerRoutes(It.IsAny<string>(), It.IsAny<object[]>())).ReturnsAsync(customers);

            var result = await _driverRoutesCustomerRoutesRequests.GetCustomers([id.ToString(), id1.ToString()]);

            Assert.IsType<Ok<List<CustomerRoutes>>>(result);
        }

        [Fact]
        public async Task UpdateCustomer_ShouldUpdate()
        {
            // Arrange
            var customer = new CustomerRoutes
            {
                Id = Guid.CreateVersion7(),
                Name = "Test Customer",
                Description = "Test Description",
                PhoneNumber = "123456789",
                RoutesId = Guid.CreateVersion7(),
                Longitude = 10.0,
                Latitude = 20.0
            };
            var forceUpdate = true;

            _mockSave.Setup(x => x.SaveCustomerRoutes(It.IsAny<CustomerRoutes>(), It.IsAny<byte[]>(), It.IsAny<bool>()))
                .Returns(Task.CompletedTask);

            _mockUpdateLogService.Setup(x => x.Insert(It.IsAny<UpdateLog>()))
                .ReturnsAsync(new UpdateLog());

            // Act
            var result = await _driverRoutesCustomerRoutesRequests.UpdateCustomer(customer, forceUpdate);

            // Assert
            Assert.IsType<Created<UpdateLog>>(result);
            _mockSave.Verify(x => x.SaveCustomerRoutes(It.Is<CustomerRoutes>(c => c == customer), It.IsAny<byte[]>(), It.IsAny<bool>()), Times.Once);
            _mockUpdateLogService.Verify(x => x.Insert(It.IsAny<UpdateLog>()), Times.Once);
        }

        [Fact]
        public async Task UpdateCustomers_ShouldUpdate()
        {
            // Arrange
            var customers = new List<CustomerRoutes>
            {
                new() {
                    Id = Guid.CreateVersion7(),
                    Name = "Customer 1",
                    Description = "Desc 1",
                    PhoneNumber = "111111111",
                    RoutesId = Guid.CreateVersion7(),
                    Longitude = 1.0,
                    Latitude = 2.0
                },
                new() {
                    Id = Guid.CreateVersion7(),
                    Name = "Customer 2",
                    Description = "Desc 2",
                    PhoneNumber = "222222222",
                    RoutesId = Guid.CreateVersion7(),
                    Longitude = 3.0,
                    Latitude = 4.0
                }
            };
            var forceUpdate = true;

            _mockSave.Setup(x => x.SaveCustomerRoutes(It.IsAny<CustomerRoutes>(), It.IsAny<byte[]>(), It.IsAny<bool>()))
                .Returns(Task.CompletedTask);

            _mockUpdateLogService.Setup(x => x.Insert(It.IsAny<UpdateLog>()))
                .ReturnsAsync(new UpdateLog());
            // Act
            var result = await _driverRoutesCustomerRoutesRequests.UpdateCustomers(customers, forceUpdate);

            // Assert
            Assert.IsType<Created<UpdateLog>>(result);
            _mockSave.Verify(x => x.SaveCustomerRoutes(It.IsAny<CustomerRoutes>(), It.IsAny<byte[]>(), It.IsAny<bool>()), Times.Exactly(customers.Count));
            _mockUpdateLogService.Verify(x => x.Insert(It.IsAny<UpdateLog>()), Times.Exactly(customers.Count));
        }

    }
}
