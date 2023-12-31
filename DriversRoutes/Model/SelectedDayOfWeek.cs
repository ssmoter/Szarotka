﻿using CommunityToolkit.Mvvm.ComponentModel;

using SQLite;

using System.Text;

namespace DriversRoutes.Model
{
    public partial class SelectedDayOfWeekRoutes : ObservableObject
    {
        public Guid Id { get; set; }
        public Guid CustomerId { get; set; }

        [ObservableProperty]
        bool sunday;

        [ObservableProperty]
        bool monday;

        [ObservableProperty]
        bool tuesday;

        [ObservableProperty]
        bool wednesday;

        [ObservableProperty]
        bool thursday;

        [ObservableProperty]
        bool friday;

        [ObservableProperty]
        bool saturday;

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
        string valuesAsString;
        public string ValuesAsString
        {
            get
            {
                if (string.IsNullOrWhiteSpace(valuesAsString))
                {
                    return this.ToString();
                }
                return valuesAsString;
            }
            set
            {
                if (SetProperty(ref valuesAsString, value))
                {
                    OnPropertyChanged(nameof(ValuesAsString));
                }
            }
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
    }
}
