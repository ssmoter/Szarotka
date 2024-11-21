using CommunityToolkit.Mvvm.ComponentModel;

using SQLite;

namespace DataBase.Model.EntitiesRoutes
{
    public partial class CustomerRoutes : BaseEntities<Guid>, IDisposable
    {
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
}
