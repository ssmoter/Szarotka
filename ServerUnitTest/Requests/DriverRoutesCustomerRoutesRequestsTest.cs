using DataBase.Data;
using DataBase.Data.Get;
using DataBase.Model.EntitiesRoutes;

using Microsoft.AspNetCore.Http.HttpResults;

using Moq;

using Server.Requests;

namespace ServerUnitTest.Requests
{
    public class DriverRoutesCustomerRoutesRequestsTest
    {
        private readonly Mock<IGetDriverRoutesAoT> _mockGet;
        private readonly Mock<IAccessDataBase> _mockDb;
        private readonly DriverRoutesCustomerRoutesRequests _driverRoutesCustomerRoutesRequests;

        public DriverRoutesCustomerRoutesRequestsTest()
        {
            _mockDb = new Mock<IAccessDataBase>();
            _mockGet = new Mock<IGetDriverRoutesAoT>();
            _driverRoutesCustomerRoutesRequests = new DriverRoutesCustomerRoutesRequests(_mockDb.Object, _mockGet.Object);
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
                _driverRoutesCustomerRoutesRequests.GetCustomers(id.ToString(), [], token.Token)
            );
        }
    }
}
