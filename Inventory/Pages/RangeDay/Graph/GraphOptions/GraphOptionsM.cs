using CommunityToolkit.Mvvm.ComponentModel;

namespace Inventory.Pages.RangeDay.Graph.GraphOptions
{
    public partial class GraphOptionsM : ObservableObject, IDisposable
    {
        [ObservableProperty]
        bool totalPriceProduct;
        [ObservableProperty]
        bool totalPriceCake;
        [ObservableProperty]
        bool totalPrice;
        [ObservableProperty]
        bool totalPriceCorrect;
        [ObservableProperty]
        bool totalPriceMoney;
        [ObservableProperty]
        bool totalPriceDifference;
        [ObservableProperty]
        bool totalPriceAfterCorrect;
        [ObservableProperty]
        bool numberOfCakes;
        [ObservableProperty]
        GraphOptionsProductM[] productMs;
        public GraphOptionsM()
        {
            ProductMs = Array.Empty<GraphOptionsProductM>();
        }

        public void Dispose()
        {
            ProductMs = null;
            GC.SuppressFinalize(this);
        }
    }

    public partial class GraphOptionsProductM : ObservableObject
    {
        [ObservableProperty]
        bool isNumber;
        [ObservableProperty]
        bool isPrice;
        [ObservableProperty]
        string name;

    }

}
