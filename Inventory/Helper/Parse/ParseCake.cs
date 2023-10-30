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
                p.Id = new Guid(m.Id.ToByteArray());
                p.DayId = new Guid(m.DayId.ToByteArray());
                p.PriceDecimal = m.Price;
                p.IsSell = m.IsSell;
            }
            return p;
        }
        public static void PareseAsCake(this CakeM from, Cake to)
        {
            if (from is not null)
            {
                to ??= new Cake();

                to.Id = new Guid(from.Id.ToByteArray());
                to.DayId = new Guid(from.DayId.ToByteArray());
                to.PriceDecimal = from.Price;
                to.IsSell = from.IsSell;
            }
        }
        public static CakeM PareseAsCakeM(this Cake p)
        {
            var m = new CakeM();
            if (p is not null)
            {
                m.Id = new Guid(p.Id.ToByteArray());
                m.DayId = new Guid(p.DayId.ToByteArray());
                m.Price = p.PriceDecimal;
                m.IsSell = p.IsSell;
            }
            return m;
        }

        public static void PareseAsCakeM(this Cake from, CakeM to)
        {
            if (from is not null)
            {
                to ??= new CakeM();
                to.Id = new Guid(from.Id.ToByteArray());
                to.DayId = new Guid(from.DayId.ToByteArray());
                to.Price = from.PriceDecimal;
                to.IsSell = from.IsSell;
            }
        }
    }
}
