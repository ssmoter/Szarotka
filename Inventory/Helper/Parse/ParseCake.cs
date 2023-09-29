using Inventory.Model;
using Inventory.Model.MVVM;

namespace Inventory.Helper.Parse
{
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
        public static void PareseAsCake(this CakeM from,Cake to)
        {
            if (from is not null)
            {
                to ??= new Cake();

                to.Id = from.Id;
                to.DayId = from.DayId;
                to.PriceDecimal = from.Price;
                to.IsSell = from.IsSell;
            }
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

        public static void PareseAsCakeM(this Cake from,CakeM to)
        {
            if (from is not null)
            {
                to ??= new CakeM();
                to.Id = from.Id;
                to.DayId = from.DayId;
                to.Price = from.PriceDecimal;
                to.IsSell = from.IsSell;
            }
        }
    }
}
