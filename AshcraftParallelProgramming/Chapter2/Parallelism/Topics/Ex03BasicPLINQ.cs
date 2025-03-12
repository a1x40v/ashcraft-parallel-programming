namespace Parallelism.Topics;

public static class Ex03BasicPLINQ
{
    private static void ExecuteLinqQuery(IList<int> numbers)
    {
        var evenNumbers = numbers.Where(n => n % 2 == 0);
        OutputNumbers(evenNumbers, "Regular");
    }

    private static void ExecuteParallelLinqQuery(IList<int> numbers)
    {
        var evenNumbers = numbers.AsParallel().Where(n => IsEven(n));
        OutputNumbers(evenNumbers, "Parallel");
    }

    private static bool IsEven(int number)
    {
        // Task.Delay(100);
        Thread.Sleep(100);
        return number % 2 == 0;
    }

    private static void OutputNumbers(IEnumerable<int> numbers, string loopType)
    {
        var numberString = string.Join(",", numbers);
        Console.WriteLine($"{loopType} number string: {numberString}");
    }

    public static void Run()
    {
        var linqNumbers = new List<int> { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16, 17, 18, 19, 20 };

        ExecuteLinqQuery(linqNumbers);
        ExecuteParallelLinqQuery(linqNumbers);
    }
}

/*
    By adding the AsParallel method to your LINQ query, you can transform it into a PLINQ query, with the
operations after AsParallel being executed on the thread pool when necessary. There are many
factors to consider when deciding when to use PLINQ. We will discuss those in some depth in Chapter
8. For this example, we will introduce PLINQ inside a LINQ Where clause that checks whether
each given integer is an even number.
*/