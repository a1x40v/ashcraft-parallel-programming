namespace BackgroundPing.Topics;

public static class Ex02ParameterizedThread
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

            // limiting iterations
            while (counter < maxCount)
            {
                bool isNetworkUp = System.Net.NetworkInformation.NetworkInterface.GetIsNetworkAvailable();

                Console.WriteLine($"Is network available? Answer:{isNetworkUp}");
                Thread.Sleep(100);

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
            Task.Delay(500);
        }

        Console.WriteLine("Done");
        Console.ReadKey();
    }
}

/*
    Creating managed threads in .NET is accomplished by instantiating a new Thread object. The
Thread class has four constructor overloads:

    - Thread(ParameterizedThreadStart): This creates a new Thread object. It does
this by passing a delegate with a constructor that takes an object as its parameter that can be
passed when calling Thread.Start().

    - Thread(ThreadStart): This creates a new Thread object that will execute the method
to be invoked, which is provided as the ThreadStart property.

    - Thread(ParameterizedThreadStart, Int32): This adds a maxStackSize
parameter. Avoid using this overload because it is best to allow .NET to manage the stack size.

    - Thread(ThreadStart, Int32): This adds a maxStackSize parameter. Avoid
using this overload because it is best to allow .NET to manage the stack size.

    Our first example used the Thread(ThreadStart) constructor. Let’s look at a version of
that code that uses ParameterizedThreadStart to pass a value by limiting the number of
iterations of the while loop
*/