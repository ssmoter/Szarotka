using CommunityToolkit.Mvvm.ComponentModel;

using CsvHelper.Configuration.Attributes;

namespace Inventory.Model.MVVM
{
    public partial class ProductM : ObservableObject
    {
        [Name("ProduktId")]
        public Guid Id { get; set; }

        [Name("ProduktDzieńId")]
        public Guid DayId { get; set; }

        [Name("ProduktNazwaId")]
        public Guid ProductNameId { get; set; }
        [ObservableProperty]
        ProductNameM name;

        [ObservableProperty]
        [Name("ProduktOpis")]
        string description;

        [ObservableProperty]
        ProductPriceM price;

        [ObservableProperty]
        [Name("ProduktUtarg")]
        decimal priceTotal;

        decimal priceTotalCorrect;
        [Name("ProduktUtargKorekta")]
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
        [Name("ProduktUtargPoKorekcie")]
        decimal priceTotalAfterCorrect;

        int number;
        [Name("ProduktIlość")]
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
        [Name("ProduktIlośćEdycja")]
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
        [Name("ProduktIlośćZwrot")]
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



        public ProductM()
        {
            Name = new ProductNameM();
            Price = new ProductPriceM();
        }

        void CalculatePrice()
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
