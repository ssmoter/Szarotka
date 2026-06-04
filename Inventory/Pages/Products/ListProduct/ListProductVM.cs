using CommunityToolkit.Maui.Alerts;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

using DataBase.Data;
using DataBase.Data.CheckUpdateDifferences;
using DataBase.Data.Get;
using DataBase.Data.Save;
using DataBase.Model.EntitiesInventory;
using DataBase.Model.EntitiesServer;
using DataBase.Model.JsonContext;
using DataBase.Service;

using Shared.CustomControls.FromCode;
using Shared.Data;
using Shared.Data.ServerHttpClients;
using Shared.Helper;
using Shared.Pages.UpdateDifference;

using System.Collections;
using System.Collections.ObjectModel;
using System.Net;

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
                if (SetProperty(ref productMs, value, nameof(ProductMs)))
                { }
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
        private readonly IUpdateLogService _updateLogService;
        private readonly IGetInventoryAoT _get;
        private readonly ISaveInventoryAoT _save;
        private readonly Data.InventoryApi.IGetProductHttp _getHttp;
        private readonly Data.InventoryApi.ISendProductHttp _sendHttp;

        public Action<int, int, ScrollToPosition, bool> ScrollTo;
        public ListProductVM(IAccessDataBase db,
                             IGetInventoryAoT get,
                             ISaveInventoryAoT save,
                             Data.InventoryApi.IGetProductHttp getHttp,
                             IUpdateLogService updateLogService,
                             Data.InventoryApi.ISendProductHttp sendHttp)
        {
            ProductMs = [];
            this._db = db;
            _get = get;
            _save = save;
            _getHttp = getHttp;
            _updateLogService = updateLogService;
            _sendHttp = sendHttp;
        }


        public async Task<bool> SelectAllProductsAsync()
        {
            try
            {
                ProductMs.Clear();
                RemovePropertyChange();

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
                SetPropertyChange();
            }
            catch (Exception ex)
            {
                _db.SaveLogExtension(ex);
            }
            return true;
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


        private void SetPropertyChange()
        {
            if (ProductMs is null)
            {
                return;
            }
            foreach (var item in ProductMs)
            {
                item.Name.PropertyChanged += Name_PropertyChanged;
            }
        }
        private void RemovePropertyChange()
        {
            if (ProductMs is null)
            {
                return;
            }
            foreach (var item in ProductMs)
            {
                item.Name.PropertyChanged -= Name_PropertyChanged;
            }
        }

        private async void Name_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(ProductName.IsVisible))
            {
                await UpdateProductName(sender as ProductName);
            }
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
        async Task UpdateProductName(ProductName value)
        {
            if (value is null)
            {
                return;
            }
            try
            {
                var user = UserAfterLogin.User.Id.ToByteArray();
                await _save.SaveProductName(value, user);
            }
            catch (Exception ex)
            {
                _db.SaveLogExtension(ex);
            }
        }

        [RelayCommand]
        async Task DeleteProduct(ListProductM value)
        {
            try
            {
                bool result = await Shell.Current.DisplayAlertAsync(value.Name.Name, "Czy na pewno chcesz usunąć?", "Tak", "Nie");
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
                        await Shell.Current.DisplayAlertAsync(value.Name.Name, "Obiekt został usunięty", "Ok");
                    }
                    catch (Exception ex)
                    {
                        await _db.SaveLogAsyncExtension(ex);
                        await Shell.Current.DisplayAlertAsync("Error", ex.Message, "Ok");
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




        async Task<HttpResponseMessage> SendData(EmptyProducts names, bool forceUpdate = false, CancellationTokenSource sourceToken = default)
        {
            await Toast.Make("Wysyłanie listy").Show();

            bool onConflict = sourceToken is not null;

            using var progress = UpdateProgressBar.CreatedUpdateProgressBar(
                            title: "Wysyłanie"
                            , description: onConflict ? "Anuluj wysyłanie" : ""
                            , icon: UpdateProgressBar.GetSyncImage()
                            , rotateIcon: true
                            , action:
                            onConflict ?
                            async () =>
                            {
                                sourceToken?.Cancel();
                                await Toast.Make("Anulowano wysyłani listy produktów").Show();
                                Shared.Pages.FlyoutHeader.FlyoutHeaderVM.OnCustomContent(null);

                            }
            : null);

            (HttpResponseMessage message, string content) resultMessage;
            if (sourceToken is not null)
            {
                resultMessage = await _sendHttp.SendProducts(names
                     , progress, forceUpdate: forceUpdate, token: sourceToken.Token);
            }
            else
            {
                resultMessage = await _sendHttp.SendProducts(names
                      , progress, forceUpdate: forceUpdate);
            }

            if (resultMessage.message.StatusCode == System.Net.HttpStatusCode.Created)
            {
                var updateJson = resultMessage.content;
                var update = System.Text.Json.JsonSerializer.Deserialize(
                    updateJson, SzarotkaJsonSerializerContext.Default.UpdateLog);

                await _updateLogService.Insert(update);

                await Toast.Make("Wysyłanie Zakończone bez komplikacji").Show();
            }

            return resultMessage.message;
        }

        [RelayCommand]
        async Task Send()
        {
            using var sourceToken = new CancellationTokenSource();
            try
            {
                await Toast.Make("Wczytywane wszystkich danych z danej trasy").Show();
                EmptyProducts empty = new(ProductMs.Select(x => new EmptyProduct
                {
                    Name = x.Name,
                    Prices = x.Prices,
                }));
                var resultMessage = await SendData(empty, false, sourceToken);

                if (resultMessage.StatusCode == HttpStatusCode.Conflict)
                {
                    await Toast.Make("Wysyłanie Zakończone. Pobierane są różnice do poprawy").Show();

                    using var progressDifference = UpdateProgressBar.CreatedUpdateProgressBar(
                            title: "Pobieranie różnic"
                            , description: "Brak możliwości anulowania"
                            , icon: UpdateProgressBar.GetSyncImage()
                            , rotateIcon: true
                            , action: null);

                    Shared.Pages.FlyoutHeader.FlyoutHeaderVM.OnCustomContent(progressDifference.Grid);

                    using var download = await resultMessage.Content.ReadAsStreamAsync();
                    var differenceJson = await HttpClientExtension.CheckProgress(
                        (progress) => UpdateProgressBar.UpdateProgress(progressDifference, progress)
                        , resultMessage.Content.Headers.ContentLength ?? 1
                        , download);

                    var exists = System.Text.Json.JsonSerializer.Deserialize(
                        differenceJson, SzarotkaJsonSerializerContext.Default.UpdateDifferenceArray);

                    if (exists.Length > 0)
                    {
                        var navigationParameter = new Dictionary<string, object>
                     {
                        { nameof(UpdateDifference), exists },
                        {nameof(Action),(Action<IEnumerable>)(async (names)
                        =>
                            {
                                foreach (ProductName item in names)
                                    {
                                        await _save.SaveProductName(item, item.UserUpdatedId.ToByteArray(), true);
                                        _ = await _updateLogService.Insert(new DataBase.Model.UpdateLog()
                                        {
                                            IsServer = true,
                                        }, item);
                                    }
                                 await SendData(new((IList<ProductName>)names), forceUpdate:true, sourceToken:null);
                                 Shared.Pages.FlyoutHeader.FlyoutHeaderVM.OnCustomContent(null);
                            })
                        }
                    };
                        await Shell.Current.GoToAsync(nameof(UpdateDifferenceV), navigationParameter);
                    }
                }
                resultMessage.EnsureSuccessStatusCode();
            }
            catch (HttpRequestException ex) when (ex.StatusCode == HttpStatusCode.Conflict)
            {
                //HttpStatusCode.Conflict został obłużony wyżej jako zwrot danych do poprawy przy edycji
            }
            catch (Exception ex)
            {
                _db.SaveLogExtension(ex);
            }
            finally
            {
                Shell.Current.FlyoutIsPresented = false;
                Shared.Pages.FlyoutHeader.FlyoutHeaderVM.OnCustomContent(null);
                sourceToken.Dispose();
            }
        }
        [RelayCommand]
        async Task Download()
        {
            await Toast.Make("Rozpoczęto Pobieranie").Show();
            using var sourceToken = new CancellationTokenSource();
            try
            {
                using var progress = UpdateProgressBar.CreatedUpdateProgressBar(
                                title: "Anuluj"
                                , description: "Pobieranie listy produktów"
                                , icon: UpdateProgressBar.GetSyncImage()
                                , rotateIcon: true
                                , action: async () =>
                                {
                                    sourceToken.Cancel();
                                    await Toast.Make("Anulowano pobieranie listy produktów").Show();
                                    Shared.Pages.FlyoutHeader.FlyoutHeaderVM.OnCustomContent(null);
                                });
                var result = await _getHttp.GetProducts(progress, sourceToken.Token);
                await Toast.Make("Pobieranie zakończone").Show();
                await Toast.Make("Rozpoczęto zapisywanie").Show();

                IList<UpdateDifference> exists = [];

                foreach (EmptyProduct item in result.Products)
                {
                    (bool canUpdate, var isExist) = await ModelsDifferences.Check(_get, item.Name, false);
                    if (canUpdate)
                    {
                        await _save.SaveProductName(item.Name, item.Name.UserUpdatedId.ToByteArray(), true);       
                        var log = await _updateLogService.Insert(new DataBase.Model.UpdateLog()
                        {
                            IsServer = false,
                        }, item.Name);

                        foreach (var price in item.Prices)
                        {
                            (bool canUpdatePrice, var isExistPrice) = await ModelsDifferences.Check(_get, price, false);
                            if (canUpdate)
                            {
                                await _save.SaveProductPrice(price, price.UserUpdatedId.ToByteArray(), true);
                            }
                        }
                    }
                    if (!canUpdate)
                    {
                        exists.Add(new UpdateDifference()
                        {
                            Update = isExist!,
                            Server = item.Name
                        });
                    }
                }
                await Toast.Make("Zapisywanie zakończone").Show();
                if (exists.Count > 0)
                {
                    await Toast.Make("Popraw różnice").Show();
                    var navigationParameter = new Dictionary<string, object>
                     {
                        { nameof(UpdateDifference), exists },
                        {nameof(Action),(Action<IEnumerable>)(async (customer)
                        =>
                            {
                                foreach (ProductName item in customer)
                                    {
                                        await _save.SaveProductName(item, item.UserUpdatedId.ToByteArray(), true);
                                        _ = await _updateLogService.Insert(new DataBase.Model.UpdateLog()
                                        {
                                            IsServer = false,
                                        }, item);
                                    }
                                 await SendData(new((IList<ProductName>)customer), forceUpdate:true,sourceToken:null);
                                 Shared.Pages.FlyoutHeader.FlyoutHeaderVM.OnCustomContent(null);
                            })
                        }
                    };

                    await Shell.Current.GoToAsync(nameof(UpdateDifferenceV), navigationParameter);
                }
                Shell.Current.FlyoutIsPresented = false;
                await SelectAllProductsAsync();
            }
            catch (HttpRequestException ex) when (ex.StatusCode == HttpStatusCode.Conflict)
            {
                //HttpStatusCode.Conflict został obłużony wyżej jako zwrot danych do poprawy przy edycji
            }
            catch (Exception ex)
            {
                _db.SaveLogExtension(ex);
            }
            finally
            {
                Shared.Pages.FlyoutHeader.FlyoutHeaderVM.OnCustomContent(null);
                sourceToken.Dispose();
            }
        }

    }
}

