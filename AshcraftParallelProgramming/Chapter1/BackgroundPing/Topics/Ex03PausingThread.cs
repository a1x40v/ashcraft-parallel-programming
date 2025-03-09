namespace BackgroundPing.Topics;

public static class Ex03PausingThread
{
    public static void Run()
    {
        Console.WriteLine($"MainThread id: {Thread.CurrentThread.ManagedThreadId}");

        var bgThread = new Thread((object? data) =>
        {

            if (data is null) return;

            int counter = 0;
            var result = int.TryParse(data.ToString(), out int maxCount);
            if (!result) return;

            Console.WriteLine($"BackgroundTthread id: {Thread.CurrentThread.ManagedThreadId}");

            while (counter < maxCount)
            {
                bool isNetworkUp = System.Net.NetworkInformation.NetworkInterface.GetIsNetworkAvailable();

                Console.WriteLine($"Is network available? Answer:{isNetworkUp}");

                // Thread.Sleep(100);
                Thread.Sleep(10);

                Console.WriteLine($"Iteration #{counter} completed");
                counter++;
            }
        });

        bgThread.IsBackground = true;

        Console.WriteLine($"BackgroundTthread state before Start: {bgThread.ThreadState}");

        bgThread.Start(12);

        Console.WriteLine($"BackgroundTthread state after Start: {bgThread.ThreadState}");

        Console.WriteLine($"BackgroundTthread IsAlive property after Start: {bgThread.IsAlive}");

        for (int i = 0; i < 10; i++)
        {
            Console.WriteLine("Main thread working...");
            // Task.Delay(500);
            Thread.Sleep(100);
        }

        Console.WriteLine("Done");
        Console.ReadKey();
    }
}

/*
    Thread.Sleep is a static method that will block the current thread for the
number of milliseconds specified. It is not possible to call Thread.Sleep on a thread other than
the current one.

    We have already used Thread.Sleep in the examples in this chapter, but let’s change the code
slightly to see how it can impact the order of events. Change the Thread.Sleep interval inside the
thread to 10, remove the code that makes it a background thread, and change the Task.Delay()
call to Thread.Sleep(100):

    When running the application again, you can see that putting a greater delay on the primary thread
allows the process inside bgThread to begin executing before the primary thread completes its work
*/

/*
    Additionally, it is possible to pass Timeout.Infinite to Thread.Sleep. This will cause
the thread to pause until it is interrupted or aborted by another thread or the managed environment.
Interrupting a blocked or paused thread is accomplished by calling Thread.Interrupt. When
a thread is interrupted, it will receive a ThreadInterruptedException exception.

    The exception handler should allow the thread to continue working or clean up any remaining work.
If the exception is unhandled, the runtime will catch the exception and stop the thread. Calling
Thread.Interrupt on a running thread will have no effect until that thread has been blocked.
*/