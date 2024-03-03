using SQLite;

namespace DataBase.Model.EntitiesInventory;

public class Day
{
    [PrimaryKey]
    public Guid Id { get; set; }
    public string Description { get; set; } = "";
    public Guid DriverGuid { get; set; }

    public string CreatedDate { get; set; }
    public long CreatedTicks { get; set; }

    [Ignore]
    public DateTime CreatedDateTime
    {
        get
        {
            return new DateTime(CreatedTicks).ToLocalTime();
        }
        set
        {
            CreatedDate = value.ToString("dd.MM.yyyy");
            CreatedTicks = value.ToUniversalTime().Ticks;
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

    //static (int, int, int) parseDate(ReadOnlySpan<char> value)
    //{

    //    int day = int.Parse(value[..2]);
    //    int month = int.Parse(value.Slice(3, 2));
    //    int year = int.Parse(value.Slice(6, 4));

    //    return (day, month, year);

    //}

    //static (int, int, int) parseTime(ReadOnlySpan<char> value)
    //{

    //    int hh = int.Parse(value[..2]);
    //    int mm = int.Parse(value.Slice(3, 2));
    //    int ss = int.Parse(value.Slice(6, 2));

    //    return (hh, mm, ss);

    //}
}
