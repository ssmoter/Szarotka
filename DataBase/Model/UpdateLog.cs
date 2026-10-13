using DataBase.Data;
using DataBase.Service;

using System.Security.Cryptography;
using System.Text;

namespace DataBase.Model
{

    public partial class UpdateLog : BaseEntities<Guid>
    {
        private UpdateEnum updateEnum;
        public UpdateEnum UpdateEnum
        {
            get => updateEnum;
            set
            {
                if (SetProperty(ref updateEnum, value, nameof(UpdateEnum))) { }
            }
        }
        private string updateId = "";
        /// <summary>
        /// Ustawiać tylko id głównych modeli
        /// </summary>
        public string UpdateId
        {
            get => updateId;
            set
            {
                if (SetProperty(ref updateId, value, nameof(UpdateId))) { }
            }
        }

        private string jsonUpdate = "";
        public string JsonUpdate
        {
            get => jsonUpdate;
            set
            {
                if (SetProperty(ref jsonUpdate, value, nameof(JsonUpdate))) { }
            }
        }

        public bool? IsServer { get; set; } = null;

        public UpdateLog()
        {

        }
        public UpdateLog(UpdateLog copy) : base(copy)
        {
            UpdateEnum = copy.UpdateEnum;
            UpdateId = copy.UpdateId;
            JsonUpdate = copy.JsonUpdate;
            IsServer = copy.IsServer;
        }
        

        public static void FirstDefaultUpdateLogForSync(IAccessDataBaseAoT db)
        {

            var sql = UpdateLogService.InsertSql();
            foreach (var item in Enum.GetValues<UpdateEnum>())
            {
                var update = Created(item);
                db.DbSyncAoT.Execute(sql, param: new()
                {
                    [nameof(UpdateLog.Id)] = update.Id,
                    [nameof(UpdateLog.UpdateEnum)] = update.UpdateEnum,
                    [nameof(UpdateLog.UpdateId)] = update.UpdateId,
                    [nameof(UpdateLog.CreatedTicks)] = update.CreatedTicks,
                    [nameof(UpdateLog.UpdatedTicks)] = update.UpdatedTicks,
                    [nameof(UpdateLog.IsDelete)] = update.IsDelete,
                    [nameof(UpdateLog.UserCreatedId)] = update.UserCreatedId,
                    [nameof(UpdateLog.UserUpdatedId)] = update.UserUpdatedId,
                    [nameof(UpdateLog.JsonUpdate)] = update.JsonUpdate,
                    [nameof(UpdateLog.IsServer)] = update.IsServer
                });
            }

            static UpdateLog Created(UpdateEnum updateEnum)
            {
                // 1. Konwertujemy wartość enuma na tekst (zawsze w UTF-8, niezależnie od platformy)
                string input = updateEnum.ToString();
                byte[] inputBytes = Encoding.UTF8.GetBytes(input);

                // 2. Definiujemy stały "Namespace" dla naszej aplikacji (dowolny stały GUID)
                byte[] namespaceBytes = Guid.Parse("a4f89d32-1234-5678-90ab-cdef12345678").ToByteArray();

                // Łączymy bajty namespace i enuma
                byte[] buffer = new byte[namespaceBytes.Length + inputBytes.Length];
                Buffer.BlockCopy(namespaceBytes, 0, buffer, 0, namespaceBytes.Length);
                Buffer.BlockCopy(inputBytes, 0, buffer, namespaceBytes.Length, inputBytes.Length);

                // 3. Haszujemy za pomocą SHA-1 (Deterministyczny UUID v5)
                byte[] hash = SHA1.HashData(buffer);

                // 4. Przygotowujemy końcowy GUID (bierzemy pierwsze 16 bajtów hasza)
                byte[] guidBytes = new byte[16];
                Array.Copy(hash, 0, guidBytes, 0, 16);

                // Ustawiamy bity wersji (5) i wariantu zgodnie z RFC
                guidBytes[6] = (byte)((guidBytes[6] & 0x0F) | 0x50);
                guidBytes[8] = (byte)((guidBytes[8] & 0x3F) | 0x80);

                Guid deterministicGuid = new(guidBytes);

                return new UpdateLog()
                {
                    Id = deterministicGuid,
                    Created = DateTime.MinValue,
                    Updated = DateTime.MinValue,
                    UpdateEnum = updateEnum,
                    UpdateId = Guid.Empty.ToString(),
                    UserCreatedId = Guid.Empty,
                    UserUpdatedId = Guid.Empty,
                    JsonUpdate = "",
                    IsServer = true,
                };
            }
        }
    }
    public enum UpdateEnum
    {



        CustomerRoutes = 100,
        ResidentialAddress,
        Routes,
        SelectedDayOfWeek,


        Day = 200,
        Cake,
        Product,
        ProductName,
        ProductPrice,

    }
}
