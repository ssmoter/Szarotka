using DataBase.Data.Get;
using DataBase.Model;
using DataBase.Model.EntitiesInventory;
using DataBase.Model.EntitiesRoutes;

namespace DataBase.Data.CheckUpdateDifferences
{
    public partial class ModelsDifferences
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

            if (!forceUpdate)
            {
                isExist = await get.CustomerRoute(update.Id);
            }

            bool canUpdate = Check(isExist: isExist, update: update);
            return (canUpdate, isExist);
        }

        /// <summary>
        /// Sprawdzenie czy inny użytkownik edytował dany rekord
        /// Zwraca True jeżeli rekord można aktualizować. Zwraca ProductName jeżeli owy istnieje
        /// </summary>
        /// <param name="get">Interface do pobierania danych</param>
        /// <param name="update">Obiekt do sprawdzenia</param>
        /// <param name="forceUpdate">Czy zignorować sprawdzenie</param>
        /// <returns></returns>
        public static async Task<(bool, ProductName?)> Check(IGetInventoryAoT get, ProductName update, bool forceUpdate)
        {
            ProductName? isExist = null;

            if (!forceUpdate)
            {
                isExist = await get.GetProductName(update.Id);
            }

            bool canUpdate = Check(isExist: isExist, update: update);
            return (canUpdate, isExist);
        }

        /// <summary>
        /// Sprawdzenie czy inny użytkownik edytował dany rekord
        /// Zwraca True jeżeli rekord można aktualizować. Zwraca ProductPrice jeżeli owy istnieje
        /// </summary>
        /// <param name="get">Interface do pobierania danych</param>
        /// <param name="update">Obiekt do sprawdzenia</param>
        /// <param name="forceUpdate">Czy zignorować sprawdzenie</param>
        /// <returns></returns>
        public static async Task<(bool, ProductPrice?)> Check(IGetInventoryAoT get, ProductPrice update, bool forceUpdate)
        {
            ProductPrice? isExist = null;

            if (!forceUpdate)
            {
                isExist = await get.GetProductPrice(update.Id);
            }

            bool canUpdate = Check(isExist: isExist, update: update);
            return (canUpdate, isExist);
        }



        /// <summary>
        /// Sprawdzenie czy inny użytkownik edytował dany rekord
        /// Zwraca True jeżeli rekord można aktualizować. Zwraca Day jeżeli owy istnieje
        /// </summary>
        /// <param name="get">Interface do pobierania danych</param>
        /// <param name="update">Obiekt do sprawdzenia</param>
        /// <param name="forceUpdate">Czy zignorować sprawdzenie</param>
        /// <returns></returns>
        public static async Task<(bool, Day?)> Check(IGetInventoryAoT get, Day update, bool forceUpdate)
        {
            Day? isExist = null;

            if (!forceUpdate)
            {
                isExist = await get.Day(update.Id);
            }

            bool canUpdate = Check(isExist: isExist, update: update);
            return (canUpdate, isExist);
        }







        private static bool Check<T>(BaseEntities<T>? isExist, BaseEntities<T> update)
        {
            bool canUpdate = true;
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
            return canUpdate;
        }








    }
}
