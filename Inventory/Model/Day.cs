using SQLite;

namespace Inventory.Model
{
    public class Day
    {
        [PrimaryKey]
        [AutoIncrement]
        public int Id { get; set; }
        public string Description { get; set; } = "";
        public Guid DriverGuid { get; set; }

        public string CreatedDate { get; set; }
        public string CreatedTime { get; set; }

        [Ignore]
        public DateTime CreatedDateTime
        {
            get
            {
                var time = parseTime(CreatedTime);
                var day = parseDate(CreatedDate);

                DateTime total = new DateTime(day.Item3, day.Item2, day.Item1, time.Item1, time.Item2, time.Item3);

                return total;
            }
            set
            {
                CreatedDate = value.ToString("dd.MM.yyyy");
                CreatedTime = value.ToString("HH:mm:ss");
            }
        }
        public int TotalPriceProducts { get; set; }
        [Ignore]
        public decimal TotalPriceProductsDecimal
        {
            get
            {
                return (decimal)TotalPriceProducts / 100m;
            }
            set
            {
                TotalPriceProducts = (int)(value * 100);
            }
        }
        public int TotalPriceCake { get; set; }
        [Ignore]
        public decimal TotalPriceCakeDecimal
        {
            get
            {
                return (decimal)TotalPriceCake / 100m;
            }
            set
            {
                TotalPriceCake = (int)(value * 100);
            }
        }
        public int TotalPrice { get; set; }
        [Ignore]
        public decimal TotalPriceDecimal
        {
            get
            {
                return (decimal)TotalPrice / 100m;
            }
            set
            {
                TotalPrice = (int)(value * 100);
            }
        }
        public int TotalPriceCorrect { get; set; }
        [Ignore]
        public decimal TotalPriceCorrectDecimal
        {
            get
            {
                return (decimal)TotalPriceCorrect / 100m;
            }
            set
            {
                TotalPriceCorrect = (int)(value * 100);
            }
        }
        public int TotalPriceAfterCorrect { get; set; }
        [Ignore]
        public decimal TotalPriceAfterCorrectDecimal
        {
            get
            {
                return (decimal)TotalPriceAfterCorrect / 100m;
            }
            set
            {
                TotalPriceAfterCorrect = (int)(value * 100);
            }
        }
        public int TotalPriceMoney { get; set; }
        [Ignore]
        public decimal TotalPriceMoneyDecimal
        {
            get
            {
                return (decimal)TotalPriceMoney / 100m;
            }
            set
            {
                TotalPriceMoney = (int)(value * 100);
            }
        }
        public int TotalPriceDifference { get; set; }
        [Ignore]
        public decimal TotalPriceDifferenceDecimal
        {
            get
            {
                return (decimal)TotalPriceDifference / 100m;
            }
            set
            {
                TotalPriceDifference = (int)(value * 100);
            }
        }
        [Ignore]
        public List<Product> Products { get; set; }
        [Ignore]
        public List<Cake> Cakes { get; set; }
        public Day()
        {
            Products = new List<Product>();
            Cakes = new List<Cake>();
        }

        (int, int, int) parseDate(ReadOnlySpan<char> value)
        {

            int day = int.Parse(value.Slice(0, 2));
            int month = int.Parse(value.Slice(3, 2));
            int year = int.Parse(value.Slice(6, 4));

            return (day, month, year);

        }
        (int, int, int) parseTime(ReadOnlySpan<char> value)
        {

            int hh = int.Parse(value.Slice(0, 2));
            int mm = int.Parse(value.Slice(3, 2));
            int ss = int.Parse(value.Slice(6, 2));

            return (hh, mm, ss);

        }
    }
}
