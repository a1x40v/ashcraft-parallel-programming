using System.Threading.Tasks.Dataflow;

namespace Dataflow.Topics;

public static class Ex01JoinBlock
{
    public static void Run()
    {
        var stringQueue = new BufferBlock<string>();
        var integerQueue = new BufferBlock<int>();
        var joinStringsAndIntegers = new JoinBlock<string, int>(
            new GroupingDataflowBlockOptions
            {
                Greedy = false
            }
        );

        var stringIntegerAction = new ActionBlock<Tuple<string, int>>(data =>
        {
            Console.WriteLine($"String received: {data.Item1}");
            Console.WriteLine($"Integer received: {data.Item2}");
        });

        /*  create the links between the blocks */
        stringQueue.LinkTo(joinStringsAndIntegers.Target1);
        integerQueue.LinkTo(joinStringsAndIntegers.Target2);
        joinStringsAndIntegers.LinkTo(stringIntegerAction);

        /*  push some data to the two BufferBlock objects, wait for a second, and then mark
            them both as complete */
        stringQueue.Post("one");
        stringQueue.Post("two");
        stringQueue.Post("three");
        integerQueue.Post(1);
        integerQueue.Post(2);
        integerQueue.Post(3);
        stringQueue.Complete();
        integerQueue.Complete();
        Thread.Sleep(1000);
        Console.WriteLine("Complete");
    }
}

/*
    A JoinBlock can be configured to receive different data types from two or three data sources. As
each set of data types is completed, the block is completed with a Tuple containing all three object
types to be acted upon. In this example, we will create a JoinBlock that accepts a string and
int pair and passes Tuple(string, int) along to an ActionBlock, which outputs their
values to the console.

    Setting the block to non-greedy mode means it will wait for an item of each type before
executing the block.
*/