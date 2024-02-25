using SQLite;

namespace DriversRoutes.Model
{
    public class Routes
    {
        [PrimaryKey]
        public Guid Id { get; set; }
        public string Name { get; set; }
    }
}
