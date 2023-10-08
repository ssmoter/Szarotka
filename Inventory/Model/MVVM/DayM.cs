using CommunityToolkit.Mvvm.ComponentModel;

using CsvHelper.Configuration.Attributes;

using System.Collections.ObjectModel;

namespace Inventory.Model.MVVM
{

    public partial class DayM : ObservableObject, IDisposable
    {
        [Name("DzieńId")]
        public Guid Id { get; set; }

        [Name("DzieńKierowcaId")]
        public Guid DriverGuid { get; set; }

        [ObservableProperty]
        [Name("DzieńOpis")]
        string description;

        [ObservableProperty]
        [Name("DzieńStowrzono")]
        DateTime created;

        decimal totalPriceProduct;
        [Name("DzieńUtargProdukty")]
        public decimal TotalPriceProduct
        {
            get => totalPriceProduct;
            set
            {
                if (SetProperty(ref totalPriceProduct, value))
                {
                    OnPropertyChanged(nameof(TotalPriceProduct));
                    UpdateTotalPrice();
                }
            }
        }

        decimal totalPriceCake;
        [Name("DzieńUtargCiasto")]
        public decimal TotalPriceCake
        {
            get => totalPriceCake;
            set
            {
                if (SetProperty(ref totalPriceCake, value))
                {
                    OnPropertyChanged(nameof(TotalPriceCake));
                    UpdateTotalPrice();
                }
            }
        }
        [ObservableProperty]
        [Name("DzieńUtargSuma")]
        decimal totalPrice;

        decimal totalPriceCorrect;
        [Name("DzieńUtargKorekta")]
        public decimal TotalPriceCorrect
        {
            get => totalPriceCorrect;
            set
            {
                if (SetProperty(ref totalPriceCorrect, value))
                {
                    OnPropertyChanged(nameof(TotalPriceCorrect));
                    UpdateTotalPrice();
                }
            }
        }

        decimal totalPriceMoney;
        [Name("DzieńUtargPieniądze")]
        public decimal TotalPriceMoney
        {
            get => totalPriceMoney;
            set
            {
                if (SetProperty(ref totalPriceMoney, value))
                {
                    OnPropertyChanged(nameof(TotalPriceMoney));
                    UpdateTotalPrice();
                }
            }
        }

        [ObservableProperty]
        [Name("DzieńUtagRóżnica")]
        decimal totalPriceDifference;


        [ObservableProperty]
        [Name("DzieńUtargUtargPoKorekcie")]
        decimal totalPriceAfterCorrect;

        ObservableCollection<ProductM> products;
        public ObservableCollection<ProductM> Products
        {
            get => products;
            set
            {
                if (SetProperty(ref products, value))
                {
                    OnPropertyChanged(nameof(Products));
                    UpdateTotalPrice();
                }
            }
        }

        ObservableCollection<CakeM> cakes;
        public ObservableCollection<CakeM> Cakes
        {
            get => cakes;
            set
            {
                if (SetProperty(ref cakes, value))
                {
                    OnPropertyChanged(nameof(Cakes));
                    UpdateTotalPrice();
                }
            }
        }

        void UpdateTotalPrice()
        {
            if (Products is not null)
                TotalPriceProduct = Products.Sum(x => x.PriceTotalAfterCorrect);
            if (Cakes is not null)
                TotalPriceCake = Cakes.Where(x => x.IsSell).Sum(x => x.Price);

            TotalPrice = TotalPriceProduct + TotalPriceCake;
            TotalPriceAfterCorrect = TotalPrice + TotalPriceCorrect;

            TotalPriceDifference = TotalPriceMoney - TotalPriceAfterCorrect;

        }

        public void Dispose()
        {
            Products.Clear();
            Cakes.Clear();
            GC.SuppressFinalize(this);
        }

        public DayM()
        {
            Products = new ObservableCollection<ProductM>();
            Cakes = new ObservableCollection<CakeM>();
            Inventory.Service.ProductUpdatePriceService.UpdatePrice += UpdateTotalPrice;
        }
    }
}
