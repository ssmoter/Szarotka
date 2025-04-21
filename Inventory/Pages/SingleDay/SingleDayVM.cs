using CommunityToolkit.Maui.Alerts;
using CommunityToolkit.Maui.Core;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

using DataBase.Data;
using DataBase.Data.Save;
using DataBase.Model.EntitiesInventory;

using Inventory.Service;

using Shared.Data;

namespace Inventory.Pages.SingleDay
{
    public partial class SingleDayVM : ObservableObject, IDisposable, IQueryAttributable
    {
        public void ApplyQueryAttributes(IDictionary<string, object> query)
        {
            if (query.TryGetValue(nameof(Day), out object day))
            {
                if (day is Day _day)
                {
                    if (Day is not null)
                    {
                        RemovePropertyChangedEvent();
                    }
                    Day = _day;
                    AddPropertyChangedEvent();
                }
            }
        }

        private Day day;
        public Day Day
        {
            get => day;
            set
            {
                if (SetProperty(ref day, value, nameof(Day)))
                {
                    //OnPropertyChanged(nameof(Day));
                }
            }
        }

        SingleDayM singleDayM;
        public SingleDayM SingleDayM
        {
            get => singleDayM;
            set
            {
                if (SetProperty(ref singleDayM, value, nameof(SingleDayM)))
                {
                    //OnPropertyChanged(nameof(SingleDayM));
                }
            }
        }
        static PeriodicTimer lastFastValuePeriodicTimer = new(TimeSpan.FromSeconds(1));
        static (string name, int value, char sign, string message) lastFastValue = new("", 0, ' ', "");
        static int lastFastValueClearTimerValue = 0;

        const char signPlus = '+';
        //const char signMinus = '-';
        private readonly IAccessDataBase _db;
        private readonly ISaveDayService _saveDay;
        private readonly ISelectDayService _selectDay;
        private readonly ISaveInventoryAoT _saveInventoryAoT;

        public SingleDayVM(IAccessDataBase db,
            ISaveDayService saveDay,
            ISelectDayService selectDay,
            ISaveInventoryAoT saveInventoryAoT)
        {
            _db = db;
            _saveDay = saveDay;
            Day ??= new();
            SingleDayM ??= new();
            _selectDay = selectDay;
            ResetLastFastValue();
            _saveInventoryAoT = saveInventoryAoT;
        }


        public void Dispose()
        {
            lastFastValuePeriodicTimer.Dispose();
            GC.SuppressFinalize(this);
        }


        #region Method

        private async void AddPropertyChangedEvent()
        {
            Day.PropertyChanged += SingleDayVM_PropertyChanged;
            for (int i = 0; i < Day.Products.Count; i++)
            {
                Day.Products[i].PropertyChanged += SingleDayVM_PropertyChanged;
            }
            for (int i = 0; i < Day.Cakes.Count; i++)
            {
                Day.Cakes[i].PropertyChanged += SingleDayVM_PropertyChanged;
            }
            if (Day.Id == Guid.Empty)
            {
                await SaveDay();
            }
        }
        public void RemovePropertyChangedEvent()
        {
            Day.PropertyChanged -= SingleDayVM_PropertyChanged;
            for (int i = 0; i < Day.Products.Count; i++)
            {
                Day.Products[i].PropertyChanged -= SingleDayVM_PropertyChanged;
            }
            for (int i = 0; i < Day.Cakes.Count; i++)
            {
                Day.Cakes[i].PropertyChanged -= SingleDayVM_PropertyChanged;
            }
        }
        private bool isPropertyChanged;
        private async void SingleDayVM_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            try
            {
                var userId = Shared.Helper.UserAfterLogin.User.Id.ToByteArray();

                if (e.PropertyName == nameof(Product.IsExpanded))
                {
                    return;
                }
                if (sender is Day day)
                {
                    if (isPropertyChanged)
                    {
                        return;
                    }

                    isPropertyChanged = true;
                    Day.UpdateTotalPrice();
                    await _saveInventoryAoT.SaveDay(day, userId);
                    isPropertyChanged = false;
                }
                if (sender is Product product)
                {
                    if (isPropertyChanged)
                    {
                        return;
                    }
                    isPropertyChanged = true;
                    product.CalculatePrice();
                    Day.UpdateTotalPrice();
                    if (product.DayId == Guid.Empty)
                    {
                        product.DayId = Day.Id;
                    }
                    await _saveInventoryAoT.SaveProduct(product, userId);
                    await _saveInventoryAoT.SaveDay(Day, userId);
                    isPropertyChanged = false;
                }
                if (sender is Cake cake)
                {
                    if (e.PropertyName == nameof(Cake.IsSell))
                    {
                        Day.UpdateTotalPrice();
                        if (cake.DayId == Guid.Empty)
                        {
                            cake.DayId = Day.Id;
                        }
                        await _saveInventoryAoT.SaveCake(cake, userId);
                        await _saveInventoryAoT.SaveDay(Day, userId);
                    }
                }
            }
            catch (Exception ex)
            {
                isPropertyChanged = false;
                _db.SaveLogExtension(ex);
            }
        }
        public async Task ShowCurrentDay()
        {
            await CommunityToolkit.Maui.Alerts.Toast.Make($"Wczytano dzień {Day.SelectedDate.ToShortDateString()}", CommunityToolkit.Maui.Core.ToastDuration.Short).Show();
        }
        static async void ResetLastFastValue()
        {
            while (await lastFastValuePeriodicTimer.WaitForNextTickAsync())
            {
                lastFastValueClearTimerValue--;
                if (lastFastValueClearTimerValue == 0)
                {
                    lastFastValue.value = 0;
                }
            }
        }

        public static void FastChangeProductNumber(Product product, int value, IView snackBar = null)
        {
            if (product is not null)
            {
                product.Number += value;
                ToastMakeFastChange(product, value, "ilość", snackBar);
            }
        }
        public static void FastChangeProductEdit(Product product, int value, IView snackBar = null)
        {
            if (product is not null)
            {

                product.NumberEdit += value;

                ToastMakeFastChange(product, value, "edycja", snackBar);
            }
        }
        public static void FastChangeProductReturn(Product product, int value, IView snackBar = null)
        {
            if (product is not null)
            {
                product.NumberReturn += value;

                ToastMakeFastChange(product, value, "zwrot", snackBar);
            }
        }
        static SnackbarOptions snackBarOptions = new SnackbarOptions()
        {
            CornerRadius = new CornerRadius(50),
            ActionButtonTextColor = Colors.Transparent,
        };
        private static void ToastMakeFastChange(Product product, int value, string message, IView snackBar = null)
        {
            if (Math.Sign(lastFastValue.value) != Math.Sign(value))
            {
                lastFastValue.value = 0;
            }
            if (lastFastValue.name == product.Name.Name)
            {
                lastFastValue.value += value;
            }
            else
            {
                lastFastValue.value = value;
                lastFastValue.name = product.Name.Name;
            }
            if (lastFastValue.message != message)
            {
                lastFastValue.value = value;
            }


            lastFastValueClearTimerValue = 10;
            char sign = value > 0 ? signPlus : ' ';
            lastFastValue.message = message;
            var toastMessage = $"{product.Name.Name} {message} {sign}{lastFastValue.value}";
#if !WINDOWS
            //view.DisplaySnackbar(toastMessage, null, "", TimeSpan.FromSeconds(3), visualOptions: snackBarOptions);
            Toast.Make(toastMessage, duration: CommunityToolkit.Maui.Core.ToastDuration.Short).Show();
#endif
        }

        #endregion

        #region Command



        [RelayCommand]
        void ChangeProductVisibility()
        {
            SingleDayM.ProductIsVisible = true;
            SingleDayM.CakeIsVisible = false;
        }
        [RelayCommand]
        void ChangeCakeVisibility()
        {
            SingleDayM.CakeIsVisible = true;
            SingleDayM.ProductIsVisible = false;
        }



        [RelayCommand]
        static void FastAddProductNumber(Product product)
        {
            SingleDayVM.FastChangeProductNumber(product, 1);
        }
        [RelayCommand]
        static void FastMinusProductNumber(Product product)
        {
            SingleDayVM.FastChangeProductNumber(product, -1);
        }
        [RelayCommand]
        static void FastAddProductEdit(Product product)
        {
            SingleDayVM.FastChangeProductEdit(product, 1);
        }
        [RelayCommand]
        static void FastMinusProductEdit(Product product)
        {
            SingleDayVM.FastChangeProductEdit(product, -1);
        }
        [RelayCommand]
        static void FastAddProductReturn(Product product)
        {
            SingleDayVM.FastChangeProductReturn(product, 1);
        }
        [RelayCommand]
        static void FastMinusProductReturn(Product product)
        {
            SingleDayVM.FastChangeProductReturn(product, -1);
        }


        [RelayCommand]
        async Task SaveDay()
        {
            //await _saveDay.SaveDayAsync(Day);
            var userId = Shared.Helper.UserAfterLogin.User.Id.ToByteArray();
            await _saveInventoryAoT.SaveDay(Day, userId);
            var dayId = Day.Id.ToByteArray();
            var taskProducts = new Task[Day.Products.Count];
            for (int i = 0; i < Day.Products.Count; i++)
            {
                Day.Products[i].CalculatePrice();
                Day.Products[i].DayId = new Guid(dayId);
                taskProducts[i] = _saveInventoryAoT.SaveProduct(Day.Products[i], userId);
            }
            await Task.WhenAll(taskProducts);

            var taskCakes = new Task[Day.Cakes.Count];
            for (int i = 0; i < Day.Cakes.Count; i++)
            {
                Day.Cakes[i].DayId = new Guid(dayId);
                taskCakes[i] = _saveInventoryAoT.SaveCake(day.Cakes[i], userId);
            }
            await Task.WhenAll(taskCakes);
            Day.UpdateTotalPrice();
            await _saveInventoryAoT.SaveDay(Day, userId);
        }

        [RelayCommand]
        async Task AddCake()
        {
            try
            {
                if (Day is null)
                {
                    return;
                }
                if (Day.Cakes is null)
                {
                    Day.Cakes = [];
                }

                var response = await Shell.Current.DisplayPromptAsync("Ciasto", "Podaj cenę ciasta", "Tak", "Anuluj", keyboard: Keyboard.Numeric);

                if (string.IsNullOrWhiteSpace(response))
                {
                    return;
                }
                response = response.Replace('.', ',');
                if (decimal.TryParse(response, DataBase.Helper.Constants.CultureInfo, out decimal value))
                {
                    var cake = new Cake
                    {
                        PriceDecimal = value,
                        DayId = Day.Id,
                        Index = Day.Cakes.Count + 1,
                        Created = DateTime.Now,
                        Updated = DateTime.Now,
                        IsSell = true
                    };
                    cake.PropertyChanged += SingleDayVM_PropertyChanged;
                    Day.Cakes.Add(cake);
                    Day.Cakes.LastOrDefault().IsSell = true;
                    Day.UpdateTotalPrice();
                    await CommunityToolkit.Maui.Alerts.Toast.Make($"Dodano ciasto z ceną {value}", CommunityToolkit.Maui.Core.ToastDuration.Short).Show();
                    await _saveInventoryAoT.SaveCake(cake, Day.Id.ToByteArray());
                }
            }
            catch (Exception ex)
            {
                _db.SaveLogExtension(ex);
            }
        }

        [RelayCommand]
        void DeleteCake(Cake cake)
        {
            try
            {
                if (Day is null)
                {
                    return;
                }
                if (Day.Cakes is null)
                {
                    return;
                }
                Day.Cakes.Remove(cake);
                cake.IsDelete = true;
                Day.UpdateTotalPrice();
                _saveInventoryAoT.SaveCake(cake, Day.Id.ToByteArray());
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
                string yes = "Tak";
                string no = "Nie";
                string cancel = "Anuluj";

                var result = await Shell.Current.DisplayActionSheet("Czy zapisać przy cofaniu", cancel, null, yes, no);

                if (result == yes)
                {
                    await SaveDay();
                    await BackWithoutSave();
                }
                else if (result == cancel)
                {
                    return;
                }
                else if (result == no)
                {
                    await BackWithoutSave();
                }
                else
                {
                    return;
                }
            }
            catch (Exception ex)
            {
                _db.SaveLogExtension(ex);
            }
        }
        [RelayCommand]
        async Task BackWithoutSave()
        {
            try
            {
                await Shell.Current.GoToAsync("..?",
                        new Dictionary<string, object>()
                        {
                            [nameof(Day)] = Day
                        });
            }
            catch (Exception ex)
            {
                _db.SaveLogExtension(ex);
            }
        }

        [RelayCommand]
        async Task AddPriceMoneyFromClipboard()
        {
            if (Clipboard.Default.HasText)
            {
                var paste = await Clipboard.Default.GetTextAsync();

                if (decimal.TryParse(paste, out decimal result))
                {
                    Day.TotalPriceMoneyDecimal = result;
                    await _saveInventoryAoT.SaveDay(Day, Shared.Helper.UserAfterLogin.User.Id.ToByteArray());
                }
            }
        }

        [RelayCommand]
        void RefreshListOfProduct()
        {
            try
            {
                bool needToSort = false;
                for (int i = 0; i < Day.Products.Count; i++)
                {
                    Day.Products[i].CalculatePrice();
                    if (Day.Products[i].Name.Arrangement <= i)
                    {
                        needToSort = true;
                    }
                }
                Day.UpdateTotalPrice();

                if (needToSort)
                {
                    var products = Day.Products.OrderBy(x => x.Name.Arrangement).ToArray();
                    Day.Products.Clear();
                    foreach (var item in products)
                    {
                        Day.Products.Add(item);
                    }
                }
                RemovePropertyChangedEvent();
                AddPropertyChangedEvent();
            }
            catch (Exception ex) { _db.SaveLogExtension(ex); }
            finally { SingleDayM.ProductIsRefreshing = false; }
        }


        [RelayCommand]
        async Task ChangeProductPrice(Product product)
        {
            try
            {
                var oldPrice = await _db.DataBaseAsync.Table<ProductPrice>().Where(x => x.ProductNameId == product.ProductNameId).ToArrayAsync();
                var priceArray = oldPrice.Select(x => x.PriceDecimal).Select(x => x.ToString()).ToList();
                priceArray.Add("Nowa");

                var result = await Shell.Current.DisplayActionSheet("Zmiana ceny",
                                                                    "Anuluj",
                                                                    null, [.. priceArray]);
                if (result == "Anuluj")
                {
                    return;
                }
                if (result == "Nowa")
                {
                    var listProduct = new Pages.Products.ListProduct.ListProductM()
                    {
                        Name = product.Name,
                        Prices = new System.Collections.ObjectModel.ObservableCollection<ProductPrice>(
                            await _db.DataBaseAsync.Table<ProductPrice>().Where(x => x.ProductNameId == product.ProductNameId).ToArrayAsync())
                    };
                    listProduct.SetActualPrice();

                    await Shell.Current.GoToAsync($"{nameof(Pages.Products.ListProduct.AddEdit.AddEditProductV)}?",
                        new Dictionary<string, object>()
                        {
                            [nameof(Pages.Products.ListProduct.ListProductM)] = listProduct
                        });
                    return;
                }

                if (decimal.TryParse(result, out decimal selectedPrice))
                {
                    var price = oldPrice.FirstOrDefault(x => x.PriceDecimal == selectedPrice);
                    if (price is not null)
                    {
                        product.Price = price;
                    }
                }
                RefreshListOfProduct();
            }
            catch (Exception ex)
            {
                _db.SaveLogExtension(ex);
            }
        }

        [RelayCommand]
        async Task DeleteSelectedProduct(Product product)
        {
            try
            {
                var result = await Shell.Current.DisplayAlert("Usuwanie", $"Czy chcesz usunąć produkt : {product.Name.Name}", "Tak", "nie");
                if (!result)
                    return;

                product.IsDelete = true;
                Day.Products.Remove(product);
                await Shell.Current.DisplayAlert("Usuwanie", $"Produkt {product.Name.Name} został usunięty", "Ok");
                await _saveInventoryAoT.SaveProduct(product, Shared.Helper.UserAfterLogin.User.Id.ToByteArray());
                RefreshListOfProduct();
            }
            catch (Exception ex)
            {
                _db.SaveLogExtension(ex);
            }

        }

        [RelayCommand]
        async Task AddProduct()
        {
            try
            {
                var allProducts = await _db.DataBaseAsync.Table<ProductName>().OrderBy(x => x.Arrangement).ToArrayAsync();
                var names = Day.Products.Select(x => x.Name);
                var a = allProducts.Except(names);
                var products = a.Select(x => x.Name).ToList();
                //var products = allProducts.Where(x => Day.Products.All(z => z.ProductNameId != x.Id)).Select(x => x.Name).ToList();
                products.Add("Dodaj nowy");

                var result = await Shell.Current.DisplayActionSheet("Dodaj produkt z list", "Anuluj", null, [.. products]);

                if (result == " Anuluj")
                    return;

                if (result == "Dodaj nowy")
                {
                    await Shell.Current.GoToAsync($"{nameof(Pages.Products.ListProduct.AddEdit.AddEditProductV)}?",
                        new Dictionary<string, object>()
                        {
                            [nameof(Pages.Products.ListProduct.ListProductM)] = new Pages.Products.ListProduct.ListProductM()
                        });
                    return;
                }

                var selectedProduct = allProducts.FirstOrDefault(x => x.Name == result);

                if (selectedProduct is not null)
                {
                    var newProduct = await _db.DataBaseAsync.Table<Product>().FirstOrDefaultAsync(x => x.DayId == Day.Id && x.ProductNameId == selectedProduct.Id);
                    newProduct ??= new Product();

                    newProduct.Name = selectedProduct;
                    if (newProduct.ProductPriceId != Guid.Empty)
                    {
                        newProduct.Price = await _db.DataBaseAsync.Table<ProductPrice>().FirstOrDefaultAsync(x => x.Id == newProduct.ProductPriceId);
                    }
                    else
                    {
                        newProduct.Price = await _db.DataBaseAsync.Table<ProductPrice>().FirstOrDefaultAsync(x => x.ProductNameId == selectedProduct.Id);
                        newProduct.ProductPriceId = newProduct.Price.Id;
                    }

                    newProduct.ProductNameId = newProduct.Name.Id;

                    Day.Products.Add(newProduct);
                    RefreshListOfProduct();
                }
            }
            catch (Exception ex)
            {
                _db.SaveLogExtension(ex);
            }

        }


        [RelayCommand]
        void SortCakesAfterPrice()
        {
            if (SingleDayM.CakeSortPriceRotateX == 0)
            {
                SingleDayM.CakeSortPriceRotateX = 180;
                Day.Cakes = new(Day.Cakes.OrderBy(x => x.Price));
            }
            else
            {
                SingleDayM.CakeSortPriceRotateX = 0;
                Day.Cakes = new(Day.Cakes.OrderByDescending(x => x.Price));
            }
            SingleDayM.CakeAllIsVisible = false;
        }

        [RelayCommand]
        void SortCakesAfterDate()
        {
            if (SingleDayM.CakeSortDateRotateX == 0)
            {
                SingleDayM.CakeSortDateRotateX = 180;
                Day.Cakes = new(Day.Cakes.OrderBy(x => x.CreatedTicks));
            }
            else
            {
                SingleDayM.CakeSortDateRotateX = 0;
                Day.Cakes = new(Day.Cakes.OrderByDescending(x => x.CreatedTicks));
            }
            SingleDayM.CakeAllIsVisible = false;
        }

        private Product lastProductHideElseExpanded = new();
        [RelayCommand]
        void HideElseExpanded(Product product)
        {
            if (product.IsExpanded)
            {
                product.IsExpanded = false;
                return;
            }

            product.IsExpanded = true;
            if (lastProductHideElseExpanded.ProductNameId == product.ProductNameId)
            {
                return;
            }
            lastProductHideElseExpanded.IsExpanded = false;
            lastProductHideElseExpanded = product;
        }

        [RelayCommand]
        async Task SetFullReturn(Product product)
        {
            if (product is not null)
            {
                product.NumberReturn = product.Number + product.NumberEdit;
                await Toast.Make($"{product.Name.Name} zwrot {product.NumberReturn}", duration: CommunityToolkit.Maui.Core.ToastDuration.Short).Show();
                await _saveInventoryAoT.SaveProduct(product, Shared.Helper.UserAfterLogin.User.Id.ToByteArray());
            }
        }



        #endregion

    }
}
