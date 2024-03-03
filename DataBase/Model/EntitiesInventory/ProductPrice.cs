using SQLite;

namespace DataBase.Model.EntitiesInventory;

public class ProductPrice
{
    [PrimaryKey]
    public Guid Id { get; set; }
    public Guid ProductNameId { get; set; }
    public int Price { get; set; }
    [Ignore]
    public decimal PriceDecimal
    {
        get
        {
            return (decimal)Price / 100m;
        }
        set
        {
            Price = (int)(value * 100);
        }
    }
    public long Created { get; set; }
    [Ignore]
    public DateTime CreatedDateTime
    {
        get
        {
            return new DateTime(Created).ToLocalTime();
        }
        set
        {
            Created = value.ToUniversalTime().Ticks;
        }
    }

}
