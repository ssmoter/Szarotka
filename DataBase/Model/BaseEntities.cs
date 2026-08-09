using CommunityToolkit.Mvvm.ComponentModel;

using DataBase.Model.EntitiesInventory;
using DataBase.Model.EntitiesRoutes;

using SQLite;

using System.Text.Json.Serialization;

namespace DataBase.Model;

[JsonDerivedType(typeof(UpdateLog), "UpdateLog")]

[JsonDerivedType(typeof(SelectedDayOfWeekRoutes), "SelectedDayOfWeekRoutes")]
[JsonDerivedType(typeof(ResidentialAddress), "ResidentialAddress")]
[JsonDerivedType(typeof(CustomerRoutes), "CustomerRoute")]

[JsonDerivedType(typeof(ProductPrice), "ProductPrice")]
[JsonDerivedType(typeof(ProductPrices), "ProductPrices")]
[JsonDerivedType(typeof(Day), "Day")]

[JsonDerivedType(typeof(Driver), "Driver")]
[JsonDerivedType(typeof(Product), "Product")]
[JsonDerivedType(typeof(ProductName), "ProductName")]
[JsonDerivedType(typeof(Cake), "Cake")]

public partial interface IDifference
{

}


public partial class BaseEntities<T> : ObservableObject, IDifference
{
    private T id = default!;
    [PrimaryKey]
    public T Id
    {
        get => id;
        set
        {
            if (SetProperty(ref id, value, nameof(Id)))
            { }
        }
    }
    [Ignore]
    [JsonConverter(typeof(SourceGenerator.CustomDateTimeConverter))]
    public DateTime Created
    {
        get
        {
            if (_createdTicks == 0)
            {
                return new DateTime();
            }
            var date = new DateTime(_createdTicks);
            return date.ToLocalTime();
        }
        set
        {
            if (SetProperty(ref _createdTicks, value.ToUniversalTime().Ticks, nameof(Created)))
            { }
        }
    }
    [Ignore]
    [JsonConverter(typeof(SourceGenerator.CustomDateTimeConverter))]
    public DateTime Updated
    {
        get
        {
            if (_updatedTicks == 0)
            {
                return new DateTime();
            }
            var date = new DateTime(_updatedTicks);
            return date.ToLocalTime();
        }
        set
        {
            if (SetProperty(ref _updatedTicks, value.ToUniversalTime().Ticks, nameof(Updated)))
            {
                OnPropertyChanged(nameof(Updated));
            }
        }
    }
    public long CreatedTicks
    {
        get => _createdTicks;
        set => _createdTicks = value;
    }
    public long UpdatedTicks
    {
        get => _updatedTicks;
        set => _updatedTicks = value;
    }
    private long _createdTicks;
    private long _updatedTicks;

    private bool isDelete;
    [JsonConverter(typeof(SourceGenerator.CustomBoolConverter))]
    public bool IsDelete
    {
        get => isDelete;
        set
        {
            if (SetProperty(ref isDelete, value, nameof(IsDelete))) { }
        }
    }

    private Guid userCreatedId;
    [JsonConverter(typeof(SourceGenerator.CustomGuidConverter))]
    public Guid UserCreatedId
    {
        get => userCreatedId;
        set
        {
            if (SetProperty(ref userCreatedId, value, nameof(UserCreatedId))) { }
        }
    }

    private Guid userUpdatedId;
    [JsonConverter(typeof(SourceGenerator.CustomGuidConverter))]
    public Guid UserUpdatedId
    {
        get => userUpdatedId;
        set
        {
            if (SetProperty(ref userUpdatedId, value, nameof(UserUpdatedId))) { }
        }
    }

    public BaseEntities()
    {
    }
    public BaseEntities(BaseEntities<T> copy)
    {

        Id = copy.Id;
        Created = copy.Created;
        CreatedTicks = copy.CreatedTicks;
        IsDelete = copy.IsDelete;
        Updated = copy.Updated;
        UpdatedTicks = copy.UpdatedTicks;
        UserUpdatedId = copy.UserUpdatedId;
        UserCreatedId = copy.UserCreatedId;
    }


    public const string AdditionalColumns = $@"
            [{nameof(CreatedTicks)}] INTEGER,
            [{nameof(UpdatedTicks)}] INTEGER,
            [{nameof(IsDelete)}] INTEGER,
            [{nameof(UserCreatedId)}] TEXT,
            [{nameof(UserUpdatedId)}] TEXT";

}

