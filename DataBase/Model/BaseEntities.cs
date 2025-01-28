using CommunityToolkit.Mvvm.ComponentModel;

using SQLite;

using System.Text.Json.Serialization;

namespace DataBase.Model
{
    public partial class BaseEntities<T> : ObservableObject
    {
        private T? id;
        [PrimaryKey]
        public T? Id
        {
            get => id;
            set
            {
                if (SetProperty(ref id, value))
                {
                    OnPropertyChanged(nameof(Id));
                }
            }
        }
        [Ignore]
        [JsonConverter(typeof(JsonContext.CustomDateTimeConverter))]
        public DateTime Created
        {
            get
            {
                if (_createdTicks == 0)
                {
                    return new DateTime();
                }
                var date = new DateTime(_createdTicks);
                return date.ToLocalTime();
            }
            set
            {
                if (SetProperty(ref _createdTicks, value.ToUniversalTime().Ticks))
                {
                    OnPropertyChanged(nameof(Created));
                }
            }
        }
        [Ignore]
        [JsonConverter(typeof(JsonContext.CustomDateTimeConverter))]
        public DateTime Updated
        {
            get
            {
                if (_updatedTicks == 0)
                {
                    return new DateTime();
                }
                var date = new DateTime(_updatedTicks);
                return date.ToLocalTime();
            }
            set
            {
                if (SetProperty(ref _updatedTicks, value.ToUniversalTime().Ticks))
                {
                    OnPropertyChanged(nameof(Updated));
                }
            }
        }
        public long CreatedTicks
        {
            get => _createdTicks;
            set => _createdTicks = value;
        }
        public long UpdatedTicks
        {
            get => _updatedTicks;
            set => _updatedTicks = value;
        }
        private long _createdTicks;
        private long _updatedTicks;

        private bool isDelete;
        public bool IsDelete
        {
            get => isDelete;
            set
            {
                if (SetProperty(ref isDelete, value, nameof(IsDelete))) { }
            }
        }

        private Guid userCreatedId;
        public Guid UserCreatedId
        {
            get => userCreatedId;
            set
            {
                if (SetProperty(ref userCreatedId, value, nameof(UserCreatedId))) { }
            }
        }

        private Guid userUpdatedId;
        public Guid UserUpdatedId
        {
            get => userUpdatedId;
            set
            {
                if (SetProperty(ref userUpdatedId, value, nameof(UserUpdatedId))) { }
            }
        }
    }
}
