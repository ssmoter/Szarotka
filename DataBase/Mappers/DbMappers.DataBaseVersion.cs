using DataBase.Model;
using Microsoft.Data.Sqlite;

namespace DataBase.Mappers;

public partial class DbMappers
{
    // Ordinals dla DataBaseVersion (nie dziedziczy po BaseOrdinals, bo klasa nie jest BaseEntities)
    public class DataBaseVersionOrdinals
    {
        public int Id { get; }
        public int DataBase { get; }
        public int Inventory { get; }
        public int DriversRoutes { get; }
        public int LastBackup { get; }

        public DataBaseVersionOrdinals(SqliteDataReader reader)
        {
            Id = reader.GetOptionalOrdinal(nameof(DataBaseVersion.Id));
            DataBase = reader.GetOptionalOrdinal(nameof(DataBaseVersion.DataBase));
            Inventory = reader.GetOptionalOrdinal(nameof(DataBaseVersion.Inventory));
            DriversRoutes = reader.GetOptionalOrdinal(nameof(DataBaseVersion.DriversRoutes));
            LastBackup = reader.GetOptionalOrdinal(nameof(DataBaseVersion.LastBackup));
        }
    }

    private static void InitDataBaseVersionMappers()
    {
        _registry.Add(typeof(DataBaseVersion), (Func<SqliteDataReader, object, DataBaseVersion>)((reader, ordinalsObj) =>
        {
            ArgumentNullException.ThrowIfNull(reader);
            ArgumentNullException.ThrowIfNull(ordinalsObj);

            if (ordinalsObj is not DataBaseVersionOrdinals ords)
                throw new ArgumentException("Invalid ordinals object for DataBaseVersion mapper", nameof(ordinalsObj));

            var dv = new DataBaseVersion();

            if (ords.Id >= 0 &&!reader.IsDBNull(ords.Id)) dv.Id = reader.GetInt32(ords.Id); else dv.Id = 0;
            if (ords.DataBase >= 0 &&!reader.IsDBNull(ords.DataBase)) dv.DataBase = reader.GetInt32(ords.DataBase); else dv.DataBase = 0;
            if (ords.Inventory >= 0 &&!reader.IsDBNull(ords.Inventory)) dv.Inventory = reader.GetInt32(ords.Inventory); else dv.Inventory = 0;
            if (ords.DriversRoutes >= 0 &&!reader.IsDBNull(ords.DriversRoutes)) dv.DriversRoutes = reader.GetInt32(ords.DriversRoutes); else dv.DriversRoutes = 0;
            if (ords.LastBackup >= 0 && !reader.IsDBNull(ords.LastBackup)) dv.LastBackup = reader.GetInt64(ords.LastBackup); else dv.LastBackup = 0L;

            return dv;
        }));
    }
}
