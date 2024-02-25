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
    'MondayTicks',SelectedDayOfWeekRoutes.SundayTicks,

	'Monday',SelectedDayOfWeekRoutes.Monday,
    'MondayTicks',SelectedDayOfWeekRoutes.MondayTicks,

	'Tuesday',SelectedDayOfWeekRoutes.Tuesday,
    'TuesdayTicks',SelectedDayOfWeekRoutes.TuesdayTicks,

	'Wednesday',SelectedDayOfWeekRoutes.Wednesday,
    'WednesdayTicks',SelectedDayOfWeekRoutes.WednesdayTicks,

	'Thursday',SelectedDayOfWeekRoutes.Thursday,
    'ThursdayTicks',SelectedDayOfWeekRoutes.ThursdayTicks,

	'Friday',SelectedDayOfWeekRoutes.Friday,
    'FridayTicks',SelectedDayOfWeekRoutes.FridayTicks,

	'Saturday',SelectedDayOfWeekRoutes.Saturday,
    'SaturdayTicks',SelectedDayOfWeekRoutes.SaturdayTicks,

	'Optional',SelectedDayOfWeekRoutes.Optional

) as 'JsonDayOfWeek',

json_object(

	'Id',ResidentialAddress.Id,
	'CustomerId',ResidentialAddress.CustomerId,
	'Name',ResidentialAddress.Name,
	'Surname',ResidentialAddress.Surname,
	'Street',ResidentialAddress.Street,
	'HouseNumber',ResidentialAddress.HouseNumber,
	'ApartmentNumber',ResidentialAddress.ApartmentNumber,
	'PostalCode',ResidentialAddress.PostalCode,
	'City',ResidentialAddress.City,
	'Country',ResidentialAddress.Country

) as 'JsonAddress'

FROM CustomerRoutes 

JOIN SelectedDayOfWeekRoutes on CustomerRoutes.Id = SelectedDayOfWeekRoutes.CustomerId 

JOIN ResidentialAddress on CustomerRoutes.Id = ResidentialAddress.CustomerId


WHERE CustomerRoutes.RoutesId == ");

            sb.Append('\'');
            sb.Append(id);
            sb.Append('\'');
            sb.AppendLine();

            #region WHERE


            if (dayOf.Sunday)
            {
                sb.Append("AND SelectedDayOfWeekRoutes.Sunday = ");
                sb.Append(dayOf.Sunday);
                sb.AppendLine();
            }
            if (dayOf.Monday)
            {
                sb.Append("AND SelectedDayOfWeekRoutes.Monday = ");
                sb.Append(dayOf.Monday);
                sb.AppendLine();
            }
            if (dayOf.Tuesday)
            {
                sb.Append("AND SelectedDayOfWeekRoutes.Tuesday = ");
                sb.Append(dayOf.Tuesday);
                sb.AppendLine();
            }
            if (dayOf.Wednesday)
            {
                sb.Append("AND SelectedDayOfWeekRoutes.Wednesday = ");
                sb.Append(dayOf.Wednesday);
                sb.AppendLine();
            }
            if (dayOf.Thursday)
            {
                sb.Append("AND SelectedDayOfWeekRoutes.Thursday = ");
                sb.Append(dayOf.Thursday);
                sb.AppendLine();
            }
            if (dayOf.Friday)
            {
                sb.Append("AND SelectedDayOfWeekRoutes.Friday = ");
                sb.Append(dayOf.Friday);
                sb.AppendLine();
            }
            if (dayOf.Saturday)
            {
                sb.Append("AND SelectedDayOfWeekRoutes.Saturday = ");
                sb.Append(dayOf.Saturday);
                sb.AppendLine();
            }
            if (dayOf.Optional)
            {
                sb.Append("AND SelectedDayOfWeekRoutes.Optional = ");
                sb.Append(dayOf.Optional);
                sb.AppendLine();
            }

            #endregion

            #region ORDER BY

            sb.AppendLine();

            if (dayOf.IfAnyIsTrue())
            {
                sb.Append("ORDER BY ");
            }


            if (dayOf.Sunday)
            {
                sb.Append(" SelectedDayOfWeekRoutes.SundayTicks ");
            }
            if (dayOf.Monday)
            {
                if (dayOf.Sunday)
                    sb.Append(',');

                sb.Append(" SelectedDayOfWeekRoutes.MondayTicks ");
            }
            if (dayOf.Tuesday)
            {
                if (dayOf.Sunday || dayOf.Monday)
                    sb.Append(',');

                sb.Append(" SelectedDayOfWeekRoutes.TuesdayTicks ");
            }
            if (dayOf.Wednesday)
            {
                if (dayOf.Sunday || dayOf.Monday || dayOf.Tuesday)
                    sb.Append(',');

                sb.Append(" SelectedDayOfWeekRoutes.WednesdayTicks ");
            }
            if (dayOf.Thursday)
            {
                if (dayOf.Sunday || dayOf.Monday || dayOf.Tuesday || dayOf.Wednesday)
                    sb.Append(',');

                sb.Append(" SelectedDayOfWeekRoutes.ThursdayTicks ");
            }
            if (dayOf.Friday)
            {
                if (dayOf.Sunday || dayOf.Monday || dayOf.Tuesday || dayOf.Wednesday || dayOf.Thursday)
                    sb.Append(',');

                sb.Append(" SelectedDayOfWeekRoutes.FridayTicks ");
            }
            if (dayOf.Saturday)
            {
                if (dayOf.Sunday || dayOf.Monday || dayOf.Tuesday || dayOf.Wednesday || dayOf.Friday || dayOf.Friday)
                    sb.Append(',');

                sb.Append(" SelectedDayOfWeekRoutes.SaturdayTicks ");
            }

            if (dayOf.IfAnyIsTrue())
            {
                sb.Append(" DESC");
            }

            #endregion


            return sb.ToString();
        }
    }
}
