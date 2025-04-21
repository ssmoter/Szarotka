using DataBase.Model.EntitiesInventory;

using System.Text;

namespace DataBase.Data.Get
{
    public static class GetInventoryAoTExtension
    {
        public static async Task<Day?> Day(this IGetInventoryAoT get, Guid id)
        {
            if (id == Guid.Empty)
            {
                throw new ArgumentNullException(nameof(id));
            }

            var where = "WHERE D.Id = ?";

            var result = await get.Days(where, id);

            return result.FirstOrDefault();
        }
        public static async Task<Day?> DaySelectedDateString(
            this IGetInventoryAoT get, string selectedDateString, Guid userId)
        {
            if (string.IsNullOrWhiteSpace(selectedDateString))
            {
                throw new ArgumentNullException(nameof(selectedDateString));
            }

            var where = @"
WHERE D.SelectedDateString = ?
AND
D.UserCreatedId = ?";

            var result = await get.Days(where, selectedDateString, userId);
            return result.FirstOrDefault();
        }


        public static async Task<IList<Day>> Days(this IGetInventoryAoT get, long from, long to, IList<Guid> userIds)
        {
            StringBuilder where = new();
            where.AppendLine(" WHERE D.SelectedDateTicks >= ? AND D.SelectedDateTicks <= ? ");
            for (int i = 0; i < userIds.Count; i++)
            {
                if (i == 0)
                {
                    where.AppendLine(" AND ( D.UserCreatedId = ? ");
                }
                if (i > 0)
                {
                    where.AppendLine(" OR D.UserCreatedId = ? ");
                }
            }
            if (userIds.Count > 0)
            {
                where.Append(')');
            }

            var result = await get.Days(where.ToString(), [from, to, .. userIds]);
            return result;
        }

    }
}
