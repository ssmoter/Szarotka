using SQLite;

using System.Text.Json.Serialization;

namespace DataBase.Model.EntitiesRoutes;

public partial class CustomerRoutes : BaseEntities<Guid>, IDisposable
{
    /// <summary>
    /// Id trasy
    /// </summary>
    private Guid routesId;
    public Guid RoutesId
    {
        get => routesId;
        set
        {
            if (SetProperty(ref routesId, value, nameof(RoutesId)))
            {
                //OnPropertyChanged(nameof(RoutesId));
            }
        }
    }
    /// <summary>
    /// Kolejność jazdy
    /// </summary>
    int queueNumber;
    [Ignore]
    public int QueueNumber
    {
        get => queueNumber;
        set
        {
            if (SetProperty(ref queueNumber, value, nameof(QueueNumber)))
            {
                //OnPropertyChanged(nameof(QueueNumber));
            }
        }
    }
    /// <summary>
    /// Nazwa
    /// </summary>
    private string name = "";
    public string Name
    {
        get => name;
        set
        {
            if (SetProperty(ref name, value, nameof(Name)))
            {
                //OnPropertyChanged(nameof(Name));
            }
        }
    }
    /// <summary>
    /// Opis
    /// </summary>
    private string description = "";
    public string Description
    {
        get => description;
        set
        {
            if (SetProperty(ref description, value, nameof(Description)))
            {
                //OnPropertyChanged(nameof(Description));
            }
        }
    }
    /// <summary>
    /// Numer telefonu
    /// </summary>
    private string phoneNumber = "";
    public string PhoneNumber
    {
        get => phoneNumber;
        set
        {
            if (SetProperty(ref phoneNumber, value, nameof(PhoneNumber)))
            {
                //OnPropertyChanged(nameof(PhoneNumber));
            }
        }
    }

    SelectedDayOfWeekRoutes dayOfWeek = new();
    [Ignore]
    public SelectedDayOfWeekRoutes DayOfWeek
    {
        get => dayOfWeek;
        set
        {
            if (SetProperty(ref dayOfWeek, value, nameof(DayOfWeek)))
            {
                //OnPropertyChanged(nameof(DayOfWeek));
            }
        }
    }

    ResidentialAddress residentialAddress = new();
    [Ignore]
    public ResidentialAddress ResidentialAddress
    {
        get => residentialAddress;
        set
        {
            if (SetProperty(ref residentialAddress, value, nameof(ResidentialAddress)))
            {
                //OnPropertyChanged(nameof(ResidentialAddress));
            }
        }
    }
    /// <summary>
    /// Długość geograficzna
    /// </summary>
    private double longitude;
    public double Longitude
    {
        get => longitude;
        set
        {
            if (SetProperty(ref longitude, value, nameof(Longitude)))
            {
                //OnPropertyChanged(nameof(Longitude));
            }
        }
    }
    /// <summary>
    /// Szerokość geograficzna
    /// </summary>
    private double latitude;
    public double Latitude
    {
        get => latitude;
        set
        {
            if (SetProperty(ref latitude, value, nameof(Latitude)))
            {
                //OnPropertyChanged(nameof(Latitude));
            }
        }
    }


    public CustomerRoutes()
    {
        DayOfWeek ??= new();
        ResidentialAddress ??= new();
    }

    public CustomerRoutes(CustomerRoutes copy)
    {
        this.Id = copy.Id;
        this.Created = copy.Created;
        this.Updated = copy.Updated;

        this.Name = copy.Name;
        this.RoutesId = copy.routesId;
        this.Description = copy.Description;
        this.PhoneNumber = copy.PhoneNumber;
        this.Latitude = copy.Latitude;
        this.Longitude = copy.Longitude;
        this.QueueNumber = copy.QueueNumber;

        this.DayOfWeek = new SelectedDayOfWeekRoutes(copy.DayOfWeek);
        this.ResidentialAddress = new ResidentialAddress(copy.residentialAddress);
    }

    public void Dispose()
    {
        Id = Guid.Empty;
        RoutesId = Guid.Empty;
        QueueNumber = 0;
        Name = string.Empty;
        Description = string.Empty;
        PhoneNumber = string.Empty;
        Longitude = 0;
        Latitude = 0;
    }
}

