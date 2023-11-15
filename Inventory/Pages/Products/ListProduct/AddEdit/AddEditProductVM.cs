using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

using Inventory.Helper.Parse;
using Inventory.Model;

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
                    Name = new Model.MVVM.ProductNameM() { Name = "" },
                    Prices = new ObservableCollection<Model.MVVM.ProductPriceM>(),
                };
            }
            ImgList ??= new();
            _db = db;
        }

        #region Method

        static PickOptions FileTyp()
        {
            var pOptions = new PickOptions();
            var dictionaryTyp = new Dictionary<DevicePlatform, IEnumerable<string>>
            {
                { DevicePlatform.WinUI, new[] { "jpg", "png","gif" } },
                { DevicePlatform.Android, new[] { "jpg", "png", "gif" } }
            };

            pOptions.FileTypes = new FilePickerFileType(dictionaryTyp);

            return pOptions;
        }

        public async Task GetPrices(Guid id)
        {
            var priceM = await _db.DataBaseAsync.Table<Model.ProductPrice>().Where(x => x.ProductNameId == id).OrderByDescending(x => x.Id).ToArrayAsync();
            Product.Prices.Clear();
            for (int i = 0; i < priceM.Length; i++)
            {
                Product.Prices.Add(priceM[i].PareseAsProductPriceM());
            }
        }
        #endregion

        #region Command
        [RelayCommand]
        async Task Back()
        {
            try
            {
                await Inventory.Service.ProductsUpdateService.OnUpdate();

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
                var productName = new Model.ProductName()
                {
                    Id = new Guid(Product.Name.Id.ToByteArray()),
                    Name = Product.Name.Name,
                    Description = Product.Name.Description,
                    Img = Product.Name.Img,
                };

                await _db.DataBaseAsync.UpdateAsync(productName);

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

                var productName = new Model.ProductName()
                {
                    Id = Guid.NewGuid(),
                    Name = Product.Name.Name,
                    Description = Product.Name.Description,
                    Img = Product.Name.Img,
                };
                productName.Img = DataBase.Helper.Img.ImgPath.Logo;
                await _db.DataBaseAsync.InsertAsync(productName);
                var id = await _db.DataBaseAsync.Table<ProductName>().Where(x => x.Name == productName.Name).FirstOrDefaultAsync();
                Product.Name = id.PareseAsProductNameM();
                await Shell.Current.DisplayAlert("Dodany", $"Produkt {Product.Name.Name} został dodany", "Ok");
                AddEdit.AddP = false;
                AddEdit.UpdateP = true;
            }
            catch (Exception ex)
            {
                _db.SaveLog(ex);
            }
        }


        [RelayCommand]
        async Task UpdatePrice()
        {
            try
            {

                if (Product.Name.Id == Guid.Empty)
                {
                    await Shell.Current.DisplayAlert("Uwaga", "Aby dodać cenę wcześniej musisz utworzyć produkt", "Ok");
                    return;
                }

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
                    Model.ProductPrice newPrice = new()
                    {
                        Id = Guid.NewGuid(),
                        CreatedDateTime = DateTime.Now,
                        PriceDecimal = price,
                        ProductNameId = Product.Name.Id
                    };
                    await _db.DataBaseAsync.InsertAsync(newPrice);
                    await GetPrices(Product.Name.Id);
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

