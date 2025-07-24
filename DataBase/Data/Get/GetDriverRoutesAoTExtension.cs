using DataBase.Model.EntitiesRoutes;

using System.Text;

namespace DataBase.Data.Get
{
    public static class GetDriverRoutesAoTExtension
    {

        public static async Task<IList<CustomerRoutes>> CustomerRoutes(this IGetDriverRoutesAoT get, params Guid[] ids)
        {
            StringBuilder sb = new();
            sb.Append(" WHERE ");
            sb.Append(nameof(Model.EntitiesRoutes.CustomerRoutes));
            sb.Append('.');
            sb.Append(nameof(Model.EntitiesRoutes.CustomerRoutes.Id));
            sb.Append(" IN ( ");

            for (int i = 0; i < ids.Length; i++)
            {
                Guid id = ids[i];
                if (id == Guid.Empty)
                {
                    throw new ArgumentNullException(nameof(id));
                }
                if (i != 0)
                {
                    sb.Append(", ");
                }
                sb.Append('?');
            }
            sb.Append(')');

            var result = await get.CustomerRoutes(sb.ToString(), [.. ids]);
            return result;
        }
        public static async Task<CustomerRoutes?> CustomerRoute(this IGetDriverRoutesAoT get, Guid id)
        {
            StringBuilder sb = new();
            if (id == Guid.Empty)
            {
                throw new ArgumentNullException(nameof(id));
            }
            sb.Append(" WHERE ");
            sb.Append(nameof(Model.EntitiesRoutes.CustomerRoutes));
            sb.Append('.');
            sb.Append(nameof(Model.EntitiesRoutes.CustomerRoutes.Id));
            sb.Append(" = ?");
            var result = await get.CustomerRoutes(sb.ToString(), id);

            return result.FirstOrDefault();
        }
        public static async Task<IList<CustomerRoutes>> CustomerRoutes(this IGetDriverRoutesAoT get, Guid routeId, DayOfWeek[] selected_day, bool isDelete = false)
        {
            if (routeId == Guid.Empty)
            {
                throw new ArgumentNullException(nameof(routeId));
            }

            StringBuilder sb = new();
            sb.Append(" WHERE ");
            sb.Append(nameof(Model.EntitiesRoutes.CustomerRoutes));
            sb.Append('.');
            sb.Append(nameof(Model.EntitiesRoutes.CustomerRoutes.RoutesId));
            sb.Append(" = ?");
            sb.AppendLine();

            for (int i = 0; i < selected_day.Length; i++)
            {
                if (i == 0)
                {
                    sb.AppendLine(" AND ( ");
                }
                if (i > 0)
                {
                    sb.Append(" OR ");
                }
                sb.Append(nameof(Model.EntitiesRoutes.SelectedDayOfWeekRoutes));
                sb.Append('.');
                sb.Append(selected_day[i].ToString());
                sb.AppendLine(" = 1 ");
            }
            if (selected_day.Length > 0)
            {
                sb.Append(')');
            }

            sb.AppendLine(" AND ( ");
            sb.Append(nameof(Model.EntitiesRoutes.SelectedDayOfWeekRoutes));
            sb.Append('.');
            sb.Append(nameof(Model.EntitiesRoutes.SelectedDayOfWeekRoutes.IsDelete));
            sb.Append(" = ");
            sb.Append(isDelete);
            sb.AppendLine(" OR ");
            sb.Append(nameof(Model.EntitiesRoutes.SelectedDayOfWeekRoutes));
            sb.Append('.');
            sb.Append(nameof(Model.EntitiesRoutes.SelectedDayOfWeekRoutes.IsDelete));
            sb.Append(" IS NULL ");
            sb.Append(')');


            var result = await get.CustomerRoutes(sb.ToString(), [routeId, .. selected_day]);

            return result;
        }











    }
}
