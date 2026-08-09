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
            JsonProducts = reader.GetOrdinal(nameof(GetInventoryAoT.DayFromQuery.JsonProducts));
            JsonCakes = reader.GetOrdinal(nameof(GetInventoryAoT.DayFromQuery.JsonCakes));
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
            if (!reader.IsDBNull(ords.Description)) day.Description = reader.GetString(ords.Description); else day.Description = string.Empty;
            if (!reader.IsDBNull(ords.SelectedDateString)) day.SelectedDateString = reader.GetString(ords.SelectedDateString); else day.SelectedDateString = string.Empty;
            if (!reader.IsDBNull(ords.SelectedDateTicks)) day.SelectedDateTicks = reader.GetInt64(ords.SelectedDateTicks); else day.SelectedDateTicks = 0L;
            if (!reader.IsDBNull(ords.TotalPriceProducts)) day.TotalPriceProducts = reader.GetInt32(ords.TotalPriceProducts); else day.TotalPriceProducts = 0;
            if (!reader.IsDBNull(ords.TotalPriceCake)) day.TotalPriceCake = reader.GetInt32(ords.TotalPriceCake); else day.TotalPriceCake = 0;
            if (!reader.IsDBNull(ords.TotalPrice)) day.TotalPrice = reader.GetInt32(ords.TotalPrice); else day.TotalPrice = 0;
            if (!reader.IsDBNull(ords.TotalPriceCorrect)) day.TotalPriceCorrect = reader.GetInt32(ords.TotalPriceCorrect); else day.TotalPriceCorrect = 0;
            if (!reader.IsDBNull(ords.TotalPriceAfterCorrect)) day.TotalPriceAfterCorrect = reader.GetInt32(ords.TotalPriceAfterCorrect); else day.TotalPriceAfterCorrect = 0;
            if (!reader.IsDBNull(ords.TotalPriceMoney)) day.TotalPriceMoney = reader.GetInt32(ords.TotalPriceMoney); else day.TotalPriceMoney = 0;
            if (!reader.IsDBNull(ords.TotalPriceDifference)) day.TotalPriceDifference = reader.GetInt32(ords.TotalPriceDifference); else day.TotalPriceDifference = 0;

            // Map Json fields
            if (!reader.IsDBNull(ords.JsonProducts)) day.JsonProducts = reader.GetString(ords.JsonProducts); else day.JsonProducts = string.Empty;
            if (!reader.IsDBNull(ords.JsonCakes)) day.JsonCakes = reader.GetString(ords.JsonCakes); else day.JsonCakes = string.Empty;

            MapBaseFields(reader, day, ords);

            return day;
        }));
    }
}
