namespace AsyncAwait.Models;

public class NetworkHelper
{
    private async Task NetworkCheckInternalAsync()
    {
        Console.WriteLine($"thId {Environment.CurrentManagedThreadId}");

        for (int i = 0; i < 10; i++)
        {
            bool isNetworkUp = System.Net.NetworkInformation.NetworkInterface.GetIsNetworkAvailable();

            Console.WriteLine($"Is network available? Answer: {isNetworkUp}");

            await Task.Delay(100);
        }
    }

    public async Task CheckNetworkStatusAsync()
    {
        Console.WriteLine($"caller thId {Environment.CurrentManagedThreadId}");

        Task t = NetworkCheckInternalAsync();

        for (int i = 0; i < 8; i++)
        {
            Console.WriteLine("Top level method working...");
            await Task.Delay(500);
        }

        await t;
    }
}

/*
    Both methods have an async modifier, indicating that they will be awaiting some work and will run asynchronously.
Inside the methods, we are using the await keyword with the calls to Task.Delay.
This will ensure that no code after this point will execute until the awaited method has been
completed. However, during this time, the active thread can be released to perform other
work elsewhere.

    Finally, look at the call to NetworkCheckInternalAsync. Instead of awaiting this
call, we are capturing the returned Task instance in a variable named t, and we don’t
await it until after the for loop. This means that the for loops in both methods will
run concurrently. If we had, instead, awaited NetworkCheckInternalAsync,
its for loop would have been completed before the for loop in
CheckNetworkStatusAsync could begin.
*/