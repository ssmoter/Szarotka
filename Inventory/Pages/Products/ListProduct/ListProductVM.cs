using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

using DataBase.Helper.Img;

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

        readonly Random random = new(2137);
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
                IsGenerateDefoultEnable = ProductMs.Count <= 0;
            }
            catch (Exception ex)
            {
                await _db.SaveLogAsync(ex);
            }
        }
        async Task<ObservableCollection<Inventory.Model.MVVM.ProductPriceM>> SelectPricesAsync(Guid id)
        {
            var price = new ObservableCollection<Inventory.Model.MVVM.ProductPriceM>();

            var priceM = await _db.DataBaseAsync.Table<ProductPrice>().Where(x => x.ProductNameId == id).OrderByDescending(z => z.Id).ToArrayAsync();
            for (int i = 0; i < priceM.Length; i++)
            {
                price.Add(priceM[i].PareseAsProductPriceM());
            }

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

        Guid GetGuidSed()
        {
            byte[] guidBytes = new byte[16];
            random.NextBytes(guidBytes);
            return new Guid(guidBytes);
        }

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
                      Name = new ProductName(){ Name ="Chleb",Img=ImgBread.chleb,Id = GetGuidSed()},
                      Price = new ProductPrice(){CreatedDateTime=DateTime.Now,PriceDecimal=5.5m,Id = GetGuidSed()},
                    },
                    new Product()
                    {
                      Name = new ProductName(){ Name ="Duży chleb",Img="chleb.png", Id = GetGuidSed()},
                      Price = new ProductPrice(){CreatedDateTime=DateTime.Now,PriceDecimal=11m,Id = GetGuidSed()},
                    },
                    new Product()
                    {
                      Name = new ProductName(){ Name ="Bułka",Img=ImgBread.Bulka,Id = GetGuidSed()},
                      Price = new ProductPrice(){CreatedDateTime=DateTime.Now,PriceDecimal=1.5m,Id = GetGuidSed()},
                    },
                    new Product()
                    {
                      Name = new ProductName(){ Name ="Mała bułka",Img=ImgBread.Bulka,Id = GetGuidSed()},
                      Price = new ProductPrice(){CreatedDateTime=DateTime.Now,PriceDecimal=0.7m,Id = GetGuidSed()},
                    },
                    new Product()
                    {
                      Name = new ProductName(){ Name ="Bułka z serem",Img=ImgBread.Bulka,Id = GetGuidSed()},
                      Price = new ProductPrice(){CreatedDateTime=DateTime.Now,PriceDecimal=2.5m,Id = GetGuidSed()},
                    },
                    new Product()
                    {
                      Name = new ProductName(){ Name ="Drożdżówka",Img=ImgBuns.Ser,Id = GetGuidSed()},
                      Price = new ProductPrice(){CreatedDateTime=DateTime.Now,PriceDecimal=2.5m,Id = GetGuidSed()},
                    },
                    new Product()
                    {
                      Name = new ProductName(){ Name ="Bułka maślana",Img=ImgPath.Logo,Id = GetGuidSed()},
                      Price = new ProductPrice(){CreatedDateTime=DateTime.Now,PriceDecimal=1.5m,Id = GetGuidSed()},
                    },
                    new Product()
                    {
                      Name = new ProductName(){ Name ="Drożdżówki na wagę (opak.)",Img=ImgBuns.Ser,Id = GetGuidSed()},
                      Price = new ProductPrice(){CreatedDateTime=DateTime.Now,PriceDecimal=9m,Id = GetGuidSed()},
                    },
                    new Product()
                    {
                      Name = new ProductName(){ Name ="Kołacz",Img=ImgBuns.Ser,Id = GetGuidSed()},
                      Price = new ProductPrice(){CreatedDateTime=DateTime.Now,PriceDecimal=5m,Id = GetGuidSed()},
                    },
                    new Product()
                    {
                      Name = new ProductName(){ Name ="Chałka",Img=ImgPath.Logo,Id = GetGuidSed()},
                      Price = new ProductPrice(){CreatedDateTime=DateTime.Now,PriceDecimal=5m,Id = GetGuidSed()},
                    },
                    new Product()
                    {
                      Name = new ProductName(){ Name ="Ciastka francuskie",Img=ImgOther.KrucheZSerem,Id = GetGuidSed()},
                      Price = new ProductPrice(){CreatedDateTime=DateTime.Now,PriceDecimal=3m,Id = GetGuidSed()},
                    },
                    new Product()
                    {
                      Name = new ProductName(){ Name ="Ciastka (opak. 400g)",Img=ImgCookies.Czekolada,Id = GetGuidSed()},
                      Price = new ProductPrice(){CreatedDateTime=DateTime.Now,PriceDecimal=10m,Id = GetGuidSed()},
                    },
                    new Product()
                    {
                      Name = new ProductName(){ Name ="Piernik/Babka",Img=ImgPath.Logo,Id = GetGuidSed()},
                      Price = new ProductPrice(){CreatedDateTime=DateTime.Now,PriceDecimal=12m,Id = GetGuidSed()},
                    },
                    new Product()
                    {
                      Name = new ProductName(){ Name ="Makowiec",Img=ImgPath.Logo,Id = GetGuidSed()},
                      Price = new ProductPrice(){CreatedDateTime=DateTime.Now,PriceDecimal=14m,Id = GetGuidSed()},
                    },
                    new Product()
                    {
                      Name = new ProductName(){ Name ="Wafle (opak. 400g)",Img=ImgCookies.Andrut,Id = GetGuidSed()},
                      Price = new ProductPrice(){CreatedDateTime=DateTime.Now,PriceDecimal=10m,Id = GetGuidSed()},
                    },
                    new Product()
                    {
                      Name = new ProductName(){ Name ="Mini Pizza",Img=ImgPath.Logo,Id = GetGuidSed()},
                      Price = new ProductPrice(){CreatedDateTime=DateTime.Now,PriceDecimal=3m,Id = GetGuidSed()},
                    },
                    new Product()
                    {
                      Name = new ProductName(){ Name ="Bułka tarta",Img=ImgPath.Logo,Id = GetGuidSed()},
                      Price = new ProductPrice(){CreatedDateTime=DateTime.Now,PriceDecimal=3.5m,Id = GetGuidSed()},
                    },
                    new Product()
                    {
                      Name = new ProductName(){ Name ="Parówka w cieście",Img=ImgPath.Logo,Id = GetGuidSed()},
                      Price = new ProductPrice(){CreatedDateTime=DateTime.Now,PriceDecimal=3.5m,Id = GetGuidSed()},
                    },
                    new Product()
                    {
                      Name = new ProductName(){ Name ="Chleb suchy",Img="chleb.png", Id = GetGuidSed()},
                      Price = new ProductPrice(){CreatedDateTime=DateTime.Now,PriceDecimal=2m,Id = GetGuidSed()},
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
