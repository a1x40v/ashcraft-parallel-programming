namespace ThreadPools.Models;

public class TimerSample : IDisposable
{
    private System.Timers.Timer? _timer;

    public void StartTimer()
    {
        if (_timer == null)
        {
            InitializeTimer();
        }

        if (_timer != null && !_timer.Enabled)
        {
            _timer.Enabled = true;
        }
    }

    public void StopTimer()
    {
        if (_timer != null && _timer.Enabled)
        {
            _timer.Enabled = false;
        }
    }

    private void InitializeTimer()
    {
        _timer = new System.Timers.Timer
        {
            Interval = 1000
        };

        _timer.Elapsed += _timer_Elapsed;
    }

    private void _timer_Elapsed(object? sender, System.Timers.ElapsedEventArgs e)
    {
        int messageCount = CheckForNewMessageCount();

        if (messageCount > 0)
        {
            AlertUser(messageCount);
        }
    }

    private void AlertUser(int messageCount)
    {
        Console.WriteLine($"Timer thread #{Environment.CurrentManagedThreadId}. You have {messageCount} new messages!");
    }

    private int CheckForNewMessageCount()
    {
        // Generate a random number of messages to return
        return new Random().Next(100);
    }

    public void Dispose()
    {
        if (_timer != null)
        {
            _timer.Elapsed -= _timer_Elapsed;
            _timer.Dispose();
        }
    }
}

/*
    This timer will raise an Elapsed event on a thread pool thread at the interval specified in the Interval
property. The mechanism can be stopped or started by using the Boolean Enabled property. If you
need the Elapsed event to only fire once, the AutoReset property can be set to false.
*/