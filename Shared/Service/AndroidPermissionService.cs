namespace Shared.Service
{
    public static class AndroidPermissionService
    {
        public static async Task<bool> CheckRead()
        {
            PermissionStatus status = PermissionStatus.Unknown;

            status = await Permissions.CheckStatusAsync<Permissions.StorageRead>();

            if (status == PermissionStatus.Granted)
                return true;


            if (Permissions.ShouldShowRationale<Permissions.StorageRead>())
            {
                await Shell.Current.DisplayAlertAsync("Pozwolenie", "Pozwolenie na odczytywanie danych z dystku jest wymagane", "Ok");
            }

            status = await Permissions.RequestAsync<Permissions.StorageRead>();

            if (status == PermissionStatus.Granted)
                return true;
            else
                return false;
        }
        public static async Task<bool> CheckWrite()
        {
            var status = PermissionStatus.Unknown;

            status = await Permissions.CheckStatusAsync<Permissions.StorageWrite>();

            if (status == PermissionStatus.Granted)
                return true;

            if (Permissions.ShouldShowRationale<Permissions.StorageWrite>())
            {
                await Shell.Current.DisplayAlertAsync("Pozwolenie", "Pozwolenie na zapis danych z dystku jest wymagane", "Ok");
            }

            status = await Permissions.RequestAsync<Permissions.StorageWrite>();
            if (status == PermissionStatus.Granted)
                return true;
            else
                return false;
        }
        public static async Task<bool> CheckMedia()
        {
            var status = PermissionStatus.Unknown;

            status = await Permissions.CheckStatusAsync<Permissions.Media>();

            if (status == PermissionStatus.Granted)
                return true;

            if (Permissions.ShouldShowRationale<Permissions.Media>())
            {
                await Shell.Current.DisplayAlertAsync("Pozwolenie", "Pozwolenie na odczyt danych z galerii jest wymagane", "Ok");
            }

            status = await Permissions.RequestAsync<Permissions.Media>();
            if (status == PermissionStatus.Granted)
                return true;
            else
                return false;
        }

        public static async Task<bool> LocationWhenInUse()
        {
            var status = PermissionStatus.Unknown;

            status = await Permissions.CheckStatusAsync<Permissions.LocationWhenInUse>();

            if (status == PermissionStatus.Granted)
                return true;

            if (Permissions.ShouldShowRationale<Permissions.LocationWhenInUse>())
            {
                await Shell.Current.DisplayAlertAsync("Pozwolenie", "Pozwolenie na sprawdzenie lokalizacji jest wymagane", "Ok");
            }

            status = await Permissions.RequestAsync<Permissions.LocationWhenInUse>();
            if (status == PermissionStatus.Granted)
                return true;
            else
                return false;
        }


        public static async Task<bool> CheckAllPermissionsAboutStorage()
        {
            if (OperatingSystem.IsWindows())
            {
                return true;
            }
            if (!await CheckMedia())
            {
                await Shell.Current.DisplayAlertAsync("Pozwolenie", "Dane pozwolenie jest wymagane", "Ok");
                return false;
            }

            if (!OperatingSystem.IsAndroidVersionAtLeast(33))
            {
                if (!await CheckWrite())
                {
                    await Shell.Current.DisplayAlertAsync("Pozwolenie", "Dane pozwolenie jest wymagane", "Ok");
                    return false;
                }

                if (!await CheckRead())
                {
                    await Shell.Current.DisplayAlertAsync("Pozwolenie", "Dane pozwolenie jest wymagane", "Ok");
                    return false;
                }
            }

            return true;
        }




        public static bool InternetCheck()
        {
            if (OperatingSystem.IsWindows())
            {
                return true;
            }
            bool isConnected = Connectivity.Current.NetworkAccess == NetworkAccess.Internet;
            if (isConnected)
                return false;

            throw new Exception("Brak dostępu do internetu, żądanie zostało anulowane");
        }
    }
}

