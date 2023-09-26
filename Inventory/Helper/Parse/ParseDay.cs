using Inventory.Model;
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
                if (to is null)
                {
                    to = new Day();
                }
                to.Id = from.Id;
                to.Description = from.Description;
                to.DriverGuid = from.DriverGuid;
                to.CreatedDateTime = from.Created;


                if (from.Products is null)
                {
                    from.Products = new System.Collections.ObjectModel.ObservableCollection<ProductM>();
                }

                for (int i = 0; i < from.Products.Count; i++)
                {
                    from.Products[i].ParseAsProduct(to.Products[i]);
                }
                if (from.Cakes is null)
                {
                    from.Cakes = new System.Collections.ObjectModel.ObservableCollection<CakeM>();
                }

                from.Cakes.Clear();
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


        public static DayM ParseAsDayM(this Day d)
        {
            var m = new DayM();
            if (d is not null)
            {
                m.Id = d.Id;
                m.Description = d.Description;
                m.DriverGuid = d.DriverGuid;
                m.Created = d.CreatedDateTime;


                for (int i = 0; i < d.Products.Count; i++)
                {
                    m.Products.Add(d.Products[i].ParseAsProductM());
                }
                for (int i = 0; i < d.Cakes.Count; i++)
                {
                    m.Cakes.Add(d.Cakes[i].PareseAsCakeM());
                }


                m.TotalPriceProduct = d.TotalPriceProductsDecimal;
                m.TotalPriceCake = d.TotalPriceCakeDecimal;
                m.TotalPrice = d.TotalPriceDecimal;
                m.TotalPriceCorrect = d.TotalPriceCorrectDecimal;
                m.TotalPriceMoney = d.TotalPriceMoneyDecimal;
                m.TotalPriceDifference = d.TotalPriceDifferenceDecimal;
                m.TotalPriceAfterCorrect = d.TotalPriceAfterCorrectDecimal;

            }
            return m;
        }

        public static void ParseAsDayM(this Day from, DayM to)
        {
            if (from is not null)
            {
                if (to is null)
                {
                    to = new DayM();
                }

                to.Id = from.Id;
                to.Description = from.Description;
                to.DriverGuid = from.DriverGuid;
                to.Created = from.CreatedDateTime;


                if (to.Products is null)
                {
                    to.Products = new System.Collections.ObjectModel.ObservableCollection<ProductM>();
                }
                for (int i = 0; i < from.Products.Count; i++)
                {
                    if (to.Products[i] is null)
                    {
                        to.Products[i] = new ProductM();
                    }
                    if (to.Products[i].ProductNameId == from.Products[i].ProductNameId)
                    {
                        from.Products[i].ParseAsProductM(to.Products[i]);
                    }
                }

                if (to.Cakes is null)
                {
                    to.Cakes = new System.Collections.ObjectModel.ObservableCollection<CakeM>();
                }
                from.Cakes.Clear();
                for (int i = 0; i < from.Cakes.Count; i++)
                {
                    if (to.Cakes[i] is null)
                    {
                        to.Cakes[i] = new CakeM();
                    }
                    from.Cakes[i].PareseAsCakeM(to.Cakes[i]);
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
