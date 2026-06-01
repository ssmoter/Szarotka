using DataBase.Data.SqlQuery;
using DataBase.Model.EntitiesRoutes;

using System.Text;
using System.Text.Json.Serialization;

namespace DataBase.Data.Get
{
    public interface IGetDriverRoutesAoT
    {
        Task<IList<CustomerRoutes>> CustomerRoutes(string where, params object[] args);
        Task<IList<Routes>> Routes();
    }

    public class GetDriverRoutesAoT : IGetDriverRoutesAoT
    {
        private readonly IAccessDataBase _db;

        public GetDriverRoutesAoT(IAccessDataBase db)
        {
            _db = db;
        }

        public async Task<IList<CustomerRoutes>> CustomerRoutes(string where, params object[] args)
        {
            var sb = new StringBuilder();
            sb.AppendLine(CustomerRoutesQuery.GetFullProcedureWithoutWhere());
            sb.AppendLine(where);
            var sql = sb.ToString();
            List<CustomerRoutesFromQuery> result = [];
            if (args is null)
            {
                result = await _db.DataBaseAsync.QueryAsync<CustomerRoutesFromQuery>(sql);
            }
            else
            {
                result = await _db.DataBaseAsync.QueryAsync<CustomerRoutesFromQuery>(sql, args);
            }

            foreach (CustomerRoutesFromQuery item in result)
            {
                var dayOfWeek = !string.IsNullOrWhiteSpace(item.JsonDayOfWeek) ?
                    System.Text.Json.JsonSerializer.Deserialize(
                        item.JsonDayOfWeek,
                        DataBase.Model.JsonContext.SzarotkaJsonSerializerContext.Default.SelectedDayOfWeekRoutes) : null;
                var address = !string.IsNullOrWhiteSpace(item.JsonAddress) ?
                    System.Text.Json.JsonSerializer.Deserialize(
                        item.JsonAddress,
                        DataBase.Model.JsonContext.SzarotkaJsonSerializerContext.Default.ResidentialAddress) : null;
                if (dayOfWeek is not null)
                {
                    item.DayOfWeek = dayOfWeek;
                }
                if (address is not null)
                {
                    item.ResidentialAddress = address;
                }
            }
            return [.. result.Select(x=>new CustomerRoutes(x))];
        }
        public async Task<IList<Routes>> Routes()
        {
            var sql = $"SELECT * FROM {nameof(Routes)}";

            var result = await _db.DataBaseAsync.QueryAsync<Routes>(sql);

            return result;
        }



        class CustomerRoutesFromQuery : CustomerRoutes
        {
            [JsonIgnore]
            public string JsonDayOfWeek { get; set; } = "";
            [JsonIgnore]
            public string JsonAddress { get; set; } = "";

        }
    }
}
