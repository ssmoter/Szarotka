using SQLite;

namespace Inventory.Model
{
    public class Cake
    {
        [PrimaryKey]
        public Guid Id { get; set; }
        public Guid DayId { get; set; }
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
