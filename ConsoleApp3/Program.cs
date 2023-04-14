using System;
using System.Threading;

class Program
{
    static object lockObject = new object(); 

    static void Main(string[] args)
    {
        Console.WriteLine("Введiть масив цiлих чисел через пробiл:");
        string input = Console.ReadLine();
        int[] arr = Array.ConvertAll(input.Split(' '), int.Parse);

        Console.WriteLine("\nНесортований масив:");
        PrintArray(arr);

       
        QuickSort(arr, 0, arr.Length - 1);

        Console.WriteLine("\nВiдсортований масив:");
        PrintArray(arr);
    }

    static void QuickSort(int[] arr, int low, int high)
    {
        if (low < high)
        {
            int pivotIndex = Partition(arr, low, high);

            Thread leftThread = new Thread(() => QuickSort(arr, low, pivotIndex - 1));
            Thread rightThread = new Thread(() => QuickSort(arr, pivotIndex + 1, high));

           
            leftThread.Start();
            rightThread.Start();

            
            leftThread.Join();
            rightThread.Join();
        }
    }

    static int Partition(int[] arr, int low, int high)
    {
        int pivot = arr[high];
        int i = low - 1;

        for (int j = low; j < high; j++)
        {
            if (arr[j] < pivot)
            {
                i++;
                Swap(arr, i, j);
            }
        }

        Swap(arr, i + 1, high);

        return i + 1;
    }

    static void Swap(int[] arr, int i, int j)
    {
        lock (lockObject) 
        {
            int temp = arr[i];
            arr[i] = arr[j];
            arr[j] = temp;
        }
    }

    static void PrintArray(int[] arr)
    {
        foreach (var item in arr)
        {
            Console.Write(item + " ");
        }
        Console.WriteLine();
    }
}
