using DataBase.Model;
using Shared.Pages.Log;

namespace Shared.Helper
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
        internal static void ParseAsLogM(this LogsModel from, LogM to)
        {
            if (from is null)
            {
                return;
            }
            to ??= new LogM();

            to.Id = from.Id;
            to.Message = from.Message;
            to.StackTrace = from.StackTrace;
            to.Created = from.CreatedDateTime;


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

        internal static void ParseAsLog(this LogM from, LogsModel to)
        {
            if (from is null)
            {
                return;

            }
            to ??= new LogsModel();

            to.Id = from.Id;
            to.Message = from.Message;
            to.StackTrace = from.StackTrace;
            to.CreatedDateTime = from.Created;
        }



    }
}
