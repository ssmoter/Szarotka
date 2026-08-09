using DataBase.Data.SqlQuery;
using DataBase.Model.EntitiesRoutes;

using System.Text;
using System.Text.Json.Serialization;

namespace DataBase.Data.Get
{
    public interface IGetDriverRoutesAoT
    {
        Task<IList<CustomerRoutes>> CustomerRoutes(string where, object? args);
        Task<IList<Routes>> Routes();
    }

    public class GetDriverRoutesAoT(IAccessDataBaseAoT db) : IGetDriverRoutesAoT
    {
        private readonly IAccessDataBaseAoT _db = db;

        public async Task<IList<CustomerRoutes>> CustomerRoutes(string where, object? args)
        {
            var sb = new StringBuilder();
            sb.AppendLine(CustomerRoutesQuery.GetFullProcedureWithoutWhere());
            sb.AppendLine(where);
            var sql = sb.ToString();
            IEnumerable<CustomerRoutesFromQuery> result = [];
            if (args is null)
            {
                result = await _db.DbAsyncAoT.QueryAsync<CustomerRoutesFromQuery>(sql);
            }
            else
            {
                result = await _db.DbAsyncAoT.QueryAsync<CustomerRoutesFromQuery>(sql, args);
            }

            foreach (CustomerRoutesFromQuery item in result)
            {
                var dayOfWeek = !string.IsNullOrWhiteSpace(item.JsonDayOfWeek) ?
                    System.Text.Json.JsonSerializer.Deserialize(
                        item.JsonDayOfWeek,
                        DataBase.Model.SourceGenerator.SzarotkaJsonSerializerContext.Default.SelectedDayOfWeekRoutes) : null;
                var address = !string.IsNullOrWhiteSpace(item.JsonAddress) ?
                    System.Text.Json.JsonSerializer.Deserialize(
                        item.JsonAddress,
                        DataBase.Model.SourceGenerator.SzarotkaJsonSerializerContext.Default.ResidentialAddress) : null;
                if (dayOfWeek is not null)
                {
                    item.DayOfWeek = dayOfWeek;
                }
                if (address is not null)
                {
                    item.ResidentialAddress = address;
                }
            }
            return [.. result.Select(x => new CustomerRoutes(x))];
        }
        public async Task<IList<Routes>> Routes()
        {
            var sql = $"SELECT * FROM {nameof(Routes)}";

            var result = await _db.DbAsyncAoT.QueryAsync<Routes>(sql);

            return [.. result];
        }



        public partial class CustomerRoutesFromQuery : CustomerRoutes
        {
            [JsonIgnore]
            public string JsonDayOfWeek { get; set; } = "";
            [JsonIgnore]
            public string JsonAddress { get; set; } = "";

        }
    }
}
