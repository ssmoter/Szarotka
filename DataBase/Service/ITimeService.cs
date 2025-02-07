namespace DataBase.Service
{
    public interface ITimeService
    {
        DateTime UtcNow();
    }

    public class CurrentUtc : ITimeService
    {
        public DateTime UtcNow() => DateTime.UtcNow;
    }

}
