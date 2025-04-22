using DataBase.Model.JsonContext;

using SQLite;

using System.Text;
using System.Text.Json.Serialization;

namespace DataBase.Model.EntitiesRoutes;

public partial class SelectedDayOfWeekRoutes : BaseEntities<Guid>
{
    private Guid customerId;
    public Guid CustomerId
    {
        get => customerId;
        set
        {
            if (SetProperty(ref customerId, value, nameof(CustomerId))) { }
        }
    }

    private bool sunday;
    [JsonConverter(typeof(CustomBoolConverter))]
    public bool Sunday
    {
        get => sunday;
        set
        {
            if (!SetProperty(ref sunday, value, nameof(Sunday))) { }
        }
    }

    long _sundayTicks;
    public long SundayTicks
    {
        get => _sundayTicks;
        set => _sundayTicks = value;
    }
    [Ignore]
    public TimeSpan SundayTimeSpan
    {
        get
        {
            return new TimeSpan(_sundayTicks);
        }
        set
        {
            if (SetProperty(ref _sundayTicks, value.Ticks, nameof(SundayTimeSpan)))
            {
                //OnPropertyChanged(nameof(SundayTimeSpan));
            }
        }
    }

    private bool monday;
    [JsonConverter(typeof(CustomBoolConverter))]
    public bool Monday
    {
        get => monday;
        set
        {
            if (SetProperty(ref monday, value, nameof(Monday))) { }
        }
    }
    long _mondayTicks;
    public long MondayTicks
    {
        get => _mondayTicks;
        set => _mondayTicks = value;
    }
    [Ignore]
    public TimeSpan MondayTimeSpan
    {
        get
        {
            return new TimeSpan(_mondayTicks);
        }
        set
        {
            if (SetProperty(ref _mondayTicks, value.Ticks, nameof(MondayTimeSpan)))
            {
                //OnPropertyChanged(nameof(MondayTimeSpan));
            }
        }
    }

    private bool tuesday;
    [JsonConverter(typeof(CustomBoolConverter))]
    public bool Tuesday
    {
        get => tuesday;
        set
        {
            if (SetProperty(ref tuesday, value, nameof(Tuesday))) { }
        }
    }
    long _tuesdayTicks;
    public long TuesdayTicks
    {
        get => _tuesdayTicks;
        set => _tuesdayTicks = value;
    }
    [Ignore]
    public TimeSpan TuesdayTimeSpan
    {
        get
        {
            return new TimeSpan(_tuesdayTicks);
        }
        set
        {
            if (SetProperty(ref _tuesdayTicks, value.Ticks, nameof(TuesdayTimeSpan)))
            {
                //OnPropertyChanged(nameof(TuesdayTimeSpan));
            }
        }
    }

    private bool wednesday;
    [JsonConverter(typeof(CustomBoolConverter))]
    public bool Wednesday
    {
        get => wednesday;
        set
        {
            if (!SetProperty(ref wednesday, value, nameof(Wednesday))) { }
        }
    }
    long _wednesdayTicks;
    public long WednesdayTicks
    {
        get => _wednesdayTicks;
        set => _wednesdayTicks = value;
    }
    [Ignore]
    public TimeSpan WednesdayTimeSpan
    {
        get
        {
            return new TimeSpan(_wednesdayTicks);
        }
        set
        {
            if (SetProperty(ref _wednesdayTicks, value.Ticks, nameof(WednesdayTimeSpan)))
            {
                //OnPropertyChanged(nameof(WednesdayTimeSpan));
            }
        }
    }

    private bool thursday;
    [JsonConverter(typeof(CustomBoolConverter))]
    public bool Thursday
    {
        get => thursday;
        set
        {
            if (SetProperty(ref thursday, value, nameof(Thursday))) { }
        }
    }
    long _thursdayTicks;
    public long ThursdayTicks
    {
        get => _thursdayTicks;
        set => _thursdayTicks = value;
    }
    [Ignore]
    public TimeSpan ThursdayTimeSpan
    {
        get
        {
            return new TimeSpan(_thursdayTicks);
        }
        set
        {
            if (SetProperty(ref _thursdayTicks, value.Ticks, nameof(ThursdayTimeSpan)))
            {
                //OnPropertyChanged(nameof(ThursdayTimeSpan));
            }
        }
    }

    private bool friday;
    [JsonConverter(typeof(CustomBoolConverter))]
    public bool Friday
    {
        get => friday;
        set
        {
            if (SetProperty(ref friday, value, nameof(Friday))) { }
        }
    }
    long _fridayTicks;
    public long FridayTicks
    {
        get => _fridayTicks;
        set => _fridayTicks = value;
    }
    [Ignore]
    public TimeSpan FridayTimeSpan
    {
        get
        {
            return new TimeSpan(_fridayTicks);
        }
        set
        {
            if (SetProperty(ref _fridayTicks, value.Ticks, nameof(FridayTimeSpan)))
            {
                //OnPropertyChanged(nameof(FridayTimeSpan));
            }
        }
    }

    private bool saturday;
    [JsonConverter(typeof(CustomBoolConverter))]
    public bool Saturday
    {
        get => saturday;
        set
        {
            if (SetProperty(ref saturday, value, nameof(Saturday))) { }
        }
    }
    long _saturdayTicks;
    public long SaturdayTicks
    {
        get => _saturdayTicks;
        set => _saturdayTicks = value;
    }
    [Ignore]
    public TimeSpan SaturdayTimeSpan
    {
        get
        {
            return new TimeSpan(_saturdayTicks);
        }
        set
        {
            if (SetProperty(ref _saturdayTicks, value.Ticks, nameof(SaturdayTimeSpan)))
            {
                //OnPropertyChanged(nameof(SaturdayTimeSpan));
            }
        }
    }


    bool setAll;
    [Ignore]
    public bool SetAll
    {
        get => setAll;
        set
        {
            if (SetProperty(ref setAll, value, nameof(SetAll)))
            {
                //OnPropertyChanged(nameof(SetAll));

                if (SetAll)
                {
                    Monday = true;
                    Tuesday = true;
                    Wednesday = true;
                    Thursday = true;
                    Friday = true;
                    Saturday = true;
                }
                else
                {
                    Monday = false;
                    Tuesday = false;
                    Wednesday = false;
                    Thursday = false;
                    Friday = false;
                    Saturday = false;
                }
            }
        }
    }

    long _setAllTicks;
    [Ignore]
    [JsonIgnore]
    public TimeSpan SetAllTimeSpan
    {
        get
        {
            return new TimeSpan(_setAllTicks);
        }
        set
        {
            if (SetProperty(ref _setAllTicks, value.Ticks, nameof(SetAllTimeSpan)))
            {
                //OnPropertyChanged(nameof(SetAllTimeSpan));
            }
            if (SetAll)
            {
                SundayTimeSpan = SetAllTimeSpan;
                MondayTimeSpan = SetAllTimeSpan;
                TuesdayTimeSpan = SetAllTimeSpan;
                WednesdayTimeSpan = SetAllTimeSpan;
                ThursdayTimeSpan = SetAllTimeSpan;
                FridayTimeSpan = SetAllTimeSpan;
                SaturdayTimeSpan = SetAllTimeSpan;
            }
        }
    }

    private bool optional;
    [JsonConverter(typeof(CustomBoolConverter))]
    public bool Optional
    {
        get => optional;
        set
        {
            if (SetProperty(ref optional, value, nameof(Optional))) { }
        }
    }

    public SelectedDayOfWeekRoutes(SelectedDayOfWeekRoutes copy)
    {
        this.Id = copy.Id;
        this.Created = copy.Created;
        this.Updated = copy.Updated;
        this.Sunday = copy.Sunday;
        this.SundayTimeSpan = copy.SundayTimeSpan;
        this.Monday = copy.Monday;
        this.MondayTimeSpan = copy.MondayTimeSpan;
        this.Tuesday = copy.Tuesday;
        this.TuesdayTimeSpan = copy.TuesdayTimeSpan;
        this.Wednesday = copy.Wednesday;
        this.WednesdayTimeSpan = copy.WednesdayTimeSpan;
        this.Thursday = copy.Thursday;
        this.ThursdayTimeSpan = copy.ThursdayTimeSpan;
        this.Friday = copy.Friday;
        this.FridayTimeSpan = copy.FridayTimeSpan;
        this.Saturday = copy.Saturday;
        this.SaturdayTimeSpan = copy.SaturdayTimeSpan;
        this.Optional = copy.Optional;

    }
    public override string ToString()
    {
        StringBuilder sb = new();

        if (Sunday)
        {
            sb.Append("Niedziela; ");
        }
        if (Monday)
        {
            sb.Append("Poniedziałek; ");
        }
        if (Tuesday)
        {
            sb.Append("Wtorek; ");
        }
        if (Wednesday)
        {
            sb.Append("Środa; ");
        }
        if (Thursday)
        {
            sb.Append("Czwartek; ");
        }
        if (Friday)
        {
            sb.Append("Piątek; ");
        }
        if (Saturday)
        {
            sb.Append("Sobota; ");
        }
        return sb.ToString();
    }

    public string ToStringWithTheTime()
    {
        StringBuilder sb = new();
        if (Sunday)
        {
            sb.Append("Niedziela-");
            sb.Append(SundayTimeSpan.ToString("hh\\:mm"));
        }
        if (Monday)
        {
            if (Sunday)
                sb.AppendLine();

            sb.Append("Poniedziałek-");
            sb.Append(MondayTimeSpan.ToString("hh\\:mm"));

        }
        if (Tuesday)
        {
            if (Sunday || Monday)
                sb.AppendLine();

            sb.Append("Wtorek-");
            sb.Append(TuesdayTimeSpan.ToString("hh\\:mm"));

        }
        if (Wednesday)
        {
            if (Sunday || Monday || Tuesday)
                sb.AppendLine();

            sb.Append("Środa-");
            sb.Append(WednesdayTimeSpan.ToString("hh\\:mm"));

        }
        if (Thursday)
        {
            if (Sunday || Monday || Tuesday || Thursday)
                sb.AppendLine();

            sb.Append("Czwartek-");
            sb.Append(ThursdayTimeSpan.ToString("hh\\:mm"));

        }
        if (Friday)
        {
            if (Sunday || Monday || Tuesday || Thursday || Friday)
                sb.AppendLine();

            sb.Append("Piątek-");
            sb.Append(FridayTimeSpan.ToString("hh\\:mm"));
        }
        if (Saturday)
        {
            if (Sunday || Monday || Tuesday || Thursday || Friday || Saturday)
                sb.AppendLine();

            sb.Append("Sobota-");
            sb.Append(SaturdayTimeSpan.ToString("hh\\:mm"));
        }
        if (Optional)
        {
            if (Sunday || Monday || Tuesday || Thursday || Friday || Saturday || Optional)
                sb.AppendLine();

            sb.Append("Klient Okazjonalny");
        }

        return sb.ToString();
    }
    public string TodayTime(DayOfWeek dayOfWeek)
    {
        return dayOfWeek switch
        {
            DayOfWeek.Sunday => SundayTimeSpan.ToString("hh\\:mm"),
            DayOfWeek.Monday => MondayTimeSpan.ToString("hh\\:mm"),
            DayOfWeek.Tuesday => TuesdayTimeSpan.ToString("hh\\:mm"),
            DayOfWeek.Wednesday => WednesdayTimeSpan.ToString("hh\\:mm"),
            DayOfWeek.Thursday => ThursdayTimeSpan.ToString("hh\\:mm"),
            DayOfWeek.Friday => FridayTimeSpan.ToString("hh\\:mm"),
            DayOfWeek.Saturday => SaturdayTimeSpan.ToString("hh\\:mm"),
            _ => "",
        };
    }
    public string TodayTime()
    {
        var today = DateTime.Today.DayOfWeek;
        return TodayTime(today);
    }
    public SelectedDayOfWeekRoutes()
    {
        var timeSpan = DateTime.Now.TimeOfDay;
        SundayTimeSpan = timeSpan;
        MondayTimeSpan = timeSpan;
        TuesdayTimeSpan = timeSpan;
        WednesdayTimeSpan = timeSpan;
        ThursdayTimeSpan = timeSpan;
        FridayTimeSpan = timeSpan;
        SaturdayTimeSpan = timeSpan;
        SetAllTimeSpan = timeSpan;
    }
}

