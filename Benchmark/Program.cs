using Benchmark.Inventory;

using BenchmarkDotNet.Running;

Console.WriteLine();

//_ = BenchmarkRunner.Run<GetCustomersList>();
//_ = BenchmarkRunner.Run<GetSingleDay>();

//var a = new CakeToCakeIsExpanded();

//var c = a.GetCakeIsExpandedCast();

_ = BenchmarkRunner.Run<DayRangeCalculationMedian>();
//_ = BenchmarkRunner.Run<CakeToCakeIsExpanded>();


Console.ReadLine();