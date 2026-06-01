using DataBase.Data;
using DataBase.Data.Get;
using DataBase.Data.Save;
using DataBase.Model;
using DataBase.Model.EntitiesInventory;
using DataBase.Service;

using Microsoft.AspNetCore.Http.HttpResults;

using Moq;

using Server.Requests;

namespace ServerUnitTest.Requests
{
    public class InventoryDayRequestsTests
    {

        private readonly Mock<IGetInventoryAoT> _mockGet;
        private readonly Mock<IAccessDataBase> _mockDb;
        private readonly Mock<ISaveInventoryAoT> _mockSave;
        private readonly Mock<IUpdateLogService> _mockUpdateLog;
        private readonly InventoryDayRequests _inventoryDayRequests;

        public InventoryDayRequestsTests()
        {
            _mockDb = new Mock<IAccessDataBase>();
            _mockGet = new Mock<IGetInventoryAoT>();
            _mockSave = new Mock<ISaveInventoryAoT>();
            _mockUpdateLog = new Mock<IUpdateLogService>();
            _inventoryDayRequests = new InventoryDayRequests(_mockDb.Object, _mockGet.Object, _mockSave.Object, _mockUpdateLog.Object);
        }

        [Fact]
        public async Task DayId_ShouldGet()
        {
            var id = Guid.CreateVersion7();
            List<Day> days = [];
            days.Add(new());

            _mockGet.Setup(x => x.Days(It.IsAny<string>(), It.IsAny<object>())).ReturnsAsync(days);

            var result = await _inventoryDayRequests.GetDay(id.ToString());

            Assert.IsType<Ok<Day>>(result);
        }
        [Fact]
        public async Task DayId_ShouldNotFound()
        {
            var id = Guid.CreateVersion7();
            List<Day> days = [];

            _mockGet.Setup(x => x.Days(It.IsAny<string>(), It.IsAny<object>())).ReturnsAsync(days);

            var result = await _inventoryDayRequests.GetDay(id.ToString());

            Assert.IsType<NotFound>(result);
        }
        [Fact]
        public async Task DayId_ShouldBadRequest()
        {
            var id = "asd";
            List<Day> days = [];

            _mockGet.Setup(x => x.Days(It.IsAny<string>(), It.IsAny<object>())).ReturnsAsync(days);

            var result = await _inventoryDayRequests.GetDay(id.ToString());

            Assert.IsType<BadRequest<string>>(result);
        }



        [Fact]
        public async Task DaySelectedDateString_ShouldGet()
        {
            var id = DateTime.Now.ToShortDateString();
            List<Day> days = [];
            days.Add(new());

            _mockGet.Setup(x => x.Days(It.IsAny<string>(), It.IsAny<object[]>())).ReturnsAsync(days);

            var result = await _inventoryDayRequests.GetDay(id, Guid.Empty.ToString());

            Assert.IsType<Ok<Day>>(result);
            Assert.NotNull(((Ok<Day>)result).Value);
        }
        [Fact]
        public async Task DaySelectedDateString_ShouldNotFound()
        {
            var id = DateTime.Now.ToShortDateString();
            List<Day> days = [];

            _mockGet.Setup(x => x.Days(It.IsAny<string>(), It.IsAny<object[]>())).ReturnsAsync(days);

            var result = await _inventoryDayRequests.GetDay(id, Guid.Empty.ToString());

            Assert.IsType<NotFound>(result);
        }
        [Fact]
        public async Task DaySelectedDateString_ShouldBadRequest_Empty()
        {
            var id = "";
            List<Day> days = [];

            _mockGet.Setup(x => x.Days(It.IsAny<string>(), It.IsAny<object>())).ReturnsAsync(days);

            var result = await _inventoryDayRequests.GetDay(id, Guid.Empty.ToString());

            Assert.IsType<BadRequest<string>>(result);
        }
        [Fact]
        public async Task DaySelectedDateString_ShouldBadRequest_WrongFormat()
        {
            var id = "asd";
            List<Day> days = [];

            _mockGet.Setup(x => x.Days(It.IsAny<string>(), It.IsAny<object>())).ReturnsAsync(days);

            var result = await _inventoryDayRequests.GetDay(id, Guid.Empty.ToString());

            Assert.IsType<BadRequest<string>>(result);
        }


        [Fact]
        public async Task DaySave_ShouldGet()
        {
            var id = Guid.CreateVersion7();
            Day day = new()
            {
                Id = id,
                UserUpdatedId = id,
                Created = DateTime.UtcNow
            };

            var result = await _inventoryDayRequests.SaveDay(day);

            Assert.IsType<Created>(result);
        }
        [Fact]
        public async Task DaySave_ShouldGet_Conflict()
        {
            var id = Guid.CreateVersion7();
            Day day = new()
            {
                Id = id,
                UserUpdatedId = id,
                Created = DateTime.UtcNow,
                UpdatedTicks = 10,
            };


            _mockGet.Setup(x => x.Days(It.IsAny<string>(), It.IsAny<object[]>())).ReturnsAsync([new Day() { Id = id, UpdatedTicks = 9 }]);


            var result = await _inventoryDayRequests.SaveDay(day);

            Assert.IsType<Conflict<Day>>(result);
        }

        [Fact]
        public async Task DaySave_ShouldBadRequest_Empty()
        {
            Day? day = null;

            _mockGet.Setup(x => x.Days(It.IsAny<string>(), It.IsAny<object>()));

            var result = _inventoryDayRequests.SaveDay(day);

            await Assert.ThrowsAsync<ArgumentNullException>(async () => { await result; });
        }


    }
}
