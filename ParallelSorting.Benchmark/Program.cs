// See https://aka.ms/new-console-template for more information

using BenchmarkDotNet.Running;

using ParallelSorting.Benchmark;

BenchmarkRunner.Run<Benchmark>();

//Benchmark benchmark = new Benchmark();
//benchmark.QuickSortSequentially();
//benchmark.QuickSortParallelNaive();
//benchmark.QuickSortParallelThreshold();
//benchmark.MergeSortSequentially();