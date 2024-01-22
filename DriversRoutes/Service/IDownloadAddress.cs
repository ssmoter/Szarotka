using DriversRoutes.Model;

namespace DriversRoutes.Service
{
    public interface IDownloadAddress
    {
        Task<GoogleApiAddress> FindAddressFromCoordinates(double latitude, double longitude);
    }
}
