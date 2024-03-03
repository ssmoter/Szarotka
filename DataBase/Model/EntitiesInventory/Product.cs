using SQLite;

namespace DataBase.Model.EntitiesInventory;

public class Product
{
    [PrimaryKey]
    public Guid Id { get; set; }
    public Guid DayId { get; set; }
    public Guid ProductNameId {  get; set; }
    public Guid ProductPriceId { get; set; }

    [Ignore]
    public ProductName Name { get; set; }
    public string Description { get; set; } = "";
    [Ignore]
    public ProductPrice Price { get; set; }
    public int PriceTotal { get; set; }
    [Ignore]
    public decimal PriceTotalDecimal
    {
        get
        {
            return (decimal)PriceTotal / 100m;
        }
        set
        {
            PriceTotal = (int)(value * 100);
        }
    }
    public int PriceTotalCorrect { get; set; }
    [Ignore]
    public decimal PriceTotalCorrectDecimal
    {
        get
        {
            return (decimal)PriceTotalCorrect / 100m;
        }
        set
        {
            PriceTotalCorrect = (int)(value * 100);
        }
    }
    public int PriceTotalAfterCorrect { get; set; }
    [Ignore]
    public decimal PriceTotalAfterCorrectDecimal
    {
        get
        {
            return (decimal)PriceTotalAfterCorrect / 100m;
        }
        set
        {
            PriceTotalAfterCorrect = (int)(value * 100);
        }
    }
    public int Number { get; set; }
    public int NumberEdit { get; set; }
    public int NumberReturn { get; set; }
}
