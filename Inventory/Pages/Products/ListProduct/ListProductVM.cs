using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

using Inventory.Helper.Parse;
using Inventory.Model;

using System.Collections.ObjectModel;

namespace Inventory.Pages.Products.ListProduct
{
    public partial class ListProductVM : ObservableObject
    {
        [ObservableProperty]
        ObservableCollection<ListProductM> productMs;

        [ObservableProperty]
        bool isGenerateDefoultEnable;

        readonly DataBase.Data.AccessDataBase _db;
        public ListProductVM(DataBase.Data.AccessDataBase db)
        {
            ProductMs = new ObservableCollection<ListProductM>();
            this._db = db;
            Task.Run(async () =>
            {
                try
                {
                    await SelectAllProductsAsync();
                }
                catch (Exception ex)
                {
                    _db.SaveLog(ex);
                }
            });
            Inventory.Service.ProductsUpdateService.Update += SelectAllProductsAsync;
        }
        #region Method

        #region Async


        public async Task SelectAllProductsAsync()
        {
            try
            {
                ProductMs.Clear();
                var names = await _db.DataBaseAsync.Table<ProductName>().ToArrayAsync();


                for (int i = 0; i < names.Length; i++)
                {
                    ProductMs.Add(new ListProductM()
                    {
                        Name = names[i].PareseAsProductNameM(),
                    });
                    ProductMs[i].Prices = await SelectPricesAsync(names[i].Id);
                    ProductMs[i].SetActualPrice();
                }
                IsGenerateDefoultEnable = ProductMs.Count() <= 0;
            }
            catch (Exception ex)
            {
                await _db.SaveLogAsync(ex);
            }
        }
        async Task<ObservableCollection<Inventory.Model.MVVM.ProductPriceM>> SelectPricesAsync(Guid id)
        {
            var price = new ObservableCollection<Inventory.Model.MVVM.ProductPriceM>();

            var priceM = await _db.DataBaseAsync.Table<ProductPrice>().Where(x => x.ProductNameId == id).OrderByDescending(z => z.Id).FirstOrDefaultAsync();
            price.Add(priceM.PareseAsProductPriceM());

            return price;
        }

        #endregion

        #region Sync

        public void SelectAllProducts()
        {
            try
            {
                ProductMs.Clear();
                var names = _db.DataBase.Table<ProductName>().ToArray();

                for (int i = 0; i < names.Length; i++)
                {
                    ProductMs.Add(new ListProductM()
                    {
                        Name = names[i].PareseAsProductNameM(),
                    });
                    ProductMs[i].Prices = SelectPrices(names[i].Id);
                    ProductMs[i].SetActualPrice();
                }
            }
            catch (Exception ex)
            {
                _db.SaveLog(ex);
            }
        }
        ObservableCollection<Inventory.Model.MVVM.ProductPriceM> SelectPrices(Guid id)
        {
            var price = new ObservableCollection<Model.MVVM.ProductPriceM>();
            var priceM = _db.DataBase.Table<ProductPrice>().Where(x => x.ProductNameId == id).OrderByDescending(z => z.Id).FirstOrDefault();
            price.Add(priceM.PareseAsProductPriceM());
            return price;
        }

        #endregion

        #endregion
        #region Commend

        [RelayCommand]
        async Task DeleteProduct(ListProductM value)
        {
            try
            {
                bool result = await Shell.Current.DisplayAlert(value.Name.Name, "Czy na pewno chcesz usunąć?", "Tak", "Nie");
                if (result)
                {
                    try
                    {
                        await _db.DataBaseAsync.DeleteAsync(value.Name.PareseAsProductName());
                        for (int i = 0; i < value.Prices.Count; i++)
                        {
                            await _db.DataBaseAsync.DeleteAsync(value.Prices[i].PareseAsProductPrice());
                        }
                        await Shell.Current.DisplayAlert(value.Name.Name, "Obiekt został usunięty", "Ok");
                        ProductMs.Remove(value);
                    }
                    catch (Exception ex)
                    {
                        await _db.SaveLogAsync(ex);
                        await Shell.Current.DisplayAlert("Error", ex.Message, "Ok");
                    }
                }
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
                await Shell.Current.GoToAsync($"{nameof(AddEdit.AddEditProductV)}?"
                    , new Dictionary<string, object>
                    {
                        [nameof(ListProductM)] = new ListProductM(),
                    });
            }
            catch (Exception ex)
            {
                _db.SaveLog(ex);
            }
        }
        [RelayCommand]
        async Task EditProduct(ListProductM value)
        {
            try
            {
                if (value is null)
                {
                    return;
                }
                await Shell.Current.GoToAsync($"{nameof(AddEdit.AddEditProductV)}?"
                    , new Dictionary<string, object>
                    {
                        [nameof(ListProductM)] = value,
                    });
            }
            catch (Exception ex)
            {
                _db.SaveLog(ex);
            }

        }

        [RelayCommand]
        async Task GenerateDefaultProducts()
        {
            try
            {

                var products = new Product[]
                {
                new Product()
                {
                  Name = new ProductName(){ Name ="Chleb",Img="chleb.png",Id = Guid.NewGuid()},
                  Price = new ProductPrice(){CreatedDateTime=DateTime.Now,PriceDecimal=5.5m,Id = Guid.NewGuid()},
                },
                new Product()
                {
                  Name = new ProductName(){ Name ="Duży chleb",Img="chleb.png", Id = Guid.NewGuid()},
                  Price = new ProductPrice(){CreatedDateTime=DateTime.Now,PriceDecimal=11m,Id = Guid.NewGuid()},
                },
                new Product()
                {
                  Name = new ProductName(){ Name ="Bułka",Img="chleb.png",Id = Guid.NewGuid()},
                  Price = new ProductPrice(){CreatedDateTime=DateTime.Now,PriceDecimal=1.5m,Id = Guid.NewGuid()},
                },
                new Product()
                {
                  Name = new ProductName(){ Name ="Drożdżówka",Img="chleb.png",Id = Guid.NewGuid()},
                  Price = new ProductPrice(){CreatedDateTime=DateTime.Now,PriceDecimal=2.5m,Id = Guid.NewGuid()},
                },
                new Product()
                {
                  Name = new ProductName(){ Name ="Drożdżówki na wagę (opak.)",Img="chleb.png",Id = Guid.NewGuid()},
                  Price = new ProductPrice(){CreatedDateTime=DateTime.Now,PriceDecimal=9m,Id = Guid.NewGuid()},
                },
                new Product()
                {
                  Name = new ProductName(){ Name ="Mała bułka",Img="chleb.png",Id = Guid.NewGuid()},
                  Price = new ProductPrice(){CreatedDateTime=DateTime.Now,PriceDecimal=0.7m,Id = Guid.NewGuid()},
                },
                new Product()
                {
                  Name = new ProductName(){ Name ="Bułka z serem",Img="chleb.png",Id = Guid.NewGuid()},
                  Price = new ProductPrice(){CreatedDateTime=DateTime.Now,PriceDecimal=2.5m,Id = Guid.NewGuid()},
                },
                new Product()
                {
                  Name = new ProductName(){ Name ="Ciastka francuskie",Img="chleb.png",Id = Guid.NewGuid()},
                  Price = new ProductPrice(){CreatedDateTime=DateTime.Now,PriceDecimal=3m,Id = Guid.NewGuid()},
                },
                new Product()
                {
                  Name = new ProductName(){ Name ="Ciastka (opak. 400g)",Img="chleb.png",Id = Guid.NewGuid()},
                  Price = new ProductPrice(){CreatedDateTime=DateTime.Now,PriceDecimal=10m,Id = Guid.NewGuid()},
                },
                new Product()
                {
                  Name = new ProductName(){ Name ="Piernik/Babka",Img="chleb.png",Id = Guid.NewGuid()},
                  Price = new ProductPrice(){CreatedDateTime=DateTime.Now,PriceDecimal=12m,Id = Guid.NewGuid()},
                },
                new Product()
                {
                  Name = new ProductName(){ Name ="Makowiec",Img="chleb.png",Id = Guid.NewGuid()},
                  Price = new ProductPrice(){CreatedDateTime=DateTime.Now,PriceDecimal=14m,Id = Guid.NewGuid()},
                },
                new Product()
                {
                  Name = new ProductName(){ Name ="Wafle (opak. 400g)",Img="chleb.png",Id = Guid.NewGuid()},
                  Price = new ProductPrice(){CreatedDateTime=DateTime.Now,PriceDecimal=10m,Id = Guid.NewGuid()},
                },
                new Product()
                {
                  Name = new ProductName(){ Name ="Kołacz",Img="chleb.png",Id = Guid.NewGuid()},
                  Price = new ProductPrice(){CreatedDateTime=DateTime.Now,PriceDecimal=5m,Id = Guid.NewGuid()},
                },
                new Product()
                {
                  Name = new ProductName(){ Name ="Chałka",Img="chleb.png",Id = Guid.NewGuid()},
                  Price = new ProductPrice(){CreatedDateTime=DateTime.Now,PriceDecimal=5m,Id = Guid.NewGuid()},
                },
                new Product()
                {
                  Name = new ProductName(){ Name ="Mini Pizza",Img="chleb.png",Id = Guid.NewGuid()},
                  Price = new ProductPrice(){CreatedDateTime=DateTime.Now,PriceDecimal=3m,Id = Guid.NewGuid()},
                },
                new Product()
                {
                  Name = new ProductName(){ Name ="Bułka tarta",Img="chleb.png",Id = Guid.NewGuid()},
                  Price = new ProductPrice(){CreatedDateTime=DateTime.Now,PriceDecimal=3.5m,Id = Guid.NewGuid()},
                },
                new Product()
                {
                  Name = new ProductName(){ Name ="Parówka w cieście",Img="chleb.png",Id = Guid.NewGuid()},
                  Price = new ProductPrice(){CreatedDateTime=DateTime.Now,PriceDecimal=3.5m,Id = Guid.NewGuid()},
                },
                };

                for (int i = 0; i < products.Length; i++)
                {
                    var id = await _db.DataBaseAsync.InsertAsync(products[i].Name);
                    var name = products[i].Name.Name;
                    products[i].Name = await _db.DataBaseAsync.Table<ProductName>().Where(x => x.Name == name).FirstOrDefaultAsync();
                    products[i].Price.ProductNameId = products[i].Name.Id;
                    await _db.DataBaseAsync.InsertAsync(products[i].Price);
                }
                await SelectAllProductsAsync();
            }
            catch (Exception ex)
            {
                _db.SaveLog(ex);
            }
        }


        #endregion


    }
}
