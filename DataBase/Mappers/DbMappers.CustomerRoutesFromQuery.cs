using DataBase.Data.Get;
using DataBase.Model.EntitiesRoutes;

using Microsoft.Data.Sqlite;
namespace DataBase.Mappers;

public partial class DbMappers
{
    // Ordinals dla CustomerRoutesFromQuery (dziedziczy CustomerRoutesOrdinals)
    public class CustomerRoutesFromQueryOrdinals : CustomerRoutesOrdinals
    {
        public int JsonDayOfWeek { get; }
        public int JsonAddress { get; }

        public CustomerRoutesFromQueryOrdinals(SqliteDataReader reader) : base(reader)
        {
            JsonDayOfWeek = reader.GetOptionalOrdinal(nameof(GetDriverRoutesAoT.CustomerRoutesFromQuery.JsonDayOfWeek));
            JsonAddress = reader.GetOptionalOrdinal(nameof(GetDriverRoutesAoT.CustomerRoutesFromQuery.JsonAddress));
        }
    }

    private static void InitCustomerRoutesFromQueryMappers()
    {
        _registry.Add(typeof(GetDriverRoutesAoT.CustomerRoutesFromQuery), (Func<SqliteDataReader, object, GetDriverRoutesAoT.CustomerRoutesFromQuery>)((reader, ordinalsObj) =>
        {
            ArgumentNullException.ThrowIfNull(reader);
            ArgumentNullException.ThrowIfNull(ordinalsObj);

            if (ordinalsObj is not CustomerRoutesFromQueryOrdinals ords)
                throw new ArgumentException("Invalid ordinals object for CustomerRoutesFromQuery mapper", nameof(ordinalsObj));

            var cr = new GetDriverRoutesAoT.CustomerRoutesFromQuery();

            // map fields from CustomerRoutesOrdinals
            if (ords.RoutesId >= 0 && !reader.IsDBNull(ords.RoutesId))
            {
                var rId = reader.GetString(ords.RoutesId);
                cr.RoutesId = string.IsNullOrEmpty(rId) ? Guid.Empty : new Guid(rId);
            }
            else
            {
                cr.RoutesId = Guid.Empty;
            }

            if (ords.Name >= 0 && !reader.IsDBNull(ords.Name)) cr.Name = reader.GetString(ords.Name); else cr.Name = string.Empty;
            if (ords.Description >= 0 && !reader.IsDBNull(ords.Description)) cr.Description = reader.GetString(ords.Description); else cr.Description = string.Empty;
            if (ords.PhoneNumber >= 0 && !reader.IsDBNull(ords.PhoneNumber)) cr.PhoneNumber = reader.GetString(ords.PhoneNumber); else cr.PhoneNumber = string.Empty;
            if (ords.Longitude >= 0 && !reader.IsDBNull(ords.Longitude)) cr.Longitude = reader.GetDouble(ords.Longitude); else cr.Longitude = 0d;
            if (ords.Latitude >= 0 && !reader.IsDBNull(ords.Latitude)) cr.Latitude = reader.GetDouble(ords.Latitude); else cr.Latitude = 0d;

            // Map JSON fields
            if (ords.JsonDayOfWeek >= 0 && !reader.IsDBNull(ords.JsonDayOfWeek)) cr.JsonDayOfWeek = reader.GetString(ords.JsonDayOfWeek); else cr.JsonDayOfWeek = string.Empty;
            if (ords.JsonAddress >= 0 && !reader.IsDBNull(ords.JsonAddress)) cr.JsonAddress = reader.GetString(ords.JsonAddress); else cr.JsonAddress = string.Empty;

            MapBaseFields(reader, cr, ords);

            return cr;
        }));
    }
}
