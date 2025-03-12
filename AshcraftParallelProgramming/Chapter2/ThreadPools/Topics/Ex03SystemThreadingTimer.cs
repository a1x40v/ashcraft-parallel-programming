using ThreadPools.Models;

namespace ThreadPools.Topics;

public static class Ex03SystemThreadingTimer
{
    public static void Run()
    {
        ThreadingTimerSample threadingTimerSample = new ThreadingTimerSample();

        Console.WriteLine($"Main thread #{Environment.CurrentManagedThreadId}. Start timer");
        threadingTimerSample.StartTimer();

        Thread.Sleep(5_000);

        Console.WriteLine("Done");
        Console.ReadKey();
    }
}
