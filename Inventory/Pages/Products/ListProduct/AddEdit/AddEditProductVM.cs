using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

using DataBase.Data;
using DataBase.Model.EntitiesInventory;

using Shared.Data;

using System.Collections.ObjectModel;

using static Inventory.Pages.Products.ListProduct.AddEdit.AddEditProductM;

namespace Inventory.Pages.Products.ListProduct.AddEdit
{
    public partial class AddEditProductVM : ObservableObject, IQueryAttributable
    {
        public void ApplyQueryAttributes(IDictionary<string, object> query)
        {
            if (query.TryGetValue(nameof(ListProductM), out object product))
            {
                if (product is ListProductM _product)
                {
                    Product = _product;
                }
            }

        }

        private ListProductM product;
        public ListProductM Product
        {
            get => product;
            set
            {
                if (SetProperty(ref product, value, nameof(Product))) { }
            }
        }

        private AddEditProductM addEdit;
        public AddEditProductM AddEdit
        {
            get => addEdit;
            set
            {
                if (SetProperty(ref addEdit, value, nameof(AddEdit))) { }
            }
        }

        private ObservableCollection<AddEditProductMImg> imgListBread = [];
        public ObservableCollection<AddEditProductMImg> ImgListBread
        {
            get => imgListBread;
            set
            {
                if (SetProperty(ref imgListBread, value, nameof(ImgListBread))) { }
            }
        }
        private ObservableCollection<AddEditProductMImg> imgListBuns = [];
        public ObservableCollection<AddEditProductMImg> ImgListBuns
        {
            get => imgListBuns;
            set
            {
                if (SetProperty(ref imgListBuns, value, nameof(ImgListBuns))) { }
            }
        }
        private ObservableCollection<AddEditProductMImg> imgListCake = [];
        public ObservableCollection<AddEditProductMImg> ImgListCake
        {
            get => imgListCake;
            set
            {
                if (SetProperty(ref imgListCake, value, nameof(ImgListCake))) { }
            }
        }
        private ObservableCollection<AddEditProductMImg> imgListCookies = [];
        public ObservableCollection<AddEditProductMImg> ImgListCookies
        {
            get => imgListCookies;
            set
            {
                if (SetProperty(ref imgListCookies, value, nameof(ImgListCookies))) { }
            }
        }
        private ObservableCollection<AddEditProductMImg> imgListOther = [];
        public ObservableCollection<AddEditProductMImg> ImgListOther
        {
            get => imgListOther;
            set
            {
                if (SetProperty(ref imgListOther, value, nameof(ImgListOther))) { }
            }
        }



        readonly IAccessDataBase _db;
        public AddEditProductVM(IAccessDataBase db)
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
            ImgListBread ??= [];

            for (int i = 0; i < Shared.Helper.Img.ImgPath.Cakes.Length; i++)
            {
                ImgListCake.Add(new(Shared.Helper.Img.ImgPath.Cakes[i]));
            }
            for (int i = 0; i < Shared.Helper.Img.ImgPath.Bread.Length; i++)
            {
                ImgListBread.Add(new(Shared.Helper.Img.ImgPath.Bread[i]));
            }
            for (int i = 0; i < Shared.Helper.Img.ImgPath.Cookies.Length; i++)
            {
                ImgListCookies.Add(new(Shared.Helper.Img.ImgPath.Cookies[i]));
            }
            for (int i = 0; i < Shared.Helper.Img.ImgPath.Buns.Length; i++)
            {
                ImgListBuns.Add(new(Shared.Helper.Img.ImgPath.Buns[i]));
            }
            for (int i = 0; i < Shared.Helper.Img.ImgPath.Other.Length; i++)
            {
                ImgListOther.Add(new(Shared.Helper.Img.ImgPath.Other[i]));
            }
            AddEdit.IsVisibleBread = true;

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
                _db.SaveLogExtension(ex);
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
                _db.SaveLogExtension(ex);
            }
        }


        [RelayCommand]
        async Task InsertProduct()
        {
            try
            {
                var productName = new ProductName()
                {
                    Id = Shared.Data.InventoryTables.GetGuidSed(),
                    Name = Product.Name.Name,
                    Description = Product.Name.Description,
                    Img = Product.Name.Img,
                    Updated = DateTime.Now,
                    Created = DateTime.Now,
                };
                if (string.IsNullOrWhiteSpace(productName.Img))
                {
                    productName.Img = Shared.Helper.Img.ImgPath.Logo;
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
                _db.SaveLogExtension(ex);
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
                        Id = Shared.Data.InventoryTables.GetGuidSed(),
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
                _db.SaveLogExtension(ex);
            }
        }


        [RelayCommand]
        void SwipeViewGesture(string selected)
        {
            var id = int.Parse(selected);
            AddEdit.IsVisibleBread = false;
            AddEdit.IsVisibleBuns = false;
            AddEdit.IsVisibleCake = false;
            AddEdit.IsVisibleCookies = false;
            AddEdit.IsVisibleOther = false;

            switch ((FrameToDisplay)id)
            {
                case FrameToDisplay.frame:
                    AddEdit.IsVisibleFrame = !AddEdit.IsVisibleFrame;
                    break;
                case FrameToDisplay.bread:
                    AddEdit.IsVisibleBread = !AddEdit.IsVisibleBread;
                    break;
                case FrameToDisplay.buns:
                    AddEdit.IsVisibleBuns = !AddEdit.IsVisibleBuns;
                    break;
                case FrameToDisplay.cake:
                    AddEdit.IsVisibleCake = !AddEdit.IsVisibleCake;
                    break;
                case FrameToDisplay.cookies:
                    AddEdit.IsVisibleCookies = !AddEdit.IsVisibleCookies;
                    break;
                case FrameToDisplay.other:
                    AddEdit.IsVisibleOther = !AddEdit.IsVisibleOther;
                    break;
                case FrameToDisplay.@default:
                    Product.Name.Img = Shared.Helper.Img.ImgPath.Logo;
                    break;
            }
        }

        [RelayCommand]
        async Task SelectImageFromDevice()
        {
            if (!await Shared.Service.AndroidPermissionService.CheckAllPermissionsAboutStorage())
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

