using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

using Inventory.Helper;
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
                await SelectAllProductsAsync();
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
        async Task<ObservableCollection<Inventory.Model.MVVM.ProductPriceM>> SelectPricesAsync(int id)
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
        ObservableCollection<Inventory.Model.MVVM.ProductPriceM> SelectPrices(int id)
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

        [RelayCommand]
        async Task AddProduct()
        {
            await Shell.Current.GoToAsync($"{nameof(AddEdit.AddEditProductV)}?"
                , new Dictionary<string, object>
                {
                    [nameof(ListProductM)] = new ListProductM(),
                });
        }
        [RelayCommand]
        async Task EditProduct(ListProductM value)
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

        [RelayCommand]
        async Task GenerateDefaultProducts()
        {
            var products = new Product[]
            {
                new Product()
                {
                  Name = new ProductName(){ Name ="Chleb",Img="chleb.png"},
                  Price = new ProductPrice(){CreatedDateTime=DateTime.Now,PriceDecimal=5.5m},
                },
                new Product()
                {
                  Name = new ProductName(){ Name ="Duży chleb",Img="chleb.png"},
                  Price = new ProductPrice(){CreatedDateTime=DateTime.Now,PriceDecimal=11m},
                },
                new Product()
                {
                  Name = new ProductName(){ Name ="Bułka",Img="chleb.png"},
                  Price = new ProductPrice(){CreatedDateTime=DateTime.Now,PriceDecimal=1.5m},
                },
                new Product()
                {
                  Name = new ProductName(){ Name ="Drożdżówka",Img="chleb.png"},
                  Price = new ProductPrice(){CreatedDateTime=DateTime.Now,PriceDecimal=2.5m},
                },
                new Product()
                {
                  Name = new ProductName(){ Name ="Drożdżówki na wagę (opak.)",Img="chleb.png"},
                  Price = new ProductPrice(){CreatedDateTime=DateTime.Now,PriceDecimal=9m},
                },
                new Product()
                {
                  Name = new ProductName(){ Name ="Mała bułka",Img="chleb.png"},
                  Price = new ProductPrice(){CreatedDateTime=DateTime.Now,PriceDecimal=0.7m},
                },
                new Product()
                {
                  Name = new ProductName(){ Name ="Bułka z serem",Img="chleb.png"},
                  Price = new ProductPrice(){CreatedDateTime=DateTime.Now,PriceDecimal=2.5m},
                },
                new Product()
                {
                  Name = new ProductName(){ Name ="Ciastka francuskie",Img="chleb.png"},
                  Price = new ProductPrice(){CreatedDateTime=DateTime.Now,PriceDecimal=3m},
                },
                new Product()
                {
                  Name = new ProductName(){ Name ="Ciastka (opak. 400g)",Img="chleb.png"},
                  Price = new ProductPrice(){CreatedDateTime=DateTime.Now,PriceDecimal=10m},
                },
                new Product()
                {
                  Name = new ProductName(){ Name ="Piernik/Babka",Img="chleb.png"},
                  Price = new ProductPrice(){CreatedDateTime=DateTime.Now,PriceDecimal=12m},
                },
                new Product()
                {
                  Name = new ProductName(){ Name ="Makowiec",Img="chleb.png"},
                  Price = new ProductPrice(){CreatedDateTime=DateTime.Now,PriceDecimal=14m},
                },
                new Product()
                {
                  Name = new ProductName(){ Name ="Wafle (opak. 400g)",Img="chleb.png"},
                  Price = new ProductPrice(){CreatedDateTime=DateTime.Now,PriceDecimal=10m},
                },
                new Product()
                {
                  Name = new ProductName(){ Name ="Kołacz",Img="chleb.png"},
                  Price = new ProductPrice(){CreatedDateTime=DateTime.Now,PriceDecimal=5m},
                },
                new Product()
                {
                  Name = new ProductName(){ Name ="Chałka",Img="chleb.png"},
                  Price = new ProductPrice(){CreatedDateTime=DateTime.Now,PriceDecimal=5m},
                },
                new Product()
                {
                  Name = new ProductName(){ Name ="Mini Pizza",Img="chleb.png"},
                  Price = new ProductPrice(){CreatedDateTime=DateTime.Now,PriceDecimal=3m},
                },
                new Product()
                {
                  Name = new ProductName(){ Name ="Bułka tarta",Img="chleb.png"},
                  Price = new ProductPrice(){CreatedDateTime=DateTime.Now,PriceDecimal=3.5m},
                },
                new Product()
                {
                  Name = new ProductName(){ Name ="Parówka w cieście",Img="chleb.png"},
                  Price = new ProductPrice(){CreatedDateTime=DateTime.Now,PriceDecimal=3.5m},
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

            //for (int i = 0; i < products.Length; i++)
            //{
            //    var id = await _db.DataBaseAsync.InsertAsync(products[i].Name);
            //    var name = products[i].Name.Name;
            //    products[i].Name = await _db.DataBaseAsync.Table<ProductName>().Where(x => x.Name == name).FirstOrDefaultAsync();
            //    products[i].Price.ProductNameId = products[i].Name.Id;
            //    await _db.DataBaseAsync.InsertAsync(products[i].Price);
            //}
            //await SelectAllProductsAsync();
        }


        #endregion


    }
}
