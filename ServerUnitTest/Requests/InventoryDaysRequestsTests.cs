using DataBase.Data;
using DataBase.Data.Get;
using DataBase.Data.Save;
using DataBase.Model;
using DataBase.Model.EntitiesInventory;
using DataBase.Model.EntitiesServer;
using DataBase.Service;

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
        private readonly Mock<ISaveInventoryAoT> _mockSave;
        private readonly Mock<IUpdateLogService> _mockUpdateLog;
        public InventoryDaysRequestsTests()
        {
            _mockDb = new Mock<IAccessDataBase>();
            _mockGet = new Mock<IGetInventoryAoT>();
            _mockSave = new Mock<ISaveInventoryAoT>();
            _mockUpdateLog = new Mock<IUpdateLogService>();
            _inventoryDayRequests = new InventoryDayRequests(_mockDb.Object, _mockGet.Object, _mockSave.Object, _mockUpdateLog.Object);
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



        [Fact]
        public async Task DaysSave_ShouldGet()
        {
            var id = Guid.CreateVersion7();
            Day day = new()
            {
                Id = id,
                UserUpdatedId = id,
                Created = DateTime.UtcNow
            };
            Day[] days = [day, day];
            var result = await _inventoryDayRequests.SaveDays(days);

            Assert.IsType<Created>(result);
        }
        [Fact]
        public async Task DaysSave_ShouldGet_Conflict()
        {
            var id = Guid.CreateVersion7();
            Day day = new()
            {
                Id = id,
                UserUpdatedId = id,
                Created = DateTime.UtcNow,
                UpdatedTicks = 10,
            };

            Day[] days = [new() { Id = id, UpdatedTicks = 9 }, day];

            _mockGet.Setup(x => x.Days(It.IsAny<string>(), It.IsAny<object[]>())).ReturnsAsync([new Day() { Id = id, UpdatedTicks = 9 }]);


            var result = await _inventoryDayRequests.SaveDays(days);

            Assert.IsType<Conflict<List<UpdateDifference>>>(result);
        }

        [Fact]
        public async Task DaysSave_ShouldBadRequest_Empty()
        {

            var result = _inventoryDayRequests.SaveDays(null!);

            await Assert.ThrowsAsync<ArgumentNullException>(async () => { await result; });
        }

    }
}
