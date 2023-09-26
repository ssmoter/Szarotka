using Inventory.Model;
using Inventory.Model.MVVM;

namespace Inventory.Helper.Parse
{
    public static class ParseDriver
    {
        public static Driver PareseAsDriver(this DriverM m)
        {
            var p = new Driver();
            if (m is not null)
            {
                p.Id = m.Id;
                p.Name = m.Name;
                p.Description = m.Description;
                p.Guid = m.Guid;
            }
            return p;
        }
        public static void PareseAsDriver(this DriverM from,Driver to)
        {
            if (from is not null)
            {
                if (to is null)
                {
                    to = new Driver();
                }
                to.Id = from.Id;
                to.Name = from.Name;
                to.Description = from.Description;
                to.Guid = from.Guid;
            }
        }
        public static DriverM PareseAsDriverM(this Driver p)
        {
            var m = new DriverM();
            if (p is not null)
            {
                m.Id = p.Id;
                m.Name = p.Name;
                m.Description = p.Description;
                m.Guid = p.Guid;
            }
            return m;
        }

        public static void PareseAsDriverM(this Driver from, DriverM to)
        {
            if (from is not null)
            {
                if (to is null)
                {
                    to = new DriverM();
                }
                to.Id = from.Id;
                to.Name = from.Name;
                to.Description = from.Description;
                to.Guid = from.Guid;
            }
        }
    }
}
