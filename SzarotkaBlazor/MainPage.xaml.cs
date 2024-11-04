
using DataBase.Data;
using DataBase.Model;

using DriversRoutes.Pages.Maps.Controls;

using SzarotkaBlazor.Pages.Options.Main;


namespace SzarotkaBlazor
{
    public partial class MainPage : ContentPage
    {
        private readonly CreatedDataBase _createdDataBase;
        private readonly AccessDataBase _db;
        public MainPage()
        {
            InitializeComponent();
            _db = new();
            _createdDataBase = new CreatedDataBase(_db);
        }

        //int n = 0;
        //private void Button_Clicked(object sender, EventArgs e)
        //{
        //    var customer = _db.DataBase.Table<DataBase.Model.EntitiesRoutes.CustomerRoutes>().Skip(n).FirstOrDefault();
        //    n++;
        //    BlazorMap.OnRemoveAdvancedMarker();
        //    BlazorMap.OnSetCustomer(customer);
        //    BlazorMap.OnSetAdvancedMarker();
        //}

        protected override async void OnNavigatedTo(NavigatedToEventArgs args)
        {
            base.OnNavigatedTo(args);
            try
            {
                var old = _createdDataBase.GetCurrentVersion();
                if (!old.Equals(new DataBaseVersion()))
                {
                    await Shell.Current.GoToAsync(nameof(DataBase.Pages.UpdateDataBase.UpdateDataBaseV));
                }
                await _createdDataBase.CreateBackUp();
            }
            catch (Exception ex)
            {
                _db.SaveLog(ex);
            }
        }

        private async void Options_Clicked(object sender, EventArgs e)
        {
            await Shell.Current.GoToAsync(nameof(MainOptionsV));
        }

        private async void Inventory_Clicked(object sender, EventArgs e)
        {
            await Shell.Current.GoToAsync(nameof(Inventory.Pages.Main.MainV));
        }

        private async void Maps_Clicked(object sender, EventArgs e)
        {
            await Shell.Current.GoToAsync(nameof(DriversRoutes.Pages.Main.MainVDriversRoutesV));
        }




        //private void Button_Clicked(object sender, EventArgs e)
        //{
        //    var width = 40 *10;
        //    var height = 58 *10;

        //    SkiaBitmapExportContext skiaBitmapExportContext = new(width, height, 1);
        //    ICanvas canvas = skiaBitmapExportContext.Canvas;
        //    DrawIconOnMap drawIconOnMap = new()
        //    {
        //        Number = 100
        //    };
        //    drawIconOnMap.Draw(canvas, new RectF(0, 0, skiaBitmapExportContext.Width, skiaBitmapExportContext.Height));
        //    gv.MinimumWidthRequest = width;
        //    gv.MinimumHeightRequest = height;
        //    gv.Drawable = drawIconOnMap;
        //    gv.Invalidate();
        //    //= ImageSource.FromStream(() => skiaBitmapExportContext.Image.AsStream());


        //}
    }
}
