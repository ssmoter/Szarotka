using DataBase.Model;
using Microsoft.Data.Sqlite;

namespace DataBase.Mappers;

public partial class DbMappers
{
    // Ordinals dla TableInfo (klasa nie dziedziczy po BaseOrdinals)
    public class TableInfoOrdinals
    {
        public int Cid { get; }
        public int Name { get; }
        public int Type { get; }
        public int NotNull { get; }
        public int DefaultValue { get; }
        public int PrimaryKey { get; }

        public TableInfoOrdinals(SqliteDataReader reader)
        {
            Cid = reader.GetOptionalOrdinal(nameof(TableInfo.Cid));
            Name = reader.GetOptionalOrdinal(nameof(TableInfo.Name));
            Type = reader.GetOptionalOrdinal(nameof(TableInfo.Type));
            NotNull = reader.GetOptionalOrdinal(nameof(TableInfo.NotNull));
            DefaultValue = reader.GetOptionalOrdinal(nameof(TableInfo.dflt_value));
            PrimaryKey = reader.GetOptionalOrdinal(nameof(TableInfo.pk));
        }
    }

    private static void InitTableInfoMappers()
    {
        _registry.Add(typeof(TableInfo), (Func<SqliteDataReader, object, TableInfo>)((reader, ordinalsObj) =>
        {
            ArgumentNullException.ThrowIfNull(reader);
            ArgumentNullException.ThrowIfNull(ordinalsObj);

            if (ordinalsObj is not TableInfoOrdinals ords)
                throw new ArgumentException("Invalid ordinals object for TableInfo mapper", nameof(ordinalsObj));

            var ti = new TableInfo();

            if (ords.Cid >= 0 && !reader.IsDBNull(ords.Cid)) ti.Cid = reader.GetInt32(ords.Cid); else ti.Cid = 0;
            if (ords.Name >= 0 && !reader.IsDBNull(ords.Name)) ti.Name = reader.GetString(ords.Name); else ti.Name = string.Empty;
            if (ords.Type >= 0 && !reader.IsDBNull(ords.Type)) ti.Type = reader.GetString(ords.Type); else ti.Type = string.Empty;
            if (ords.NotNull >= 0 && !reader.IsDBNull(ords.NotNull)) ti.NotNull = reader.GetInt32(ords.NotNull); else ti.NotNull = 0;
            if (ords.DefaultValue >= 0 && !reader.IsDBNull(ords.DefaultValue)) ti.dflt_value = reader.GetString(ords.DefaultValue); else ti.dflt_value = string.Empty;
            if (ords.PrimaryKey >= 0 && !reader.IsDBNull(ords.PrimaryKey)) ti.pk = reader.GetInt32(ords.PrimaryKey); else ti.pk = 0;

            return ti;
        }));
    }
}
