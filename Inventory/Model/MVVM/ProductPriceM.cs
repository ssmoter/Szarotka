using CommunityToolkit.Mvvm.ComponentModel;

namespace Inventory.Model.MVVM
{
    public partial class ProductPriceM : ObservableObject
    {
        public Guid Id { get; set; }
        public Guid ProductNameId { get; set; }
        [ObservableProperty]
        decimal price;
        public DateTime Created { get; set; }
    }
}
