using DataBase.Model.EntitiesInventory;

using Inventory.Pages.RangeDay;

namespace Inventory.Helper
{
    internal class RangeCalculations
    {
        public static IList<RangeDayM> SumTotalOfRange(IList<RangeDayM> range)
        {
            var uniqueDriver = range.DistinctBy(x => x.Driver.Id).Select(z => z.Driver).ToArray();


            RangeDayM[] sumRange = new RangeDayM[uniqueDriver.Length];
            List<Product> products = new List<Product>();
            for (int i = 0; i < uniqueDriver.Length; i++)
            {
                var driver = uniqueDriver[i];
                var product = range.Where(z => z.Driver.Id == driver.Id);
                var cake = range.Where(z => z.Driver.Id == driver.Id);
                var price = range.Where(z => z.Driver.Id == driver.Id);
                var correct = range.Where(z => z.Driver.Id == driver.Id);
                var money = range.Where(z => z.Driver.Id == driver.Id);
                var difference = range.Where(z => z.Driver.Id == driver.Id);
                var after = range.Where(z => z.Driver.Id == driver.Id);

                var uniqueproducts = range.Where(z => z.Driver.Id == driver.Id).Select(x => x.Day.Products);

                List<Product> productList = [];

                foreach (var item in uniqueproducts)
                {
                    for (int j = 0; j < item.Count; j++)
                    {
                        if (productList.Exists(x => x.Name.Id == item[j].Name.Id))
                        {
                            var index = productList.FindIndex(x => x.Name.Id == item[j].Name.Id);
                            if (productList[index].Name.Id == item[j].Name.Id)
                            {
                                productList[index].Number += item[j].Number;
                                productList[index].NumberEdit += item[j].NumberEdit;
                                productList[index].NumberReturn += item[j].NumberReturn;
                                productList[index].PriceTotal += item[j].PriceTotal;
                                productList[index].PriceTotalCorrect += item[j].PriceTotalCorrect;
                                productList[index].PriceTotalAfterCorrect += item[j].PriceTotalAfterCorrect;
                                continue;
                            }
                        }
                        else
                        {
                            productList.Add(item[j]);
                        }
                    }
                }

                var day = new DataBase.Model.EntitiesInventory.Day
                {
                    TotalPriceProducts = product.Sum(x => x.Day.TotalPriceProducts),
                    TotalPriceCake = cake.Sum(x => x.Day.TotalPriceCake),
                    TotalPrice = price.Sum(x => x.Day.TotalPrice),
                    TotalPriceCorrect = correct.Sum(x => x.Day.TotalPriceCorrect),
                    TotalPriceMoney = money.Sum(x => x.Day.TotalPriceMoney),
                    TotalPriceDifference = difference.Sum(x => x.Day.TotalPriceDifference),
                    TotalPriceAfterCorrect = after.Sum(x => x.Day.TotalPriceAfterCorrect),
                    Products = new System.Collections.ObjectModel.ObservableCollection<DataBase.Model.EntitiesInventory.Product>(productList)
                };


                sumRange[i] = new()
                {
                    Driver = uniqueDriver[i],
                    Day = day,
                };
            }

            return sumRange;
        }


    }
}
