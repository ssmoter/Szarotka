namespace DataBase.Service
{
    public static class AppServiceProvider
    {
        public static TService GetService<TService>()
            => Current.GetService<TService>();
        public static IServiceProvider Current
            =>
#if WINDOWS10_0_17763_0_OR_GREATER
            MauiWinUIApplication.Current.Services;
#elif ANDROID
            IPlatformApplication.Current.Services;
        //MauiApplication.Current.Services;
#elif IOS || MACCATALYST
            IPlatformApplication.Current.Services;
            //MauiUIApplicationDelegate.Current.Services;
#else
            null;
#endif
    }
}
