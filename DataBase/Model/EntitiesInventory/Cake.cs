using CommunityToolkit.Mvvm.ComponentModel;

using SQLite;

namespace DataBase.Model.EntitiesInventory;

public partial class Cake : BaseEntities<Guid>
{
    [ObservableProperty]
    private Guid dayId;
    private bool isSell;
    public bool IsSell
    {
        get => isSell;
        set
        {
            if (SetProperty(ref isSell, value))
            {
                OnPropertyChanged(nameof(IsSell));
                SetColor();
                ProductUpdatePriceService.OnUpdate();
            }
        }
    }


    private int index;
    [Ignore]
    public int Index
    {
        get => index;
        set
        {
            if (SetProperty(ref index, value))
                OnPropertyChanged(nameof(index));
        }
    }


    private int price;
    public int Price
    {
        get => price;
        set
        {
            if (SetProperty(ref price, value))
            {
                OnPropertyChanged(nameof(Price));
                OnPropertyChanged(nameof(PriceDecimal));
            }
        }
    }

    [Ignore]
    public decimal PriceDecimal
    {
        get
        {
            return (decimal)price / 100m;
        }
        set
        {
            if (SetProperty(ref price, (int)(value * 100)))
            {
                OnPropertyChanged(nameof(Price));
                OnPropertyChanged(nameof(PriceDecimal));
                SetColor();
                ProductUpdatePriceService.OnUpdate();
            }
        }
    }

    private Color color;
    [Ignore]
    public Color Color
    {
        get => color;
        set
        {
            if (SetProperty(ref color, value))
                OnPropertyChanged(nameof(Color));
        }
    }

    public Cake()
    {
        SetColor();
    }
    public void SetColor()
    {
        if (IsSell)
            Color = Colors.Green;
        else
            Color = Colors.Red;
    }
}

