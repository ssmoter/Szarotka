using DataBase.Data.SqlQuery;
using DataBase.Model.EntitiesRoutes;

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
            var sql = CustomerRoutesQuery.GetFullProcedureWithoutWhere() + where;
            List<CustomerRoutesFromQuery> result = [];
            if (args is null)
            {
                result = await _db.DataBaseAsync.QueryAsync<CustomerRoutesFromQuery>(sql);
            }
            else
            {
                result = await _db.DataBaseAsync.QueryAsync<CustomerRoutesFromQuery>(sql, args);
            }

            for (int i = 0; i < result.Count; i++)
            {
                var dayOfWeek =
                    System.Text.Json.JsonSerializer.Deserialize(
                        result[i].JsonDayOfWeek,
                        DataBase.Model.JsonContext.SzarotkaJsonSerializerContext.Default.SelectedDayOfWeekRoutes);
                var address =
                    System.Text.Json.JsonSerializer.Deserialize(
                        result[i].JsonAddress,
                        DataBase.Model.JsonContext.SzarotkaJsonSerializerContext.Default.ResidentialAddress);
                if (dayOfWeek is not null)
                {
                    result[i].DayOfWeek = dayOfWeek;
                }
                if (address is not null)
                {
                    result[i].ResidentialAddress = address;
                }
            }
            return [.. result.Select(x => x as CustomerRoutes)];
        }
        public async Task<IList<Routes>> Routes()
        {
            var sql = $"SELECT * FROM {nameof(Routes)}";

            var result = await _db.DataBaseAsync.QueryAsync<Routes>(sql);

            return result;
        }



        class CustomerRoutesFromQuery : CustomerRoutes
        {
            public string JsonDayOfWeek { get; set; } = "";
            public string JsonAddress { get; set; } = "";

        }
    }
}
