using DriversRoutes.Model;

using System.Text;

namespace DriversRoutes.Helper
{
    public static class SqlQuery
    {
        public static string GetQueryForSelectedRoutes(string id, SelectedDayOfWeekRoutes dayOf)
        {
            var sb = new StringBuilder();
            sb.Append(@"
SELECT 
CustomerRoutes.Id,
CustomerRoutes.RoutesId,
CustomerRoutes.Name,
CustomerRoutes.Description,
CustomerRoutes.PhoneNumber,
CustomerRoutes.Created,
CustomerRoutes.Longitude,
CustomerRoutes.Latitude,
json_object(
	'Id',SelectedDayOfWeekRoutes.Id,
	'CustomerId',SelectedDayOfWeekRoutes.CustomerId,
	'Sunday',SelectedDayOfWeekRoutes.Sunday,
	'Monday',SelectedDayOfWeekRoutes.Monday,
	'Tuesday',SelectedDayOfWeekRoutes.Tuesday,
	'Wednesday',SelectedDayOfWeekRoutes.Wednesday,
	'Thursday',SelectedDayOfWeekRoutes.Thursday,
	'Friday',SelectedDayOfWeekRoutes.Friday,
	'Saturday',SelectedDayOfWeekRoutes.Saturday,
	'ValuesAsString',SelectedDayOfWeekRoutes.ValuesAsString
) as 'JsonDayOfWeek'

FROM CustomerRoutes 

JOIN SelectedDayOfWeekRoutes on CustomerRoutes.Id = SelectedDayOfWeekRoutes.CustomerId 

WHERE CustomerRoutes.RoutesId == ");

            sb.Append('\'');
            sb.Append(id);
            sb.Append('\'');
            sb.AppendLine();

            if (dayOf.Sunday)
            {
                sb.Append("AND SelectedDayOfWeekRoutes.Sunday =");
                sb.Append(dayOf.Sunday);
                sb.AppendLine();
            }
            if (dayOf.Monday)
            {
                sb.Append("AND SelectedDayOfWeekRoutes.Monday =");
                sb.Append(dayOf.Monday);
                sb.AppendLine();
            }
            if (dayOf.Tuesday)
            {
                sb.Append("AND SelectedDayOfWeekRoutes.Tuesday =");
                sb.Append(dayOf.Tuesday);
                sb.AppendLine();
            }
            if (dayOf.Wednesday)
            {
                sb.Append("AND SelectedDayOfWeekRoutes.Wednesday =");
                sb.Append(dayOf.Wednesday);
                sb.AppendLine();
            }
            if (dayOf.Thursday)
            {
                sb.Append("AND SelectedDayOfWeekRoutes.Thursday =");
                sb.Append(dayOf.Thursday);
                sb.AppendLine();
            }
            if (dayOf.Friday)
            {
                sb.Append("AND SelectedDayOfWeekRoutes.Friday =");
                sb.Append(dayOf.Friday);
                sb.AppendLine();
            }
            if (dayOf.Saturday)
            {
                sb.Append("AND SelectedDayOfWeekRoutes.Saturday =");
                sb.Append(dayOf.Saturday);
                sb.AppendLine();
            }


            return sb.ToString();
        }
    }
}
