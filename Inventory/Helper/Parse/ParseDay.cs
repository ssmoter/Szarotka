using DataBase.Model.EntitiesInventory;

using Inventory.Model.MVVM;

namespace Inventory.Helper.Parse
{
    public static class ParseDay
    {
        public static Day ParseAsDay(this DayM m)
        {
            var d = new Day();
            if (m is not null)
            {
                d.Id = m.Id;
                d.Description = m.Description;
                d.DriverGuid = m.DriverGuid;
                d.CreatedDateTime = m.Created;


                for (int i = 0; i < m.Products.Count; i++)
                {
                    d.Products.Add(m.Products[i].ParseAsProduct());
                }
                for (int i = 0; i < m.Cakes.Count; i++)
                {
                    d.Cakes.Add(m.Cakes[i].PareseAsCake());
                }


                d.TotalPriceProductsDecimal = m.TotalPriceProduct;
                d.TotalPriceCakeDecimal = m.TotalPriceCake;
                d.TotalPriceDecimal = m.TotalPrice;
                d.TotalPriceCorrectDecimal = m.TotalPriceCorrect;
                d.TotalPriceMoneyDecimal = m.TotalPriceMoney;
                d.TotalPriceDifferenceDecimal = m.TotalPriceDifference;
                d.TotalPriceAfterCorrectDecimal = m.TotalPriceAfterCorrect;

            }

            return d;
        }

        public static void ParseAsDay(this DayM from, Day to)
        {
            if (from is not null)
            {
                to ??= new Day();
                to.Id = from.Id;
                to.Description = from.Description;
                to.DriverGuid = from.DriverGuid;
                to.CreatedDateTime = from.Created;


                to.Products ??= new List<Product>();
                to.Products.Clear();
                for (int i = 0; i < from.Products.Count; i++)
                {
                    from.Products[i].ParseAsProduct(to.Products[i]);
                }
                to.Cakes ??= new List<Cake>();

                to.Cakes.Clear();
                for (int i = 0; i < from.Cakes.Count; i++)
                {
                    from.Cakes[i].PareseAsCake(to.Cakes[i]);
                }


                to.TotalPriceProductsDecimal = from.TotalPriceProduct;
                to.TotalPriceCakeDecimal = from.TotalPriceCake;
                to.TotalPriceDecimal = from.TotalPrice;
                to.TotalPriceCorrectDecimal = from.TotalPriceCorrect;
                to.TotalPriceMoneyDecimal = from.TotalPriceMoney;
                to.TotalPriceDifferenceDecimal = from.TotalPriceDifference;
                to.TotalPriceAfterCorrectDecimal = from.TotalPriceAfterCorrect;

            }
        }


        public static DayM ParseAsDayM(this Day from)
        {
            var to = new DayM();
            if (from is not null)
            {
                to.Id = from.Id;
                to.Description = from.Description;
                to.DriverGuid = from.DriverGuid;
                to.Created = from.CreatedDateTime;


                for (int i = 0; i < from.Products.Count; i++)
                {
                    to.Products.Add(from.Products[i].ParseAsProductM());
                }
                for (int i = 0; i < from.Cakes.Count; i++)
                {
                    to.Cakes.Add(from.Cakes[i].PareseAsCakeM());
                    to.Cakes[i].Index = i + 1;
                }

                to.TotalPriceProduct = from.TotalPriceProductsDecimal;
                to.TotalPriceCake = from.TotalPriceCakeDecimal;
                to.TotalPrice = from.TotalPriceDecimal;
                to.TotalPriceCorrect = from.TotalPriceCorrectDecimal;
                to.TotalPriceMoney = from.TotalPriceMoneyDecimal;
                to.TotalPriceDifference = from.TotalPriceDifferenceDecimal;
                to.TotalPriceAfterCorrect = from.TotalPriceAfterCorrectDecimal;
            }
            return to;
        }

        public static void ParseAsDayM(this Day from, DayM to)
        {
            if (from is not null)
            {
                to ??= new DayM();

                to.Id = from.Id;
                to.Description = from.Description;
                to.DriverGuid = from.DriverGuid;
                to.Created = from.CreatedDateTime;


                to.Products ??= new System.Collections.ObjectModel.ObservableCollection<ProductM>();

                for (int i = 0; i < from.Products.Count; i++)
                {
                    if (to.Products.Count < i)
                    {
                        to.Products.Add(from.Products[i].ParseAsProductM());
                    }
                    else if (to.Products[i].ProductNameId == from.Products[i].ProductNameId)
                    {
                        from.Products[i].ParseAsProductM(to.Products[i]);
                    }
                }

                to.Cakes ??= new System.Collections.ObjectModel.ObservableCollection<CakeM>();
                to.Cakes.Clear();
                for (int i = 0; i < from.Cakes.Count; i++)
                {
                    to.Cakes.Add(from.Cakes[i].PareseAsCakeM());
                    to.Cakes[i].Index = i + 1;
                }

                to.TotalPriceProduct = from.TotalPriceProductsDecimal;
                to.TotalPriceCake = from.TotalPriceCakeDecimal;
                to.TotalPrice = from.TotalPriceDecimal;
                to.TotalPriceCorrect = from.TotalPriceCorrectDecimal;
                to.TotalPriceMoney = from.TotalPriceMoneyDecimal;
                to.TotalPriceDifference = from.TotalPriceDifferenceDecimal;
                to.TotalPriceAfterCorrect = from.TotalPriceAfterCorrectDecimal;

            }
        }
    }
}
