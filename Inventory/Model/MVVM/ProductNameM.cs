using CommunityToolkit.Mvvm.ComponentModel;

namespace Inventory.Model.MVVM
{
    public partial class ProductNameM : ObservableObject
    {
        public int Id { get; set; }
        [ObservableProperty]
        string name;
        [ObservableProperty]
        string description;
        [ObservableProperty]
        string img;
    }
}
