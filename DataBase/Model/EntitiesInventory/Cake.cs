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
            if (SetProperty(ref index, value, nameof(Index)))
            {
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
                OnPropertyChanged(nameof(PriceDecimal));

            }
        }
    }

    public Cake()
    { }
    public Cake(Cake copy) : base(copy)
    {
        DayId = copy.DayId;
        IsSell = copy.IsSell;
        Index = copy.Index;
        Price = copy.Price;
        PriceDecimal = copy.PriceDecimal;
    }

}

