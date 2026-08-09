using DataBase.Data;
using DataBase.Data.MySqliteConnection;
using DataBase.Service;

using Moq;

namespace DataBaseUnitTest
{
    public class MockDb
    {
        public static IAccessDataBaseAoT CreateMockDb()
        {
            var mockDb = new Mock<IAccessDataBaseAoT>();
            var mockTimeService = new Mock<ITimeService>();
            var mockDbSync = new Mock<IMyDbConnection>();
            var mockDbAsync = new Mock<IMyDbAsyncConnection>();
            // Setup mock behaviors as needed
            mockTimeService.Setup(ts => ts.UtcNow()).Returns(DateTime.UtcNow);
            // Add more setups for mockDbSync and mockDbAsync if necessary
            return new AccessDataBaseAoT(mockDb.Object, mockDbSync.Object, mockDbAsync.Object, mockTimeService.Object);
        }
    }
}
