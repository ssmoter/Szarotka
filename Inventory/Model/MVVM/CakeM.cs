using CommunityToolkit.Mvvm.ComponentModel;

namespace Inventory.Model.MVVM
{
    public partial class CakeM : ObservableObject
    {
        public int Id { get; set; }
        public int DayId { get; set; }

        [ObservableProperty]
        decimal price;


        bool isSell;
        public bool IsSell
        {
            get => isSell;
            set
            {
                if (SetProperty(ref isSell, value))
                {
                    OnPropertyChanged(nameof(IsSell));
                    SetColor();
                    Inventory.Service.ProductUpdatePriceService.OnUpdate();
                }
            }
        }

        public CakeM()
        {
            SetColor();
        }

        [ObservableProperty]
        Color color;

        public void SetColor()
        {
            if (IsSell)
                Color = Colors.Green;
            else
                Color = Colors.Red;
        }
    }
}
