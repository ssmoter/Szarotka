namespace DataBase.Model
{

    public class UpdateLog : BaseEntities<Guid>
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

    }
    public enum UpdateEnum
    {



        CustomerRoutes = 100,
        ResidentialAddress,
        Routes,
        SelectedDayOfWeek,



    }
}
