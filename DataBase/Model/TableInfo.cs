namespace DataBase.Model
{
    public class TableInfo
    {
        public int Cid { get; set; }
        public string Name { get; set; } = "";
        public string Type { get; set; } = "";
        public int NotNull { get; set; }
        public string dflt_value { get; set; } = "";
        public int pk { get; set; }
    }
}
