namespace BackgroundPing.MyExamples;

public static class MyEx01Interrupt
{
    public static void Run()
    {
        var bgThread = new Thread(() =>
        {
            try
            {
                // only sleeping(paused) thread can be interrupted
                Thread.Sleep(Timeout.Infinite);

                while (true)
                {
                    Console.WriteLine($"Background thread is running...");
                }
            }
            catch (ThreadInterruptedException ex)
            {
                Console.WriteLine($"--- ThreadInterruptedException is thrown!");
                Console.WriteLine($"interrupted. Reason: {ex.Message}");
            }
        });

        bgThread.IsBackground = true;

        Console.WriteLine($"MainThread: starting background thread");
        bgThread.Start();

        Console.WriteLine($"MainThread: sleeping...");
        Thread.Sleep(500);

        Console.WriteLine($"MainThread: interrupting background thread");
        bgThread.Interrupt();

        Console.WriteLine("Done");
        Console.ReadKey();
    }
}
