using CommunityToolkit.Mvvm.ComponentModel;

namespace Inventory.Model.MVVM
{
    public partial class ProductM : ObservableObject
    {
        public int Id { get; set; }
        public int DayId { get; set; }

        public int ProductNameId { get; set; }
        [ObservableProperty]
        ProductNameM name;

        [ObservableProperty]
        string description;

        [ObservableProperty]
        ProductPriceM price;

        [ObservableProperty]
        decimal priceTotal;

        decimal priceTotalCorrect;
        public decimal PriceTotalCorrect
        {
            get => priceTotalCorrect;
            set
            {
                if (SetProperty(ref priceTotalCorrect, value))
                {
                    OnPropertyChanged(nameof(PriceTotalCorrect));
                    calculatePrice();
                }
            }
        }

        [ObservableProperty]
        decimal priceTotalAfterCorrect;

        int number;
        public int Number
        {
            get => number;
            set
            {
                if (SetProperty(ref number, value))
                {
                    OnPropertyChanged(nameof(Number));
                    calculatePrice();
                }
            }
        }

        int numberEdit;
        public int NumberEdit
        {
            get => numberEdit;
            set
            {
                if (SetProperty(ref numberEdit, value))
                {
                    OnPropertyChanged(nameof(NumberEdit));
                    calculatePrice();
                }
            }
        }

        int numberReturn;
        public int NumberReturn
        {
            get => numberReturn;
            set
            {
                if (SetProperty(ref numberReturn, value))
                {
                    OnPropertyChanged(nameof(NumberReturn));
                    calculatePrice();
                }
            }
        }



        public ProductM()
        {
            Name = new ProductNameM();
            Price = new ProductPriceM();
        }

        void calculatePrice()
        {
            if (Service.ProductUpdatePriceService.EnableUpdate)
            {
                PriceTotal = (Number + NumberEdit - NumberReturn) * Price.Price;
                PriceTotalAfterCorrect = PriceTotal + PriceTotalCorrect;
                Inventory.Service.ProductUpdatePriceService.OnUpdate();
            }
        }
    }
}
