namespace Leapay.Client.Runners;

public sealed class DebouncedSearchRunner
{
    private readonly Func<string, CancellationToken, Task> _searchAction;
    private readonly int _delayMs;

    private CancellationTokenSource _currentCts;
    private readonly SemaphoreSlim _searchLock = new(1, 1);

    public DebouncedSearchRunner(
        Func<string, CancellationToken, Task> searchAction,
        int delayMs = 1000)
    {
        _searchAction = searchAction;
        _delayMs = delayMs;
    }

    public void Trigger(string text)
    {
        // Cancel previous debounce cycle
        _currentCts?.Cancel();

        // Create a brand-new CTS for this trigger
        var cts = new CancellationTokenSource();
        _currentCts = cts;

        _ = RunDebounced(text, cts);
    }

    private async Task RunDebounced(string text, CancellationTokenSource cts)
    {
        try
        {
            // Proper debounce: if Trigger() is called again,
            // cts.Cancel() immediately cancels this delay.
            await Task.Delay(_delayMs, cts.Token);

            // Ensure only one search logic runs at a time
            await _searchLock.WaitAsync(cts.Token);

            _ = _searchAction(text, cts.Token).ContinueWith(_ =>
            {
                _searchLock.Release();
            });
        }
        catch (OperationCanceledException)
        {
            // Expected — typing reset the cooldown
        }
        catch (Exception ex)
        {
            Console.WriteLine("DebouncedSearchRunner error: " + ex);
        }
    }
}