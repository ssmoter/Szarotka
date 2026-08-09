using SQLite;

using System.Text.Json.Serialization;

namespace DataBase.Model
{
    public class LogsModel
    {
        [PrimaryKey]
        [AutoIncrement]
        public int Id { get; set; }
        public string StackTrace { get; set; } = "";
        public string Message { get; set; } = "";
        public string Created { get; set; } = "";
        [Ignore]
        [JsonConverter(typeof(SourceGenerator.CustomDateTimeConverter))]
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
