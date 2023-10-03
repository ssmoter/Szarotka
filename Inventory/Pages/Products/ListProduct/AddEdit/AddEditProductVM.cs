using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

using DataBase.Helper.Img;

using Inventory.Helper.Parse;
using Inventory.Model;

using System.Collections.ObjectModel;

namespace Inventory.Pages.Products.ListProduct.AddEdit
{
    [QueryProperty(nameof(Product), nameof(ListProductM))]
    public partial class AddEditProductVM : ObservableObject
    {
        [ObservableProperty]
        ListProductM product;

        [ObservableProperty]
        AddEditProductM addEdit;

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
            _db = db;
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
                    Id = Product.Name.Id,
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
    }

    #endregion


}

