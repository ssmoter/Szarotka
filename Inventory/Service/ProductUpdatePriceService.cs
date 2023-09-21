namespace Inventory.Service
{
    public static class ProductUpdatePriceService
    {
        public static bool EnableUpdate = false;
        public static event Action UpdatePrice;
        /// <summary>
        /// Aktualizowanie utargu
        /// </summary>
        public static void OnUpdate()
        {
            UpdatePrice?.Invoke();
        }
    }

    public static class ProductsUpdateService
    {
        public static event Func<Task> Update;
        /// <summary>
        /// Ponowne pobranie danych o produktach
        /// </summary>
        public static Task OnUpdate()
        {
            return Update?.Invoke();
        }
    }

    public static class DriverNameUpdateService
    {
        public static event Action Update;

        public static void OnUpdate()
        {
            Update?.Invoke();
        }
    }
}
