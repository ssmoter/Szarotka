using DataBase.Helper;

using Microsoft.Data.Sqlite;


namespace DataBase.Mappers
{
    public partial class DbMappers
    {
        public class HelperTableOrdinals
        {
            public int Id { get; }
            public int Name { get; }
            public int Value { get; }
            public int CreatedTicks { get; }
            public int UpdatedTicks { get; }
            public int IsDelete { get; }
            public int UserCreatedId { get; }
            public int UserUpdatedId { get; }
            public HelperTableOrdinals(SqliteDataReader reader)
            {
                Id = reader.GetOptionalOrdinal(nameof(HelperTable.Id));
                Name = reader.GetOptionalOrdinal(nameof(HelperTable.Name));
                Value = reader.GetOptionalOrdinal(nameof(HelperTable.Value));
                CreatedTicks = reader.GetOptionalOrdinal(nameof(HelperTable.CreatedTicks));
                UpdatedTicks = reader.GetOptionalOrdinal(nameof(HelperTable.UpdatedTicks));
                IsDelete = reader.GetOptionalOrdinal(nameof(HelperTable.IsDelete));
                UserCreatedId = reader.GetOptionalOrdinal(nameof(HelperTable.UserCreatedId));
                UserUpdatedId = reader.GetOptionalOrdinal(nameof(HelperTable.UserUpdatedId));
            }
        }
        private static void InitHelperTableMappers()
        {
            _registry.Add(typeof(HelperTable), (Func<SqliteDataReader, object, HelperTable>)((reader, ordinalsObj) =>
            {
                ArgumentNullException.ThrowIfNull(reader);
                ArgumentNullException.ThrowIfNull(ordinalsObj);

                if (ordinalsObj is not HelperTableOrdinals ords)
                    throw new ArgumentException("Invalid ordinals object for HelperTable mapper", nameof(ordinalsObj));

                var ht = new HelperTable();
                if (ords.Id >= 0 && !reader.IsDBNull(ords.Id))
                {
                    var id = reader.GetString(ords.Id);
                    ht.Id = Guid.TryParse(id, out var guid) ? guid : Guid.Empty;
                }
                else ht.Id = Guid.Empty;
                if (ords.Name >= 0 && !reader.IsDBNull(ords.Name)) ht.Name = reader.GetString(ords.Name); else ht.Name = string.Empty;
                if (ords.Value >= 0 && !reader.IsDBNull(ords.Value)) ht.Value = reader.GetString(ords.Value); else ht.Value = string.Empty;
                if (ords.CreatedTicks >= 0 && !reader.IsDBNull(ords.CreatedTicks)) ht.CreatedTicks = reader.GetInt64(ords.CreatedTicks); else ht.CreatedTicks = 0;
                if (ords.UpdatedTicks >= 0 && !reader.IsDBNull(ords.UpdatedTicks)) ht.UpdatedTicks = reader.GetInt64(ords.UpdatedTicks); else ht.UpdatedTicks = 0;
                if (ords.IsDelete >= 0 && !reader.IsDBNull(ords.IsDelete)) ht.IsDelete = reader.GetBoolean(ords.IsDelete); else ht.IsDelete = false;
                if (ords.UserCreatedId >= 0 && !reader.IsDBNull(ords.UserCreatedId))
                {
                    var guid = reader.GetString(ords.UserCreatedId);
                    ht.UserCreatedId = Guid.TryParse(guid, out var guid2) ? guid2 : Guid.Empty;
                }
                else ht.UserCreatedId = Guid.Empty;
                if (ords.UserUpdatedId >= 0 && !reader.IsDBNull(ords.UserUpdatedId))
                {
                    var guid = reader.GetString(ords.UserUpdatedId);
                    ht.UserUpdatedId = Guid.TryParse(guid, out var guid2) ? guid2 : Guid.Empty;
                }
                else ht.UserUpdatedId = Guid.Empty;

                return ht;
            }));
        }
    }
}

