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
            Id = reader.GetOptionalOrdinal(nameof(LogsModel.Id));
            StackTrace = reader.GetOptionalOrdinal(nameof(LogsModel.StackTrace));
            Message = reader.GetOptionalOrdinal(nameof(LogsModel.Message));
            Created = reader.GetOptionalOrdinal(nameof(LogsModel.Created));
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

            if (ords.Id >= 0 &&!reader.IsDBNull(ords.Id)) lm.Id = reader.GetInt32(ords.Id); else lm.Id = 0;
            if (ords.StackTrace >= 0 &&!reader.IsDBNull(ords.StackTrace)) lm.StackTrace = reader.GetString(ords.StackTrace); else lm.StackTrace = string.Empty;
            if (ords.Message >= 0 &&!reader.IsDBNull(ords.Message)) lm.Message = reader.GetString(ords.Message); else lm.Message = string.Empty;
            if (ords.Created >= 0 && !reader.IsDBNull(ords.Created)) lm.Created = reader.GetString(ords.Created); else lm.Created = string.Empty;

            return lm;
        }));
    }
}
