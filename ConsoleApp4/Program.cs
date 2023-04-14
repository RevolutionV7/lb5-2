using System;
using System.Threading;

class Program
{
    static int[,] matrix1; 
    static int[,] matrix2; 
    static int[,] result; 
    static int matrixSize = 3; 
    static Semaphore semaphore; 
    static int maxThreads = 2; 
    static int runningThreads = 0; 
    static object lockObject = new object();

    static void Main(string[] args)
    {
        matrix1 = new int[matrixSize, matrixSize];
        matrix2 = new int[matrixSize, matrixSize];
        result = new int[matrixSize, matrixSize];
        semaphore = new Semaphore(1, 1);

       
        Random random = new Random();
        for (int i = 0; i < matrixSize; i++)
        {
            for (int j = 0; j < matrixSize; j++)
            {
                matrix1[i, j] = random.Next(1, 10);
                matrix2[i, j] = random.Next(1, 10);
            }
        }

        Console.WriteLine("Перша матриця:");
        PrintMatrix(matrix1);

        Console.WriteLine("\nДруга матриця:");
        PrintMatrix(matrix2);

        Console.WriteLine("\nРезультат множення матриць:");
        MultiplyMatrices();

        PrintMatrix(result);
    }

    static void MultiplyMatrices()
    {
        for (int i = 0; i < matrixSize; i++)
        {
            for (int j = 0; j < matrixSize; j++)
            {
              
                Monitor.Enter(lockObject);
                try
                {
                    
                    while (runningThreads >= maxThreads)
                    {
                        Monitor.Wait(lockObject);
                    }
                    Thread thread = new Thread(() =>
                    {
                        MultiplyMatrixElements(i, j);
                        
                        semaphore.Release();
                       
                        Interlocked.Decrement(ref runningThreads);
                        
                        Monitor.Pulse(lockObject);
                    });
                    thread.Start();
                   
                    Interlocked.Increment(ref runningThreads);
                }
                finally
                {
                    Monitor.Exit(lockObject);
                }
            }
        }
       
        while (runningThreads > 0)
        {
            Thread.Sleep(10);
        }
    }

    static void MultiplyMatrixElements(int row, int col)
    {
        int sum = 0;
        for (int i = 0; i < matrixSize; i++)
        {
            sum += matrix1[row, i] * matrix2[i, col];
        }
       
        semaphore.WaitOne();
        result[row, col] = sum;
    }

    static void PrintMatrix(int[,] matrix)
    {
        for (int i = 0; i < matrixSize; i++)
        {
            for (int j = 0; j < matrixSize; j++)
            {
                Console.Write(matrix[i, j] + " ");
            }
            Console.WriteLine();
        }
    }
}


