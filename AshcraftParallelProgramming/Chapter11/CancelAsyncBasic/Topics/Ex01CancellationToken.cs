namespace CancelAsyncBasic.Topics;

public static class Ex01CancellationToken
{
    public static void Run()
    {
        CancelThread();
        Console.ReadKey();
    }

    private static void CancelThread()
    {
        using CancellationTokenSource tokenSource = new();
        Console.WriteLine("Starting operation.");
        ThreadPool.QueueUserWorkItem(new WaitCallback(ProcessText), tokenSource.Token);
        Thread.Sleep(5000);
        Console.WriteLine("Requesting cancellation.");
        tokenSource.Cancel();
        Console.WriteLine("Cancellation requested.");
    }

    public static void ProcessText(object? cancelToken)
    {
        var token = cancelToken as CancellationToken?;
        string text = "";

        for (int x = 0; x < 75000; x++)
        {
            if (token != null && token.Value.IsCancellationRequested)
            {
                Console.WriteLine($"Cancellation request received.String value: {text}");
                break;
            }

            text += x + " ";
            Thread.Sleep(500);
        }
    }
}
