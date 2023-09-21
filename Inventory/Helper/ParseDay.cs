using Inventory.Model;
using Inventory.Model.MVVM;

namespace Inventory.Helper
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
                d.TotalPriceProductsDecimal = m.TotalPriceProduct;
                d.TotalPriceCakeDecimal = m.TotalPriceCake;
                d.TotalPriceDecimal = m.TotalPrice;
                d.TotalPriceCorrectDecimal = m.TotalPriceCorrect;
                d.TotalPriceMoneyDecimal = m.TotalPriceMoney;
                d.TotalPriceDifferenceDecimal = m.TotalPriceDifference;
                d.TotalPriceAfterCorrectDecimal = m.TotalPriceAfterCorrect;
                for (int i = 0; i < m.Products.Count; i++)
                {
                    d.Products.Add(m.Products[i].ParseAsProduct());
                }
                for (int i = 0; i < m.Cakes.Count; i++)
                {
                    d.Cakes.Add(m.Cakes[i].PareseAsCake());
                }
            }

            return d;
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
                m.TotalPriceProduct = d.TotalPriceProductsDecimal;
                m.TotalPriceCake = d.TotalPriceCakeDecimal;
                m.TotalPrice = d.TotalPriceDecimal;
                m.TotalPriceCorrect = d.TotalPriceCorrectDecimal;
                m.TotalPriceAfterCorrect = d.TotalPriceAfterCorrectDecimal;
                m.TotalPriceDifference = d.TotalPriceDifferenceDecimal;
                m.TotalPriceMoney = d.TotalPriceMoneyDecimal;

                for (int i = 0; i < d.Products.Count; i++)
                {
                    m.Products.Add(d.Products[i].ParseAsProductM());
                }
                for (int i = 0; i < d.Cakes.Count; i++)
                {
                    m.Cakes.Add(d.Cakes[i].PareseAsCakeM());
                }
            }

            return m;
        }

        public static void ParseAsDayMOnly(this Day d, DayM m)
        {
            if (d is not null && m is not null)
            {
                m.Id = d.Id;
                m.Description = d.Description;
                m.Created = d.CreatedDateTime;
                m.TotalPriceProduct = d.TotalPriceProductsDecimal;
                m.TotalPriceCake = d.TotalPriceCakeDecimal;
                m.TotalPrice = d.TotalPriceDecimal;
                m.TotalPriceCorrect = d.TotalPriceCorrectDecimal;
                m.TotalPriceAfterCorrect = d.TotalPriceAfterCorrectDecimal;
                m.TotalPriceDifference = d.TotalPriceDifferenceDecimal;
                m.TotalPriceMoney = d.TotalPriceMoneyDecimal;
            }
        }

    }

    public static class ParseProduct
    {
        public static Product ParseAsProduct(this ProductM m)
        {
            var p = new Product();
            if (m is not null)
            {
                p.Id = m.Id;
                p.DayId = m.DayId;
                p.ProductNameId = m.ProductNameId;
                p.Description = m.Description;
                p.PriceTotalDecimal = m.PriceTotal;
                p.PriceTotalCorrectDecimal = m.PriceTotalCorrect;
                p.PriceTotalAfterCorrectDecimal = m.PriceTotalAfterCorrect;
                p.Number = m.Number;
                p.NumberEdit = m.NumberEdit;
                p.NumberReturn = m.NumberReturn;
                p.Name = m.Name.PareseAsProductName();
                p.Price = m.Price.PareseAsProductPrice();
            }

            return p;
        }
        public static ProductM ParseAsProductM(this Product p)
        {
            var m = new ProductM();
            if (p is not null)
            {
                m.Id = p.Id;
                m.DayId = p.DayId;
                m.ProductNameId = p.ProductNameId;
                m.Description = p.Description;
                m.PriceTotal = p.PriceTotalDecimal;
                m.PriceTotalCorrect = p.PriceTotalCorrectDecimal;
                m.PriceTotalAfterCorrect = p.PriceTotalAfterCorrectDecimal;
                m.Number = p.Number;
                m.NumberEdit = p.NumberEdit;
                m.NumberReturn = p.NumberReturn;
                m.Name = p.Name.PareseAsProductNameM();
                m.Price = p.Price.PareseAsProductPriceM();
            }

            return m;
        }

        public static ProductName PareseAsProductName(this ProductNameM m)
        {
            var p = new ProductName();
            if (m is not null)
            {
                p.Id = m.Id;
                p.Name = m.Name;
                p.Description = m.Description;
                p.Img = m.Img;
            }
            return p;
        }

        public static ProductNameM PareseAsProductNameM(this ProductName p)
        {
            var m = new ProductNameM();
            if (p is not null)
            {
                m.Id = p.Id;
                m.Name = p.Name;
                m.Description = p.Description;
                m.Img = p.Img;
            }
            return m;
        }

        public static ProductPrice PareseAsProductPrice(this ProductPriceM m)
        {
            var p = new ProductPrice();
            if (m is not null)
            {
                p.Id = m.Id;
                p.ProductNameId = m.productNameId;
                p.PriceDecimal = m.Price;
                p.CreatedDateTime = m.Created;
            }
            return p;
        }

        public static ProductPriceM PareseAsProductPriceM(this ProductPrice p)
        {
            var m = new ProductPriceM();
            if (p is not null)
            {
                m.Id = p.Id;
                m.productNameId = p.ProductNameId;
                m.Price = p.PriceDecimal;
                m.Created = p.CreatedDateTime;
            }
            return m;
        }

    }

    public static class ParseCake
    {
        public static Cake PareseAsCake(this CakeM m)
        {
            var p = new Cake();
            if (m is not null)
            {
                p.Id = m.Id;
                p.DayId = m.DayId;
                p.PriceDecimal = m.Price;
                p.IsSell = m.IsSell;
            }
            return p;
        }

        public static CakeM PareseAsCakeM(this Cake p)
        {
            var m = new CakeM();
            if (p is not null)
            {
                m.Id = p.Id;
                m.DayId = p.DayId;
                m.Price = p.PriceDecimal;
                m.IsSell = p.IsSell;
            }
            return m;
        }
    }

}
