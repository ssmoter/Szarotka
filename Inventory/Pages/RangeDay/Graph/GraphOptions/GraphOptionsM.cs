using CommunityToolkit.Mvvm.ComponentModel;

namespace Inventory.Pages.RangeDay.Graph.GraphOptions
{
    public partial class GraphOptionsM : ObservableObject, IDisposable
    {
        private bool totalPriceProduct;
        public bool TotalPriceProduct
        {
            get => totalPriceProduct;
            set
            {
                if (SetProperty(ref totalPriceProduct, value, nameof(TotalPriceProduct))) { }
            }
        }

        private bool totalPriceCake;
        public bool TotalPriceCake
        {
            get => totalPriceCake;
            set
            {
                if (SetProperty(ref totalPriceCake, value, nameof(TotalPriceCake))) { }
            }
        }

        private bool totalPrice;
        public bool TotalPrice
        {
            get => totalPrice;
            set
            {
                if (SetProperty(ref totalPrice, value, nameof(TotalPrice))) { }
            }
        }

        private bool totalPriceCorrect;
        public bool TotalPriceCorrect
        {
            get => totalPriceCorrect;
            set
            {
                if (SetProperty(ref totalPriceCorrect, value, nameof(TotalPriceCorrect))) { }
            }
        }

        private bool totalPriceMoney;
        public bool TotalPriceMoney
        {
            get => totalPriceMoney;
            set
            {
                if (SetProperty(ref totalPriceMoney, value, nameof(TotalPriceMoney))) { }
            }
        }

        private bool totalPriceDifference;
        public bool TotalPriceDifference
        {
            get => totalPriceDifference;
            set
            {
                if (SetProperty(ref totalPriceDifference, value, nameof(TotalPriceDifference))) { }
            }
        }

        private bool totalPriceAfterCorrect;
        public bool TotalPriceAfterCorrect
        {
            get => totalPriceAfterCorrect;
            set
            {
                if (SetProperty(ref totalPriceAfterCorrect, value, nameof(TotalPriceAfterCorrect))) { }
            }
        }

        private bool numberOfCakes;
        public bool NumberOfCakes
        {
            get => numberOfCakes;
            set
            {
                if (SetProperty(ref numberOfCakes, value, nameof(NumberOfCakes))) { }
            }
        }

        private GraphOptionsProductM[] productMs;
        public GraphOptionsProductM[] ProductMs
        {
            get => productMs;
            set
            {
                if (SetProperty(ref productMs, value, nameof(ProductMs))) { }
            }
        }
        public GraphOptionsM()
        {
            ProductMs = [];
        }

        public void Dispose()
        {
            ProductMs = null;
            GC.SuppressFinalize(this);
        }
    }

    public partial class GraphOptionsProductM : ObservableObject
    {
        private bool isNumber;
        public bool IsNumber
        {
            get => isNumber;
            set
            {
                if (SetProperty(ref isNumber, value, nameof(IsNumber))) { }
            }
        }
        private bool isPrice;
        public bool IsPrice
        {
            get => isPrice;
            set
            {
                if (SetProperty(ref isPrice, value, nameof(IsPrice))) { }
            }
        }
        private string name;
        public string Name
        {
            get => name;
            set
            {
                if (SetProperty(ref name, value, nameof(Name))) { }
            }
        }
    }

}
