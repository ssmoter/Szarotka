using SQLite;

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

    public CustomerRoutes(CustomerRoutes copy) : base(copy)
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


public static class CustomerRoutesExtensions
{
    // Sorts by all enabled days in DayOfWeek, in the order of the days in the week
    public static IEnumerable<CustomerRoutes> SortByEnabledDays(this IEnumerable<CustomerRoutes> list)
    {
        return list.OrderBy(x => GetFirstEnabledDayTicks(x.DayOfWeek));
    }

    // Sorts by a specific day, if enabled, otherwise puts at the end
    public static IEnumerable<CustomerRoutes> SortByDay(this IEnumerable<CustomerRoutes> list, DayOfWeek dayOfWeek)
    {
        return list.OrderBy(x => IsDayEnabled(x.DayOfWeek, dayOfWeek) ? GetDayTicks(x.DayOfWeek, dayOfWeek) : long.MaxValue);
    }

    // Sorts by multiple days in order, using the first enabled day in the provided list
    public static IEnumerable<CustomerRoutes> SortByDays(this IEnumerable<CustomerRoutes> list, params DayOfWeek[] days)
    {
        return list.OrderBy(x => GetFirstEnabledDayTicks(x.DayOfWeek, days));
    }

    private static bool IsDayEnabled(SelectedDayOfWeekRoutes dayOfWeek, DayOfWeek day)
    {
        return day switch
        {
            DayOfWeek.Sunday => dayOfWeek.Sunday,
            DayOfWeek.Monday => dayOfWeek.Monday,
            DayOfWeek.Tuesday => dayOfWeek.Tuesday,
            DayOfWeek.Wednesday => dayOfWeek.Wednesday,
            DayOfWeek.Thursday => dayOfWeek.Thursday,
            DayOfWeek.Friday => dayOfWeek.Friday,
            DayOfWeek.Saturday => dayOfWeek.Saturday,
            _ => false
        };
    }

    private static long GetDayTicks(SelectedDayOfWeekRoutes dayOfWeek, DayOfWeek day)
    {
        return day switch
        {
            DayOfWeek.Sunday => dayOfWeek.SundayTicks,
            DayOfWeek.Monday => dayOfWeek.MondayTicks,
            DayOfWeek.Tuesday => dayOfWeek.TuesdayTicks,
            DayOfWeek.Wednesday => dayOfWeek.WednesdayTicks,
            DayOfWeek.Thursday => dayOfWeek.ThursdayTicks,
            DayOfWeek.Friday => dayOfWeek.FridayTicks,
            DayOfWeek.Saturday => dayOfWeek.SaturdayTicks,
            _ => long.MaxValue
        };
    }

    // Returns the ticks of the first enabled day in the week (Sunday to Saturday)
    private static long GetFirstEnabledDayTicks(SelectedDayOfWeekRoutes dayOfWeek)
    {
        foreach (DayOfWeek day in Enum.GetValues(typeof(DayOfWeek)))
        {
            if (IsDayEnabled(dayOfWeek, day))
                return GetDayTicks(dayOfWeek, day);
        }
        return long.MaxValue;
    }

    // Returns the ticks of the first enabled day in the provided days array
    private static long GetFirstEnabledDayTicks(SelectedDayOfWeekRoutes dayOfWeek, DayOfWeek[] days)
    {
        foreach (var day in days)
        {
            if (IsDayEnabled(dayOfWeek, day))
                return GetDayTicks(dayOfWeek, day);
        }
        return long.MaxValue;
    }
}
