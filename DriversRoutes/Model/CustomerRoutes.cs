using CommunityToolkit.Mvvm.ComponentModel;

using SQLite;

namespace DriversRoutes.Model
{
    public partial class CustomerRoutes : ObservableObject, IDisposable
    {

        /// <summary>
        /// Id
        /// </summary>
        Guid id;
        [PrimaryKey]
        public Guid Id
        {
            get => id;
            set
            {
                if (SetProperty(ref id, value))
                    OnPropertyChanged(nameof(Id));
            }
        }


        /// <summary>
        /// Id trasy
        /// </summary>
        [ObservableProperty]
        Guid routesId;
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
                if (SetProperty(ref queueNumber, value))
                    OnPropertyChanged(nameof(QueueNumber));
            }
        }
        /// <summary>
        /// Nazwa
        /// </summary>
        [ObservableProperty]
        string name;
        /// <summary>
        /// Opis
        /// </summary>
        [ObservableProperty]
        string description;
        /// <summary>
        /// Numer telefonu
        /// </summary>
        [ObservableProperty]
        string phoneNumber;
        /// <summary>
        /// Czas w Datetime
        /// </summary>
        [Ignore]
        public DateTime CreatedDate
        {           
            get
            { 
                return new DateTime(Created).ToLocalTime();
            }
            set
            {
                Created = value.ToUniversalTime().Ticks;
                OnPropertyChanged(nameof(CreatedDate));
            }
        }
        /// <summary>
        /// Czas w Tick
        /// </summary>
        public long Created { get; set; }

        SelectedDayOfWeekRoutes dayOfWeek;
        [Ignore]
        public SelectedDayOfWeekRoutes DayOfWeek
        {
            get => dayOfWeek;
            set
            {
                if (SetProperty(ref dayOfWeek, value))
                    OnPropertyChanged(nameof(DayOfWeek));
            }
        }

        ResidentialAddress residentialAddress;
        [Ignore]
        public ResidentialAddress ResidentialAddress
        {
            get => residentialAddress;
            set
            {
                if (SetProperty(ref residentialAddress, value))
                    OnPropertyChanged(nameof(ResidentialAddress));
            }
        }
        /// <summary>
        /// Długość geograficzna
        /// </summary>
        [ObservableProperty]
        double longitude;
        /// <summary>
        /// Szerokość geograficzna
        /// </summary>
        [ObservableProperty]
        double latitude;

        public CustomerRoutes()
        {
            DayOfWeek ??= new();
            ResidentialAddress ??= new();
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
            Created = 0;
        }
    }
}
