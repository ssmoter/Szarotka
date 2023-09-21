﻿using SQLite;

namespace Inventory.Model
{
    public class ProductName
    {
        [PrimaryKey]
        [AutoIncrement]
        public int Id { get; set; }
        public string Name { get; set; } = "";
        public string Description { get; set; } = "";
        public string Img { get; set; }

    }
}
