using SQLite;

namespace DriversRoutes.Model
{
    public class CustomerRoutes : IDisposable
    {

        /// <summary>
        /// Id
        /// </summary>
        public Guid Id { get; set; }
        /// <summary>
        /// Id trasy
        /// </summary>
        public Guid RoutesId { get; set; }
        /// <summary>
        /// Kolejność jazdy
        /// </summary>

        public int Index { get; set; }
        /// <summary>
        /// Nazwa
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// Opis
        /// </summary>
        public string Description { get; set; }
        /// <summary>
        /// Numer telefonu
        /// </summary>
        public string PhoneNumber { get; set; }
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
            }
        }
        /// <summary>
        /// Czas w Tick
        /// </summary>
        public long Created { get; set; }
        [Ignore]
        public SelectedDayOfWeekRoutes DayOfWeek { get; set; }
        /// <summary>
        /// Długość geograficzna
        /// </summary>
        public double Longitude { get; set; }
        /// <summary>
        /// Szerokość geograficzna
        /// </summary>
        public double Latitude { get; set; }

        public void Dispose()
        {
            Id = Guid.Empty;
            RoutesId = Guid.Empty;
            Index = 0;
            Name = string.Empty;
            Description = string.Empty;
            PhoneNumber = string.Empty;
            Longitude = 0;
            Latitude = 0;
            Created = 0;
        }
    }
}
