using DataBase.Data;
using DataBase.Data.Get;
using DataBase.Model.EntitiesInventory;

using Microsoft.AspNetCore.Http.HttpResults;

using Moq;

using Server.Requests;

namespace ServerUnitTest.Requests
{
    public class InventoryDaysRequestsTests
    {

        private readonly Mock<IGetInventoryAoT> _mockGet;
        private readonly Mock<IAccessDataBase> _mockDb;
        private readonly InventoryDayRequests _inventoryDayRequests;

        public InventoryDaysRequestsTests()
        {
            _mockDb = new Mock<IAccessDataBase>();
            _mockGet = new Mock<IGetInventoryAoT>();
            _inventoryDayRequests = new InventoryDayRequests(_mockDb.Object, _mockGet.Object);
        }

        [Fact]
        public async Task DaysId_ShouldGet()
        {
            List<Day> days = [];
            days.Add(new());
            days.Add(new());
            days.Add(new());

            _mockGet.Setup(x => x.Days(It.IsAny<string>(), It.IsAny<object[]>())).ReturnsAsync(days);

            var result = await _inventoryDayRequests.GetDays(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string[]>());

            Assert.IsType<Ok<IList<Day>>>(result);
        }
        [Fact]
        public async Task DaysId_OperationCanceledException()
        {
            List<Day> days = [];
            days.Add(new());
            days.Add(new());
            days.Add(new());

            _mockGet.Setup(x => x.Days(It.IsAny<string>(), It.IsAny<object[]>())).ReturnsAsync(days);

            var token = new CancellationTokenSource();
            token.Cancel();


            await Assert.ThrowsAsync<OperationCanceledException>(() =>
               _inventoryDayRequests.GetDays(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string[]>(), token.Token)
                  );
        }
    }
}
