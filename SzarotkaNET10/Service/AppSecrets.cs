using System.Reflection;

namespace Szarotka.Service
{
    public static class AppSecrets
    {
        private static readonly Lazy<string> _googleMapsApiKey =
            new(() => GetMetadata("GoogleMapsApiKey"));

        public static string GoogleMapsApiKey => _googleMapsApiKey.Value;

        private static string GetMetadata(string key)
        {
            var assembly = typeof(AppSecrets).Assembly;
            var attributes = assembly.GetCustomAttributes<AssemblyMetadataAttribute>();

            foreach (var attribute in attributes)
            {
                if (attribute.Key == key)
                {
                    return attribute.Value ?? string.Empty;
                }
            }

            return string.Empty;
        }
    }
}
