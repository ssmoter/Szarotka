using DataBase.Model.JsonContext;

using SQLite;

using System.Text.Json.Serialization;

namespace DataBase.Model.EntitiesInventory;

public partial class Cake : BaseEntities<Guid>
{
    private Guid dayId;
    public Guid DayId
    {
        get => dayId;
        set
        {
            if (SetProperty(ref dayId, value, nameof(DayId)))
            {
                //OnPropertyChanged(nameof(DayId));
            }
        }


    }

    private bool isSell;
    [JsonConverter(typeof(CustomBoolConverter))]
    public bool IsSell
    {
        get => isSell;
        set
        {
            if (SetProperty(ref isSell, value, nameof(IsSell)))
            {
                //OnPropertyChanged(nameof(IsSell));
                OnSell?.Invoke();
                ProductUpdatePriceService.OnUpdate();
            }
        }
    }

    public Action? OnSell;

    private int index;
    [Ignore]
    public int Index
    {
        get => index;
        set
        {
            if (SetProperty(ref index, value, nameof(Index)))
            {
                //OnPropertyChanged(nameof(index));
            }
        }
    }


    private int price;
    public int Price
    {
        get => price;
        set
        {
            if (SetProperty(ref price, value, nameof(Price)))
            {
                //OnPropertyChanged(nameof(Price));
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
            if (SetProperty(ref price, (int)(value * 100), nameof(PriceDecimal)))
            {
                //OnPropertyChanged(nameof(Price));
                OnPropertyChanged(nameof(PriceDecimal));
                ProductUpdatePriceService.OnUpdate();
            }
        }
    }




}

