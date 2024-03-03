using CommunityToolkit.Mvvm.ComponentModel;

using SQLite;

using System.Text;

namespace DataBase.Model.EntitiesRoutes
{
    public partial class SelectedDayOfWeekRoutes : ObservableObject
    {
        [PrimaryKey]
        public Guid Id { get; set; }
        public Guid CustomerId { get; set; }

        [ObservableProperty]
        bool sunday;

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
                if (SetProperty(ref _sundayTicks, value.Ticks))
                {
                    OnPropertyChanged(nameof(SundayTimeSpan));
                }
            }
        }

        [ObservableProperty]
        bool monday;
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
                if (SetProperty(ref _mondayTicks, value.Ticks))
                {
                    OnPropertyChanged(nameof(MondayTimeSpan));
                }
            }
        }

        [ObservableProperty]
        bool tuesday;
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
                if (SetProperty(ref _tuesdayTicks, value.Ticks))
                {
                    OnPropertyChanged(nameof(TuesdayTimeSpan));
                }
            }
        }

        [ObservableProperty]
        bool wednesday;
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
                if (SetProperty(ref _wednesdayTicks, value.Ticks))
                {
                    OnPropertyChanged(nameof(WednesdayTimeSpan));
                }
            }
        }

        [ObservableProperty]
        bool thursday;
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
                if (SetProperty(ref _thursdayTicks, value.Ticks))
                {
                    OnPropertyChanged(nameof(ThursdayTimeSpan));
                }
            }
        }

        [ObservableProperty]
        bool friday;
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
                if (SetProperty(ref _fridayTicks, value.Ticks))
                {
                    OnPropertyChanged(nameof(FridayTimeSpan));
                }
            }
        }

        [ObservableProperty]
        bool saturday;
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
                if (SetProperty(ref _saturdayTicks, value.Ticks))
                {
                    OnPropertyChanged(nameof(SaturdayTimeSpan));
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
                if (SetProperty(ref setAll, value))
                {
                    OnPropertyChanged(nameof(SetAll));

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
        public TimeSpan SetAllTimeSpan
        {
            get
            {
                return new TimeSpan(_setAllTicks);
            }
            set
            {
                if (SetProperty(ref _setAllTicks, value.Ticks))
                {
                    OnPropertyChanged(nameof(SetAllTimeSpan));
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

        [ObservableProperty]
        bool optional;

        //string valuesAsString;
        //public string ValuesAsString
        //{
        //    get
        //    {
        //        if (string.IsNullOrWhiteSpace(valuesAsString))
        //        {
        //            return this.ToString();
        //        }
        //        return valuesAsString;
        //    }
        //    set
        //    {
        //        if (SetProperty(ref valuesAsString, value))
        //        {
        //            OnPropertyChanged(nameof(ValuesAsString));
        //        }
        //    }
        //}
        //string valuesAsStringWithTheTime;
        //[Ignore]
        //public string ValuesAsStringWithTheTime
        //{
        //    get
        //    {
        //        if (string.IsNullOrWhiteSpace(valuesAsStringWithTheTime))
        //        {
        //            return this.ToStringWithTheTime();
        //        }
        //        return valuesAsStringWithTheTime;
        //    }
        //    set
        //    {
        //        if (SetProperty(ref valuesAsStringWithTheTime, value))
        //        {
        //            OnPropertyChanged(nameof(ValuesAsStringWithTheTime));
        //        }
        //    }
        //}


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
}
