using DataBase.Data.Get;
using DataBase.Model.EntitiesRoutes;

namespace DataBase.Data.CheckUpdateDifferences
{
    public partial class RoutesDifferences
    {
        /// <summary>
        /// Sprawdzenie czy inny użytkownik edytował dany rekord
        /// Zwraca True jeżeli rekord można aktualizować. Zwraca CustomerRoutes jeżeli owy istnieje
        /// </summary>
        /// <param name="get">Interface do pobierania danych</param>
        /// <param name="update">Obiekt do sprawdzenia</param>
        /// <param name="forceUpdate">Czy zignorować sprawdzenie</param>
        /// <returns></returns>
        public static async Task<(bool, CustomerRoutes?)> Check(IGetDriverRoutesAoT get, CustomerRoutes update, bool forceUpdate)
        {
            CustomerRoutes? isExist = null;
            bool canUpdate = true;

            if (!forceUpdate)
            {
                isExist = await get.CustomerRoute(update.Id);
            }
            if (isExist is not null)
            {
                if (update.UserUpdatedId != isExist.UserUpdatedId)
                {
                    canUpdate = false;
                }
                if (update.UpdatedTicks < isExist.UpdatedTicks)
                {
                    canUpdate = false;
                }
            }
            return (canUpdate, isExist);
        }
    }
}
