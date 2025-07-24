using DataBase.Service;

namespace DataBaseUnitTest.DataSave
{
    public class UpdateLogServiceTest
    {
        private readonly ITimeService _time;
        public UpdateLogServiceTest()
        {
            _time = new CurrentUtc();
        }

        [Fact]
        public async Task Insert_ShouldSetCreatedAndUpdatedAndCallExecuteAsync()
        {
            var db = await Helper.CreatedDataBaseUpdateLogForTest(nameof(Insert_ShouldSetCreatedAndUpdatedAndCallExecuteAsync));

            var updateLogs = new List<DataBase.Model.UpdateLog>();

            for (int i = 0; i < 10; i++)
            {
                var user = Guid.CreateVersion7();
                var log = new DataBase.Model.UpdateLog
                {
                    Id = Guid.CreateVersion7(),
                    UpdateEnum = DataBase.Model.UpdateEnum.CustomerRoutes,
                    UserCreatedId = user,
                    UserUpdatedId = user,
                    UpdateId = user.ToString(),
                    JsonUpdate = user.ToString(),
                };

                updateLogs.Add(log);
            }
            var updateService = new UpdateLogService(db, _time);
            for (int i = 0; i < updateLogs.Count; i++)
            {
                await updateService.Insert(updateLogs[i]);
            }

            var jsonInsert = System.Text.Json.JsonSerializer.Serialize(updateLogs.OrderBy(x => x.CreatedTicks));
            var dbLogs = await db.DataBaseAsync.Table<DataBase.Model.UpdateLog>().ToListAsync();
            var jsonDb = System.Text.Json.JsonSerializer.Serialize(dbLogs.OrderBy(x => x.CreatedTicks));

            Assert.Equal(jsonInsert, jsonDb);

        }


        [Fact]
        public async Task Select_ShouldSetCreatedAndUpdatedAndCallExecuteAsync()
        {
            var db = await Helper.CreatedDataBaseUpdateLogForTest(nameof(Select_ShouldSetCreatedAndUpdatedAndCallExecuteAsync));

            var updateLogs = new List<DataBase.Model.UpdateLog>();

            for (int i = 0; i < 10; i++)
            {
                var user = Guid.CreateVersion7();
                var log = new DataBase.Model.UpdateLog
                {
                    Id = user,
                    UpdateEnum = DataBase.Model.UpdateEnum.CustomerRoutes,
                    UserCreatedId = user,
                    UserUpdatedId = user,
                    UpdateId = user.ToString(),
                    JsonUpdate = user.ToString(),
                };

                updateLogs.Add(log);
            }
            var updateService = new UpdateLogService(db, _time);

            await db.DataBaseAsync.InsertAllAsync(updateLogs);


            var jsonInsert = System.Text.Json.JsonSerializer.Serialize(updateLogs.OrderBy(x => x.CreatedTicks));
            var dbLogs = await updateService.Select(updateLogs[0].Id);
            var jsonDb = System.Text.Json.JsonSerializer.Serialize(dbLogs.OrderBy(x => x.CreatedTicks));

            Assert.Equal(jsonInsert, jsonDb);
        }


        [Fact]
        public async Task SelectFirstFromServer_ShouldSetCreatedAndUpdatedAndCallExecuteAsync()
        {
            var db = await Helper.CreatedDataBaseUpdateLogForTest(nameof(SelectFirstFromServer_ShouldSetCreatedAndUpdatedAndCallExecuteAsync));

            var updateLogs = new List<DataBase.Model.UpdateLog>();

            for (int i = 0; i < 10; i++)
            {
                var user = Guid.CreateVersion7();
                var log = new DataBase.Model.UpdateLog
                {
                    Id = Guid.CreateVersion7(),
                    UpdateEnum = DataBase.Model.UpdateEnum.CustomerRoutes,
                    UserCreatedId = user,
                    UserUpdatedId = user,
                    UpdateId = user.ToString(),
                    JsonUpdate = user.ToString(),
                };

                if (i == 5)
                {
                    log.IsServer = true;
                }

                updateLogs.Add(log);
            }
            var updateService = new UpdateLogService(db, _time);

            await db.DataBaseAsync.InsertAllAsync(updateLogs);


            var jsonInsert = System.Text.Json.JsonSerializer.Serialize(updateLogs[5]);
            var dbLogs = await updateService.SelectFirst(true, DataBase.Model.UpdateEnum.CustomerRoutes, DataBase.Model.UpdateEnum.CustomerRoutes);
            var jsonDb = System.Text.Json.JsonSerializer.Serialize(dbLogs);

            Assert.Equal(jsonInsert, jsonDb);

        }
    }
}
