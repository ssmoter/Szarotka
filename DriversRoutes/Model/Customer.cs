using Microsoft.Maui.Controls.Maps;

namespace DriversRoutes.Model
{
    public class Customer : Location
    {
        public int Index;
        public Guid Id;
        public string Name;
        public string Description;



        public Customer()
        {
            Pin pin = new Pin
            {
                Label = "Santa Cruz",
                Address = "The city with a boardwalk",
                Type = PinType.Place,
                Location = new Location(36.9628066, -122.0194722)
            };
            var a = new Location() { };
        }
    }
}
