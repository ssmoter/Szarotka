using CommunityToolkit.Mvvm.ComponentModel;

namespace Inventory.Pages.RangeDay.Graph
{
    public partial class GraphM : ObservableObject
    {
        [ObservableProperty]
        string name;
        [ObservableProperty]
        Color color;
    }
    public partial class TypeOfGraphM : ObservableObject
    {
        int graphType;

        bool column;
        public bool Column
        {
            get { return column; }
            set
            {
                if (SetProperty(ref column, value))
                {
                    OnPropertyChanged(nameof(Column));
                    if (value)
                    {
                        graphType = 0;
                        OnChangeGraphType(graphType);
                    }
                }
            }
        }
        bool line;
        public bool Line
        {
            get { return line; }
            set
            {
                if (SetProperty(ref line, value))
                {
                    OnPropertyChanged(nameof(Line));
                    if (value)
                    {
                        graphType = 1;
                        OnChangeGraphType(graphType);
                    }
                }
            }
        }
        bool point;
        public bool Point
        {
            get { return point; }
            set
            {
                if (SetProperty(ref point, value))
                {
                    OnPropertyChanged(nameof(Point));
                    if (value)
                    {
                        graphType = 2;
                        OnChangeGraphType(graphType);
                    }
                }
            }
        }
        bool linePoint;
        public bool LinePoint
        {
            get { return linePoint; }
            set
            {
                if (SetProperty(ref linePoint, value))
                {
                    OnPropertyChanged(nameof(LinePoint));
                    if (value)
                    {
                        graphType = 3;
                        OnChangeGraphType(graphType);
                    }
                }
            }
        }



        public Action<int> ChangeGraphType { get; set; }
        void OnChangeGraphType(int graphType)
        {
            SetRestToFalse();
            ChangeGraphType?.Invoke(graphType);
        }
        void SetRestToFalse()
        {
            if (graphType != 0)
                Column = false;
            if (graphType != 1)
                Line = false;
            if (graphType != 2)
                Point = false;
            if (graphType != 3)
                LinePoint = false;
        }


    }

}
