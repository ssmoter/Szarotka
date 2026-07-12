using Microsoft.Extensions.Configuration;

namespace Shared.Service
{
    public static class SharedHttpClients
    {
        public static IServiceCollection AddMyServiceHttpClients(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddHttpClient(MyHttpClientsType.Szarotka, client =>
            {
                string urlSzarotka = configuration[$"HttpClients:{MyHttpClientsType.Szarotka}"];
#if DEBUG
#if ANDROID
                urlSzarotka = configuration[$"HttpClients:{MyHttpClientsType.SzarotkaDebugAndorid}"];
#endif
#if WINDOWS
                urlSzarotka = configuration[$"HttpClients:{MyHttpClientsType.SzarotkaDebugWin}"];
#endif
#endif

                client.BaseAddress = new Uri(urlSzarotka);
                client.DefaultRequestHeaders.Add("Accept", "application/json");
            });
            services.AddHttpClient(MyHttpClientsType.GoogleRoutes, client =>
            {
                string urlGoogleMaps = configuration[$"HttpClients:{MyHttpClientsType.GoogleRoutes}"];
                client.BaseAddress = new Uri(urlGoogleMaps);
                client.DefaultRequestHeaders.Add("Accept", "application/json");
            });
            services.AddHttpClient(MyHttpClientsType.GoogleApis, client =>
            {
                string urlGoogleMaps = configuration[$"HttpClients:{MyHttpClientsType.GoogleApis}"];
                client.BaseAddress = new Uri(urlGoogleMaps);
                client.DefaultRequestHeaders.Add("Accept", "application/json");
            });
            return services;
        }
    }

    public static class MyHttpClientsType
    {
        public static string Szarotka => "szarotka";
        internal static string SzarotkaDebugWin => "szarotkaDebugWin";
        internal static string SzarotkaDebugAndorid => "szarotkaDebugAndorid";
        public static string GoogleRoutes => "googleRoutes";
        public static string GoogleApis => "googleApis";

    }
}
