namespace ThreadPools.Topics;

public static class Ex01Basic
{
    public static void Run()
    {
        Console.WriteLine("Hello, World!");

        ThreadPool.QueueUserWorkItem((o) =>
        {
            for (int i = 0; i < 20; i++)
            {
                bool isNetworkUp = System.Net.NetworkInformation.NetworkInterface.GetIsNetworkAvailable();
                Console.WriteLine($"Pool-thread #{Environment.CurrentManagedThreadId} Is network available? Answer: {isNetworkUp}");
                Thread.Sleep(100);
            }
        });

        for (int i = 0; i < 10; i++)
        {
            Console.WriteLine($"Main thread #{Environment.CurrentManagedThreadId} working...");
            // Task.Delay(500);
            Thread.Sleep(500);
        }

        Console.WriteLine("Done");
        Console.ReadKey();
    }
}

/*
    The ThreadPool class in the System.Threading namespace has been part of .NET since
the beginning. It provides developers with a pool of worker threads that they can leverage to perform
tasks in the background. In fact, that is one of the key characteristics of thread pool threads. They are
background threads that run at the default priority.

    If you were to use the ThreadPool class in a .NET 6 application, you would typically do so through the
TPL, but let’s explore how it can be used directly with ThreadPool.QueueUserWorkItem. The
following code takes the example scenario of Chapter 1, but uses a ThreadPool thread to perform the
background process

    Here, the key differences are that there is no need to set IsBackground to true, and you do
not call Start(). The process will start either as soon as the item is queued on ThreadPool or
when the next ThreadPool becomes available. While you might not explicitly use ThreadPool
frequently in your code, it is leveraged by many of the common threading features in .NET. So, it’s
important to have some understanding of how it works.
*/