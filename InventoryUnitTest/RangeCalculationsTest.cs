using DataBase.Helper;
using DataBase.Model.EntitiesInventory;

using FluentAssertions;

using Inventory.Pages.RangeDay;

using System.Collections.ObjectModel;


namespace InventoryUnitTest
{
    public class RangeCalculationsTest
    {
        public static IEnumerable<object[]> TestSumTotalOfRangeValue =>
        [
             [
                new RangeDayM[]
                {
                    new()
                        {
                         Driver=new(){Id=new("ae86aaeb-1080-4ae2-9226-62cf3a042ca8")},
                         Day = new()
                            {
                               SelectedDateTicks=DateTime.Now.AddDays(0).Ticks,
                               DriverGuid=new("ae86aaeb-1080-4ae2-9226-62cf3a042ca8"),
                               TotalPriceProducts = 100,
                               TotalPriceCake = 100,
                               TotalPrice = 200,
                               TotalPriceCorrect = 0,
                               TotalPriceAfterCorrect=200,
                               TotalPriceMoney = 300,
                               TotalPriceDifference =100,

                               Products = new ObservableCollection<Product>()
                                    {
                                        new Product()
                                        {
                                            Number = 1,
                                            NumberEdit = 1,
                                            NumberReturn = 1,
                                            PriceTotal = 100,
                                            PriceTotalCorrect = 100,
                                            PriceTotalAfterCorrect = 100,
                                            ProductNameId = new("d2034a6a-3a0d-9b49-05a3-bc49adfc329a"),
                                            Name = new(){Id=new("d2034a6a-3a0d-9b49-05a3-bc49adfc329a")},
                                        },

                                    },
                            },
                        },
                    new()
                        {
                         Driver=new(){Id=new("ae86aaeb-1080-4ae2-9226-62cf3a042ca8")},
                         Day = new()
                            {
                               SelectedDateTicks=DateTime.Now.AddDays(0).Ticks,
                               DriverGuid=new("ae86aaeb-1080-4ae2-9226-62cf3a042ca8"),
                               TotalPriceProducts = 100,
                               TotalPriceCake = 100,
                               TotalPrice = 200,
                               TotalPriceCorrect = 0,
                               TotalPriceAfterCorrect=200,
                               TotalPriceMoney = 300,
                               TotalPriceDifference =100,

                               Products = new ObservableCollection<Product>()
                                    {
                                        new Product()
                                        {
                                            Number = 1,
                                            NumberEdit = 1,
                                            NumberReturn = 1,
                                            PriceTotal = 100,
                                            PriceTotalCorrect = 100,
                                            PriceTotalAfterCorrect = 100,
                                            ProductNameId = new("d2034a6a-3a0d-9b49-05a3-bc49adfc329a"),
                                            Name = new(){Id=new("d2034a6a-3a0d-9b49-05a3-bc49adfc329a")},
                                        },

                                    },
                            },
                        },
                    new()
                        {
                         Driver=new(){Id=new("ae86aaeb-1080-4ae2-9226-62cf3a042ca8")},
                         Day = new()
                            {
                               SelectedDateTicks=DateTime.Now.AddDays(0).Ticks,
                               DriverGuid=new("ae86aaeb-1080-4ae2-9226-62cf3a042ca8"),
                               TotalPriceProducts = 100,
                               TotalPriceCake = 100,
                               TotalPrice = 200,
                               TotalPriceCorrect = 0,
                               TotalPriceAfterCorrect=200,
                               TotalPriceMoney = 300,
                               TotalPriceDifference =100,

                               Products = new ObservableCollection<Product>()
                                    {
                                        new Product()
                                        {
                                            Number = 1,
                                            NumberEdit = 1,
                                            NumberReturn = 1,
                                            PriceTotal = 100,
                                            PriceTotalCorrect = 100,
                                            PriceTotalAfterCorrect = 100,
                                            ProductNameId = new("d2034a6a-3a0d-9b49-05a3-bc49adfc329a"),
                                            Name = new(){Id=new("d2034a6a-3a0d-9b49-05a3-bc49adfc329a")},
                                        },

                                    },
                            },
                        },
                },
                new RangeDayM[]
                 {
                        new()
                        {
                         Driver=new(){Id=new("ae86aaeb-1080-4ae2-9226-62cf3a042ca8")},
                         Day = new()
                            {
                               DriverGuid=new("ae86aaeb-1080-4ae2-9226-62cf3a042ca8"),
                               TotalPriceProducts = 100*3,
                               TotalPriceCake = 100*3,
                               TotalPrice = 200*3,
                               TotalPriceCorrect = 0,
                               TotalPriceAfterCorrect=200*3,
                               TotalPriceMoney = 300*3,
                               TotalPriceDifference =100*3,
                               Products = new ObservableCollection<Product>()
                                    {
                                        new Product()
                                        {
                                            Number = 1*3,
                                            NumberEdit = 1*3,
                                            NumberReturn = 1*3,
                                            PriceTotal = 100*3,
                                            PriceTotalCorrect = 100*3,
                                            PriceTotalAfterCorrect = 100*3,
                                            ProductNameId = new("d2034a6a-3a0d-9b49-05a3-bc49adfc329a"),
                                            Name = new(){Id=new("d2034a6a-3a0d-9b49-05a3-bc49adfc329a")},
                                        },

                                    },
                            },
                        },
                 }

              ]
        ];


        [Theory]
        [MemberData(nameof(TestSumTotalOfRangeValue))]
        public void TestSumTotalOfRange(RangeDayM[] request, RangeDayM[] result)
        {

            var obj = Inventory.Helper.RangeCalculations.SumTotalOfRangeCalculateAverages(request).ToArray();

            var objJson = Newtonsoft.Json.JsonConvert.SerializeObject(obj);
            var resultJson = Newtonsoft.Json.JsonConvert.SerializeObject(result);

            objJson.Should().Be(resultJson);

        }



        public static IEnumerable<object[]> TestSumTotalOfRangeTestSumTotalOfRangeValueCalculateAveragesValue =>
[
             [
                new RangeDayM[]
                {
                    new()
                        {
                         Driver=new(){Id=new("ae86aaeb-1080-4ae2-9226-62cf3a042ca8")},
                         Day = new()
                            {
                               SelectedDateTicks=DateTime.Now.AddDays(0).Ticks,
                               DriverGuid=new("ae86aaeb-1080-4ae2-9226-62cf3a042ca8"),
                               TotalPriceProducts = 100,
                               TotalPriceCake = 100,
                               TotalPrice = 200,
                               TotalPriceCorrect = 0,
                               TotalPriceAfterCorrect=200,
                               TotalPriceMoney = 300,
                               TotalPriceDifference =100,

                               Products = new ObservableCollection<Product>()
                                    {
                                        new Product()
                                        {
                                            Number = 1,
                                            NumberEdit = 1,
                                            NumberReturn = 1,
                                            PriceTotal = 100,
                                            PriceTotalCorrect = 100,
                                            PriceTotalAfterCorrect = 100,
                                            ProductNameId = new("d2034a6a-3a0d-9b49-05a3-bc49adfc329a"),
                                            Name = new(){Id=new("d2034a6a-3a0d-9b49-05a3-bc49adfc329a")},
                                        },
                                        new Product()
                                        {
                                            Number = 2,
                                            NumberEdit = 2,
                                            NumberReturn = 2,
                                            PriceTotal = 200,
                                            PriceTotalCorrect = 200,
                                            PriceTotalAfterCorrect = 200,
                                            ProductNameId = new("24ebc827-bb4e-6fac-4eb4-8f88f524eaf1"),
                                            Name = new(){Id=new("24ebc827-bb4e-6fac-4eb4-8f88f524eaf1")},
                                        },
                                        new Product()
                                        {
                                            Number = 3,
                                            NumberEdit = 3,
                                            NumberReturn = 3,
                                            PriceTotal = 300,
                                            PriceTotalCorrect = 300,
                                            PriceTotalAfterCorrect = 300,
                                            ProductNameId = new("e5226138-0e10-9f4c-cd7b-b91b87ef9f24"),
                                            Name = new(){Id=new("e5226138-0e10-9f4c-cd7b-b91b87ef9f24")},
                                        },
                                    },
                            },
                        },
                    new()
                        {
                         Driver=new(){Id=new("ae86aaeb-1080-4ae2-9226-62cf3a042ca8")},
                         Day = new()
                            {
                               SelectedDateTicks=DateTime.Now.AddDays(0).Ticks,
                               DriverGuid=new("ae86aaeb-1080-4ae2-9226-62cf3a042ca8"),
                               TotalPriceProducts = 100,
                               TotalPriceCake = 100,
                               TotalPrice = 200,
                               TotalPriceCorrect = 0,
                               TotalPriceAfterCorrect=200,
                               TotalPriceMoney = 300,
                               TotalPriceDifference =100,

                               Products = new ObservableCollection<Product>()
                                    {
                                        new Product()
                                        {
                                            Number = 1,
                                            NumberEdit = 1,
                                            NumberReturn = 1,
                                            PriceTotal = 100,
                                            PriceTotalCorrect = 100,
                                            PriceTotalAfterCorrect = 100,
                                            ProductNameId = new("d2034a6a-3a0d-9b49-05a3-bc49adfc329a"),
                                            Name = new(){Id=new("d2034a6a-3a0d-9b49-05a3-bc49adfc329a")},
                                        },
                                        new Product()
                                        {
                                            Number = 2,
                                            NumberEdit = 2,
                                            NumberReturn = 2,
                                            PriceTotal = 200,
                                            PriceTotalCorrect = 200,
                                            PriceTotalAfterCorrect = 200,
                                            ProductNameId = new("24ebc827-bb4e-6fac-4eb4-8f88f524eaf1"),
                                            Name = new(){Id=new("24ebc827-bb4e-6fac-4eb4-8f88f524eaf1")},
                                        },
                                        new Product()
                                        {
                                            Number = 3,
                                            NumberEdit = 3,
                                            NumberReturn = 3,
                                            PriceTotal = 300,
                                            PriceTotalCorrect = 300,
                                            PriceTotalAfterCorrect = 300,
                                            ProductNameId = new("e5226138-0e10-9f4c-cd7b-b91b87ef9f24"),
                                            Name = new(){Id=new("e5226138-0e10-9f4c-cd7b-b91b87ef9f24")},
                                        },
                                        new Product()
                                        {
                                            Number = 3,
                                            NumberEdit = 3,
                                            NumberReturn = 3,
                                            PriceTotal = 300,
                                            PriceTotalCorrect = 300,
                                            PriceTotalAfterCorrect = 300,
                                            ProductNameId = new("61b42773-0b9f-aa21-fe8a-8e19335fe799"),
                                            Name = new(){Id=new("61b42773-0b9f-aa21-fe8a-8e19335fe799")},
                                        },
                                    },
                            },
                        },
                    new()
                        {
                         Driver=new(){Id=new("ae86aaeb-1080-4ae2-9226-62cf3a042ca8")},
                         Day = new()
                            {
                               SelectedDateTicks=DateTime.Now.AddDays(0).Ticks,
                               DriverGuid=new("ae86aaeb-1080-4ae2-9226-62cf3a042ca8"),
                               TotalPriceProducts = 100,
                               TotalPriceCake = 100,
                               TotalPrice = 200,
                               TotalPriceCorrect = 0,
                               TotalPriceAfterCorrect=200,
                               TotalPriceMoney = 300,
                               TotalPriceDifference =100,

                               Products = new ObservableCollection<Product>()
                                    {
                                        new Product()
                                        {
                                            Number = 1,
                                            NumberEdit = 1,
                                            NumberReturn = 1,
                                            PriceTotal = 100,
                                            PriceTotalCorrect = 100,
                                            PriceTotalAfterCorrect = 100,
                                            ProductNameId = new("d2034a6a-3a0d-9b49-05a3-bc49adfc329a"),
                                            Name = new(){Id=new("d2034a6a-3a0d-9b49-05a3-bc49adfc329a")},
                                        },
                                        new Product()
                                        {
                                            Number = 2,
                                            NumberEdit = 2,
                                            NumberReturn = 2,
                                            PriceTotal = 200,
                                            PriceTotalCorrect = 200,
                                            PriceTotalAfterCorrect = 200,
                                            ProductNameId = new("24ebc827-bb4e-6fac-4eb4-8f88f524eaf1"),
                                            Name = new(){Id=new("24ebc827-bb4e-6fac-4eb4-8f88f524eaf1")},
                                        },
                                        new Product()
                                        {
                                            Number = 3,
                                            NumberEdit = 3,
                                            NumberReturn = 3,
                                            PriceTotal = 300,
                                            PriceTotalCorrect = 300,
                                            PriceTotalAfterCorrect = 300,
                                            ProductNameId = new("e5226138-0e10-9f4c-cd7b-b91b87ef9f24"),
                                            Name = new(){Id=new("e5226138-0e10-9f4c-cd7b-b91b87ef9f24")},
                                        },
                                        new Product()
                                        {
                                            Number = 0,
                                            NumberEdit = 0,
                                            NumberReturn = 0,
                                            PriceTotal = 0,
                                            PriceTotalCorrect = 0,
                                            PriceTotalAfterCorrect = 0,
                                            ProductNameId = new("61b42773-0b9f-aa21-fe8a-8e19335fe799"),
                                            Name = new(){Id=new("61b42773-0b9f-aa21-fe8a-8e19335fe799")},
                                        },
                                    },
                            },
                        },
                },
                new RangeDayM[]
                 {
                        new()
                        {
                         Driver=new(){Id=new("ae86aaeb-1080-4ae2-9226-62cf3a042ca8")},
                         Day = new()
                            {
                               DriverGuid=new("ae86aaeb-1080-4ae2-9226-62cf3a042ca8"),
                               TotalPriceProducts = 100,
                               TotalPriceCake = 100,
                               TotalPrice = 200,
                               TotalPriceCorrect = 0,
                               TotalPriceAfterCorrect=200,
                               TotalPriceMoney = 300,
                               TotalPriceDifference =100,
                               Products = new ObservableCollection<Product>()
                                    {
                                        new Product()
                                        {
                                            Number = 1,
                                            NumberEdit = 1,
                                            NumberReturn = 1,
                                            PriceTotal = 100,
                                            PriceTotalCorrect = 100,
                                            PriceTotalAfterCorrect = 100,
                                            ProductNameId = new("d2034a6a-3a0d-9b49-05a3-bc49adfc329a"),
                                            Name = new(){Id=new("d2034a6a-3a0d-9b49-05a3-bc49adfc329a")},
                                        },
                                        new Product()
                                        {
                                            Number = 2,
                                            NumberEdit = 2,
                                            NumberReturn = 2,
                                            PriceTotal = 200,
                                            PriceTotalCorrect = 200,
                                            PriceTotalAfterCorrect = 200,
                                            ProductNameId = new("24ebc827-bb4e-6fac-4eb4-8f88f524eaf1"),
                                            Name = new(){Id=new("24ebc827-bb4e-6fac-4eb4-8f88f524eaf1")},
                                        },
                                        new Product()
                                        {
                                            Number = 3,
                                            NumberEdit = 3,
                                            NumberReturn = 3,
                                            PriceTotal = 300,
                                            PriceTotalCorrect = 300,
                                            PriceTotalAfterCorrect = 300,
                                            ProductNameId = new("e5226138-0e10-9f4c-cd7b-b91b87ef9f24"),
                                            Name = new(){Id=new("e5226138-0e10-9f4c-cd7b-b91b87ef9f24")},
                                        },
                                        new Product()
                                        {
                                            Number = 3,
                                            NumberEdit = 3,
                                            NumberReturn = 3,
                                            PriceTotal = 300,
                                            PriceTotalCorrect = 300,
                                            PriceTotalAfterCorrect = 300,
                                            ProductNameId = new("61b42773-0b9f-aa21-fe8a-8e19335fe799"),
                                            Name = new(){Id=new("61b42773-0b9f-aa21-fe8a-8e19335fe799")},
                                        },
                                    },
                            },
                        },
                 }
              ],

             [
                new RangeDayM[]
                {
                    new()
                        {
                         Driver=new(){Id=new("ae86aaeb-1080-4ae2-9226-62cf3a042ca8")},
                         Day = new()
                            {
                               SelectedDateTicks=DateTime.Now.AddDays(0).Ticks,
                               DriverGuid=new("ae86aaeb-1080-4ae2-9226-62cf3a042ca8"),
                               TotalPriceProducts = 100,
                               TotalPriceCake = 100,
                               TotalPrice = 200,
                               TotalPriceCorrect = 0,
                               TotalPriceAfterCorrect=200,
                               TotalPriceMoney = 300,
                               TotalPriceDifference =100,

                               Products = new ObservableCollection<Product>()
                                    {
                                        new Product()
                                        {
                                            Number = 1,
                                            NumberEdit = 1,
                                            NumberReturn = 1,
                                            PriceTotal = 100,
                                            PriceTotalCorrect = 100,
                                            PriceTotalAfterCorrect = 100,
                                            ProductNameId = new("d2034a6a-3a0d-9b49-05a3-bc49adfc329a"),
                                            Name = new(){Id=new("d2034a6a-3a0d-9b49-05a3-bc49adfc329a")},
                                        },
                                        new Product()
                                        {
                                            Number = 2,
                                            NumberEdit = 2,
                                            NumberReturn = 2,
                                            PriceTotal = 200,
                                            PriceTotalCorrect = 200,
                                            PriceTotalAfterCorrect = 200,
                                            ProductNameId = new("24ebc827-bb4e-6fac-4eb4-8f88f524eaf1"),
                                            Name = new(){Id=new("24ebc827-bb4e-6fac-4eb4-8f88f524eaf1")},
                                        },
                                        new Product()
                                        {
                                            Number = 3,
                                            NumberEdit = 3,
                                            NumberReturn = 3,
                                            PriceTotal = 300,
                                            PriceTotalCorrect = 300,
                                            PriceTotalAfterCorrect = 300,
                                            ProductNameId = new("e5226138-0e10-9f4c-cd7b-b91b87ef9f24"),
                                            Name = new(){Id=new("e5226138-0e10-9f4c-cd7b-b91b87ef9f24")},
                                        },
                                    },
                            },
                        },
                    new()
                        {
                         Driver=new(){Id=new("ae86aaeb-1080-4ae2-9226-62cf3a042ca8")},
                         Day = new()
                            {
                               SelectedDateTicks=DateTime.Now.AddDays(0).Ticks,
                               DriverGuid=new("ae86aaeb-1080-4ae2-9226-62cf3a042ca8"),
                               TotalPriceProducts = 100,
                               TotalPriceCake = 100,
                               TotalPrice = 200,
                               TotalPriceCorrect = 0,
                               TotalPriceAfterCorrect=200,
                               TotalPriceMoney = 300,
                               TotalPriceDifference =100,

                               Products = new ObservableCollection<Product>()
                                    {
                                        new Product()
                                        {
                                            Number = 1,
                                            NumberEdit = 1,
                                            NumberReturn = 1,
                                            PriceTotal = 100,
                                            PriceTotalCorrect = 100,
                                            PriceTotalAfterCorrect = 100,
                                            ProductNameId = new("d2034a6a-3a0d-9b49-05a3-bc49adfc329a"),
                                            Name = new(){Id=new("d2034a6a-3a0d-9b49-05a3-bc49adfc329a")},
                                        },
                                        new Product()
                                        {
                                            Number = 2,
                                            NumberEdit = 2,
                                            NumberReturn = 2,
                                            PriceTotal = 200,
                                            PriceTotalCorrect = 200,
                                            PriceTotalAfterCorrect = 200,
                                            ProductNameId = new("24ebc827-bb4e-6fac-4eb4-8f88f524eaf1"),
                                            Name = new(){Id=new("24ebc827-bb4e-6fac-4eb4-8f88f524eaf1")},
                                        },
                                        new Product()
                                        {
                                            Number = 3,
                                            NumberEdit = 3,
                                            NumberReturn = 3,
                                            PriceTotal = 300,
                                            PriceTotalCorrect = 300,
                                            PriceTotalAfterCorrect = 300,
                                            ProductNameId = new("e5226138-0e10-9f4c-cd7b-b91b87ef9f24"),
                                            Name = new(){Id=new("e5226138-0e10-9f4c-cd7b-b91b87ef9f24")},
                                        },
                                        new Product()
                                        {
                                            Number = 3,
                                            NumberEdit = 3,
                                            NumberReturn = 3,
                                            PriceTotal = 300,
                                            PriceTotalCorrect = 300,
                                            PriceTotalAfterCorrect = 300,
                                            ProductNameId = new("61b42773-0b9f-aa21-fe8a-8e19335fe799"),
                                            Name = new(){Id=new("61b42773-0b9f-aa21-fe8a-8e19335fe799")},
                                        },
                                    },
                            },
                        },
                    new()
                        {
                         Driver=new(){Id=new("ae86aaeb-1080-4ae2-9226-62cf3a042ca8")},
                         Day = new()
                            {
                               SelectedDateTicks=DateTime.Now.AddDays(0).Ticks,
                               DriverGuid=new("ae86aaeb-1080-4ae2-9226-62cf3a042ca8"),
                               TotalPriceProducts = 100,
                               TotalPriceCake = 100,
                               TotalPrice = 200,
                               TotalPriceCorrect = 0,
                               TotalPriceAfterCorrect=200,
                               TotalPriceMoney = 300,
                               TotalPriceDifference =100,

                               Products = new ObservableCollection<Product>()
                                    {
                                        new Product()
                                        {
                                            Number = 1,
                                            NumberEdit = 1,
                                            NumberReturn = 1,
                                            PriceTotal = 100,
                                            PriceTotalCorrect = 100,
                                            PriceTotalAfterCorrect = 100,
                                            ProductNameId = new("d2034a6a-3a0d-9b49-05a3-bc49adfc329a"),
                                            Name = new(){Id=new("d2034a6a-3a0d-9b49-05a3-bc49adfc329a")},
                                        },
                                        new Product()
                                        {
                                            Number = 2,
                                            NumberEdit = 2,
                                            NumberReturn = 2,
                                            PriceTotal = 200,
                                            PriceTotalCorrect = 200,
                                            PriceTotalAfterCorrect = 200,
                                            ProductNameId = new("24ebc827-bb4e-6fac-4eb4-8f88f524eaf1"),
                                            Name = new(){Id=new("24ebc827-bb4e-6fac-4eb4-8f88f524eaf1")},
                                        },
                                        new Product()
                                        {
                                            Number = 3,
                                            NumberEdit = 3,
                                            NumberReturn = 3,
                                            PriceTotal = 300,
                                            PriceTotalCorrect = 300,
                                            PriceTotalAfterCorrect = 300,
                                            ProductNameId = new("e5226138-0e10-9f4c-cd7b-b91b87ef9f24"),
                                            Name = new(){Id=new("e5226138-0e10-9f4c-cd7b-b91b87ef9f24")},
                                        },
                                        new Product()
                                        {
                                            Number = 0,
                                            NumberEdit = 0,
                                            NumberReturn = 0,
                                            PriceTotal = 0,
                                            PriceTotalCorrect = 0,
                                            PriceTotalAfterCorrect = 0,
                                            ProductNameId = new("61b42773-0b9f-aa21-fe8a-8e19335fe799"),
                                            Name = new(){Id=new("61b42773-0b9f-aa21-fe8a-8e19335fe799")},
                                        },
                                    },
                            },
                        },
                },
                new RangeDayM[]
                 {
                    new()
                        {
                         Driver=new(){Id=new("ae86aaeb-1080-4ae2-9226-62cf3a042ca8")},
                         Day = new()
                            {
                               DriverGuid=new("ae86aaeb-1080-4ae2-9226-62cf3a042ca8"),
                               TotalPriceProducts = 100,
                               TotalPriceCake = 100,
                               TotalPrice = 200,
                               TotalPriceCorrect = 0,
                               TotalPriceAfterCorrect=200,
                               TotalPriceMoney = 300,
                               TotalPriceDifference =100,
                               Products = new ObservableCollection<Product>()
                                    {
                                        new Product()
                                        {
                                            Number = 1,
                                            NumberEdit = 1,
                                            NumberReturn = 1,
                                            PriceTotal = 100,
                                            PriceTotalCorrect = 100,
                                            PriceTotalAfterCorrect = 100,
                                            ProductNameId = new("d2034a6a-3a0d-9b49-05a3-bc49adfc329a"),
                                            Name = new(){Id=new("d2034a6a-3a0d-9b49-05a3-bc49adfc329a")},
                                        },
                                        new Product()
                                        {
                                            Number = 2,
                                            NumberEdit = 2,
                                            NumberReturn = 2,
                                            PriceTotal = 200,
                                            PriceTotalCorrect = 200,
                                            PriceTotalAfterCorrect = 200,
                                            ProductNameId = new("24ebc827-bb4e-6fac-4eb4-8f88f524eaf1"),
                                            Name = new(){Id=new("24ebc827-bb4e-6fac-4eb4-8f88f524eaf1")},
                                        },
                                        new Product()
                                        {
                                            Number = 3,
                                            NumberEdit = 3,
                                            NumberReturn = 3,
                                            PriceTotal = 300,
                                            PriceTotalCorrect = 300,
                                            PriceTotalAfterCorrect = 300,
                                            ProductNameId = new("e5226138-0e10-9f4c-cd7b-b91b87ef9f24"),
                                            Name = new(){Id=new("e5226138-0e10-9f4c-cd7b-b91b87ef9f24")},
                                        },
                                        new Product()
                                        {
                                            Number = 3,
                                            NumberEdit = 3,
                                            NumberReturn = 3,
                                            PriceTotal = 300,
                                            PriceTotalCorrect = 300,
                                            PriceTotalAfterCorrect = 300,
                                            ProductNameId = new("61b42773-0b9f-aa21-fe8a-8e19335fe799"),
                                            Name = new(){Id=new("61b42773-0b9f-aa21-fe8a-8e19335fe799")},
                                        },
                                    },
                            },
                        },
                 }

              ]


];

        [Theory]
        [MemberData(nameof(TestSumTotalOfRangeTestSumTotalOfRangeValueCalculateAveragesValue))]
        public void TestSumTotalOfRangeCalculateAverages(RangeDayM[] request, RangeDayM[] result)
        {

            var obj = Inventory.Helper.RangeCalculations.SumTotalOfRangeCalculateAverages(request, true).ToArray();

            var objJson = Newtonsoft.Json.JsonConvert.SerializeObject(obj);
            var resultJson = Newtonsoft.Json.JsonConvert.SerializeObject(result);

            objJson.Should().Be(resultJson);

        }


        public static IEnumerable<object[]> TestSumDayOfWeekValue =>
[
[
                new RangeDayM[]
                {
                    new()
                        {
                         Driver=new(){Id=new("ae86aaeb-1080-4ae2-9226-62cf3a042ca8")},
                         Day = new()
                            {
                               SelectedDateTicks=DateTime.Now.AddDays(0).Ticks,
                               DriverGuid=new("ae86aaeb-1080-4ae2-9226-62cf3a042ca8"),
                               TotalPriceProducts = 100,
                               TotalPriceCake = 100,
                               TotalPrice = 200,
                               TotalPriceCorrect = 0,
                               TotalPriceAfterCorrect=200,
                               TotalPriceMoney = 300,
                               TotalPriceDifference =100,

                               Products = new ObservableCollection<Product>()
                                    {
                                        new Product()
                                        {
                                            Number = 1,
                                            NumberEdit = 1,
                                            NumberReturn = 1,
                                            PriceTotal = 100,
                                            PriceTotalCorrect = 100,
                                            PriceTotalAfterCorrect = 100,
                                            ProductNameId = new("d2034a6a-3a0d-9b49-05a3-bc49adfc329a"),
                                            Name = new(){Id=new("d2034a6a-3a0d-9b49-05a3-bc49adfc329a")},
                                        },
                                        new Product()
                                        {
                                            Number = 2,
                                            NumberEdit = 2,
                                            NumberReturn = 2,
                                            PriceTotal = 200,
                                            PriceTotalCorrect = 200,
                                            PriceTotalAfterCorrect = 200,
                                            ProductNameId = new("24ebc827-bb4e-6fac-4eb4-8f88f524eaf1"),
                                            Name = new(){Id=new("24ebc827-bb4e-6fac-4eb4-8f88f524eaf1")},
                                        },
                                        new Product()
                                        {
                                            Number = 3,
                                            NumberEdit = 3,
                                            NumberReturn = 3,
                                            PriceTotal = 300,
                                            PriceTotalCorrect = 300,
                                            PriceTotalAfterCorrect = 300,
                                            ProductNameId = new("e5226138-0e10-9f4c-cd7b-b91b87ef9f24"),
                                            Name = new(){Id=new("e5226138-0e10-9f4c-cd7b-b91b87ef9f24")},
                                        },
                                    },
                            },
                        },
                    new()
                        {
                         Driver=new(){Id=new("ae86aaeb-1080-4ae2-9226-62cf3a042ca8")},
                         Day = new()
                            {
                               SelectedDateTicks=DateTime.Now.AddDays(7).Ticks,
                               DriverGuid=new("ae86aaeb-1080-4ae2-9226-62cf3a042ca8"),
                               TotalPriceProducts = 100,
                               TotalPriceCake = 100,
                               TotalPrice = 200,
                               TotalPriceCorrect = 0,
                               TotalPriceAfterCorrect=200,
                               TotalPriceMoney = 300,
                               TotalPriceDifference =100,

                               Products = new ObservableCollection<Product>()
                                    {
                                        new Product()
                                        {
                                            Number = 1,
                                            NumberEdit = 1,
                                            NumberReturn = 1,
                                            PriceTotal = 100,
                                            PriceTotalCorrect = 100,
                                            PriceTotalAfterCorrect = 100,
                                            ProductNameId = new("d2034a6a-3a0d-9b49-05a3-bc49adfc329a"),
                                            Name = new(){Id=new("d2034a6a-3a0d-9b49-05a3-bc49adfc329a")},
                                        },
                                        new Product()
                                        {
                                            Number = 2,
                                            NumberEdit = 2,
                                            NumberReturn = 2,
                                            PriceTotal = 200,
                                            PriceTotalCorrect = 200,
                                            PriceTotalAfterCorrect = 200,
                                            ProductNameId = new("24ebc827-bb4e-6fac-4eb4-8f88f524eaf1"),
                                            Name = new(){Id=new("24ebc827-bb4e-6fac-4eb4-8f88f524eaf1")},
                                        },
                                        new Product()
                                        {
                                            Number = 3,
                                            NumberEdit = 3,
                                            NumberReturn = 3,
                                            PriceTotal = 300,
                                            PriceTotalCorrect = 300,
                                            PriceTotalAfterCorrect = 300,
                                            ProductNameId = new("e5226138-0e10-9f4c-cd7b-b91b87ef9f24"),
                                            Name = new(){Id=new("e5226138-0e10-9f4c-cd7b-b91b87ef9f24")},
                                        },
                                        new Product()
                                        {
                                            Number = 3,
                                            NumberEdit = 3,
                                            NumberReturn = 3,
                                            PriceTotal = 300,
                                            PriceTotalCorrect = 300,
                                            PriceTotalAfterCorrect = 300,
                                            ProductNameId = new("61b42773-0b9f-aa21-fe8a-8e19335fe799"),
                                            Name = new(){Id=new("61b42773-0b9f-aa21-fe8a-8e19335fe799")},
                                        },
                                    },
                            },
                        },
                    new()
                        {
                         Driver=new(){Id=new("ae86aaeb-1080-4ae2-9226-62cf3a042ca8")},
                         Day = new()
                            {
                               SelectedDateTicks=DateTime.Now.AddDays(14).Ticks,
                               DriverGuid=new("ae86aaeb-1080-4ae2-9226-62cf3a042ca8"),
                               TotalPriceProducts = 100,
                               TotalPriceCake = 100,
                               TotalPrice = 200,
                               TotalPriceCorrect = 0,
                               TotalPriceAfterCorrect=200,
                               TotalPriceMoney = 300,
                               TotalPriceDifference =100,

                               Products = new ObservableCollection<Product>()
                                    {
                                        new Product()
                                        {
                                            Number = 1,
                                            NumberEdit = 1,
                                            NumberReturn = 1,
                                            PriceTotal = 100,
                                            PriceTotalCorrect = 100,
                                            PriceTotalAfterCorrect = 100,
                                            ProductNameId = new("d2034a6a-3a0d-9b49-05a3-bc49adfc329a"),
                                            Name = new(){Id=new("d2034a6a-3a0d-9b49-05a3-bc49adfc329a")},
                                        },
                                        new Product()
                                        {
                                            Number = 2,
                                            NumberEdit = 2,
                                            NumberReturn = 2,
                                            PriceTotal = 200,
                                            PriceTotalCorrect = 200,
                                            PriceTotalAfterCorrect = 200,
                                            ProductNameId = new("24ebc827-bb4e-6fac-4eb4-8f88f524eaf1"),
                                            Name = new(){Id=new("24ebc827-bb4e-6fac-4eb4-8f88f524eaf1")},
                                        },
                                        new Product()
                                        {
                                            Number = 3,
                                            NumberEdit = 3,
                                            NumberReturn = 3,
                                            PriceTotal = 300,
                                            PriceTotalCorrect = 300,
                                            PriceTotalAfterCorrect = 300,
                                            ProductNameId = new("e5226138-0e10-9f4c-cd7b-b91b87ef9f24"),
                                            Name = new(){Id=new("e5226138-0e10-9f4c-cd7b-b91b87ef9f24")},
                                        },
                                        new Product()
                                        {
                                            Number = 0,
                                            NumberEdit = 0,
                                            NumberReturn = 0,
                                            PriceTotal = 0,
                                            PriceTotalCorrect = 0,
                                            PriceTotalAfterCorrect = 0,
                                            ProductNameId = new("61b42773-0b9f-aa21-fe8a-8e19335fe799"),
                                            Name = new(){Id=new("61b42773-0b9f-aa21-fe8a-8e19335fe799")},
                                        },
                                    },
                            },
                        },
                    new()
                        {
                         Driver=new(){Id=new("ae86aaeb-1080-4ae2-9226-62cf3a042ca8")},
                         Day = new()
                            {
                               SelectedDateTicks=DateTime.Now.AddDays(21).Ticks,
                               DriverGuid=new("ae86aaeb-1080-4ae2-9226-62cf3a042ca8"),
                               TotalPriceProducts = 100,
                               TotalPriceCake = 100,
                               TotalPrice = 200,
                               TotalPriceCorrect = 0,
                               TotalPriceAfterCorrect=200,
                               TotalPriceMoney = 300,
                               TotalPriceDifference =100,

                               Products = new ObservableCollection<Product>()
                                    {
                                        new Product()
                                        {
                                            Number = 1,
                                            NumberEdit = 1,
                                            NumberReturn = 1,
                                            PriceTotal = 100,
                                            PriceTotalCorrect = 100,
                                            PriceTotalAfterCorrect = 100,
                                            ProductNameId = new("d2034a6a-3a0d-9b49-05a3-bc49adfc329a"),
                                            Name = new(){Id=new("d2034a6a-3a0d-9b49-05a3-bc49adfc329a")},
                                        },
                                        new Product()
                                        {
                                            Number = 2,
                                            NumberEdit = 2,
                                            NumberReturn = 2,
                                            PriceTotal = 200,
                                            PriceTotalCorrect = 200,
                                            PriceTotalAfterCorrect = 200,
                                            ProductNameId = new("24ebc827-bb4e-6fac-4eb4-8f88f524eaf1"),
                                            Name = new(){Id=new("24ebc827-bb4e-6fac-4eb4-8f88f524eaf1")},
                                        },
                                        new Product()
                                        {
                                            Number = 3,
                                            NumberEdit = 3,
                                            NumberReturn = 3,
                                            PriceTotal = 300,
                                            PriceTotalCorrect = 300,
                                            PriceTotalAfterCorrect = 300,
                                            ProductNameId = new("e5226138-0e10-9f4c-cd7b-b91b87ef9f24"),
                                            Name = new(){Id=new("e5226138-0e10-9f4c-cd7b-b91b87ef9f24")},
                                        },
                                        new Product()
                                        {
                                            Number = 0,
                                            NumberEdit = 0,
                                            NumberReturn = 0,
                                            PriceTotal = 0,
                                            PriceTotalCorrect = 0,
                                            PriceTotalAfterCorrect = 0,
                                            ProductNameId = new("61b42773-0b9f-aa21-fe8a-8e19335fe799"),
                                            Name = new(){Id=new("61b42773-0b9f-aa21-fe8a-8e19335fe799")},
                                        },
                                    },
                            },
                        },
                    new()
                        {
                         Driver=new(){Id=new("ae86aaeb-1080-4ae2-9226-62cf3a042ca8")},
                         Day = new()
                            {
                               SelectedDateTicks=DateTime.Now.AddDays(28).Ticks,
                               DriverGuid=new("ae86aaeb-1080-4ae2-9226-62cf3a042ca8"),
                               TotalPriceProducts = 100,
                               TotalPriceCake = 100,
                               TotalPrice = 200,
                               TotalPriceCorrect = 0,
                               TotalPriceAfterCorrect=200,
                               TotalPriceMoney = 300,
                               TotalPriceDifference =100,

                               Products = new ObservableCollection<Product>()
                                    {
                                        new Product()
                                        {
                                            Number = 1,
                                            NumberEdit = 1,
                                            NumberReturn = 1,
                                            PriceTotal = 100,
                                            PriceTotalCorrect = 100,
                                            PriceTotalAfterCorrect = 100,
                                            ProductNameId = new("d2034a6a-3a0d-9b49-05a3-bc49adfc329a"),
                                            Name = new(){Id=new("d2034a6a-3a0d-9b49-05a3-bc49adfc329a")},
                                        },
                                        new Product()
                                        {
                                            Number = 2,
                                            NumberEdit = 2,
                                            NumberReturn = 2,
                                            PriceTotal = 200,
                                            PriceTotalCorrect = 200,
                                            PriceTotalAfterCorrect = 200,
                                            ProductNameId = new("24ebc827-bb4e-6fac-4eb4-8f88f524eaf1"),
                                            Name = new(){Id=new("24ebc827-bb4e-6fac-4eb4-8f88f524eaf1")},
                                        },
                                        new Product()
                                        {
                                            Number = 3,
                                            NumberEdit = 3,
                                            NumberReturn = 3,
                                            PriceTotal = 300,
                                            PriceTotalCorrect = 300,
                                            PriceTotalAfterCorrect = 300,
                                            ProductNameId = new("e5226138-0e10-9f4c-cd7b-b91b87ef9f24"),
                                            Name = new(){Id=new("e5226138-0e10-9f4c-cd7b-b91b87ef9f24")},
                                        },
                                        new Product()
                                        {
                                            Number = 0,
                                            NumberEdit = 0,
                                            NumberReturn = 0,
                                            PriceTotal = 0,
                                            PriceTotalCorrect = 0,
                                            PriceTotalAfterCorrect = 0,
                                            ProductNameId = new("61b42773-0b9f-aa21-fe8a-8e19335fe799"),
                                            Name = new(){Id=new("61b42773-0b9f-aa21-fe8a-8e19335fe799")},
                                        },
                                    },
                            },
                        },
                    new()
                        {
                         Driver=new(){Id=new("ae86aaeb-1080-4ae2-9226-62cf3a042ca8")},
                         Day = new()
                            {
                               SelectedDateTicks=DateTime.Now.AddDays(35).Ticks,
                               DriverGuid=new("ae86aaeb-1080-4ae2-9226-62cf3a042ca8"),
                               TotalPriceProducts = 100,
                               TotalPriceCake = 100,
                               TotalPrice = 200,
                               TotalPriceCorrect = 0,
                               TotalPriceAfterCorrect=200,
                               TotalPriceMoney = 300,
                               TotalPriceDifference =100,

                               Products = new ObservableCollection<Product>()
                                    {
                                        new Product()
                                        {
                                            Number = 1,
                                            NumberEdit = 1,
                                            NumberReturn = 1,
                                            PriceTotal = 100,
                                            PriceTotalCorrect = 100,
                                            PriceTotalAfterCorrect = 100,
                                            ProductNameId = new("d2034a6a-3a0d-9b49-05a3-bc49adfc329a"),
                                            Name = new(){Id=new("d2034a6a-3a0d-9b49-05a3-bc49adfc329a")},
                                        },
                                        new Product()
                                        {
                                            Number = 2,
                                            NumberEdit = 2,
                                            NumberReturn = 2,
                                            PriceTotal = 200,
                                            PriceTotalCorrect = 200,
                                            PriceTotalAfterCorrect = 200,
                                            ProductNameId = new("24ebc827-bb4e-6fac-4eb4-8f88f524eaf1"),
                                            Name = new(){Id=new("24ebc827-bb4e-6fac-4eb4-8f88f524eaf1")},
                                        },
                                        new Product()
                                        {
                                            Number = 3,
                                            NumberEdit = 3,
                                            NumberReturn = 3,
                                            PriceTotal = 300,
                                            PriceTotalCorrect = 300,
                                            PriceTotalAfterCorrect = 300,
                                            ProductNameId = new("e5226138-0e10-9f4c-cd7b-b91b87ef9f24"),
                                            Name = new(){Id=new("e5226138-0e10-9f4c-cd7b-b91b87ef9f24")},
                                        },
                                        new Product()
                                        {
                                            Number = 0,
                                            NumberEdit = 0,
                                            NumberReturn = 0,
                                            PriceTotal = 0,
                                            PriceTotalCorrect = 0,
                                            PriceTotalAfterCorrect = 0,
                                            ProductNameId = new("61b42773-0b9f-aa21-fe8a-8e19335fe799"),
                                            Name = new(){Id=new("61b42773-0b9f-aa21-fe8a-8e19335fe799")},
                                        },
                                    },
                            },
                        },
                    new()
                        {
                         Driver=new(){Id=new("ae86aaeb-1080-4ae2-9226-62cf3a042ca8")},
                         Day = new()
                            {
                               SelectedDateTicks=DateTime.Now.AddDays(42).Ticks,
                               DriverGuid=new("ae86aaeb-1080-4ae2-9226-62cf3a042ca8"),
                               TotalPriceProducts = 100,
                               TotalPriceCake = 100,
                               TotalPrice = 200,
                               TotalPriceCorrect = 0,
                               TotalPriceAfterCorrect=200,
                               TotalPriceMoney = 300,
                               TotalPriceDifference =100,

                               Products = new ObservableCollection<Product>()
                                    {
                                        new Product()
                                        {
                                            Number = 1,
                                            NumberEdit = 1,
                                            NumberReturn = 1,
                                            PriceTotal = 100,
                                            PriceTotalCorrect = 100,
                                            PriceTotalAfterCorrect = 100,
                                            ProductNameId = new("d2034a6a-3a0d-9b49-05a3-bc49adfc329a"),
                                            Name = new(){Id=new("d2034a6a-3a0d-9b49-05a3-bc49adfc329a")},
                                        },
                                        new Product()
                                        {
                                            Number = 2,
                                            NumberEdit = 2,
                                            NumberReturn = 2,
                                            PriceTotal = 200,
                                            PriceTotalCorrect = 200,
                                            PriceTotalAfterCorrect = 200,
                                            ProductNameId = new("24ebc827-bb4e-6fac-4eb4-8f88f524eaf1"),
                                            Name = new(){Id=new("24ebc827-bb4e-6fac-4eb4-8f88f524eaf1")},
                                        },
                                        new Product()
                                        {
                                            Number = 3,
                                            NumberEdit = 3,
                                            NumberReturn = 3,
                                            PriceTotal = 300,
                                            PriceTotalCorrect = 300,
                                            PriceTotalAfterCorrect = 300,
                                            ProductNameId = new("e5226138-0e10-9f4c-cd7b-b91b87ef9f24"),
                                            Name = new(){Id=new("e5226138-0e10-9f4c-cd7b-b91b87ef9f24")},
                                        },
                                        new Product()
                                        {
                                            Number = 0,
                                            NumberEdit = 0,
                                            NumberReturn = 0,
                                            PriceTotal = 0,
                                            PriceTotalCorrect = 0,
                                            PriceTotalAfterCorrect = 0,
                                            ProductNameId = new("61b42773-0b9f-aa21-fe8a-8e19335fe799"),
                                            Name = new(){Id=new("61b42773-0b9f-aa21-fe8a-8e19335fe799")},
                                        },
                                    },
                            },
                        },
                    new()
                        {
                         Driver=new(){Id=new("ae86aaeb-1080-4ae2-9226-62cf3a042ca8")},
                         Day = new()
                            {
                               SelectedDateTicks=DateTime.Now.AddDays(49).Ticks,
                               DriverGuid=new("ae86aaeb-1080-4ae2-9226-62cf3a042ca8"),
                               TotalPriceProducts = 100,
                               TotalPriceCake = 100,
                               TotalPrice = 200,
                               TotalPriceCorrect = 0,
                               TotalPriceAfterCorrect=200,
                               TotalPriceMoney = 300,
                               TotalPriceDifference =100,

                               Products = new ObservableCollection<Product>()
                                    {
                                        new Product()
                                        {
                                            Number = 1,
                                            NumberEdit = 1,
                                            NumberReturn = 1,
                                            PriceTotal = 100,
                                            PriceTotalCorrect = 100,
                                            PriceTotalAfterCorrect = 100,
                                            ProductNameId = new("d2034a6a-3a0d-9b49-05a3-bc49adfc329a"),
                                            Name = new(){Id=new("d2034a6a-3a0d-9b49-05a3-bc49adfc329a")},
                                        },
                                        new Product()
                                        {
                                            Number = 2,
                                            NumberEdit = 2,
                                            NumberReturn = 2,
                                            PriceTotal = 200,
                                            PriceTotalCorrect = 200,
                                            PriceTotalAfterCorrect = 200,
                                            ProductNameId = new("24ebc827-bb4e-6fac-4eb4-8f88f524eaf1"),
                                            Name = new(){Id=new("24ebc827-bb4e-6fac-4eb4-8f88f524eaf1")},
                                        },
                                        new Product()
                                        {
                                            Number = 3,
                                            NumberEdit = 3,
                                            NumberReturn = 3,
                                            PriceTotal = 300,
                                            PriceTotalCorrect = 300,
                                            PriceTotalAfterCorrect = 300,
                                            ProductNameId = new("e5226138-0e10-9f4c-cd7b-b91b87ef9f24"),
                                            Name = new(){Id=new("e5226138-0e10-9f4c-cd7b-b91b87ef9f24")},
                                        },
                                        new Product()
                                        {
                                            Number = 0,
                                            NumberEdit = 0,
                                            NumberReturn = 0,
                                            PriceTotal = 0,
                                            PriceTotalCorrect = 0,
                                            PriceTotalAfterCorrect = 0,
                                            ProductNameId = new("61b42773-0b9f-aa21-fe8a-8e19335fe799"),
                                            Name = new(){Id=new("61b42773-0b9f-aa21-fe8a-8e19335fe799")},
                                        },
                                    },
                            },
                        },
                },
                new RangeDayM[]
                 {
                        new()
                        {
                         Driver=new(){Id=new("ae86aaeb-1080-4ae2-9226-62cf3a042ca8")},
                         Day = new()
                            {
                               DriverGuid=new("ae86aaeb-1080-4ae2-9226-62cf3a042ca8"),
                               TotalPriceProducts = 100*8,
                               SelectedDateString=DateTime.Now.DayOfWeek.TranslateSelectedDay(),
                               TotalPriceCake = 100*8,
                               TotalPrice = 200*8,
                               TotalPriceCorrect = 0*8,
                               TotalPriceAfterCorrect=200*8,
                               TotalPriceMoney = 300*8,
                               TotalPriceDifference =100*8,
                               Products = new ObservableCollection<Product>()
                                    {
                                        new Product()
                                        {
                                            Number = 1*8,
                                            NumberEdit = 1*8,
                                            NumberReturn = 1*8,
                                            PriceTotal = 100*8,
                                            PriceTotalCorrect = 100*8,
                                            PriceTotalAfterCorrect = 100*8,
                                            ProductNameId = new("d2034a6a-3a0d-9b49-05a3-bc49adfc329a"),
                                            Name = new(){Id=new("d2034a6a-3a0d-9b49-05a3-bc49adfc329a")},
                                        },
                                        new Product()
                                        {
                                            Number = 2*8,
                                            NumberEdit = 2*8,
                                            NumberReturn = 2*8,
                                            PriceTotal = 200*8,
                                            PriceTotalCorrect = 200*8,
                                            PriceTotalAfterCorrect = 200*8,
                                            ProductNameId = new("24ebc827-bb4e-6fac-4eb4-8f88f524eaf1"),
                                            Name = new(){Id=new("24ebc827-bb4e-6fac-4eb4-8f88f524eaf1")},
                                        },
                                        new Product()
                                        {
                                            Number = 3*8,
                                            NumberEdit = 3*8,
                                            NumberReturn = 3*8,
                                            PriceTotal = 300*8,
                                            PriceTotalCorrect = 300*8,
                                            PriceTotalAfterCorrect = 300*8,
                                            ProductNameId = new("e5226138-0e10-9f4c-cd7b-b91b87ef9f24"),
                                            Name = new(){Id=new("e5226138-0e10-9f4c-cd7b-b91b87ef9f24")},
                                        },
                                        new Product()
                                        {
                                            Number = 3,
                                            NumberEdit = 3,
                                            NumberReturn = 3,
                                            PriceTotal = 300,
                                            PriceTotalCorrect = 300,
                                            PriceTotalAfterCorrect = 300,
                                            ProductNameId = new("61b42773-0b9f-aa21-fe8a-8e19335fe799"),
                                            Name = new(){Id=new("61b42773-0b9f-aa21-fe8a-8e19335fe799")},
                                        },
                                    },
                            },
                        },
                 }

              ]
];

        [Theory]
        [MemberData(nameof(TestSumDayOfWeekValue))]
        public void TestSumDayOfWeek(RangeDayM[] request, RangeDayM[] result)
        {
            Inventory.Helper.RangeCalculations.GetUniqueDriver(request);
            var obj = Inventory.Helper.RangeCalculations.SumDayOfWeek(request).ToArray();

            var objJson = Newtonsoft.Json.JsonConvert.SerializeObject(obj);
            var resultJson = Newtonsoft.Json.JsonConvert.SerializeObject(result);

            objJson.Should().Be(resultJson);

        }

        public static IEnumerable<object[]> TestAveragesDayOfWeekValue =>
[
[
                new RangeDayM[]
                {
                    new()
                        {
                         Driver=new(){Id=new("ae86aaeb-1080-4ae2-9226-62cf3a042ca8")},
                         Day = new()
                            {
                               SelectedDateTicks=DateTime.Now.AddDays(0).Ticks,
                               DriverGuid=new("ae86aaeb-1080-4ae2-9226-62cf3a042ca8"),
                               TotalPriceProducts = 100,
                               TotalPriceCake = 100,
                               TotalPrice = 200,
                               TotalPriceCorrect = 0,
                               TotalPriceAfterCorrect=200,
                               TotalPriceMoney = 300,
                               TotalPriceDifference =100,

                               Products = new ObservableCollection<Product>()
                                    {
                                        new Product()
                                        {
                                            Number = 1,
                                            NumberEdit = 1,
                                            NumberReturn = 1,
                                            PriceTotal = 100,
                                            PriceTotalCorrect = 100,
                                            PriceTotalAfterCorrect = 100,
                                            ProductNameId = new("d2034a6a-3a0d-9b49-05a3-bc49adfc329a"),
                                            Name = new(){Id=new("d2034a6a-3a0d-9b49-05a3-bc49adfc329a")},
                                        },
                                        new Product()
                                        {
                                            Number = 2,
                                            NumberEdit = 2,
                                            NumberReturn = 2,
                                            PriceTotal = 200,
                                            PriceTotalCorrect = 200,
                                            PriceTotalAfterCorrect = 200,
                                            ProductNameId = new("24ebc827-bb4e-6fac-4eb4-8f88f524eaf1"),
                                            Name = new(){Id=new("24ebc827-bb4e-6fac-4eb4-8f88f524eaf1")},
                                        },
                                        new Product()
                                        {
                                            Number = 3,
                                            NumberEdit = 3,
                                            NumberReturn = 3,
                                            PriceTotal = 300,
                                            PriceTotalCorrect = 300,
                                            PriceTotalAfterCorrect = 300,
                                            ProductNameId = new("e5226138-0e10-9f4c-cd7b-b91b87ef9f24"),
                                            Name = new(){Id=new("e5226138-0e10-9f4c-cd7b-b91b87ef9f24")},
                                        },
                                    },
                            },
                        },
                    new()
                        {
                         Driver=new(){Id=new("ae86aaeb-1080-4ae2-9226-62cf3a042ca8")},
                         Day = new()
                            {
                               SelectedDateTicks=DateTime.Now.AddDays(7).Ticks,
                               DriverGuid=new("ae86aaeb-1080-4ae2-9226-62cf3a042ca8"),
                               TotalPriceProducts = 100,
                               TotalPriceCake = 100,
                               TotalPrice = 200,
                               TotalPriceCorrect = 0,
                               TotalPriceAfterCorrect=200,
                               TotalPriceMoney = 300,
                               TotalPriceDifference =100,

                               Products = new ObservableCollection<Product>()
                                    {
                                        new Product()
                                        {
                                            Number = 1,
                                            NumberEdit = 1,
                                            NumberReturn = 1,
                                            PriceTotal = 100,
                                            PriceTotalCorrect = 100,
                                            PriceTotalAfterCorrect = 100,
                                            ProductNameId = new("d2034a6a-3a0d-9b49-05a3-bc49adfc329a"),
                                            Name = new(){Id=new("d2034a6a-3a0d-9b49-05a3-bc49adfc329a")},
                                        },
                                        new Product()
                                        {
                                            Number = 2,
                                            NumberEdit = 2,
                                            NumberReturn = 2,
                                            PriceTotal = 200,
                                            PriceTotalCorrect = 200,
                                            PriceTotalAfterCorrect = 200,
                                            ProductNameId = new("24ebc827-bb4e-6fac-4eb4-8f88f524eaf1"),
                                            Name = new(){Id=new("24ebc827-bb4e-6fac-4eb4-8f88f524eaf1")},
                                        },
                                        new Product()
                                        {
                                            Number = 3,
                                            NumberEdit = 3,
                                            NumberReturn = 3,
                                            PriceTotal = 300,
                                            PriceTotalCorrect = 300,
                                            PriceTotalAfterCorrect = 300,
                                            ProductNameId = new("e5226138-0e10-9f4c-cd7b-b91b87ef9f24"),
                                            Name = new(){Id=new("e5226138-0e10-9f4c-cd7b-b91b87ef9f24")},
                                        },
                                        new Product()
                                        {
                                            Number = 3,
                                            NumberEdit = 3,
                                            NumberReturn = 3,
                                            PriceTotal = 300,
                                            PriceTotalCorrect = 300,
                                            PriceTotalAfterCorrect = 300,
                                            ProductNameId = new("61b42773-0b9f-aa21-fe8a-8e19335fe799"),
                                            Name = new(){Id=new("61b42773-0b9f-aa21-fe8a-8e19335fe799")},
                                        },
                                    },
                            },
                        },
                    new()
                        {
                         Driver=new(){Id=new("ae86aaeb-1080-4ae2-9226-62cf3a042ca8")},
                         Day = new()
                            {
                               SelectedDateTicks=DateTime.Now.AddDays(14).Ticks,
                               DriverGuid=new("ae86aaeb-1080-4ae2-9226-62cf3a042ca8"),
                               TotalPriceProducts = 100,
                               TotalPriceCake = 100,
                               TotalPrice = 200,
                               TotalPriceCorrect = 0,
                               TotalPriceAfterCorrect=200,
                               TotalPriceMoney = 300,
                               TotalPriceDifference =100,

                               Products = new ObservableCollection<Product>()
                                    {
                                        new Product()
                                        {
                                            Number = 1,
                                            NumberEdit = 1,
                                            NumberReturn = 1,
                                            PriceTotal = 100,
                                            PriceTotalCorrect = 100,
                                            PriceTotalAfterCorrect = 100,
                                            ProductNameId = new("d2034a6a-3a0d-9b49-05a3-bc49adfc329a"),
                                            Name = new(){Id=new("d2034a6a-3a0d-9b49-05a3-bc49adfc329a")},
                                        },
                                        new Product()
                                        {
                                            Number = 2,
                                            NumberEdit = 2,
                                            NumberReturn = 2,
                                            PriceTotal = 200,
                                            PriceTotalCorrect = 200,
                                            PriceTotalAfterCorrect = 200,
                                            ProductNameId = new("24ebc827-bb4e-6fac-4eb4-8f88f524eaf1"),
                                            Name = new(){Id=new("24ebc827-bb4e-6fac-4eb4-8f88f524eaf1")},
                                        },
                                        new Product()
                                        {
                                            Number = 3,
                                            NumberEdit = 3,
                                            NumberReturn = 3,
                                            PriceTotal = 300,
                                            PriceTotalCorrect = 300,
                                            PriceTotalAfterCorrect = 300,
                                            ProductNameId = new("e5226138-0e10-9f4c-cd7b-b91b87ef9f24"),
                                            Name = new(){Id=new("e5226138-0e10-9f4c-cd7b-b91b87ef9f24")},
                                        },
                                        new Product()
                                        {
                                            Number = 0,
                                            NumberEdit = 0,
                                            NumberReturn = 0,
                                            PriceTotal = 0,
                                            PriceTotalCorrect = 0,
                                            PriceTotalAfterCorrect = 0,
                                            ProductNameId = new("61b42773-0b9f-aa21-fe8a-8e19335fe799"),
                                            Name = new(){Id=new("61b42773-0b9f-aa21-fe8a-8e19335fe799")},
                                        },
                                    },
                            },
                        },
                    new()
                        {
                         Driver=new(){Id=new("ae86aaeb-1080-4ae2-9226-62cf3a042ca8")},
                         Day = new()
                            {
                               SelectedDateTicks=DateTime.Now.AddDays(21).Ticks,
                               DriverGuid=new("ae86aaeb-1080-4ae2-9226-62cf3a042ca8"),
                               TotalPriceProducts = 100,
                               TotalPriceCake = 100,
                               TotalPrice = 200,
                               TotalPriceCorrect = 0,
                               TotalPriceAfterCorrect=200,
                               TotalPriceMoney = 300,
                               TotalPriceDifference =100,

                               Products = new ObservableCollection<Product>()
                                    {
                                        new Product()
                                        {
                                            Number = 1,
                                            NumberEdit = 1,
                                            NumberReturn = 1,
                                            PriceTotal = 100,
                                            PriceTotalCorrect = 100,
                                            PriceTotalAfterCorrect = 100,
                                            ProductNameId = new("d2034a6a-3a0d-9b49-05a3-bc49adfc329a"),
                                            Name = new(){Id=new("d2034a6a-3a0d-9b49-05a3-bc49adfc329a")},
                                        },
                                        new Product()
                                        {
                                            Number = 2,
                                            NumberEdit = 2,
                                            NumberReturn = 2,
                                            PriceTotal = 200,
                                            PriceTotalCorrect = 200,
                                            PriceTotalAfterCorrect = 200,
                                            ProductNameId = new("24ebc827-bb4e-6fac-4eb4-8f88f524eaf1"),
                                            Name = new(){Id=new("24ebc827-bb4e-6fac-4eb4-8f88f524eaf1")},
                                        },
                                        new Product()
                                        {
                                            Number = 3,
                                            NumberEdit = 3,
                                            NumberReturn = 3,
                                            PriceTotal = 300,
                                            PriceTotalCorrect = 300,
                                            PriceTotalAfterCorrect = 300,
                                            ProductNameId = new("e5226138-0e10-9f4c-cd7b-b91b87ef9f24"),
                                            Name = new(){Id=new("e5226138-0e10-9f4c-cd7b-b91b87ef9f24")},
                                        },
                                        new Product()
                                        {
                                            Number = 0,
                                            NumberEdit = 0,
                                            NumberReturn = 0,
                                            PriceTotal = 0,
                                            PriceTotalCorrect = 0,
                                            PriceTotalAfterCorrect = 0,
                                            ProductNameId = new("61b42773-0b9f-aa21-fe8a-8e19335fe799"),
                                            Name = new(){Id=new("61b42773-0b9f-aa21-fe8a-8e19335fe799")},
                                        },
                                    },
                            },
                        },
                    new()
                        {
                         Driver=new(){Id=new("ae86aaeb-1080-4ae2-9226-62cf3a042ca8")},
                         Day = new()
                            {
                               SelectedDateTicks=DateTime.Now.AddDays(28).Ticks,
                               DriverGuid=new("ae86aaeb-1080-4ae2-9226-62cf3a042ca8"),
                               TotalPriceProducts = 100,
                               TotalPriceCake = 100,
                               TotalPrice = 200,
                               TotalPriceCorrect = 0,
                               TotalPriceAfterCorrect=200,
                               TotalPriceMoney = 300,
                               TotalPriceDifference =100,

                               Products = new ObservableCollection<Product>()
                                    {
                                        new Product()
                                        {
                                            Number = 1,
                                            NumberEdit = 1,
                                            NumberReturn = 1,
                                            PriceTotal = 100,
                                            PriceTotalCorrect = 100,
                                            PriceTotalAfterCorrect = 100,
                                            ProductNameId = new("d2034a6a-3a0d-9b49-05a3-bc49adfc329a"),
                                            Name = new(){Id=new("d2034a6a-3a0d-9b49-05a3-bc49adfc329a")},
                                        },
                                        new Product()
                                        {
                                            Number = 2,
                                            NumberEdit = 2,
                                            NumberReturn = 2,
                                            PriceTotal = 200,
                                            PriceTotalCorrect = 200,
                                            PriceTotalAfterCorrect = 200,
                                            ProductNameId = new("24ebc827-bb4e-6fac-4eb4-8f88f524eaf1"),
                                            Name = new(){Id=new("24ebc827-bb4e-6fac-4eb4-8f88f524eaf1")},
                                        },
                                        new Product()
                                        {
                                            Number = 3,
                                            NumberEdit = 3,
                                            NumberReturn = 3,
                                            PriceTotal = 300,
                                            PriceTotalCorrect = 300,
                                            PriceTotalAfterCorrect = 300,
                                            ProductNameId = new("e5226138-0e10-9f4c-cd7b-b91b87ef9f24"),
                                            Name = new(){Id=new("e5226138-0e10-9f4c-cd7b-b91b87ef9f24")},
                                        },
                                        new Product()
                                        {
                                            Number = 0,
                                            NumberEdit = 0,
                                            NumberReturn = 0,
                                            PriceTotal = 0,
                                            PriceTotalCorrect = 0,
                                            PriceTotalAfterCorrect = 0,
                                            ProductNameId = new("61b42773-0b9f-aa21-fe8a-8e19335fe799"),
                                            Name = new(){Id=new("61b42773-0b9f-aa21-fe8a-8e19335fe799")},
                                        },
                                    },
                            },
                        },
                    new()
                        {
                         Driver=new(){Id=new("ae86aaeb-1080-4ae2-9226-62cf3a042ca8")},
                         Day = new()
                            {
                               SelectedDateTicks=DateTime.Now.AddDays(35).Ticks,
                               DriverGuid=new("ae86aaeb-1080-4ae2-9226-62cf3a042ca8"),
                               TotalPriceProducts = 100,
                               TotalPriceCake = 100,
                               TotalPrice = 200,
                               TotalPriceCorrect = 0,
                               TotalPriceAfterCorrect=200,
                               TotalPriceMoney = 300,
                               TotalPriceDifference =100,

                               Products = new ObservableCollection<Product>()
                                    {
                                        new Product()
                                        {
                                            Number = 1,
                                            NumberEdit = 1,
                                            NumberReturn = 1,
                                            PriceTotal = 100,
                                            PriceTotalCorrect = 100,
                                            PriceTotalAfterCorrect = 100,
                                            ProductNameId = new("d2034a6a-3a0d-9b49-05a3-bc49adfc329a"),
                                            Name = new(){Id=new("d2034a6a-3a0d-9b49-05a3-bc49adfc329a")},
                                        },
                                        new Product()
                                        {
                                            Number = 2,
                                            NumberEdit = 2,
                                            NumberReturn = 2,
                                            PriceTotal = 200,
                                            PriceTotalCorrect = 200,
                                            PriceTotalAfterCorrect = 200,
                                            ProductNameId = new("24ebc827-bb4e-6fac-4eb4-8f88f524eaf1"),
                                            Name = new(){Id=new("24ebc827-bb4e-6fac-4eb4-8f88f524eaf1")},
                                        },
                                        new Product()
                                        {
                                            Number = 3,
                                            NumberEdit = 3,
                                            NumberReturn = 3,
                                            PriceTotal = 300,
                                            PriceTotalCorrect = 300,
                                            PriceTotalAfterCorrect = 300,
                                            ProductNameId = new("e5226138-0e10-9f4c-cd7b-b91b87ef9f24"),
                                            Name = new(){Id=new("e5226138-0e10-9f4c-cd7b-b91b87ef9f24")},
                                        },
                                        new Product()
                                        {
                                            Number = 0,
                                            NumberEdit = 0,
                                            NumberReturn = 0,
                                            PriceTotal = 0,
                                            PriceTotalCorrect = 0,
                                            PriceTotalAfterCorrect = 0,
                                            ProductNameId = new("61b42773-0b9f-aa21-fe8a-8e19335fe799"),
                                            Name = new(){Id=new("61b42773-0b9f-aa21-fe8a-8e19335fe799")},
                                        },
                                    },
                            },
                        },
                    new()
                        {
                         Driver=new(){Id=new("ae86aaeb-1080-4ae2-9226-62cf3a042ca8")},
                         Day = new()
                            {
                               SelectedDateTicks=DateTime.Now.AddDays(42).Ticks,
                               DriverGuid=new("ae86aaeb-1080-4ae2-9226-62cf3a042ca8"),
                               TotalPriceProducts = 100,
                               TotalPriceCake = 100,
                               TotalPrice = 200,
                               TotalPriceCorrect = 0,
                               TotalPriceAfterCorrect=200,
                               TotalPriceMoney = 300,
                               TotalPriceDifference =100,

                               Products = new ObservableCollection<Product>()
                                    {
                                        new Product()
                                        {
                                            Number = 1,
                                            NumberEdit = 1,
                                            NumberReturn = 1,
                                            PriceTotal = 100,
                                            PriceTotalCorrect = 100,
                                            PriceTotalAfterCorrect = 100,
                                            ProductNameId = new("d2034a6a-3a0d-9b49-05a3-bc49adfc329a"),
                                            Name = new(){Id=new("d2034a6a-3a0d-9b49-05a3-bc49adfc329a")},
                                        },
                                        new Product()
                                        {
                                            Number = 2,
                                            NumberEdit = 2,
                                            NumberReturn = 2,
                                            PriceTotal = 200,
                                            PriceTotalCorrect = 200,
                                            PriceTotalAfterCorrect = 200,
                                            ProductNameId = new("24ebc827-bb4e-6fac-4eb4-8f88f524eaf1"),
                                            Name = new(){Id=new("24ebc827-bb4e-6fac-4eb4-8f88f524eaf1")},
                                        },
                                        new Product()
                                        {
                                            Number = 3,
                                            NumberEdit = 3,
                                            NumberReturn = 3,
                                            PriceTotal = 300,
                                            PriceTotalCorrect = 300,
                                            PriceTotalAfterCorrect = 300,
                                            ProductNameId = new("e5226138-0e10-9f4c-cd7b-b91b87ef9f24"),
                                            Name = new(){Id=new("e5226138-0e10-9f4c-cd7b-b91b87ef9f24")},
                                        },
                                        new Product()
                                        {
                                            Number = 0,
                                            NumberEdit = 0,
                                            NumberReturn = 0,
                                            PriceTotal = 0,
                                            PriceTotalCorrect = 0,
                                            PriceTotalAfterCorrect = 0,
                                            ProductNameId = new("61b42773-0b9f-aa21-fe8a-8e19335fe799"),
                                            Name = new(){Id=new("61b42773-0b9f-aa21-fe8a-8e19335fe799")},
                                        },
                                    },
                            },
                        },
                    new()
                        {
                         Driver=new(){Id=new("ae86aaeb-1080-4ae2-9226-62cf3a042ca8")},
                         Day = new()
                            {
                               SelectedDateTicks=DateTime.Now.AddDays(49).Ticks,
                               DriverGuid=new("ae86aaeb-1080-4ae2-9226-62cf3a042ca8"),
                               TotalPriceProducts = 100,
                               TotalPriceCake = 100,
                               TotalPrice = 200,
                               TotalPriceCorrect = 0,
                               TotalPriceAfterCorrect=200,
                               TotalPriceMoney = 300,
                               TotalPriceDifference =100,

                               Products = new ObservableCollection<Product>()
                                    {
                                        new Product()
                                        {
                                            Number = 1,
                                            NumberEdit = 1,
                                            NumberReturn = 1,
                                            PriceTotal = 100,
                                            PriceTotalCorrect = 100,
                                            PriceTotalAfterCorrect = 100,
                                            ProductNameId = new("d2034a6a-3a0d-9b49-05a3-bc49adfc329a"),
                                            Name = new(){Id=new("d2034a6a-3a0d-9b49-05a3-bc49adfc329a")},
                                        },
                                        new Product()
                                        {
                                            Number = 2,
                                            NumberEdit = 2,
                                            NumberReturn = 2,
                                            PriceTotal = 200,
                                            PriceTotalCorrect = 200,
                                            PriceTotalAfterCorrect = 200,
                                            ProductNameId = new("24ebc827-bb4e-6fac-4eb4-8f88f524eaf1"),
                                            Name = new(){Id=new("24ebc827-bb4e-6fac-4eb4-8f88f524eaf1")},
                                        },
                                        new Product()
                                        {
                                            Number = 3,
                                            NumberEdit = 3,
                                            NumberReturn = 3,
                                            PriceTotal = 300,
                                            PriceTotalCorrect = 300,
                                            PriceTotalAfterCorrect = 300,
                                            ProductNameId = new("e5226138-0e10-9f4c-cd7b-b91b87ef9f24"),
                                            Name = new(){Id=new("e5226138-0e10-9f4c-cd7b-b91b87ef9f24")},
                                        },
                                        new Product()
                                        {
                                            Number = 0,
                                            NumberEdit = 0,
                                            NumberReturn = 0,
                                            PriceTotal = 0,
                                            PriceTotalCorrect = 0,
                                            PriceTotalAfterCorrect = 0,
                                            ProductNameId = new("61b42773-0b9f-aa21-fe8a-8e19335fe799"),
                                            Name = new(){Id=new("61b42773-0b9f-aa21-fe8a-8e19335fe799")},
                                        },
                                    },
                            },
                        },
                },
                new RangeDayM[]
                 {
                        new()
                        {
                         Driver=new(){Id=new("ae86aaeb-1080-4ae2-9226-62cf3a042ca8")},
                         Day = new()
                            {
                               DriverGuid=new("ae86aaeb-1080-4ae2-9226-62cf3a042ca8"),
                               TotalPriceProducts = 100,
                               SelectedDateString=DateTime.Now.DayOfWeek.TranslateSelectedDay(),
                               TotalPriceCake = 100,
                               TotalPrice = 200,
                               TotalPriceCorrect = 0,
                               TotalPriceAfterCorrect=200,
                               TotalPriceMoney = 300,
                               TotalPriceDifference =100,
                               Products = new ObservableCollection<Product>()
                                    {
                                        new Product()
                                        {
                                            Number = 1,
                                            NumberEdit = 1,
                                            NumberReturn = 1,
                                            PriceTotal = 100,
                                            PriceTotalCorrect = 100,
                                            PriceTotalAfterCorrect = 100,
                                            ProductNameId = new("d2034a6a-3a0d-9b49-05a3-bc49adfc329a"),
                                            Name = new(){Id=new("d2034a6a-3a0d-9b49-05a3-bc49adfc329a")},
                                        },
                                        new Product()
                                        {
                                            Number = 2,
                                            NumberEdit = 2,
                                            NumberReturn = 2,
                                            PriceTotal = 200,
                                            PriceTotalCorrect = 200,
                                            PriceTotalAfterCorrect = 200,
                                            ProductNameId = new("24ebc827-bb4e-6fac-4eb4-8f88f524eaf1"),
                                            Name = new(){Id=new("24ebc827-bb4e-6fac-4eb4-8f88f524eaf1")},
                                        },
                                        new Product()
                                        {
                                            Number = 3,
                                            NumberEdit = 3,
                                            NumberReturn = 3,
                                            PriceTotal = 300,
                                            PriceTotalCorrect = 300,
                                            PriceTotalAfterCorrect = 300,
                                            ProductNameId = new("e5226138-0e10-9f4c-cd7b-b91b87ef9f24"),
                                            Name = new(){Id=new("e5226138-0e10-9f4c-cd7b-b91b87ef9f24")},
                                        },
                                        new Product()
                                        {
                                            Number = 3,
                                            NumberEdit = 3,
                                            NumberReturn = 3,
                                            PriceTotal = 300,
                                            PriceTotalCorrect = 300,
                                            PriceTotalAfterCorrect = 300,
                                            ProductNameId = new("61b42773-0b9f-aa21-fe8a-8e19335fe799"),
                                            Name = new(){Id=new("61b42773-0b9f-aa21-fe8a-8e19335fe799")},
                                        },
                                    },
                            },
                        },
                 }

              ]
];

        [Theory]
        [MemberData(nameof(TestAveragesDayOfWeekValue))]
        public void TestAveragesDayOfWeek(RangeDayM[] request, RangeDayM[] result)
        {
            Inventory.Helper.RangeCalculations.GetUniqueDriver(request);
            var obj = Inventory.Helper.RangeCalculations.AveragesDayOfWeek(request).ToArray();

            var objJson = Newtonsoft.Json.JsonConvert.SerializeObject(obj);
            var resultJson = Newtonsoft.Json.JsonConvert.SerializeObject(result);

            objJson.Should().Be(resultJson);
        }

    }
}
