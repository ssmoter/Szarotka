using CommunityToolkit.Maui.Views;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

using DataBase.Data;
using DataBase.Model.EntitiesInventory;

using Inventory.Model;
using Inventory.Pages.RangeDay.Graph.GraphOptions;

using Shared.Data;

using System.Collections.ObjectModel;

namespace Inventory.Pages.RangeDay.Graph
{
    public partial class GraphVM : ObservableObject, IDisposable, IQueryAttributable
    {
        public void ApplyQueryAttributes(IDictionary<string, object> query)
        {
            if (query.TryGetValue(nameof(RangeDayM), out object rangeDayMs))
            {
                if (rangeDayMs is RangeDayM[] _rangeDayM)
                {
                    RangeDayMs = _rangeDayM;
                }
            }
        }

        public RangeDayM[] RangeDayMs { get; set; }

        private ObservableCollection<GraphM> legend;
        public ObservableCollection<GraphM> Legend
        {
            get => legend;
            set
            {
                if (SetProperty(ref legend, value, nameof(Legend))) { }
            }
        }

        private Data.Draw.DrawGraph drawGraph;
        public Data.Draw.DrawGraph DrawGraph
        {
            get => drawGraph;
            set
            {
                if (SetProperty(ref drawGraph, value, nameof(DrawGraph))) { }
            }
        }

        private bool isRefreshingGraph;
        public bool IsRefreshingGraph
        {
            get => isRefreshingGraph;
            set
            {
                if (SetProperty(ref isRefreshingGraph, value, nameof(IsRefreshingGraph))) { }
            }
        }

        private bool isVisibleFrame;
        public bool IsVisibleFrame
        {
            get => isVisibleFrame;
            set
            {
                if (SetProperty(ref isVisibleFrame, value, nameof(IsVisibleFrame))) { }
            }
        }
        private TypeOfGraphM typeOfGraphM;
        public TypeOfGraphM TypeOfGraphM
        {
            get => typeOfGraphM;
            set
            {
                if (SetProperty(ref typeOfGraphM, value, nameof(TypeOfGraphM))) { }
            }
        }
        GraphOptionsM _optionsM;

        public Action<IDrawable> ReDraw { get; set; }
        const string _szt = " _szt.";
        const string _zl = " zł";
        readonly Driver[] _allDrivers;
        readonly AccessDataBase _db;
        public GraphVM(AccessDataBase db)
        {
            RangeDayMs ??= [];
            Legend ??= [];
            TypeOfGraphM ??= new();
            TypeOfGraphM.Column = true;
            TypeOfGraphM.ChangeGraphType += OnReDrawTypeOfGraph;

            _db = db;
            DrawGraph ??= new();

            var driver = _db.DataBase.Table<Driver>().ToArray();
            _allDrivers = driver;
            DeviceDisplay.MainDisplayInfoChanged += OnMainDisplayInfoChanged;
        }



        #region Method

        public void OnReDraw(IDrawable drawable)
        {

            ReDraw?.Invoke(drawable);
        }
        void OnReDrawTypeOfGraph(int graphType)
        {
            DrawGraph.TypeOfGraph = graphType;
            try
            {
                OnReDraw(DrawGraph);
            }
            catch { }
        }

        void SetValuesToDraw(GraphOptionsM optionsM)
        {
            if (RangeDayMs is null)
                return;

            if (RangeDayMs.Length == 0)
                return;


            int n = SetNumberOfDraw(optionsM);

            if (n == 0)
            {
                DrawGraph.XValues = null;
                DrawGraph.GraphValues = null;
                OnReDraw(DrawGraph);
                return;
            }

            var drivers = GetInvidualsDrivers(RangeDayMs);

            n *= drivers.Length;

            DrawGraph.GraphValues = new GraphValues[n];
            // Legend = new GraphM[n];
            Legend.Clear();
            var theme = Application.Current.RequestedTheme;
            int startColorFrom = 0;

            if (theme == AppTheme.Dark)
            {
                startColorFrom = -NamedColor.All.Length + 40;
            }
            if (theme == AppTheme.Light)
            {
                startColorFrom = 0;
            }


            for (int j = 0; j < n; j++)
            {
                if (startColorFrom <= 1 && theme == AppTheme.Dark)
                {
                    startColorFrom = -NamedColor.All.Length + 40;
                }
                if (startColorFrom >= 100 && theme == AppTheme.Light)
                {
                    startColorFrom = 0;
                }

                DrawGraph.GraphValues[j] = new GraphValues(Math.Abs(startColorFrom + j));
                Legend.Add(new GraphM());
            }

            int current = 0;

            for (int d = 0; d < drivers.Length; d++)
            {

                if (optionsM.TotalPriceProduct)
                {
                    int x = 0;
                    for (int i = 0; i < RangeDayMs.Length; i++)
                    {
                        if (RangeDayMs[i].Day.DriverGuid == drivers[d].Id)
                        {
                            var point = new PointF(x, -(float)RangeDayMs[i].Day.TotalPriceProductsDecimal);
                            DrawGraph.GraphValues[current].Path.LineTo(point);
                            x++;
                        }
                    }
                    DrawGraph.GraphValues[current].Name = "Utarg Produkty " + drivers[d].Name;
                    Legend[current] = new GraphM() { Name = DrawGraph.GraphValues[current].Name, Color = DrawGraph.GraphValues[current].Color.Color };
                    current++;
                }
                if (optionsM.TotalPriceCake)
                {
                    int x = 0;
                    for (int i = 0; i < RangeDayMs.Length; i++)
                    {
                        if (RangeDayMs[i].Day.DriverGuid == drivers[d].Id)
                        {
                            var point = new PointF(x, -(float)RangeDayMs[i].Day.TotalPriceCakeDecimal);
                            DrawGraph.GraphValues[current].Path.LineTo(point);
                            x++;
                        }
                    }
                    DrawGraph.GraphValues[current].Name = "Utarg Ciasto " + drivers[d].Name;
                    Legend[current] = new GraphM() { Name = DrawGraph.GraphValues[current].Name, Color = DrawGraph.GraphValues[current].Color.Color };

                    current++;
                }
                if (optionsM.TotalPrice)
                {
                    int x = 0;
                    for (int i = 0; i < RangeDayMs.Length; i++)
                    {
                        if (RangeDayMs[i].Day.DriverGuid == drivers[d].Id)
                        {
                            var point = new PointF(x, -(float)RangeDayMs[i].Day.TotalPriceDecimal);
                            DrawGraph.GraphValues[current].Path.LineTo(point);
                            x++;
                        }
                    }
                    DrawGraph.GraphValues[current].Name = "Utarg Suma " + drivers[d].Name;
                    Legend[current] = new GraphM() { Name = DrawGraph.GraphValues[current].Name, Color = DrawGraph.GraphValues[current].Color.Color };

                    current++;
                }
                if (optionsM.TotalPriceCorrect)
                {
                    int x = 0;
                    for (int i = 0; i < RangeDayMs.Length; i++)
                    {
                        if (RangeDayMs[i].Day.DriverGuid == drivers[d].Id)
                        {
                            var point = new PointF(x, -(float)RangeDayMs[i].Day.TotalPriceCorrectDecimal);
                            DrawGraph.GraphValues[current].Path.LineTo(point);
                            x++;
                        }
                    }
                    DrawGraph.GraphValues[current].Name = "Utarg Korekta " + drivers[d].Name;
                    Legend[current] = new GraphM() { Name = DrawGraph.GraphValues[current].Name, Color = DrawGraph.GraphValues[current].Color.Color };

                    current++;
                }
                if (optionsM.TotalPriceAfterCorrect)
                {
                    int x = 0;
                    for (int i = 0; i < RangeDayMs.Length; i++)
                    {
                        if (RangeDayMs[i].Day.DriverGuid == drivers[d].Id)
                        {
                            var point = new PointF(x, -(float)RangeDayMs[i].Day.TotalPriceAfterCorrectDecimal);
                            DrawGraph.GraphValues[current].Path.LineTo(point);
                            x++;
                        }
                    }
                    DrawGraph.GraphValues[current].Name = "Utarg Po Korekcie " + drivers[d].Name;
                    Legend[current] = new GraphM() { Name = DrawGraph.GraphValues[current].Name, Color = DrawGraph.GraphValues[current].Color.Color };

                    current++;
                }
                if (optionsM.TotalPriceMoney)
                {
                    int x = 0;
                    for (int i = 0; i < RangeDayMs.Length; i++)
                    {
                        if (RangeDayMs[i].Day.DriverGuid == drivers[d].Id)
                        {
                            var point = new PointF(x, -(float)RangeDayMs[i].Day.TotalPriceMoneyDecimal);
                            DrawGraph.GraphValues[current].Path.LineTo(point);
                            x++;
                        }
                    }
                    DrawGraph.GraphValues[current].Name = "Zapłacono " + drivers[d].Name;
                    Legend[current] = new GraphM() { Name = DrawGraph.GraphValues[current].Name, Color = DrawGraph.GraphValues[current].Color.Color };

                    current++;
                }
                if (optionsM.TotalPriceDifference)
                {
                    int x = 0;
                    for (int i = 0; i < RangeDayMs.Length; i++)
                    {
                        if (RangeDayMs[i].Day.DriverGuid == drivers[d].Id)
                        {
                            var point = new PointF(x, -(float)RangeDayMs[i].Day.TotalPriceDifferenceDecimal);
                            DrawGraph.GraphValues[current].Path.LineTo(point);
                            x++;
                        }
                    }
                    DrawGraph.GraphValues[current].Name = "Różnica " + drivers[d].Name;
                    Legend[current] = new GraphM() { Name = DrawGraph.GraphValues[current].Name, Color = DrawGraph.GraphValues[current].Color.Color };

                    current++;
                }
                if (optionsM.NumberOfCakes)
                {
                    int x = 0;
                    for (int i = 0; i < RangeDayMs.Length; i++)
                    {
                        if (RangeDayMs[i].Day.DriverGuid == drivers[d].Id)
                        {
                            var point = new PointF(x, -(float)RangeDayMs[i].Day.Cakes.Count(x => x.IsSell));
                            DrawGraph.GraphValues[current].Path.LineTo(point);
                            x++;
                        }
                    }
                    DrawGraph.GraphValues[current].Name = "Ilość Sprzedanych Ciast " + drivers[d].Name;
                    Legend[current] = new GraphM() { Name = DrawGraph.GraphValues[current].Name, Color = DrawGraph.GraphValues[current].Color.Color };

                    current++;
                }

                if (optionsM.ProductMs is not null)
                {
                    for (int j = 0; j < optionsM.ProductMs.Length; j++)
                    {
                        if (optionsM.ProductMs[j].IsNumber)
                        {
                            int x = 0;
                            for (int i = 0; i < RangeDayMs.Length; i++)
                            {
                                if (RangeDayMs[i].Day.DriverGuid == drivers[d].Id)
                                {
                                    for (int k = 0; k < RangeDayMs[i].Day.Products.Count; k++)
                                    {
                                        if (RangeDayMs[i].Day.Products[k].Name.Name == optionsM.ProductMs[j].Name)
                                        {
                                            var point = new PointF(x, -(float)(RangeDayMs[i].Day.Products[k].Number
                                                + RangeDayMs[i].Day.Products[k].NumberEdit
                                                - RangeDayMs[i].Day.Products[k].NumberReturn));
                                            DrawGraph.GraphValues[current].Path.LineTo(point);
                                            x++;
                                            break;
                                        }
                                    }
                                }
                            }
                            DrawGraph.GraphValues[current].Name = $"{optionsM.ProductMs[j].Name} {_szt} {drivers[d].Name}";
                            Legend[current] = new GraphM() { Name = DrawGraph.GraphValues[current].Name, Color = DrawGraph.GraphValues[current].Color.Color };

                            current++;
                        }
                    }

                    for (int j = 0; j < optionsM.ProductMs.Length; j++)
                    {
                        if (optionsM.ProductMs[j].IsPrice)
                        {
                            int x = 0;
                            for (int i = 0; i < RangeDayMs.Length; i++)
                            {
                                if (RangeDayMs[i].Day.DriverGuid == drivers[d].Id)
                                {
                                    for (int k = 0; k < RangeDayMs[i].Day.Products.Count; k++)
                                    {
                                        if (RangeDayMs[i].Day.Products[k].Name.Name == optionsM.ProductMs[j].Name)
                                        {
                                            var point = new PointF(x, -(float)(RangeDayMs[i].Day.Products[k].PriceTotalAfterCorrectDecimal));
                                            DrawGraph.GraphValues[current].Path.LineTo(point);
                                            x++;
                                            break;
                                        }
                                    }
                                }
                            }
                            DrawGraph.GraphValues[current].Name = $"{optionsM.ProductMs[j].Name} {_zl} {drivers[d].Name}";
                            Legend[current] = new GraphM() { Name = DrawGraph.GraphValues[current].Name, Color = DrawGraph.GraphValues[current].Color.Color };

                            current++;
                        }
                    }
                }
            }

            DrawGraph.XValues = new DateTime[DrawGraph.GraphValues[0].Path.Count];

            for (int i = 0; i < DrawGraph.XValues.Length; i++)
            {
                if (RangeDayMs[i].Day.DriverGuid == drivers[0].Id)
                {
                    DrawGraph.XValues[i] = RangeDayMs[i].Day.SelectedDate;
                }
            }

            OnReDraw(DrawGraph);
        }
        static int SetNumberOfDraw(GraphOptionsM optionsM)
        {
            int n = 0;
            if (optionsM.TotalPriceProduct) n++;
            if (optionsM.TotalPriceCake) n++;
            if (optionsM.TotalPrice) n++;
            if (optionsM.TotalPriceCorrect) n++;
            if (optionsM.TotalPriceAfterCorrect) n++;
            if (optionsM.TotalPriceMoney) n++;
            if (optionsM.TotalPriceDifference) n++;
            if (optionsM.NumberOfCakes) n++;

            if (optionsM.ProductMs is not null)
            {
                for (int i = 0; i < optionsM.ProductMs.Length; i++)
                {
                    if (optionsM.ProductMs[i].IsNumber) n++;
                    if (optionsM.ProductMs[i].IsPrice) n++;
                }
            }
            return n;
        }

        static string[] GetProductNames(RangeDayM[] rangeDayMs)
        {
            if (rangeDayMs is null)
                return [];
            if (rangeDayMs.Length == 0)
                return [];


            var first = rangeDayMs.MaxBy(x => x.Day.Products.Count);
            if (first is null)
                return [];

            var names = new string[first.Day.Products.Count];

            for (int i = 0; i < names.Length; i++)
            {
                names[i] = first.Day.Products[i].Name.Name;
            }
            return names;
        }

        static Driver[] GetInvidualsDrivers(RangeDayM[] rangeDayMs)
        {
            var drivers = new List<Driver>();

            for (int i = 0; i < rangeDayMs.Length; i++)
            {
                if (!drivers.Any(x => x.Id == rangeDayMs[i].Driver.Id))
                {
                    drivers.Add(rangeDayMs[i].Driver);
                }
            }

            return [.. drivers];
        }

        void OnMainDisplayInfoChanged(object sender, DisplayInfoChangedEventArgs e)
        {
            try
            {
                OnReDraw(DrawGraph);
            }
            catch { }
        }
        public void Dispose()
        {
            DeviceDisplay.MainDisplayInfoChanged -= OnMainDisplayInfoChanged;
            if (RangeDayMs is not null)
            {
                for (int i = 0; i < RangeDayMs.Length; i++)
                {
                    RangeDayMs[i].Day.Dispose();
                }
            }
            RangeDayMs = null;
            Legend.Clear();
            DrawGraph.Dispose();
        }


        #endregion


        #region Command


        [RelayCommand]
        void GetRandomData()
        {
            try
            {
                int n = 2;
                DrawGraph.GraphValues = new GraphValues[n];

                var theme = Application.Current.RequestedTheme;
                int startColorFrom = 0;

                if (theme == AppTheme.Dark)
                {
                    startColorFrom = -NamedColor.All.Length + 40;
                }
                if (theme == AppTheme.Light)
                {
                    startColorFrom = 0;
                }

                int test = 15;
                for (int j = 0; j < n; j++)
                {
                    if (startColorFrom <= 1 && theme == AppTheme.Dark)
                    {
                        startColorFrom = -NamedColor.All.Length + 40;
                    }
                    if (startColorFrom >= 100 && theme == AppTheme.Light)
                    {
                        startColorFrom = 0;
                    }


                    DrawGraph.GraphValues[j] = new GraphValues(Math.Abs(startColorFrom + j))
                    {
                        Name = Path.GetRandomFileName()
                    };

                    for (int i = 0; i < test; i++)
                    {
                        var y = Random.Shared.Next(500, 5000);
                        var point = new PointF(i, -y);
                        DrawGraph.GraphValues[j].Path.LineTo(point);
                    }
                }
                Legend.Clear();
                DrawGraph.XValues = new DateTime[test];

                var date = new DateTime(DateTime.Today.Year, DateTime.Today.Month, 1);
                for (int i = 0; i < test; i++)
                {
                    DrawGraph.XValues[i] = new DateTime(date.Year, date.Month, 1).AddDays(i);
                }

                OnReDraw(DrawGraph);
            }

            catch (Exception ex)
            {
                _db.SaveLogExtension(ex);
            }
        }

        [RelayCommand]
        void RefreshGraph()
        {
            try
            {
                OnReDraw(DrawGraph);
            }
            catch { }
            finally { IsRefreshingGraph = false; }
        }

        [RelayCommand]
        async Task OpenOptonsGraph()
        {
            try
            {
                var names = GraphVM.GetProductNames(RangeDayMs);

                var popup = new GraphOptions.GraphOptionsV(names);

                var result = await Shell.Current.ShowPopupAsync(popup);

                if (result is GraphOptionsM optionsM)
                {
                    _optionsM = optionsM;
                    SetValuesToDraw(optionsM);
                }
            }
            catch (Exception ex)
            {
                _db.SaveLogExtension(ex);
            }
        }

        [RelayCommand]
        async Task Back()
        {
            try
            {
                await Shell.Current.GoToAsync("..");
            }
            catch (Exception ex)
            {
                _db.SaveLogExtension(ex);
            }
            finally
            {
                Dispose();
            }
        }

        [RelayCommand]
        void SwipeViewGesture()
        {
            IsVisibleFrame = !IsVisibleFrame;
        }

        #endregion


    }
}
