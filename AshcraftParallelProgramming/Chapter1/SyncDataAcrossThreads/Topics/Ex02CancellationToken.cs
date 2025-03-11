namespace SyncDataAcrossThreads.Topics;

public static class Ex02CancellationToken
{
    private static void CheckNetworkStatus(object? data)
    {
        var cancelToken = (CancellationToken)data!;

        while (!cancelToken.IsCancellationRequested)
        {
            bool isNetworkUp = System.Net.NetworkInformation.NetworkInterface.GetIsNetworkAvailable();

            Console.WriteLine($"Is network available? Answer: {isNetworkUp} ");
        }
    }

    public static void Run()
    {
        var pingThread = new Thread(CheckNetworkStatus);
        var ctSource = new CancellationTokenSource();

        pingThread.Start(ctSource.Token);

        for (int i = 0; i < 10; i++)
        {
            Console.WriteLine("Main thread working...");
            Thread.Sleep(100);
        }

        ctSource.Cancel();
        pingThread.Join();
        ctSource.Dispose();
    }
}

/*
    Another way to listen for cancellation is by registering a delegate to be invoked when a cancellation
has been requested. Pass the delegate to the Token.Register method inside the managed thread
to receive a cancellation callback.

    bool finish = false;
    var cancelToken = (CancellationToken)data;
    cancelToken.Register(() => {
        // Clean up and end pending work
        finish = true;
    });
*/