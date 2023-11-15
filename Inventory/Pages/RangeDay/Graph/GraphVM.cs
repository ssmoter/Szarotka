using CommunityToolkit.Maui.Views;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

using Inventory.Helper.Parse;
using Inventory.Model;
using Inventory.Model.MVVM;
using Inventory.Pages.RangeDay.Graph.GraphOptions;

using System.Collections.ObjectModel;
using System.Text;

namespace Inventory.Pages.RangeDay.Graph
{
    [QueryProperty(nameof(RangeDayMs), nameof(RangeDayM))]
    public partial class GraphVM : ObservableObject, IDisposable
    {
        public RangeDayM[] RangeDayMs { get; set; }

        [ObservableProperty]
        ObservableCollection<GraphM> legend;

        [ObservableProperty]
        Data.Draw.DrawGraph drawGraph;

        [ObservableProperty]
        bool isRefreshingGraph;

        [ObservableProperty]
        bool isVisibleFrame;

        [ObservableProperty]
        TypeOfGraphM typeOfGraphM;

        GraphOptionsM _optionsM;

        public Action<IDrawable> ReDraw { get; set; }
        const string _szt = " _szt.";
        const string _zl = " zł";
        readonly Driver[] _allDrivers;
        readonly DataBase.Data.AccessDataBase _db;
        readonly Service.ISelectDayService _selectDayService;
        public GraphVM(DataBase.Data.AccessDataBase db, Service.ISelectDayService selectDayService)
        {
            RangeDayMs ??= Array.Empty<RangeDayM>();
            Legend ??= new ObservableCollection<GraphM>();
            TypeOfGraphM ??= new TypeOfGraphM();
            TypeOfGraphM.Column = true;
            TypeOfGraphM.ChangeGraphType += OnReDrawTypeOfGraph;

            _db = db;
            DrawGraph ??= new();

            var driver = _db.DataBase.Table<Driver>().ToArray();
            _allDrivers = driver;
            _selectDayService = selectDayService;

            DeviceDisplay.MainDisplayInfoChanged += OnMainDisplayInfoChanged;
        }



        #region Method

        async Task<RangeDayM[]> SelectDays(long from, long to, Guid[] selectedDriverName, bool moreData)
        {
            Day[] days = Array.Empty<Day>();
            if (selectedDriverName is null || selectedDriverName.Length == 0)
            {
                days = await _db.DataBaseAsync.Table<Inventory.Model.Day>().
                    Where(x => x.CreatedTicks >= from && x.CreatedTicks <= to).
                    OrderByDescending(x => x.CreatedTicks).ToArrayAsync();
            }
            else
            {
                var sb = new StringBuilder();

                sb.Append("SELECT * FROM Day ");
                sb.Append("WHERE CreatedTicks >= ");
                sb.Append(from);
                sb.Append(" AND CreatedTicks <= ");
                sb.Append(to);

                if (selectedDriverName.Length > 0)
                {
                    sb.Append(" AND (");
                }

                for (int i = 0; i < selectedDriverName.Length; i++)
                {
                    if (i == 0)
                        sb.Append(" DriverGuid == '");
                    else if (i < selectedDriverName.Length)
                        sb.Append(" OR DriverGuid == '");

                    sb.Append(selectedDriverName[i]);
                    sb.Append('\'');
                }

                if (selectedDriverName.Length > 0)
                {
                    sb.Append(" ) ");
                }

                sb.Append(" ORDER BY CreatedTicks DESC");

                var result = await _db.DataBaseAsync.QueryAsync<Day>(sb.ToString());
                days = result.ToArray();
                result.Clear();

                //days = await _db.DataBaseAsync.Table<Inventory.Model.Day>().
                //    Where(x => x.CreatedTicks >= from && x.CreatedTicks <= to && x.DriverGuid == selectedDriverName).
                //    OrderByDescending(x => x.CreatedTicks).ToArrayAsync();

            }

            RangeDayM[] dayM = new RangeDayM[days.Length];
            for (int i = 0; i < days.Length; i++)
            {
                dayM[i] = new RangeDayM();
                dayM[i].DayM = days[i].ParseAsDayM();
                var guid = days[i].DriverGuid;
                var driver = await _db.DataBaseAsync.Table<Driver>().FirstOrDefaultAsync(x => x.Id == guid);
                if (driver is not null)
                {
                    dayM[i].Driver = driver.PareseAsDriverM();
                }
            }

            if (moreData)
            {
                for (int i = 0; i < dayM.Length; i++)
                {
                    dayM[i].DayM = await _selectDayService.GetDayProcedure(dayM[i].DayM.Id);
                }
            }

            return dayM.Reverse().ToArray();
        }

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
                        if (RangeDayMs[i].DayM.DriverGuid == drivers[d].Id)
                        {
                            var point = new PointF(x, -(float)RangeDayMs[i].DayM.TotalPriceProduct);
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
                        if (RangeDayMs[i].DayM.DriverGuid == drivers[d].Id)
                        {
                            var point = new PointF(x, -(float)RangeDayMs[i].DayM.TotalPriceCake);
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
                        if (RangeDayMs[i].DayM.DriverGuid == drivers[d].Id)
                        {
                            var point = new PointF(x, -(float)RangeDayMs[i].DayM.TotalPrice);
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
                        if (RangeDayMs[i].DayM.DriverGuid == drivers[d].Id)
                        {
                            var point = new PointF(x, -(float)RangeDayMs[i].DayM.TotalPriceCorrect);
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
                        if (RangeDayMs[i].DayM.DriverGuid == drivers[d].Id)
                        {
                            var point = new PointF(x, -(float)RangeDayMs[i].DayM.TotalPriceAfterCorrect);
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
                        if (RangeDayMs[i].DayM.DriverGuid == drivers[d].Id)
                        {
                            var point = new PointF(x, -(float)RangeDayMs[i].DayM.TotalPriceMoney);
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
                        if (RangeDayMs[i].DayM.DriverGuid == drivers[d].Id)
                        {
                            var point = new PointF(x, -(float)RangeDayMs[i].DayM.TotalPriceDifference);
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
                        if (RangeDayMs[i].DayM.DriverGuid == drivers[d].Id)
                        {
                            var point = new PointF(x, -(float)RangeDayMs[i].DayM.Cakes.Count(x => x.IsSell));
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
                                if (RangeDayMs[i].DayM.DriverGuid == drivers[d].Id)
                                {
                                    for (int k = 0; k < RangeDayMs[i].DayM.Products.Count; k++)
                                    {
                                        if (RangeDayMs[i].DayM.Products[k].Name.Name == optionsM.ProductMs[j].Name)
                                        {
                                            var point = new PointF(x, -(float)(RangeDayMs[i].DayM.Products[k].Number
                                                + RangeDayMs[i].DayM.Products[k].NumberEdit
                                                - RangeDayMs[i].DayM.Products[k].NumberReturn));
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
                                if (RangeDayMs[i].DayM.DriverGuid == drivers[d].Id)
                                {
                                    for (int k = 0; k < RangeDayMs[i].DayM.Products.Count; k++)
                                    {
                                        if (RangeDayMs[i].DayM.Products[k].Name.Name == optionsM.ProductMs[j].Name)
                                        {
                                            var point = new PointF(x, -(float)(RangeDayMs[i].DayM.Products[k].PriceTotalAfterCorrect));
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
                if (RangeDayMs[i].DayM.DriverGuid == drivers[0].Id)
                {
                    DrawGraph.XValues[i] = RangeDayMs[i].DayM.Created;
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
                return Array.Empty<string>();
            if (rangeDayMs.Length == 0)
                return Array.Empty<string>();


            var first = rangeDayMs.FirstOrDefault();
            if (first is null)
                return Array.Empty<string>();

            var names = new string[first.DayM.Products.Count];

            for (int i = 0; i < names.Length; i++)
            {
                names[i] = first.DayM.Products[i].Name.Name;
            }
            return names;
        }

        static DriverM[] GetInvidualsDrivers(RangeDayM[] rangeDayMs)
        {
            var drivers = new List<DriverM>();

            for (int i = 0; i < rangeDayMs.Length; i++)
            {
                if (!drivers.Any(x => x.Id == rangeDayMs[i].Driver.Id))
                {
                    drivers.Add(rangeDayMs[i].Driver);
                }
            }

            return drivers.ToArray();
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
                    RangeDayMs[i].DayM.Dispose();
                }
            }
            RangeDayMs = null;
            Legend.Clear();
            DrawGraph.Dispose();
        }


        #endregion


        #region Command

        [RelayCommand]
        async Task SelectMoreDate()
        {
            try
            {
                var popup = new PopupSelectRangeDate.PopupSelectRangeDateV(_allDrivers);
                var result = await Shell.Current.ShowPopupAsync(popup);

                if (result is PopupDateModel model)
                {
                    RangeDayMs = await SelectDays(model.From, model.To, model.DriverId, model.MoreData);

                    try
                    {
                        DrawGraph.Dispose();
                        Legend.Clear();
                        SetValuesToDraw(_optionsM);
                        OnReDraw(DrawGraph);
                    }
                    catch { }
                    finally { IsRefreshingGraph = false; }
                }
            }
            catch (Exception ex)
            {
                _db.SaveLog(ex);
            }
        }

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
                _db.SaveLog(ex);
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
                _db.SaveLog(ex);
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
                _db.SaveLog(ex);
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
