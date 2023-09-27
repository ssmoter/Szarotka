using CommunityToolkit.Mvvm.ComponentModel;

using CsvHelper.Configuration.Attributes;

namespace Inventory.Model.MVVM
{
    public partial class DriverM : ObservableObject
    {
        [ObservableProperty]
        [Name("KierowcaId")]
        Guid id;

        [ObservableProperty]
        [Name("KierowcaNazwa")]
        string name;

        [ObservableProperty]
        [Name("KierowcaOpis")]
        string description;

        [ObservableProperty]
        [Name("KierowcaGuid")]
        Guid guid;
    }
}
