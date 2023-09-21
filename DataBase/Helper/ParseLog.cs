using DataBase.Model;
using DataBase.Pages.Log;

namespace DataBase.Helper
{
    internal static class ParseLog
    {
        internal static LogM ParseAsLogM(this LogsModel model)
        {
            var log = new LogM()
            {
                Id = model.Id,
                Message = model.Message,
                StackTrace = model.StackTrace,
                Created = model.CreatedDateTime
            };

            return log;
        }
        internal static LogsModel ParseAsLog(this LogM model)
        {
            var log = new LogsModel()
            {
                Id = model.Id,
                Message = model.Message,
                StackTrace = model.StackTrace,
                CreatedDateTime = model.Created
            };

            return log;
        }
    }
}
