using DataBase.Model.EntitiesRoutes;

using System.Text;

namespace DataBase.Data.Get
{
    public static class GetDriverRoutesAoTExtension
    {
        public static async Task<CustomerRoutes?> CustomerRoute(this IGetDriverRoutesAoT get, Guid id)
        {
            if (id == Guid.Empty)
            {
                throw new ArgumentNullException(nameof(id));
            }

            var where = "WHERE CustomerRoutes.Id = ?";

            var result = await get.CustomerRoutes(where, id);

            return result.FirstOrDefault();
        }
        public static async Task<IList<CustomerRoutes>> CustomerRoutes(this IGetDriverRoutesAoT get, Guid routeId, DayOfWeek[] selected_day)
        {
            if (routeId == Guid.Empty)
            {
                throw new ArgumentNullException(nameof(routeId));
            }

            StringBuilder where = new();

            where.AppendLine(" WHERE CustomerRoutes.RoutesId = ? ");

            for (int i = 0; i < selected_day.Length; i++)
            {
                if (i == 0)
                {
                    where.AppendLine(" AND ( ");
                }
                if (i > 0)
                {
                    where.Append(" OR ");
                }
                where.Append("SelectedDayOfWeekRoutes.");
                where.Append(selected_day[i].ToString());
                where.AppendLine(" = 1 ");
            }
            if (selected_day.Length > 0)
            {
                where.Append(')');
            }


            var result = await get.CustomerRoutes(where.ToString(), [routeId, .. selected_day]);

            return result;
        }
    }
}
