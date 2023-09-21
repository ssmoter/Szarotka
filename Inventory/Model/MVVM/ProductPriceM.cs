using CommunityToolkit.Mvvm.ComponentModel;

namespace Inventory.Model.MVVM
{
    public partial class ProductPriceM : ObservableObject
    {
        public int Id { get; set; }
        public int productNameId { get; set; }
        [ObservableProperty]
        decimal price;
        public DateTime Created { get; set; }
    }
}
