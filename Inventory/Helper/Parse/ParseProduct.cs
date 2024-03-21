using DataBase.Model.EntitiesInventory;

using Inventory.Model.MVVM;

namespace Inventory.Helper.Parse
{
    public static class ParseProduct
    {
        public static Product ParseAsProduct(this ProductM m)
        {
            var p = new Product();
            if (m is not null)
            {
                m.CanUpadte = false;
                p.Id = m.Id;
                p.DayId = m.DayId;
                p.ProductNameId = m.ProductNameId;
                p.ProductPriceId = m.ProductPriceId;
                p.Description = m.Description;
                p.PriceTotalDecimal = m.PriceTotal;
                p.PriceTotalCorrectDecimal = m.PriceTotalCorrect;
                p.PriceTotalAfterCorrectDecimal = m.PriceTotalAfterCorrect;
                p.Number = m.Number;
                p.NumberEdit = m.NumberEdit;
                p.NumberReturn = m.NumberReturn;
                p.Name = m.Name.PareseAsProductName();
                p.Price = m.Price.PareseAsProductPrice();
                m.CanUpadte = true;
            }

            return p;
        }
        public static void ParseAsProduct(this ProductM from, Product to)
        {
            if (from is not null)
            {
                from.CanUpadte = false;
                to ??= new Product();
                to.Id = from.Id;
                to.DayId = from.DayId;
                to.ProductNameId = from.ProductNameId;
                to.ProductPriceId = from.ProductPriceId;
                to.Description = from.Description;
                to.PriceTotalDecimal = from.PriceTotal;
                to.PriceTotalCorrectDecimal = from.PriceTotalCorrect;
                to.PriceTotalAfterCorrectDecimal = from.PriceTotalAfterCorrect;
                to.Number = from.Number;
                to.NumberEdit = from.NumberEdit;
                to.NumberReturn = from.NumberReturn;
                from.Name.PareseAsProductName(to.Name);
                from.Price.PareseAsProductPrice(to.Price);
                from.CanUpadte = true;
            }
        }
        public static ProductM ParseAsProductM(this Product p)
        {
            var m = new ProductM();
            if (p is not null)
            {
                m.CanUpadte = false;
                m.Id = p.Id;
                m.DayId = p.DayId;
                m.ProductNameId = p.ProductNameId;
                m.ProductPriceId = p.ProductPriceId;
                m.Description = p.Description;
                m.PriceTotal = p.PriceTotalDecimal;
                m.PriceTotalCorrect = p.PriceTotalCorrectDecimal;
                m.PriceTotalAfterCorrect = p.PriceTotalAfterCorrectDecimal;
                m.Number = p.Number;
                m.NumberEdit = p.NumberEdit;
                m.NumberReturn = p.NumberReturn;
                m.Name = p.Name.PareseAsProductNameM();
                m.Price = p.Price.PareseAsProductPriceM();
                m.CanUpadte = true;
            }

            return m;
        }
        public static void ParseAsProductM(this Product from, ProductM to)
        {
            if (from is not null)
            {
                to ??= new ProductM();
                to.CanUpadte = false;
                to.Id = from.Id;
                to.DayId = from.DayId;
                to.ProductNameId = from.ProductNameId;
                to.ProductPriceId = from.ProductPriceId;

                to.Description = from.Description;
                to.PriceTotal = from.PriceTotalDecimal;
                to.PriceTotalCorrect = from.PriceTotalCorrectDecimal;
                to.PriceTotalAfterCorrect = from.PriceTotalAfterCorrectDecimal;
                to.Number = from.Number;
                to.NumberEdit = from.NumberEdit;
                to.NumberReturn = from.NumberReturn;
                from.Name.PareseAsProductNameM(to.Name);
                from.Price.PareseAsProductPriceM(to.Price);
                to.CanUpadte = false;
            }
        }


        #region Name


        public static ProductName PareseAsProductName(this ProductNameM m)
        {
            var p = new ProductName();
            if (m is not null)
            {
                p.Id = m.Id;
                p.Name = m.Name;
                p.Description = m.Description;
                p.Img = m.Img;
                p.Arrangement = m.Arrangement;
            }
            return p;
        }
        public static void PareseAsProductName(this ProductNameM from, ProductName to)
        {
            if (from is not null)
            {
                to.Id = from.Id;
                to.Name = from.Name;
                to.Description = from.Description;
                to.Img = from.Img;
                to.Arrangement = from.Arrangement;
            }
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
                m.Arrangement = p.Arrangement;
            }
            return m;
        }

        public static void PareseAsProductNameM(this ProductName from, ProductNameM to)
        {
            if (from is not null)
            {
                to.Id = from.Id;
                to.Name = from.Name;
                to.Description = from.Description;
                to.Img = from.Img;
                to.Arrangement = from.Arrangement;
            }
        }

        #endregion

        #region Price



        public static ProductPrice PareseAsProductPrice(this ProductPriceM m)
        {
            var p = new ProductPrice();
            if (m is not null)
            {
                p.Id = m.Id;
                p.ProductNameId = m.ProductNameId;
                p.PriceDecimal = m.Price;
                p.Created = m.Created;
            }
            return p;
        }
        public static void PareseAsProductPrice(this ProductPriceM from, ProductPrice to)
        {
            if (from is not null)
            {
                to ??= new();
                to.Id = from.Id;
                to.ProductNameId = from.ProductNameId;
                to.PriceDecimal = from.Price;
                to.Created = from.Created;
            }
        }
        public static ProductPriceM PareseAsProductPriceM(this ProductPrice p)
        {
            var m = new ProductPriceM();
            if (p is not null)
            {
                m.Id = p.Id;
                m.ProductNameId = p.ProductNameId;
                m.Price = p.PriceDecimal;
                m.Created = p.Created;
            }
            return m;
        }
        public static void PareseAsProductPriceM(this ProductPrice from, ProductPriceM to)
        {
            if (from is not null)
            {
                to ??= new ProductPriceM();

                to.Id = from.Id;
                to.ProductNameId = from.ProductNameId;
                to.Price = from.PriceDecimal;
                to.Created = from.Created;
            }
        }

        #endregion
    }
}
