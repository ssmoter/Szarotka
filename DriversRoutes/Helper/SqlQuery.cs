using DataBase.Model.EntitiesRoutes;

using System.Text;

namespace DriversRoutes.Helper
{
    public static class SqlQuery
    {
        public const char _equals = '=';
        public const char _more = '>';
        public const char _less = '<';

        public const string _ASC = "ASC";
        public const string _DESC = "DESC";
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
CustomerRoutes.CreatedTicks,
CustomerRoutes.UpdatedTicks,
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

	'Optional',SelectedDayOfWeekRoutes.Optional,
	
	'CreatedTicks',CustomerRoutes.CreatedTicks,
	'UpdatedTicks',CustomerRoutes.UpdatedTicks
	

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
	'Country',ResidentialAddress.Country,
	
	'CreatedTicks',CustomerRoutes.CreatedTicks,
	'UpdatedTicks',CustomerRoutes.UpdatedTicks

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
                sb.Append(" ASC");
            }

            #endregion


            return sb.ToString();
        }

        public static string GetSelectedDayOfWeekRoutesNearestDate(DateTime date, char sing,string orderBy)
        {
            SelectedDayOfWeekRoutes today = new();
            today.SetTodayDayOfWeek(date);

            today.SundayTimeSpan =new TimeSpan(date.Hour, date.Minute, date.Second);
            today.MondayTimeSpan = new TimeSpan(date.Hour, date.Minute, date.Second);
            today.TuesdayTimeSpan = new TimeSpan(date.Hour, date.Minute, date.Second);
            today.WednesdayTimeSpan = new TimeSpan(date.Hour, date.Minute, date.Second);
            today.ThursdayTimeSpan = new TimeSpan(date.Hour, date.Minute, date.Second);
            today.FridayTimeSpan = new TimeSpan(date.Hour, date.Minute, date.Second);
            today.SaturdayTimeSpan = new TimeSpan(date.Hour, date.Minute, date.Second);

            var sb = new StringBuilder();


            sb.AppendLine("SELECT * FROM ");
            sb.Append(nameof(SelectedDayOfWeekRoutes));
            sb.AppendLine(" WHERE ");
            AddParametr(sb, nameof(today.SundayTicks), today.SundayTicks.ToString(), today.Sunday, sing);
            AddParametr(sb, nameof(today.MondayTicks), today.MondayTicks.ToString(), today.Monday, sing);
            AddParametr(sb, nameof(today.TuesdayTicks), today.TuesdayTicks.ToString(), today.Tuesday, sing);
            AddParametr(sb, nameof(today.WednesdayTicks), today.WednesdayTicks.ToString(), today.Wednesday, sing);
            AddParametr(sb, nameof(today.ThursdayTicks), today.ThursdayTicks.ToString(), today.Thursday, sing);
            AddParametr(sb, nameof(today.FridayTicks), today.FridayTicks.ToString(), today.Friday, sing);
            AddParametr(sb, nameof(today.SaturdayTicks), today.SaturdayTicks.ToString(), today.Saturday, sing);
            sb.Append(" AND ");
            AddParametr(sb, nameof(today.Sunday), today.Sunday.ToString(), today.Sunday, _equals);
            AddParametr(sb, nameof(today.Monday), today.Monday.ToString(), today.Monday, _equals);
            AddParametr(sb, nameof(today.Tuesday), today.Tuesday.ToString(), today.Tuesday, _equals);
            AddParametr(sb, nameof(today.Wednesday), today.Wednesday.ToString(), today.Wednesday, _equals);
            AddParametr(sb, nameof(today.Thursday), today.Thursday.ToString(), today.Thursday, _equals);
            AddParametr(sb, nameof(today.Friday), today.Friday.ToString(), today.Friday, _equals);
            AddParametr(sb, nameof(today.Saturday), today.Saturday.ToString(), today.Saturday, _equals);

            sb.Append(' ');

            AddOrderBy(sb, orderBy, nameof(today.SundayTicks), today.Sunday);
            AddOrderBy(sb, orderBy, nameof(today.MondayTicks), today.Monday);
            AddOrderBy(sb, orderBy, nameof(today.TuesdayTicks), today.Tuesday);
            AddOrderBy(sb, orderBy, nameof(today.WednesdayTicks), today.Wednesday);
            AddOrderBy(sb, orderBy, nameof(today.ThursdayTicks), today.Thursday);
            AddOrderBy(sb, orderBy, nameof(today.FridayTicks), today.Friday);
            AddOrderBy(sb, orderBy, nameof(today.SaturdayTicks), today.Saturday);


            return sb.ToString();
        }
        private static void AddParametr(StringBuilder sb, string columnName, string value, bool isEnable, char sign)
        {
            if (isEnable)
            {
                sb.Append(columnName);
                sb.Append(' ');
                sb.Append(sign);
                sb.Append(' ');
                sb.AppendLine(value);
            }
        }
        private static void AddOrderBy(StringBuilder sb, string orderBy, string columnName, bool isEnable)
        {
            if (isEnable)
            {
                sb.Append("ORDER BY ");
                sb.Append(columnName);
                sb.Append(' ');
                sb.Append(orderBy);

            }
        }
    }
}
