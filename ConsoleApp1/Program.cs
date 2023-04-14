using System;
using System.Collections.Generic;
using System.Threading;

namespace ProducerConsumerExample
{
    class Program
    {
        static Queue<int> buffer = new Queue<int>(); 
        static object bufferLock = new object(); 

        static void Main(string[] args)
        {
            
            Thread producerThread = new Thread(Producer);
            Thread consumerThread = new Thread(Consumer);

           
            producerThread.Start();
            consumerThread.Start();

           
            producerThread.Join();
            consumerThread.Join();
        }

        static void Producer()
        {
            Random random = new Random();

            while (true)
            {
                int number = random.Next(1, 101); 
                lock (bufferLock) 
                {
                    buffer.Enqueue(number);
                    Console.WriteLine($"[Producer] Produced: {number}");
                }
                Thread.Sleep(1000); 
            }
        }

        static void Consumer()
        {
            while (true)
            {
                int number;
                lock (bufferLock) 
                {
                    if (buffer.Count > 0)
                    {
                        number = buffer.Dequeue(); 
                        Console.WriteLine($"[Consumer] Consumed: {number}");
                    }
                    else
                    {
                        number = -1;
                    }
                }

                if (number == -1)
                {
                    Thread.Sleep(1000);
                }
            }
        }
    }
}