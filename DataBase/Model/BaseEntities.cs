using CommunityToolkit.Mvvm.ComponentModel;

using SQLite;

using System.Text.Json.Serialization;

namespace DataBase.Model
{
    public partial class BaseEntities<T> : ObservableObject
    {
        [PrimaryKey]
        public T? Id { get; set; }
        [Ignore]
        public DateTime Created
        {
            get => new(_createdTicks, DateTimeKind.Local);
            set
            {
                if (SetProperty(ref _createdTicks, value.ToLocalTime().Ticks))
                {
                    OnPropertyChanged(nameof(Created));
                }
            }
        }
        [Ignore]
        public DateTime Updated
        {
            get => new(_updatedTicks, DateTimeKind.Local);
            set
            {
                if (SetProperty(ref _updatedTicks, value.ToLocalTime().Ticks))
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
    }
}
