using CommunityToolkit.Mvvm.ComponentModel;

using SQLite;

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
        public DateTime Created
        {
            get => new(_createdTicks, DateTimeKind.Local);
            set
            {
                if (SetProperty(ref _createdTicks, value.ToUniversalTime().Ticks))
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

        public BaseEntities()
        {
            Created = DateTime.MaxValue;
            Updated = DateTime.MaxValue;
        }
    }
}
