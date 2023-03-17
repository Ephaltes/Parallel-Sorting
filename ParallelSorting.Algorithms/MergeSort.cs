using System;
using System.Threading.Tasks;

namespace ParallelSorting.Algorithms;

public class MergeSort
{
    public void SortSequentially(int[] array, int leftIndex, int rightIndex)
    {
        if (leftIndex >= rightIndex)
            return;

        int middleIndex = (leftIndex + rightIndex) / 2;

        SortSequentially(array, leftIndex, middleIndex);
        SortSequentially(array, middleIndex + 1, rightIndex);

        Merge(array, leftIndex, middleIndex, rightIndex);
    }

    public async Task SortParallelNaiveAsync(int[] array, int leftIndex, int rightIndex)
    {
        if (leftIndex >= rightIndex)
            return;

        int middleIndex = (leftIndex + rightIndex) / 2;

        Task lowerSort = Task.Factory.StartNew(() => SortParallelNaiveAsync(array, leftIndex, middleIndex));
        Task higherSort = Task.Factory.StartNew(() => SortParallelNaiveAsync(array, middleIndex + 1, rightIndex));

        await Task.WhenAll(lowerSort, higherSort);
        Merge(array, leftIndex, middleIndex, rightIndex);
    }

    public async Task SortParallelThresholdAsync(int[] array, int leftIndex, int rightIndex, int threshold)
    {
        if (leftIndex >= rightIndex)
            return;

        int middleIndex = (leftIndex + rightIndex) / 2;

        Task lowerSort = Task.CompletedTask;
        Task higherSort = Task.CompletedTask;
        
        if(middleIndex-leftIndex > threshold)
            lowerSort = Task.Factory.StartNew(() => SortParallelThresholdAsync(array, leftIndex, middleIndex, threshold));
        else
            SortSequentially(array, leftIndex, middleIndex);

        if(rightIndex-middleIndex > threshold)
            higherSort = Task.Factory.StartNew(() => SortParallelThresholdAsync(array, middleIndex + 1, rightIndex, threshold));
        else
            SortSequentially(array, middleIndex + 1, rightIndex);

        await Task.WhenAll(lowerSort, higherSort);
        Merge(array, leftIndex, middleIndex, rightIndex);
    }

    private void Merge(int[] array, int leftIndex, int middleIndex, int rightIndex)
    {
        int[] leftArray = new int[middleIndex - leftIndex + 1];
        int[] rightArray = new int[rightIndex - middleIndex];

        Array.Copy(array, leftIndex, leftArray, 0, leftArray.Length);
        Array.Copy(array, middleIndex + 1, rightArray, 0, rightArray.Length);

        int leftIndexArray = 0;
        int rightIndexArray = 0;
        int arrayIndex = leftIndex;

        while (leftIndexArray < leftArray.Length && rightIndexArray < rightArray.Length)
        {
            if (leftArray[leftIndexArray] < rightArray[rightIndexArray])
            {
                array[arrayIndex] = leftArray[leftIndexArray];
                leftIndexArray++;
            }
            else
            {
                array[arrayIndex] = rightArray[rightIndexArray];
                rightIndexArray++;
            }

            arrayIndex++;
        }

        while (leftIndexArray < leftArray.Length)
        {
            array[arrayIndex] = leftArray[leftIndexArray];
            leftIndexArray++;
            arrayIndex++;
        }

        while (rightIndexArray < rightArray.Length)
        {
            array[arrayIndex] = rightArray[rightIndexArray];
            rightIndexArray++;
            arrayIndex++;
        }
    }
}