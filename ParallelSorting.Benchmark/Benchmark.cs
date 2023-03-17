using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using BenchmarkDotNet.Attributes;

using Bogus;

using ParallelSorting.Algorithms;

namespace ParallelSorting.Benchmark;

[MemoryDiagnoser]
[RPlotExporter]
public class Benchmark
{
    private const int ArraySize = 10_000_000;
    private IReadOnlyCollection<int> _arrayToSort = null!;
    private MergeSort _mergeSort = null!;
    private QuickSort _quickSort = null!;

    [Params(2_500_000, 1_250_000, 100_000, 10_000, 1000)]
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
    public async Task QuickSortParallelNaive()
    {
        int[] array = _arrayToSort.ToArray();
        await _quickSort.SortParallelNaiveAsync(array, 0, array.Length - 1);
        //Console.WriteLine(string.Join(' ',array.Select(number => number)));
        //Console.WriteLine(string.Join(' ',_arrayToSort.Select(number => number)));
    }

    [Benchmark]
    public async Task QuickSortParallelThreshold()
    {
        int[] array = _arrayToSort.ToArray();
        await _quickSort.SortParallelThresholdAsync(array, 0, array.Length - 1, Threshold);
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
    public async Task MergeSortParallelNaive()
    {
        int[] array = _arrayToSort.ToArray();
        await _mergeSort.SortParallelNaiveAsync(array, 0, array.Length - 1);
        //Console.WriteLine(string.Join(' ',array.Select(number => number)));
        //Console.WriteLine(string.Join(' ',_arrayToSort.Select(number => number)));
    }

    [Benchmark]
    public async Task MergeSortParallelThreshold()
    {
        int[] array = _arrayToSort.ToArray();
        await _mergeSort.SortParallelThresholdAsync(array, 0, array.Length - 1, Threshold);
        //Console.WriteLine(string.Join(' ',array.Select(number => number)));
        //Console.WriteLine(string.Join(' ',_arrayToSort.Select(number => number)));
    }
}