using BenchmarkDotNet.Attributes;

using Inventory.Helper.Calculations;

namespace Benchmark.Inventory
{
    [Config(typeof(AntiVirusFriendlyConfig))]
    [MemoryDiagnoser]
    public class DayRangeCalculationMedian
    {
        readonly double[] Range;
        public DayRangeCalculationMedian()
        {
            Range = [.. Enumerable.Range(0, 100)];
        }
        [Benchmark]
        public decimal SpanMedian()
        {
            decimal result = Range.MedianSpan(x => (decimal)x);
            return result;
        }
        [Benchmark]
        public double? Median()
        {
            double? result = Range.Median(x => x);
            return result;
        }
    }
}
