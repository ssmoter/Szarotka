namespace Inventory.Service
{
    public static class ProductUpdatePriceService
    {
        public static event Action UpdatePrice;
        /// <summary>
        /// Aktualizowanie utargu
        /// </summary>
        public static void OnUpdate()
        {
            UpdatePrice?.Invoke();
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
