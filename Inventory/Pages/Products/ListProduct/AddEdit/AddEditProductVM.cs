using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

using DataBase.Model.EntitiesInventory;

using System.Collections.ObjectModel;

using static Inventory.Pages.Products.ListProduct.AddEdit.AddEditProductM;

namespace Inventory.Pages.Products.ListProduct.AddEdit
{
    [QueryProperty(nameof(Product), nameof(ListProductM))]
    public partial class AddEditProductVM : ObservableObject
    {
        [ObservableProperty]
        ListProductM product;

        [ObservableProperty]
        AddEditProductM addEdit;

        [ObservableProperty]
        ObservableCollection<string> imgList;


        readonly DataBase.Data.AccessDataBase _db;

        public AddEditProductVM(DataBase.Data.AccessDataBase db)
        {
            AddEdit = new AddEditProductM();
            if (Product is null)
            {
                AddEdit.AddP = true;
                AddEdit.UpdateP = false;
                Product = new ListProductM()
                {
                    Name = new ProductName() { Name = "" },
                    Prices = [],
                };
            }
            ImgList ??= [];
            _db = db;
        }

        private static readonly string[] extensionsValues = ["jpg", "png", "gif"];

        #region Method

        static PickOptions FileTyp()
        {
            var pOptions = new PickOptions();
            var dictionaryTyp = new Dictionary<DevicePlatform, IEnumerable<string>>
            {
                { DevicePlatform.WinUI, extensionsValues },
                { DevicePlatform.Android, extensionsValues }
            };

            pOptions.FileTypes = new FilePickerFileType(dictionaryTyp);

            return pOptions;
        }

        public async Task GetPrices(Guid id)
        {
            var price = await _db.DataBaseAsync.Table<ProductPrice>().Where(x => x.ProductNameId == id).OrderByDescending(x => x.CreatedTicks).ToArrayAsync();
            Product.Prices.Clear();
            Product.Prices = new(price);
        }
        #endregion

        #region Command
        [RelayCommand]
        async Task Back()
        {
            try
            {
                Product.Prices.Clear();

                await Shell.Current.GoToAsync("..");
            }
            catch (Exception ex)
            {
                _db.SaveLog(ex);
            }
        }

        [RelayCommand]
        async Task UpdateProduct()
        {
            try
            {
                var productName = new ProductName()
                {
                    Id = new Guid(Product.Name.Id.ToByteArray()),
                    Name = Product.Name.Name,
                    Description = Product.Name.Description,
                    Img = Product.Name.Img,
                    Updated = DateTime.Now,
                    IsVisible = Product.Name.IsVisible,
                    Arrangement = Product.Name.Arrangement,
                };
                await _db.DataBaseAsync.UpdateAsync(productName);

                var count = await _db.DataBaseAsync.Table<ProductPrice>().CountAsync(x => x.ProductNameId == productName.Id);

                for (int i = 0; i < (Product.Prices.Count - count); i++)
                {
                    await _db.DataBaseAsync.InsertAsync(Product.Prices[i]);
                }

                await Shell.Current.DisplayAlert("Aktualizacja", $"Produkt {Product.Name.Name} został zaktualizowany", "Ok");
            }
            catch (Exception ex)
            {
                _db.SaveLog(ex);
            }
        }


        [RelayCommand]
        async Task InsertProduct()
        {
            try
            {
                var productName = new ProductName()
                {
                    Id = Guid.NewGuid(),
                    Name = Product.Name.Name,
                    Description = Product.Name.Description,
                    Img = Product.Name.Img,
                    Updated = DateTime.Now,
                    Created = DateTime.Now,
                };
                if (string.IsNullOrWhiteSpace(productName.Img))
                {
                    productName.Img = DataBase.Helper.Img.ImgPath.Logo;
                }
                productName.Arrangement = _db.DataBase.Table<ProductName>().Count() + 1;


                await _db.DataBaseAsync.InsertAsync(productName);

                if (Product.Prices is not null)
                    for (int i = 0; i < Product.Prices.Count; i++)
                    {
                        Product.Prices[i].ProductNameId = new Guid(productName.Id.ToByteArray());
                        await _db.DataBaseAsync.InsertAllAsync(Product.Prices);
                    }

                Product.Name = productName;

                await Shell.Current.DisplayAlert("Dodany", $"Produkt {Product.Name.Name} został dodany", "Ok");
            }
            catch (Exception ex)
            {
                _db.SaveLog(ex);
            }
            finally
            {
                AddEdit.AddP = false;
                AddEdit.UpdateP = true;
            }
        }


        [RelayCommand]
        async Task UpdatePrice()
        {
            try
            {

#if __ANDROID_24__
                var result = await Shell.Current.DisplayPromptAsync
                    ("Nowa cena", $"Dodaj nową cenę do {Product.Name.Name}", "Tak", "Nie", keyboard: Keyboard.Telephone);
#else
                var result = await Shell.Current.DisplayPromptAsync
                    ("Nowa cena", $"Dodaj nową cenę do {Product.Name.Name}", "Tak", "Nie", keyboard: Keyboard.Numeric);
#endif
                if (string.IsNullOrWhiteSpace(result))
                {
                    return;
                }

                result = result.Replace('.', ',');
                if (decimal.TryParse(result, out decimal price))
                {
                    ProductPrice newPrice = new()
                    {
                        Id = Guid.NewGuid(),
                        Created = DateTime.Now,
                        Updated = DateTime.Now,
                        PriceDecimal = price,
                        ProductNameId = Product.Name.Id
                    };
                    Product.Prices.Insert(0, newPrice);
                }
            }
            catch (Exception ex)
            {
                _db.SaveLog(ex);
            }
        }


        [RelayCommand]
        void SwipeViewGesture(string selected)
        {
            var id = int.Parse(selected);
            ImgList.Clear();
            switch ((FrameToDisplay)id)
            {
                case FrameToDisplay.frame:
                    AddEdit.IsVisibleFrame = !AddEdit.IsVisibleFrame;
                    break;
                case FrameToDisplay.bread:
                    AddEdit.IsVisibleBread = !AddEdit.IsVisibleBread;
                    ImgList = new ObservableCollection<string>(DataBase.Helper.Img.ImgPath.Bread);
                    break;
                case FrameToDisplay.buns:
                    AddEdit.IsVisibleBuns = !AddEdit.IsVisibleBuns;
                    ImgList = new ObservableCollection<string>(DataBase.Helper.Img.ImgPath.Buns);
                    break;
                case FrameToDisplay.cake:
                    AddEdit.IsVisibleCake = !AddEdit.IsVisibleCake;
                    ImgList = new ObservableCollection<string>(DataBase.Helper.Img.ImgPath.Cakes);
                    break;
                case FrameToDisplay.cookies:
                    AddEdit.IsVisibleCookies = !AddEdit.IsVisibleCookies;
                    ImgList = new ObservableCollection<string>(DataBase.Helper.Img.ImgPath.Cookies);
                    break;
                case FrameToDisplay.other:
                    AddEdit.IsVisibleOther = !AddEdit.IsVisibleOther;
                    ImgList = new ObservableCollection<string>(DataBase.Helper.Img.ImgPath.Other);
                    break;
                case FrameToDisplay.@default:
                    Product.Name.Img = DataBase.Helper.Img.ImgPath.Logo;
                    ImgList.Clear();
                    break;
            }
        }

        [RelayCommand]
        async Task SelectImageFromDevice()
        {
            if (!await DataBase.Service.AndroidPermissionService.CheckAllPermissionsAboutStorage())
            {
                return;
            }

            var file = await FilePicker.PickAsync(FileTyp());

            if (file is null)
            {
                return;
            }

            var bytes = File.ReadAllBytes(file.FullPath);
            var stringBase = System.Convert.ToBase64String(bytes);

            Product.Name.Img = stringBase;

        }

        [RelayCommand]
        void SetCurrentImg(string imgPath)
        {
            if (!string.IsNullOrWhiteSpace(imgPath))
            {
                Product.Name.Img = imgPath;
            }
        }



    }

    #endregion


}

