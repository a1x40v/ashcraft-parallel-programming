namespace CancelAsyncBasic.Topics;

public static class Ex02CancelParallelLoop
{
    public static void Run()
    {
        CancelParallelFor();
        Console.ReadKey();
    }

    private static void CancelParallelFor()
    {
        using CancellationTokenSource tokenSource = new();
        Console.WriteLine("Press a key to start, then press 'x' to send cancellation.");
        Console.ReadKey();

        Task.Run(() =>
        {
            if (Console.ReadKey().KeyChar == 'x') tokenSource.Cancel();
            Console.WriteLine();
            Console.WriteLine("press a key");
        });

        ProcessTextParallel(tokenSource.Token);
    }

    public static void ProcessTextParallel(object? cancelToken)
    {
        var token = cancelToken as CancellationToken?;

        if (token == null) return;

        string text = "";

        ParallelOptions options = new()
        {
            CancellationToken = token.Value,
            MaxDegreeOfParallelism =
                Environment.ProcessorCount
        };

        try
        {
            Parallel.For(0, 75000, options, (x) =>
            {
                text += x + " ";
                Thread.Sleep(500);
            });
        }
        catch (OperationCanceledException e)
        {
            Console.WriteLine($"Text value: {text}.{Environment.NewLine} Exception encountered: {e.Message}");
        }
    }
}
