namespace ThreadPools.Models;

public class ThreadingTimerSample
{
    private System.Threading.Timer? _timer;

    public void StartTimer()
    {
        if (_timer == null)
        {
            InitializeTimer();
        }
    }

    public async Task DisposeTimerAsync()
    {
        if (_timer != null)
        {
            await _timer.DisposeAsync();
        }
    }

    private void InitializeTimer()
    {
        var updater = new MessageUpdater();

        _timer = new System.Threading.Timer(
            callback: new TimerCallback(TimerFired),
            state: updater,
            dueTime: 500,
            period: 1000
        );
    }

    private void TimerFired(object? state)
    {
        int messageCount = CheckForNewMessageCount();

        if (messageCount > 0 && state is MessageUpdater updater)
        {
            updater.Update(messageCount);
        }
    }

    private int CheckForNewMessageCount()
    {
        // Generate a random number of messages to return
        return new Random().Next(100);
    }
}

internal class MessageUpdater
{
    internal void Update(int messageCount)
    {
        Console.WriteLine($"Timer thread #{Environment.CurrentManagedThreadId}. You have {messageCount} new messages!");
    }
}

/*
    The constructor for the Timer class takes four parameters. The callback parameter is a
delegate to invoke on the thread pool when the timer period elapses. The state parameter
is an object to pass to the callback delegate. The dueTime parameter tells the timer
how long (in milliseconds) to wait before triggering the timer for the first time. Finally,
the period parameter specifies the interval (in milliseconds) between each delegate
invocation.

    After instantiating the timer, it will immediately start. There is no Enabled property to start or
stop this timer. When you are done with it, you should dispose of it with either the Dispose
method or the DisposeAsync method. This is happening in our DisposeTimerAsync
method
*/

/*
    The two timers serve similar purposes; however, most of the time, you will want to use System.
Threading.Timer to leverage its async nature. However, if you need to frequently stop and start
your timer processes, the System.Timers.Timer class is a better choice.
*/