using SQLite;

namespace DataBase.Model.EntitiesInventory;

    public class Driver
    {
        [PrimaryKey]
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
    }
    public class SelectedDriver
    {
        [PrimaryKey]
        [AutoIncrement]
        public int Id { get; set; }
        public Guid SelectedGuid { get; set; }

    }

