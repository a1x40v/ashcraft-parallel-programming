namespace BackgroundPing.Topics;

public static class Ex01SimpleThread
{
    public static void Run()
    {
        Console.WriteLine($"MainThread id: {Thread.CurrentThread.ManagedThreadId}");

        var bgThread = new Thread(() =>
        {
            Console.WriteLine($"BackgroundTthread id: {Thread.CurrentThread.ManagedThreadId}");

            while (true)
            {
                bool isNetworkUp = System.Net.NetworkInformation.NetworkInterface.GetIsNetworkAvailable();

                Console.WriteLine($"Is network available? Answer:{isNetworkUp}");
                Thread.Sleep(100);
            }
        });

        bgThread.IsBackground = true;

        Console.WriteLine($"BackgroundTthread state before Start: {bgThread.ThreadState}");

        bgThread.Start();

        Console.WriteLine($"BackgroundTthread state after Start: {bgThread.ThreadState}");

        Console.WriteLine($"BackgroundTthread IsAlive property after Start: {bgThread.IsAlive}");

        for (int i = 0; i < 10; i++)
        {
            Console.WriteLine("Main thread working...");
            Task.Delay(500);
        }

        Console.WriteLine("Done");
        Console.ReadKey();
    }
}

/*
    Threads also have a Name property that defaults to null if they have never been set. Once a Name
property is set on a thread, it cannot be changed. If you attempt to set the Name property of a thread
that is not null, it will throw InvalidOperationException.
*/