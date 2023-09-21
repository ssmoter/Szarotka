using SQLite;

namespace Inventory.Model
{
    public class ProductPrice
    {
        [PrimaryKey]
        [AutoIncrement]
        public int Id { get; set; }
        public int ProductNameId { get; set; }
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
        public string Created { get; set; }
        [Ignore]
        public DateTime CreatedDateTime
        {
            get
            {
                return DateTime.Parse(Created);
            }
            set
            {
                Created = value.ToString();
            }
        }

    }
}
