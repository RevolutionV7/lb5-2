using System;
using System.Threading;

class Program
{
    static Semaphore semaphore = new Semaphore(2, 2); 
    static object lockObject = new object(); 

    static bool isGreenLightNorthSouth = true;
    static bool isGreenLightWestEast = false; 

    static void Main()
    {
        
        Thread northSouthThread = new Thread(NorthSouthThreadFunction);
        Thread westEastThread = new Thread(WestEastThreadFunction);
        northSouthThread.Start();
        westEastThread.Start();

        
        northSouthThread.Join();
        westEastThread.Join();
    }

    static void NorthSouthThreadFunction()
    {
        while (true)
        {
            lock (lockObject)
            {
                isGreenLightNorthSouth = true; 
                isGreenLightWestEast = false; 
                Console.WriteLine("Світлофор північ-південь: Зелене світло");
                Monitor.PulseAll(lockObject);
            }

           
            Thread.Sleep(3000);

            lock (lockObject)
            {
                isGreenLightNorthSouth = false; 
                Console.WriteLine("Світлофор північ-південь: Червоне світло");
                Monitor.PulseAll(lockObject); 
            }

          
            lock (lockObject)
            {
                while (!isGreenLightWestEast)
                {
                    Monitor.Wait(lockObject);
                }
            }
            Thread.Sleep(3000);

           
            semaphore.WaitOne();
            Console.WriteLine("Автомобіль проїжджає захід-схід");
            semaphore.Release();

        }
    }

    static void WestEastThreadFunction()
    {
        while (true)
        {
            lock (lockObject)
            {
                isGreenLightNorthSouth = false; 
                isGreenLightWestEast = true; 
                Console.WriteLine("Світлофор захід-схід: Зелене світло");
                Monitor.PulseAll(lockObject); 
            }
            Thread.Sleep(3000);

            lock (lockObject)
            {
                isGreenLightWestEast = false; 
                Console.WriteLine("Світлофор захід-схід: Червоне світло");
                Monitor.PulseAll(lockObject);
            }

            
            lock (lockObject)
            {
                while (!isGreenLightNorthSouth)
                {
                    Monitor.Wait(lockObject);
                }
            }

            semaphore.WaitOne();
            Console.WriteLine("Автомобіль проїжджає північ-південь");
            semaphore.Release();
        }
    }
}