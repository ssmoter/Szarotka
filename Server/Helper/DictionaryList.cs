using DataBase.Model.EntitiesServer;

using System.Collections.Concurrent;

namespace Server.Helper
{
    public static class DictionaryList
    {
        public static ConcurrentDictionary<int, (RegisterUser user, ConfirmCode code)> RegisterUser { get; private set; } = new();
        public static ConcurrentDictionary<int,  ConfirmCode > ResetPasswordCodes { get; private set; } = new();
    }
}
