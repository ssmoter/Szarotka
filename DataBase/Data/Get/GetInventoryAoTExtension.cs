using DataBase.Model.EntitiesInventory;

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

        public static async Task<Day?> DaySelectedDateString(this IGetInventoryAoT get, string selectedDateString)
        {
            if (string.IsNullOrWhiteSpace(selectedDateString))
            {
                throw new ArgumentNullException(nameof(selectedDateString));
            }

            var where = "WHERE D.SelectedDateString = ?";

            var result = await get.Days(where, selectedDateString);
            return result.FirstOrDefault();
        }


    }
}
