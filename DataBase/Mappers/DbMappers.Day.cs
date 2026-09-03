using DataBase.Model.EntitiesInventory;

using Microsoft.Data.Sqlite;
namespace DataBase.Mappers;

public partial class DbMappers
{
    // 1. Struktura przechowująca indeksy kolumn dla TEGO konkretnego zapytania (Day)
    public class DayOrdinals : BaseOrdinals
    {
        public int Description { get; }
        public int SelectedDateString { get; }
        public int SelectedDateTicks { get; }
        public int TotalPriceProducts { get; }
        public int TotalPriceCake { get; }
        public int TotalPrice { get; }
        public int TotalPriceCorrect { get; }
        public int TotalPriceAfterCorrect { get; }
        public int TotalPriceMoney { get; }
        public int TotalPriceDifference { get; }

        public DayOrdinals(SqliteDataReader reader) : base(reader)
        {
            // Tutaj GetOptionalOrdinal uruchamia się TYLKO RAZ przed pętlą!
            Description = reader.GetOptionalOrdinal(nameof(Day.Description));
            SelectedDateString = reader.GetOptionalOrdinal(nameof(Day.SelectedDateString));
            SelectedDateTicks = reader.GetOptionalOrdinal(nameof(Day.SelectedDateTicks));
            TotalPriceProducts = reader.GetOptionalOrdinal(nameof(Day.TotalPriceProducts));
            TotalPriceCake = reader.GetOptionalOrdinal(nameof(Day.TotalPriceCake));
            TotalPrice = reader.GetOptionalOrdinal(nameof(Day.TotalPrice));
            TotalPriceCorrect = reader.GetOptionalOrdinal(nameof(Day.TotalPriceCorrect));
            TotalPriceAfterCorrect = reader.GetOptionalOrdinal(nameof(Day.TotalPriceAfterCorrect));
            TotalPriceMoney = reader.GetOptionalOrdinal(nameof(Day.TotalPriceMoney));
            TotalPriceDifference = reader.GetOptionalOrdinal(nameof(Day.TotalPriceDifference));
        }
    }

    // 2. Rejestracja mappera dedykowanego dla Day (EntitiesInventory)
    private static void InitDayMappers()
    {
        _registry.Add(typeof(Day), (Func<SqliteDataReader, object, Day>)((reader, ordinalsObj) =>
        {
            ArgumentNullException.ThrowIfNull(reader);
            ArgumentNullException.ThrowIfNull(ordinalsObj);

            if (ordinalsObj is not DayOrdinals ords)
            {
                throw new ArgumentException("Invalid ordinals object for Day mapper", nameof(ordinalsObj));
            }

            var day = new Day();

            if (ords.Description >= 0 && !reader.IsDBNull(ords.Description))
            {
                day.Description = reader.GetString(ords.Description);
            }
            else
            {
                day.Description = string.Empty;
            }

            if (ords.SelectedDateString >= 0 && !reader.IsDBNull(ords.SelectedDateString))
            {
                day.SelectedDateString = reader.GetString(ords.SelectedDateString);
            }
            else
            {
                day.SelectedDateString = string.Empty;
            }

            if (ords.SelectedDateTicks >= 0 && !reader.IsDBNull(ords.SelectedDateTicks))
            {
                day.SelectedDateTicks = reader.GetInt64(ords.SelectedDateTicks);
            }
            else
            {
                day.SelectedDateTicks = 0L;
            }

            if (ords.TotalPriceProducts >= 0 && !reader.IsDBNull(ords.TotalPriceProducts))
            {
                day.TotalPriceProducts = reader.GetInt32(ords.TotalPriceProducts);
            }
            else
            {
                day.TotalPriceProducts = 0;
            }

            if (ords.TotalPriceCake >= 0 && !reader.IsDBNull(ords.TotalPriceCake))
            {
                day.TotalPriceCake = reader.GetInt32(ords.TotalPriceCake);
            }
            else
            {
                day.TotalPriceCake = 0;
            }

            if (ords.TotalPrice >= 0 && !reader.IsDBNull(ords.TotalPrice))
            {
                day.TotalPrice = reader.GetInt32(ords.TotalPrice);
            }
            else
            {
                day.TotalPrice = 0;
            }

            if (ords.TotalPriceCorrect >= 0 && !reader.IsDBNull(ords.TotalPriceCorrect))
            {
                day.TotalPriceCorrect = reader.GetInt32(ords.TotalPriceCorrect);
            }
            else
            {
                day.TotalPriceCorrect = 0;
            }

            if (ords.TotalPriceAfterCorrect >= 0 && !reader.IsDBNull(ords.TotalPriceAfterCorrect))
            {
                day.TotalPriceAfterCorrect = reader.GetInt32(ords.TotalPriceAfterCorrect);
            }
            else
            {
                day.TotalPriceAfterCorrect = 0;
            }

            if (ords.TotalPriceMoney >= 0 && !reader.IsDBNull(ords.TotalPriceMoney))
            {
                day.TotalPriceMoney = reader.GetInt32(ords.TotalPriceMoney);
            }
            else
            {
                day.TotalPriceMoney = 0;
            }

            if (ords.TotalPriceDifference >= 0 && !reader.IsDBNull(ords.TotalPriceDifference))
            {
                day.TotalPriceDifference = reader.GetInt32(ords.TotalPriceDifference);
            }
            else
            {
                day.TotalPriceDifference = 0;
            }

            MapBaseFields(reader, day, ords);

            return day;
        }));
    }
}
