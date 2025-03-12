namespace Parallelism.Topics;

public static class Ex02ParallelForEach
{
    private static void ExecuteParallelForEach(IList<int> numbers)
    {
        Parallel.ForEach(numbers, number =>
        {
            bool timeContainsNumber = DateTime.Now.ToLongTimeString().Contains(number.ToString());

            if (timeContainsNumber)
            {
                Console.WriteLine($"The current time contains number {number}. Thread id: {Environment.CurrentManagedThreadId}");
            }
            else
            {
                Console.WriteLine($"The current time does not contain number {number}. Thread id: {Environment.CurrentManagedThreadId}");
            }
        });
    }

    public static void Run()
    {
        var numbers = new List<int> { 1, 3, 5, 7, 9, 0 };

        ExecuteParallelForEach(numbers);
    }
}

/*
    Parallel.ForEach is probably the most used member of the Parallel class in .NET. This is
because, in many cases, you can simply take the body of a standard foreach loop and use it inside
a Parallel.ForEach loop. However, when introducing any parallelism into a code base, you
must be sure that the code being invoked is thread-safe. If the body of a Parallel.ForEach loop
modifies any of the collections, you will either need to employ one of the synchronization methods
discussed in Chapter 1, or use one of .NET’s concurrent collections.
*/