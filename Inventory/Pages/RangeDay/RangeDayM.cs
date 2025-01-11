using CommunityToolkit.Mvvm.ComponentModel;

using DataBase.Model.EntitiesInventory;

using System.Text.Json.Serialization;

namespace Inventory.Pages.RangeDay
{
    [JsonSerializable(typeof(RangeDayM))]
    [JsonSerializable(typeof(RangeDayM[]))]
    [JsonSourceGenerationOptions(WriteIndented = true,
    DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull,
    PropertyNameCaseInsensitive = true)]
    public partial class RangeDayMJsonSerializerContext : JsonSerializerContext
    { }
    public partial class RangeDayM : ObservableObject
    {
        private Day day;
        public Day Day
        {
            get => day;
            set
            {
                if (SetProperty(ref day, value))
                {
                    OnPropertyChanged(nameof(Day));
                }
            }
        }
        private Driver driver;
        public Driver Driver
        {
            get => driver;
            set
            {
                if (SetProperty(ref driver, value))
                {
                    OnPropertyChanged(nameof(Driver));
                }
            }
        }
        public RangeDayM(Day day, Driver driver)
        {
            Day = day;
            Driver = driver;
        }
        public RangeDayM()
        {
            Day = new();
            Driver = new();
        }
    }

}
