using CommunityToolkit.Mvvm.ComponentModel;

namespace Inventory.Model.MVVM
{
    public partial class CakeM : ObservableObject
    {
        public Guid Id { get; set; }
        public Guid DayId { get; set; }

        [ObservableProperty]
        decimal price;

        [ObservableProperty]
        int index;

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
