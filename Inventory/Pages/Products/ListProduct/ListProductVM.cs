using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

using DataBase.Data;
using DataBase.Data.Get;
using DataBase.Data.Save;
using DataBase.Model.EntitiesInventory;

using Shared.Data;
using Shared.Helper;

using System.Collections.ObjectModel;

namespace Inventory.Pages.Products.ListProduct
{
    public partial class ListProductVM : ObservableObject
    {
        private ObservableCollection<ListProductM> productMs;
        public ObservableCollection<ListProductM> ProductMs
        {
            get => productMs;
            set
            {
                if (SetProperty(ref productMs, value, nameof(ProductMs))) { }
            }
        }

        private ListProductM dragAndDropProduct;
        public ListProductM DragAndDropProduct
        {
            get => dragAndDropProduct;
            set
            {
                if (SetProperty(ref dragAndDropProduct, value, nameof(DragAndDropProduct))) { }
            }
        }

        private readonly IAccessDataBase _db;
        private readonly IGetInventoryAoT _get;
        private readonly ISaveInventoryAoT _save;

        public Action<int, int, ScrollToPosition, bool> ScrollTo;
        public ListProductVM(IAccessDataBase db, IGetInventoryAoT get, ISaveInventoryAoT save)
        {
            ProductMs = [];
            this._db = db;
            _get = get;
            _save = save;
        }
        public async Task SelectAllProductsAsync()
        {
            try
            {
                ProductMs.Clear();

                IList<(ProductName Name, IList<ProductPrice> Prices)> product = await _get.EmptyProductsNameAndPrices();

                for (int i = 0; i < product.Count; i++)
                {
                    ProductMs.Add(new ListProductM()
                    {
                        Name = product[i].Name,
                        Prices = [.. product[i].Prices]
                    });
                    ProductMs[i].SetActualPrice();
                }
            }
            catch (Exception ex)
            {
                _db.SaveLogExtension(ex);
            }
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
            for (int i = 0; i < ProductMs.Count; i++)
            {
                int arrangement = i + 1;
                ProductMs[i].Name.Arrangement = arrangement;
                await _save.SaveProductName(ProductMs[i].Name, UserAfterLogin.User.Id.ToByteArray());
            }
        }


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
                        await _db.SaveLogAsyncExtension(ex);
                        await Shell.Current.DisplayAlert("Error", ex.Message, "Ok");
                    }
                }
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
                await Shell.Current.GoToAsync($"{nameof(AddEdit.AddEditProductV)}?"
                    , new Dictionary<string, object>
                    {
                        [nameof(ListProductM)] = new ListProductM(),
                    });
            }
            catch (Exception ex)
            {
                _db.SaveLogExtension(ex);
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
                _db.SaveLogExtension(ex);
            }

        }


        [RelayCommand]
        async Task SetUp(ListProductM value)
        {
            try
            {
                var index = ProductMs.IndexOf(value);
                index += 1;
                if (index >= ProductMs.Count)
                {
                    return;
                }
                await SetPositions(index, value);
            }
            catch (Exception ex)
            {
                _db.SaveLogExtension(ex);
            }

        }
        [RelayCommand]
        async Task SetDown(ListProductM value)
        {
            try
            {


                var index = ProductMs.IndexOf(value);
                index -= 1;
                if (0 > index)
                {
                    return;
                }
                await SetPositions(index, value);
            }
            catch (Exception ex)
            {
                _db.SaveLogExtension(ex);
            }
        }


        [RelayCommand]
        void OnDrag(ListProductM value)
        {
            try
            {
                if (value is null)
                    return;

                DragAndDropProduct = value;
            }
            catch (Exception ex)
            {
                _db.SaveLogExtension(ex);
            }
        }

        [RelayCommand]
        async Task OnDrop(ListProductM value)
        {
            try
            {
                if (value is null)
                    return;

                var index = ProductMs.IndexOf(value);

                await SetPositions(index, DragAndDropProduct);

                DragAndDropProduct = null;
            }
            catch (Exception ex)
            {
                _db.SaveLogExtension(ex);
            }
        }

    }
}
