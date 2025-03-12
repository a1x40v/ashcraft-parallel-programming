using ThreadPools.Models;

namespace ThreadPools.Topics;

public static class Ex02SystemTimersTimer
{
    public static void Run()
    {
        TimerSample timerSample = new TimerSample();

        Console.WriteLine($"Main thread #{Environment.CurrentManagedThreadId}. start timer");
        timerSample.StartTimer();

        Thread.Sleep(5_000);
        Console.WriteLine($"Main thread #{Environment.CurrentManagedThreadId}. Stop timer");
        timerSample.StopTimer();
        Thread.Sleep(5_000);

        Console.WriteLine("Done");
        Console.ReadKey();
    }
}

