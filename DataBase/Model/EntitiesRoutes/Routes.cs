using SQLite;

namespace DataBase.Model.EntitiesRoutes
{
    public class Routes
    {
        [PrimaryKey]
        public Guid Id { get; set; }
        public string Name { get; set; }
    }
}
