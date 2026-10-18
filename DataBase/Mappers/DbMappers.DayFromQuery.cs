using DataBase.Data.Get;

using Microsoft.Data.Sqlite;
namespace DataBase.Mappers;

public partial class DbMappers
{
    // Ordinals dla DayFromQuery (dziedziczy DayOrdinals)
    public class DayFromQueryOrdinals : DayOrdinals
    {
        public int JsonProducts { get; }
        public int JsonCakes { get; }

        public DayFromQueryOrdinals(SqliteDataReader reader) : base(reader)
        {
            JsonProducts = reader.GetOptionalOrdinal(nameof(GetInventoryAoT.DayFromQuery.JsonProducts));
            JsonCakes = reader.GetOptionalOrdinal(nameof(GetInventoryAoT.DayFromQuery.JsonCakes));
        }
    }

    private static void InitDayFromQueryMappers()
    {
        _registry.Add(typeof(GetInventoryAoT.DayFromQuery), (Func<SqliteDataReader, object, GetInventoryAoT.DayFromQuery>)((reader, ordinalsObj) =>
        {
            ArgumentNullException.ThrowIfNull(reader);
            ArgumentNullException.ThrowIfNull(ordinalsObj);

            if (ordinalsObj is not DayFromQueryOrdinals ords)
                throw new ArgumentException("Invalid ordinals object for DayFromQuery mapper", nameof(ordinalsObj));

            var day = new GetInventoryAoT.DayFromQuery();            
            // Map fields from DayOrdinals
            if (ords.Description >= 0 && !reader.IsDBNull(ords.Description)) day.Description = reader.GetString(ords.Description); else day.Description = string.Empty;
            if (ords.SelectedDateString >= 0 && !reader.IsDBNull(ords.SelectedDateString)) day.SelectedDateString = reader.GetString(ords.SelectedDateString); else day.SelectedDateString = string.Empty;
            if (ords.SelectedDateTicks >= 0 && !reader.IsDBNull(ords.SelectedDateTicks)) day.SelectedDateTicks = reader.GetInt64(ords.SelectedDateTicks); else day.SelectedDateTicks = 0L;
            if (ords.TotalPriceProducts >= 0 && !reader.IsDBNull(ords.TotalPriceProducts)) day.TotalPriceProducts = reader.GetInt32(ords.TotalPriceProducts); else day.TotalPriceProducts = 0;
            if (ords.TotalPriceCake >= 0 && !reader.IsDBNull(ords.TotalPriceCake)) day.TotalPriceCake = reader.GetInt32(ords.TotalPriceCake); else day.TotalPriceCake = 0;
            if (ords.TotalPrice >= 0 && !reader.IsDBNull(ords.TotalPrice)) day.TotalPrice = reader.GetInt32(ords.TotalPrice); else day.TotalPrice = 0;
            if (ords.TotalPriceCorrect >= 0 && !reader.IsDBNull(ords.TotalPriceCorrect)) day.TotalPriceCorrect = reader.GetInt32(ords.TotalPriceCorrect); else day.TotalPriceCorrect = 0;
            if (ords.TotalPriceAfterCorrect >= 0 && !reader.IsDBNull(ords.TotalPriceAfterCorrect)) day.TotalPriceAfterCorrect = reader.GetInt32(ords.TotalPriceAfterCorrect); else day.TotalPriceAfterCorrect = 0;
            if (ords.TotalPriceMoney >= 0 && !reader.IsDBNull(ords.TotalPriceMoney)) day.TotalPriceMoney = reader.GetInt32(ords.TotalPriceMoney); else day.TotalPriceMoney = 0;
            if (ords.TotalPriceDifference >= 0 && !reader.IsDBNull(ords.TotalPriceDifference)) day.TotalPriceDifference = reader.GetInt32(ords.TotalPriceDifference); else day.TotalPriceDifference = 0;
            if (ords.SelectedRoute >= 0 && !reader.IsDBNull(ords.SelectedRoute))
            {
                var uu = reader.GetString(ords.SelectedRoute);
                day.SelectedRoute = string.IsNullOrEmpty(uu) ? Guid.Empty : new Guid(uu);
            }
            else
                day.SelectedRoute = Guid.Empty;


            // Map Json fields
            if (ords.JsonProducts >= 0 && !reader.IsDBNull(ords.JsonProducts)) day.JsonProducts = reader.GetString(ords.JsonProducts); else day.JsonProducts = string.Empty;
            if (ords.JsonCakes >= 0 && !reader.IsDBNull(ords.JsonCakes)) day.JsonCakes = reader.GetString(ords.JsonCakes); else day.JsonCakes = string.Empty;

            MapBaseFields(reader, day, ords);

            return day;
        }));
    }
}
