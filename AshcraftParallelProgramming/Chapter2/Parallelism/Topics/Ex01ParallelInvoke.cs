
namespace Parallelism.Topics;

public static class Ex01ParallelInvoke
{
    private static void DoComplexWork()
    {
        Console.WriteLine($"Hello from DoComplexWork method. Thread id: {Environment.CurrentManagedThreadId}");
    }

    public static void Run()
    {

        Parallel.Invoke(
            DoComplexWork,
            () =>
            {
                Console.WriteLine($"Hello from lambda expression. Thread id: {Environment.CurrentManagedThreadId}");
            },
            new Action(() =>
            {
                Console.WriteLine($"Hello from Action.Thread id: {Environment.CurrentManagedThreadId}");
            }),
            delegate ()
            {
                Console.WriteLine($"Hello from delegate. Thread id: {Environment.CurrentManagedThreadId}");
            }
        );
    }
}

/*
    Parallel.Invoke is a method that can execute multiple actions, and they could be executed in
parallel. There is no guarantee of the order in which they will execute. Each action will be queued in
the thread pool. The Invoke call will return when all the actions have been completed.
In this example, the Parallel.Invoke call will execute four actions: another method in the
ParallelInvokeExample class named DoComplexWork, a lambda expression, an Action
declared inline, and a delegate.
*/