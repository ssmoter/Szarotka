using DataBase.Data;
using DataBase.Model;
using DataBase.Service;

using Microsoft.AspNetCore.Http.HttpResults;

using Moq;

using Server.Requests;

namespace ServerUnitTest.Requests
{
    public class UpdateLogRequestsTests
    {
        private readonly Mock<IAccessDataBaseAoT> _mockDb;
        private readonly Mock<IUpdateLogService> _mockUpdateLogsService;
        private readonly UpdateLogRequests _updateLogRequests;
        public UpdateLogRequestsTests()
        {
            _mockDb = new Mock<IAccessDataBaseAoT>();
            _mockUpdateLogsService = new Mock<IUpdateLogService>();

            _updateLogRequests = new UpdateLogRequests(_mockDb.Object, _mockUpdateLogsService.Object);
        }


        [Fact]
        public async Task UpdateLog_ShouldGet()
        {
            var id = Guid.CreateVersion7();
            var logs = new List<UpdateLog>
            {
                new() { Id = id }
            };

            _mockUpdateLogsService.Setup(x => x.Select(id)).ReturnsAsync(logs);

            var result = await _updateLogRequests.GetLogs(id.ToString());


            Assert.IsType<Ok<IList<UpdateLog>>>(result);
        }
        [Fact]
        public async Task UpdateLog_Canceled()
        {
            var id = Guid.CreateVersion7();
            var logs = new List<UpdateLog>
            {
                new() { Id = id }
            };
            var source = new CancellationTokenSource();
            source.Cancel();

            _mockUpdateLogsService.Setup(x => x.Select(id)).ReturnsAsync(logs);

            var result = _updateLogRequests.GetLogs(id.ToString(), source.Token);
            await Assert.ThrowsAsync<OperationCanceledException>(() => result);
        }
        [Fact]
        public async Task UpdateLog_Throw()
        {
            var id = Guid.CreateVersion7();
            var logs = new List<UpdateLog>
            {
                new() { Id = id }
            };

            _mockUpdateLogsService.Setup(x => x.Select(id)).ReturnsAsync(logs);

            var result = await _updateLogRequests.GetLogs("");

            Assert.IsType<BadRequest<string>>(result);
        }
    }
}
