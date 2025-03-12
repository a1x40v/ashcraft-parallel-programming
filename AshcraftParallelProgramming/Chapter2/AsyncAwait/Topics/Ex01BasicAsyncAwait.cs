
using AsyncAwait.Models;

namespace AsyncAwait.Topics;

public static class Ex01BasicAsyncAwait
{
    public async static Task Run()
    {
        var networkHelper = new NetworkHelper();

        await networkHelper.CheckNetworkStatusAsync();

        Console.WriteLine("Main method complete.");
        Console.ReadKey();
    }
}
