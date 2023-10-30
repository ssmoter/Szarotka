using CommunityToolkit.Mvvm.ComponentModel;

namespace Inventory.Model.MVVM
{
    public partial class ProductM : ObservableObject
    {

        public Guid Id { get; set; }

        public Guid DayId { get; set; }

        public Guid ProductNameId { get; set; }
        public Guid ProductPriceId { get; set; }

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
                    CalculatePrice();
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
                    CalculatePrice();
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
                    CalculatePrice();
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
                    CalculatePrice();
                }
            }
        }


        public bool CanUpadte { get; set; }

        public ProductM()
        {
            Name = new ProductNameM();
            Price = new ProductPriceM();
        }

        public void CalculatePrice()
        {
            if (CanUpadte)
            {
                PriceTotal = (Number + numberEdit - NumberReturn) * Price.Price;
                PriceTotalAfterCorrect = PriceTotal + PriceTotalCorrect;
                Inventory.Service.ProductUpdatePriceService.OnUpdate();
            }
        }
    }
}
