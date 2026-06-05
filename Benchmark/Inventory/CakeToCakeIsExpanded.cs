using BenchmarkDotNet.Attributes;

using DataBase.Model.EntitiesInventory;

using Inventory.Pages.SingleDayPreview.SingleDayPreviewSmall;

using System.Collections.ObjectModel;

namespace Benchmark.Inventory
{

    [Config(typeof(AntiVirusFriendlyConfig))]
    [MemoryDiagnoser]
    public class CakeToCakeIsExpanded
    {
        private static ObservableCollection<Cake> cakes = [];
        public CakeToCakeIsExpanded()
        {
            for (int i = 0; i < 100; i++)
            {
                cakes.Add(new Cake() { Index = i });
            }
        }


        [Benchmark]
        public static ObservableCollection<CakeIsExpanded> GetCakeIsExpandedCast()
        {
            ObservableCollection<CakeIsExpanded> result = [.. cakes.OrderByDescending(x => x.IsSell).Cast<CakeIsExpanded>()];
            return result;
        }
        [Benchmark]
        public static ObservableCollection<CakeIsExpanded> GetCakeIsExpandedSelect()
        {
            ObservableCollection<CakeIsExpanded> result = [.. cakes.OrderByDescending(x => x.IsSell).Select(x => (x as CakeIsExpanded)!)];
            return result;
        }

    }
}
