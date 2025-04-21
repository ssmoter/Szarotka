using DataBase.Data;
using DataBase.Data.Get;
using DataBase.Model.EntitiesInventory;

using Microsoft.AspNetCore.Http.HttpResults;

using Moq;

using Server.Requests;

namespace ServerUnitTest.Requests
{
    public class InventoryDayRequestsTests
    {

        private readonly Mock<IGetInventoryAoT> _mockGet;
        private readonly Mock<IAccessDataBase> _mockDb;
        private readonly InventoryDayRequests _inventoryDayRequests;

        public InventoryDayRequestsTests()
        {
            _mockDb = new Mock<IAccessDataBase>();
            _mockGet = new Mock<IGetInventoryAoT>();
            _inventoryDayRequests = new InventoryDayRequests(_mockDb.Object, _mockGet.Object);
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


    }
}
