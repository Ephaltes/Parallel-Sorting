using System;
using System.Collections.Generic;
using System.Linq;

using BenchmarkDotNet.Attributes;

using Bogus;

using ParallelSorting.Algorithms;

namespace ParallelSorting.Benchmark;

[MemoryDiagnoser]
[RPlotExporter]
public class Benchmark
{
    private const int ArraySize = 10_000_000;
    private IReadOnlyCollection<int> _arrayToSort;
    private MergeSort _mergeSort;
    private QuickSort _quickSort;

    [Params(5_000_000, 2_500_000, 1_250_000, 100_000, Int32.MinValue)]
    public int Threshold;

    [GlobalSetup]
    public void Setup()
    {
        _quickSort = new QuickSort();
        _mergeSort = new MergeSort();

        Faker faker = new();
        _arrayToSort = faker.Make(ArraySize, () => faker.Random.Int()).ToArray();
    }

    [Benchmark]
    public void QuickSortSequentially()
    {
        int[] array = _arrayToSort.ToArray();
        _quickSort.SortSequentially(array, 0, array.Length - 1);
        //Console.WriteLine(string.Join(' ',array.Select(number => number)));
        //Console.WriteLine(string.Join(' ',_arrayToSort.Select(number => number)));
    }

    [Benchmark]
    public void QuickSortParallelNaive()
    {
        int[] array = _arrayToSort.ToArray();
        _quickSort.SortParallelNaive(array, 0, array.Length - 1);
        //Console.WriteLine(string.Join(' ',array.Select(number => number)));
        //Console.WriteLine(string.Join(' ',_arrayToSort.Select(number => number)));
    }
    
    [Benchmark]
    public void QuickSortParallelThreshold()
    {
        int[] array = _arrayToSort.ToArray();
        _quickSort.SortParallelThreshold(array, 0, array.Length - 1, Threshold);
        //Console.WriteLine(string.Join(' ',array.Select(number => number)));
        //Console.WriteLine(string.Join(' ',_arrayToSort.Select(number => number)));
    }
    
    [Benchmark]
    public void MergeSortSequentially()
    {
        int[] array = _arrayToSort.ToArray();
        _mergeSort.SortSequentially(array, 0, array.Length - 1);
        //Console.WriteLine(string.Join(' ',array.Select(number => number)));
        //Console.WriteLine(string.Join(' ',_arrayToSort.Select(number => number)));
    }
    
    [Benchmark]
    public void MergeSortParallelNaive()
    {
        int[] array = _arrayToSort.ToArray();
        _mergeSort.SortParallelNaive(array, 0, array.Length - 1);
        //Console.WriteLine(string.Join(' ',array.Select(number => number)));
        //Console.WriteLine(string.Join(' ',_arrayToSort.Select(number => number)));
    }
    
    [Benchmark]
    public void MergeSortParallelThreshold()
    {
        int[] array = _arrayToSort.ToArray();
        _mergeSort.SortParallelThreshold(array, 0, array.Length - 1, Threshold);
        //Console.WriteLine(string.Join(' ',array.Select(number => number)));
        //Console.WriteLine(string.Join(' ',_arrayToSort.Select(number => number)));
    }
}