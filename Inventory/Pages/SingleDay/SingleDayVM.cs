﻿using CommunityToolkit.Maui.Alerts;
using CommunityToolkit.Maui.Views;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

using DataBase.Model.EntitiesInventory;

using Inventory.Service;

namespace Inventory.Pages.SingleDay
{
    [QueryProperty(nameof(Day), nameof(Day))]
    public partial class SingleDayVM : ObservableObject
    {
        [ObservableProperty]
        Day day;

        [ObservableProperty]
        SingleDayM singleDayM;

        static (string name, int value, char sign) lastFastValue = new("", 0, ' ');
        const char signPlus = '+';
        //const char signMinus = '-';
        readonly DataBase.Data.AccessDataBase _db;
        readonly ISaveDayService _saveDay;
        readonly ISelectDayService _selectDay;

        public SingleDayVM(DataBase.Data.AccessDataBase db, ISaveDayService saveDay, ISelectDayService selectDay)
        {
            _db = db;
            _saveDay = saveDay;
            Day ??= new();
            SingleDayM ??= new();
            _selectDay = selectDay;
        }



        #region Method
        public async Task ShowCurrentDay()
        {
            await CommunityToolkit.Maui.Alerts.Toast.Make($"Wczytano dzień {Day.SelectedDate.ToShortDateString()}", CommunityToolkit.Maui.Core.ToastDuration.Short).Show();
        }

        static void FastChangeProductNumber(Product product, int value)
        {
            if (product is not null)
            {
                if (product.CanUpadte == false)
                {
                    SetCanUpdate(product);
                }
                product.Number += value;

                ToastMakeFastChange(product, value, "ilość");
            }
        }

        static void FastChangeProductEdit(Product product, int value)
        {
            if (product is not null)
            {
                if (product.CanUpadte == false)
                {
                    SetCanUpdate(product);
                }
                product.NumberEdit += value;

                ToastMakeFastChange(product, value, "edycja");
            }
        }

        static void FastChangeProductReturn(Product product, int value)
        {
            if (product is not null)
            {
                if (product.CanUpadte == false)
                {
                    SetCanUpdate(product);
                }
                product.NumberReturn += value;

                ToastMakeFastChange(product, value, "zwrot");
            }
        }
        private static void ToastMakeFastChange(Product product, int value, string message)
        {
            if (Math.Sign(lastFastValue.value) != Math.Sign(value))
            {
                lastFastValue.value = 0;
            }

            if (lastFastValue.name == product.Name.Name)
                lastFastValue.value += value;
            else
            {
                lastFastValue.value = 1;
                lastFastValue.name = product.Name.Name;
            }
            char sign = value > 0 ? signPlus : ' ';
            Toast.Make($"{product.Name.Name} {message} {sign} {lastFastValue.value}", duration: CommunityToolkit.Maui.Core.ToastDuration.Short).Show();
        }
        static void SetCanUpdate(Product product)
        {
            product.CanUpadte = true;
            product.CalculatePrice();
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
            await _saveDay.SaveDayAsync(Day);
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

                var response = await Shell.Current.DisplayPromptAsync("Ciasto", "Podaj cene ciasta", "Tak", "Anuluj", keyboard: Keyboard.Numeric);
#if __ANDROID_24__
                //var response = await Shell.Current.DisplayPromptAsync("Ciasto", "Podaj cene ciasta", "Tak", "Anuluj", keyboard: Keyboard.Telephone);
#else
#endif

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
                        Updated=DateTime.Now,
                    };

                    Day.Cakes.Add(cake);
                    Day.Cakes.LastOrDefault().IsSell = true;
                    DataBase.Model.EntitiesInventory.ProductUpdatePriceService.OnUpdate();

                    await CommunityToolkit.Maui.Alerts.Toast.Make($"Dodano ciasto z ceną {value}", CommunityToolkit.Maui.Core.ToastDuration.Short).Show();
                }
            }
            catch (Exception ex)
            {
                _db.SaveLog(ex);
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
                DataBase.Model.EntitiesInventory.ProductUpdatePriceService.OnUpdate();
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
                var result = await Shell.Current.DisplayAlert("Zapisać", "Czy zapisać przy cofaniu", "Tak", "Nie");
                if (result)
                    await SaveDay();
                await Shell.Current.GoToAsync("..?",
                    new Dictionary<string, object>()
                    {
                        [nameof(Day)] = Day
                    });
            }
            catch (Exception ex)
            {
                _db.SaveLog(ex);
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
                _db.SaveLog(ex);
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
                }
            }
        }

        [RelayCommand]
        void RefreshListOfProduct()
        {
            try
            {
                Day.CanUpadte = true;
                for (int i = 0; i < Day.Products.Count; i++)
                {
                    SetCanUpdate(Day.Products[i]);
                }
                Day.UpdateTotalPrice();
            }
            catch (Exception ex) { _db.SaveLog(ex); }
            finally { SingleDayM.ProductIsRefreshing = false; }
        }

        [RelayCommand]
        static async Task PopupToAddValueFromList(Product product)
        {
            var popup = new DataBase.Pages.Popups.SubAddLastValue.SubAddLastValueV(product.NumberEdit, product.Name.Name);

            var result = await Shell.Current.ShowPopupAsync(popup);

            if (result is not null)
            {
                product.NumberEdit = (int)result;
            }
        }

        [RelayCommand]
        async Task ChangeProductPrice(Product product)
        {
            try
            {
                var oldPrice = await _db.DataBaseAsync.Table<ProductPrice>().Where(x => x.ProductNameId == product.ProductNameId).ToArrayAsync();
                var priceArrya = oldPrice.Select(x => x.PriceDecimal).Select(x => x.ToString()).ToList();
                priceArrya.Add("Nowa");

                var result = await Shell.Current.DisplayActionSheet("Zmiana ceny",
                                                                    "Anuluj",
                                                                    null, [.. priceArrya]);
                if (result == "Anuluj")
                {
                    return;
                }
                if (result == "Nowa")
                {
                    var listProduct = new Pages.Products.ListProduct.ListProductM()
                    {
                        Name = product.Name,
                        Prices = new(await _db.DataBaseAsync.Table<ProductPrice>().Where(x => x.ProductNameId == product.ProductNameId).ToArrayAsync())
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
                _db.SaveLog(ex);
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

                Day.Products.Remove(product);
                await Shell.Current.DisplayAlert("Usuwanie", $"Produkt {product.Name.Name} został usunięty", "Ok");

                RefreshListOfProduct();
            }
            catch (Exception ex)
            {
                _db.SaveLog(ex);
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
                    var newProduct = new Product()
                    {
                        Name = selectedProduct,
                        Price = await _db.DataBaseAsync.Table<ProductPrice>().FirstOrDefaultAsync(x => x.ProductNameId == selectedProduct.Id),
                    };
                    newProduct.ProductNameId = newProduct.Name.Id;
                    newProduct.ProductPriceId = newProduct.Price.Id;
                    Day.Products.Add(newProduct);
                    RefreshListOfProduct();
                }
            }
            catch (Exception ex)
            {
                _db.SaveLog(ex);
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
        }
        #endregion

    }
}
