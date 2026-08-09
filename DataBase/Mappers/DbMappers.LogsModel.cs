using DataBase.Model;
using Microsoft.Data.Sqlite;

namespace DataBase.Mappers;

public partial class DbMappers
{
    // Ordinals dla LogsModel (klasa nie dziedziczy po BaseEntities)
    public class LogsModelOrdinals
    {
        public int Id { get; }
        public int StackTrace { get; }
        public int Message { get; }
        public int Created { get; }

        public LogsModelOrdinals(SqliteDataReader reader)
        {
            Id = reader.GetOrdinal(nameof(LogsModel.Id));
            StackTrace = reader.GetOrdinal(nameof(LogsModel.StackTrace));
            Message = reader.GetOrdinal(nameof(LogsModel.Message));
            Created = reader.GetOrdinal(nameof(LogsModel.Created));
        }
    }

    private static void InitLogsModelMappers()
    {
        _registry.Add(typeof(LogsModel), (Func<SqliteDataReader, object, LogsModel>)((reader, ordinalsObj) =>
        {
            ArgumentNullException.ThrowIfNull(reader);
            ArgumentNullException.ThrowIfNull(ordinalsObj);

            if (ordinalsObj is not LogsModelOrdinals ords)
                throw new ArgumentException("Invalid ordinals object for LogsModel mapper", nameof(ordinalsObj));

            var lm = new LogsModel();

            if (!reader.IsDBNull(ords.Id)) lm.Id = reader.GetInt32(ords.Id); else lm.Id = 0;
            if (!reader.IsDBNull(ords.StackTrace)) lm.StackTrace = reader.GetString(ords.StackTrace); else lm.StackTrace = string.Empty;
            if (!reader.IsDBNull(ords.Message)) lm.Message = reader.GetString(ords.Message); else lm.Message = string.Empty;
            if (!reader.IsDBNull(ords.Created)) lm.Created = reader.GetString(ords.Created); else lm.Created = string.Empty;

            return lm;
        }));
    }
}
