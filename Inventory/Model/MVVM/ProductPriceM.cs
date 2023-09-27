using CommunityToolkit.Mvvm.ComponentModel;

using CsvHelper.Configuration.Attributes;

namespace Inventory.Model.MVVM
{
    public partial class ProductPriceM : ObservableObject
    {
        [Name("CenaProduktuId")]
        public Guid Id { get; set; }

        [Name("CenaProduktuNazwaId")]
        public Guid ProductNameId { get; set; }

        [ObservableProperty]
        [Name("CenaProduktuCena")]
        decimal price;
        [Name("CenaProduktuStworzono")]
        public DateTime Created { get; set; }
    }
}
