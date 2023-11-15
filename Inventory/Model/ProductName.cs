using SQLite;

namespace Inventory.Model
{
    public class ProductName
    {
        [PrimaryKey]
        public Guid Id { get; set; }
        public int Arrangement { get; set; }
        public string Name { get; set; } = "";
        public string Description { get; set; } = "";
        public string Img { get; set; }

    }
}
