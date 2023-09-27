using CommunityToolkit.Mvvm.ComponentModel;

using CsvHelper.Configuration.Attributes;

namespace Inventory.Model.MVVM
{
    public partial class ProductNameM : ObservableObject
    {
        [Name("NazwaProduktuId")]
        public Guid Id { get; set; }

        [ObservableProperty]
        [Name("NazwaProduktuNazwa")]
        string name;

        [ObservableProperty]
        [Name("NazwaProduktuOpis")]
        string description;

        [ObservableProperty]
        [Name("NazwaProduktuZdjęcie")]
        string img;
    }
}
