using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

using DataBase.Data;

using DataBase.Model.EntitiesInventory;

using System.Collections.ObjectModel;

namespace Inventory.Pages.Products.ListProduct
{
    public partial class ListProductVM : ObservableObject
    {
        [ObservableProperty]
        ObservableCollection<ListProductM> productMs;

        [ObservableProperty]
        ListProductM dragAndDropProduct;

        [ObservableProperty]
        bool isGenerateDefaultEnable;

        readonly Random random = new(2137);
        readonly AccessDataBase _db;

        public Action<int, int, ScrollToPosition, bool> ScrollTo;
        public ListProductVM(AccessDataBase db)
        {
            ProductMs = [];
            this._db = db;
        }
        #region Method

        #region Async


        public async Task SelectAllProductsAsync()
        {
            try
            {
                ProductMs.Clear();
                var names = await _db.DataBaseAsync.Table<ProductName>().OrderBy(x => x.Arrangement).ToArrayAsync();

                for (int i = 0; i < names.Length; i++)
                {
                    ProductMs.Add(new ListProductM()
                    {
                        Name = names[i],
                    });
                    ProductMs[i].Prices = new(await SelectPricesAsync(names[i].Id));
                    ProductMs[i].SetActualPrice();
                }
                IsGenerateDefaultEnable = ProductMs.Count <= 0;
            }
            catch (Exception ex)
            {
                _db.SaveLog(ex);
            }
        }
        async Task<ProductPrice[]> SelectPricesAsync(Guid id)
        {
            var price = await _db.DataBaseAsync.Table<ProductPrice>().Where(x => x.ProductNameId == id).OrderByDescending(z => z.CreatedTicks).ToArrayAsync();
            return price;
        }

        #endregion

        #region Sync

        public void SelectAllProducts()
        {
            try
            {
                ProductMs.Clear();
                var names = _db.DataBase.Table<ProductName>().OrderBy(x => x.Arrangement).ToArray();

                for (int i = 0; i < names.Length; i++)
                {
                    ProductMs.Add(new ListProductM()
                    {
                        Name = names[i],
                    });
                    ProductMs[i].Prices = new(SelectPrices(names[i].Id));
                    ProductMs[i].SetActualPrice();
                }
                IsGenerateDefaultEnable = ProductMs.Count <= 0;
            }
            catch (Exception ex)
            {
                _db.SaveLog(ex);
            }
        }
        ProductPrice[] SelectPrices(Guid id)
        {
            var price = _db.DataBase.Table<ProductPrice>().Where(x => x.ProductNameId == id).OrderByDescending(z => z.CreatedTicks).ToArray();
            return price;
        }
        void OnScrollTo(int index, int groupIndex = -1, ScrollToPosition position = ScrollToPosition.MakeVisible, bool animate = true)
        {
            ScrollTo?.Invoke(index, groupIndex, position, animate);
        }
        async Task SetPositions(int index, ListProductM value)
        {
            var oldIndex = ProductMs.IndexOf(value);
            ProductMs.Move(oldIndex, index);

            await SetArrangement();
        }

        private async Task SetArrangement()
        {
            var task = new Task[ProductMs.Count];
            for (int i = 0; i < ProductMs.Count; i++)
            {
                int arrangement = i + 1;
                ProductMs[i].Name.Arrangement = arrangement;

                var entities = ProductMs[i].Name;
                entities.Updated = DateTime.Now;
                task[i] = _db.DataBaseAsync.UpdateAsync(entities);
            }
            await Task.WhenAll(task);
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
                        await _db.DataBaseAsync.DeleteAsync(value.Name);
                        for (int i = 0; i < value.Prices.Count; i++)
                        {
                            await _db.DataBaseAsync.DeleteAsync(value.Prices[i]);
                        }
                        ProductMs.Remove(value);
                        await Shell.Current.DisplayAlert(value.Name.Name, "Obiekt został usunięty", "Ok");
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
                var products = Shared.Data.InventoryTables.DefaultProducts;
                for (int i = 0; i < products.Length; i++)
                {
                    products[i].Name.Arrangement = i;
                    var id = _db.DataBase.Insert(products[i].Name);
                    var name = products[i].Name.Name;
                    products[i].Name = _db.DataBase.Table<ProductName>().FirstOrDefault(x => x.Name == name);
                    products[i].Price.ProductNameId = products[i].Name.Id;
                    _db.DataBase.Insert(products[i].Price);
                }
                await SelectAllProductsAsync();
            }
            catch (Exception ex)
            {
                _db.SaveLog(ex);
            }
        }


        [RelayCommand]
        async Task SetUp(ListProductM value)
        {
            var index = ProductMs.IndexOf(value);
            index += 1;
            if (index >= ProductMs.Count)
            {
                return;
            }
            await SetPositions(index, value);
        }
        [RelayCommand]
        async Task SetDown(ListProductM value)
        {
            var index = ProductMs.IndexOf(value);
            index -= 1;
            if (0 > index)
            {
                return;
            }
            await SetPositions(index, value);
        }


        [RelayCommand]
        void OnDrag(ListProductM value)
        {
            if (value is null)
                return;

            DragAndDropProduct = value;
        }

        [RelayCommand]
        void OnDropCompleted()
        {
            if (DragAndDropProduct is null)
                return;

            if (ProductMs.Count != _db.DataBase.Table<ProductName>().Count())
            {
                ProductMs.Clear();
                SelectAllProducts();
            }
        }

        [RelayCommand]
        async Task OnDrop(ListProductM value)
        {
            if (value is null)
                return;


            var index = ProductMs.IndexOf(value);

            await SetPositions(index, DragAndDropProduct);

            DragAndDropProduct = null;
        }
        #endregion


    }
}
