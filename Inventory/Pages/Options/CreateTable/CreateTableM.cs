using CommunityToolkit.Mvvm.ComponentModel;

namespace Inventory.Pages.Options.CreateTable
{
    public partial class CreateTableM : ObservableObject
    {
        private string tableName;
        public string TableName
        {
            get => tableName;
            set
            {
                if (SetProperty(ref tableName, value, nameof(TableName))) { }
            }
        }

        public string RealTableName { get; set; }

        bool isExist;
        public bool IsExist
        {
            get => isExist;
            set
            {
                if (SetProperty(ref isExist, value, nameof(IsExist)))
                {
                    //OnPropertyChanged(nameof(IsExist));
                    SetColor();
                }
            }
        }
        private Color color;
        public Color Color
        {
            get => color;
            set
            {
                if (SetProperty(ref color, value, nameof(Color))) { }
            }
        }

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
