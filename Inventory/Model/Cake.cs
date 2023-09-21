using SQLite;

namespace Inventory.Model
{
    public class Cake
    {
        [PrimaryKey]
        [AutoIncrement]
        public int Id { get; set; }
        public int DayId { get; set; }
        public bool IsSell { get; set; }
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
    }
}
