using System.Threading.Tasks;

namespace ParallelSorting.Algorithms;

public class QuickSort
{
    public void SortSequentially(int[] array, int leftIndex, int rightIndex)
    {
        if (leftIndex >= rightIndex)
            return;

        int pivotIndex = Partition(array, leftIndex, rightIndex);

        SortSequentially(array, leftIndex, pivotIndex - 1);
        SortSequentially(array, pivotIndex + 1, rightIndex);
    }

    public async Task SortParallelNaiveAsync(int[] array, int leftIndex, int rightIndex)
    {
        if (leftIndex >= rightIndex)
            return;

        int pivotIndex = Partition(array, leftIndex, rightIndex);

        Task lowerSort = Task.Factory.StartNew(() => SortParallelNaiveAsync(array, leftIndex, pivotIndex - 1));

        Task higherSort = Task.Factory.StartNew(() => SortParallelNaiveAsync(array, pivotIndex + 1, rightIndex));

        await Task.WhenAll(lowerSort, higherSort);
    }

    public async Task SortParallelThresholdAsync(int[] array, int leftIndex, int rightIndex, int threshold)
    {
        if (leftIndex >= rightIndex)
            return;

        int pivotIndex = Partition(array, leftIndex, rightIndex);

        Task lowerTask = Task.CompletedTask;
        Task higherTask = Task.CompletedTask;

        if (pivotIndex - leftIndex > threshold)
            lowerTask = Task.Factory.StartNew(() => SortParallelThresholdAsync(array, leftIndex, pivotIndex - 1, threshold));
        else
            SortSequentially(array, leftIndex, pivotIndex - 1);

        if (rightIndex - pivotIndex > threshold)
            higherTask = Task.Factory.StartNew(() => SortParallelThresholdAsync(array, pivotIndex + 1, rightIndex, threshold));
        else
            SortSequentially(array, pivotIndex + 1, rightIndex);

        await Task.WhenAll(lowerTask, higherTask);
    }

    private int Partition(int[] array, int leftIndex, int rightIndex)
    {
        int pivot = array[rightIndex];
        int i = leftIndex - 1;

        for (int j = leftIndex; j < rightIndex; j++)
        {
            if (array[j] < pivot)
            {
                i++;
                Swap(array, i, j);
            }
        }

        Swap(array, i + 1, rightIndex);

        return i + 1;
    }

    private void Swap(int[] array, int leftIndex, int rightIndex)
    {
        (array[leftIndex], array[rightIndex]) = (array[rightIndex], array[leftIndex]);
    }
}