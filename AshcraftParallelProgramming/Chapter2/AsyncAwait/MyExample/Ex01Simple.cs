namespace AsyncAwait.MyExample;

public static class Ex01Simple
{
    private static async Task DoWorkAsync()
    {
        Console.WriteLine($"DoWorkAsync start. Thread ID: {Thread.CurrentThread.ManagedThreadId}");

        await Task.Delay(1000);

        Console.WriteLine($"DoWorkAsync after await. Thread ID: {Thread.CurrentThread.ManagedThreadId}");
    }

    public static async Task Run()
    {
        Console.WriteLine($"Main method start. Thread ID: {Thread.CurrentThread.ManagedThreadId}");

        await DoWorkAsync();

        Console.WriteLine($"Main method complete. Thread ID: {Thread.CurrentThread.ManagedThreadId}");
    }
}
