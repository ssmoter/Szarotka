using DataBase.Model;
using Microsoft.Data.Sqlite;

namespace DataBase.Mappers;

public partial class DbMappers
{
    public class UpdateLogOrdinals : BaseOrdinals
    {
        public int UpdateEnum { get; }
        public int UpdateId { get; }
        public int JsonUpdate { get; }
        public int IsServer { get; }

        public UpdateLogOrdinals(SqliteDataReader reader) : base(reader)
        {
            UpdateEnum = reader.GetOrdinal(nameof(UpdateLog.UpdateEnum));
            UpdateId = reader.GetOrdinal(nameof(UpdateLog.UpdateId));
            JsonUpdate = reader.GetOrdinal(nameof(UpdateLog.JsonUpdate));
            IsServer = reader.GetOrdinal(nameof(UpdateLog.IsServer));
        }
    }

    private static void InitUpdateLogMappers()
    {
        _registry.Add(typeof(UpdateLog), (Func<SqliteDataReader, object, UpdateLog>)((reader, ordinalsObj) =>
        {
            ArgumentNullException.ThrowIfNull(reader);
            ArgumentNullException.ThrowIfNull(ordinalsObj);

            if (ordinalsObj is not UpdateLogOrdinals ords)
                throw new ArgumentException("Invalid ordinals object for UpdateLog mapper", nameof(ordinalsObj));

            var ul = new UpdateLog();

            if (!reader.IsDBNull(ords.UpdateEnum)) ul.UpdateEnum = (UpdateEnum)reader.GetInt32(ords.UpdateEnum);
            else ul.UpdateEnum = default;

            if (!reader.IsDBNull(ords.UpdateId)) ul.UpdateId = reader.GetString(ords.UpdateId); else ul.UpdateId = string.Empty;

            if (!reader.IsDBNull(ords.JsonUpdate)) ul.JsonUpdate = reader.GetString(ords.JsonUpdate); else ul.JsonUpdate = string.Empty;

            if (!reader.IsDBNull(ords.IsServer)) ul.IsServer = reader.GetInt32(ords.IsServer) == 1; else ul.IsServer = null;

            MapBaseFields(reader, ul, ords);

            return ul;
        }));
    }
}
