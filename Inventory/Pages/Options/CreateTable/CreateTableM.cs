using CommunityToolkit.Mvvm.ComponentModel;

namespace Inventory.Pages.Options.CreateTable
{
    public partial class CreateTableM : ObservableObject
    {
        [ObservableProperty]
        string tableName;

        public string RealTableName { get; set; }

        bool isExist;
        public bool IsExist
        {
            get => isExist;
            set
            {
                if (SetProperty(ref isExist, value))
                {
                    OnPropertyChanged(nameof(IsExist));
                    SetColor();
                }
            }
        }
        [ObservableProperty]
        Color color;

        public CreateTableM()
        {
            SetColor();
        }
        public void SetColor()
        {
            if (IsExist)
                Color = Colors.Green;
            else
                Color = Colors.Red;
        }
    }
}
