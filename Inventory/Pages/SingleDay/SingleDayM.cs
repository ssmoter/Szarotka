using CommunityToolkit.Mvvm.ComponentModel;

namespace Inventory.Pages.SingleDay
{
    public partial class SingleDayM : ObservableObject
    {
        private bool productIsVisible = true;
        public bool ProductIsVisible
        {
            get => productIsVisible;
            set
            {
                if (SetProperty(ref productIsVisible, value, nameof(ProductIsVisible))) { }
            }
        }

        private bool cakeIsVisible = false;
        public bool CakeIsVisible
        {
            get => cakeIsVisible;
            set
            {
                if (SetProperty(ref cakeIsVisible, value, nameof(CakeIsVisible))) { }
            }
        }

        private bool productIsRefreshing;
        public bool ProductIsRefreshing
        {
            get => productIsRefreshing;
            set
            {
                if (SetProperty(ref productIsRefreshing, value, nameof(ProductIsRefreshing))) { }
            }
        }

        private int cakeSortPriceRotateX;
        public int CakeSortPriceRotateX
        {
            get => cakeSortPriceRotateX;
            set
            {
                if (SetProperty(ref cakeSortPriceRotateX, value, nameof(CakeSortPriceRotateX))) { }
            }
        }

        private int cakeSortDateRotateX;
        public int CakeSortDateRotateX
        {
            get => cakeSortDateRotateX;
            set
            {
                if (SetProperty(ref cakeSortDateRotateX, value, nameof(CakeSortDateRotateX))) { }
            }
        }

        private bool cakeAllIsVisible;
        public bool CakeAllIsVisible
        {
            get => cakeAllIsVisible;
            set
            {
                if (SetProperty(ref cakeAllIsVisible, value, nameof(CakeAllIsVisible))) { }
            }
        }

        private bool isModelSend;
        public bool IsModelSend
        {
            get => isModelSend;
            set
            {
                if (SetProperty(ref isModelSend, value, nameof(IsModelSend))) { }
            }
        }
    }
}
