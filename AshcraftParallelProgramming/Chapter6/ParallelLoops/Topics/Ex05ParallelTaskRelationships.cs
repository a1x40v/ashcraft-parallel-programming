namespace ParallelLoops.Topics;

public static class Ex05ParallelTaskRelationships
{
    public static void Run()
    {
        var parallelWork = new ParallelWork();
        // parallelWork.DoAllWork();
        // parallelWork.DoAllWorkAttached();
        parallelWork.DoAllWorkDenyAttach();
        Console.ReadKey();
    }

    private class ParallelWork
    {
        private void DoFirstItem()
        {
            Console.WriteLine("Starting DoFirstItem");
            Thread.SpinWait(1000000);
            Console.WriteLine("Finishing DoFirstItem");
        }

        private void DoSecondItem()
        {
            Console.WriteLine("Starting DoSecondItem");
            Thread.SpinWait(1000000);
            Console.WriteLine("Finishing DoSecondItem");
        }

        private void DoThirdItem()
        {
            Console.WriteLine("Starting DoThirdItem");
            Thread.SpinWait(1000000);
            Console.WriteLine("Finishing DoThirdItem");
        }

        public void DoAllWork()
        {
            Console.WriteLine("Starting DoAllWork");
            Task parentTask = Task.Factory.StartNew(() =>
            {
                var child1 = Task.Factory.StartNew(DoFirstItem);
                var child2 = Task.Factory.StartNew(DoSecondItem);
                var child3 = Task.Factory.StartNew(DoThirdItem);
            });
            parentTask.Wait();
            Console.WriteLine("Finishing DoAllWork");
        }

        public void DoAllWorkAttached()
        {
            Console.WriteLine("Starting DoAllWorkAttached");
            Task parentTask = Task.Factory.StartNew(() =>
            {
                var child1 = Task.Factory.StartNew(DoFirstItem, TaskCreationOptions.AttachedToParent);
                var child2 = Task.Factory.StartNew(DoSecondItem, TaskCreationOptions.AttachedToParent);
                var child3 = Task.Factory.StartNew(DoThirdItem, TaskCreationOptions.AttachedToParent);
            });
            parentTask.Wait();
            Console.WriteLine("Finishing DoAllWorkAttached");
        }

        public void DoAllWorkDenyAttach()
        {
            Console.WriteLine("Starting DoAllWorkDenyAttach");
            Task parentTask = Task.Factory.StartNew(() =>
            {
                var child1 = Task.Factory.StartNew(DoFirstItem, TaskCreationOptions.AttachedToParent);
                var child2 = Task.Factory.StartNew(DoSecondItem, TaskCreationOptions.AttachedToParent);
                var child3 = Task.Factory.StartNew(DoThirdItem, TaskCreationOptions.AttachedToParent);
            },
            TaskCreationOptions.DenyChildAttach);

            parentTask.Wait();
            Console.WriteLine("Finishing DoAllWorkDenyAttach");
        }
    }
}

/*
    When executing nested tasks, by default, the parent task will not wait for its child tasks unless we use
the Wait() method or await statements. However, this default behavior can be controlled with
some options when using Task.Factory.StartNew().
*/

/*
    One final note about this example. You may have noticed that we used Task.Factory.StartNew
instead of Task.Run, even when we didn’t need to set TaskCreationOption. That is because
Task.Run will prohibit any child tasks from attaching to a parent. If you used Task.Run for the
parent task in the DoAllWorkAttached method, the parent would have completed first, as it
did in the other methods.
*/