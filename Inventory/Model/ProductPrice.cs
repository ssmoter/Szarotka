using SQLite;

namespace Inventory.Model
{
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
        public string Created { get; set; }
        [Ignore]
        public DateTime CreatedDateTime
        {
            get
            {
                return DateTime.Parse(Created, DataBase.Helper.Constants.CultureInfo);
            }
            set
            {
                Created = value.ToString(DataBase.Helper.Constants.CultureInfo);
            }
        }

    }
}
