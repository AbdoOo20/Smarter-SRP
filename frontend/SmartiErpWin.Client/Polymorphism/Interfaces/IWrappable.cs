using SmartErpWin.Core.Extensions;

namespace SmartiErpWin.Client.Polymorphism.Interfaces;

public interface IWrappable
{
    public void WrapOne(Action action)
    {
        PreWrap();
        action();
        PostWrap();
    }

    public async Task WrapOneAsync(Func<Task> action)
    {
        PreWrap();
        
        try
        {
            await action();
        }
        finally
        {
            PostWrap();
        }
    }

    public void WrapOneIf(bool predicate, Action action)
    {
        if (predicate)
        {
            WrapOne(action);
        }
    }

    public unsafe void WrapOneUnmanaged(delegate*<void> action)
    {
        PreWrap();
        action();
        PostWrap();
    }
    
    public void WrapOneIf<TSpecific>(
        TSpecific target,
        Predicate<TSpecific> predicate,
        Action action)
        => WrapOneIf(predicate(target), action);

    public async Task WrapOneIfAsync(bool predicate, Func<Task> action)
    {
        if (predicate)
        {
            await WrapOneAsync(action);
        }
    }

    public async Task WrapOneIfAsync<TSpecific>(
        TSpecific target,
        Predicate<TSpecific> predicate,
        Func<Task> action)
        => await WrapOneIfAsync(predicate.Invoke(target), action);

    public void WrapMany(params Action[] actions)
    {
        WrapOne(() =>
        {
            actions.ToList().ForEach(a => a());
        });
    }
    
    public void WrapManyIf(bool predicate, params Action[] actions)
    {
        if (predicate)
        {
            WrapMany(actions);
        }
    }
    
    /// <summary>
    /// Represents a <see cref="Task"/> that iterates over an <see cref="Array"/> of specified <see cref="Task"/>s asynchronously,
    /// which waits for each <see cref="Task"/> to execute asynchronously, then executes <see cref="PostWrap"/> logic.
    /// </summary>
    /// <param name="actions">The <see cref="Array"/> of <see cref="Task"/>s to asynchronously iterate over
    /// using <see cref="IAsyncEnumerable{T}"/>.</param>
    /// <returns>A <see cref="Task"/> that wraps up the <see cref="IAsyncResult"/> of this process.</returns>
    public async Task<IAsyncResult> WrapManyAsync(params Func<Task>[] actions)
    {
        await WrapOneAsync(async () =>
        {
            await using var enumerator = actions
                .ToAsyncEnumerable()
                .GetAsyncEnumerator();

            while (await enumerator.MoveNextAsync())
            {
                await enumerator.Current();
            }
        });

        return Task.CompletedTask;
    }

    public async Task WrapManyIfAsync(bool predicate, params Func<Task>[] actions)
    {
        if (predicate)
        {
            await WrapManyAsync(actions);
        }
    }
    
    public async Task WrapManyIfAsync<TSpecific>(
        TSpecific target,
        Predicate<TSpecific> predicate,
        params Func<Task>[] action)
        => await WrapManyIfAsync(predicate(target), action);
    
    /// <summary>
    /// Executes <see cref="PreWrap"/> logic, then asynchronously waits for an asynchronous task
    /// to return a true <see cref="Boolean"/>, after that executes <see cref="PostWrap"/> logic.
    /// </summary>
    /// <param name="condition">The asynchronous condition to be met.</param>
    /// <param name="action">The <see cref="Action{T}"/> to wrap in this logic.</param>
    public async Task WrapWhileAsync(Func<Task<bool>> condition, Action action)
    {
        PreWrap();

        await Task.WhenAny(condition());

        action();
        PostWrap();
    }
    
    /// <summary>
    /// Executes <see cref="PreWrap"/> logic, then asynchronously waits for an asynchronous task
    /// to finish execution, after that executes <see cref="PostWrap"/> logic.
    /// </summary>
    /// <param name="action">The <see cref="Func{TResult}"/> to wrap in this logic,
    /// which carries a <see cref="Task"/> that is awaited.</param>
    /// <code>
    /// var loader = Api.LoadAsync();
    /// await WrapUntilAsync(loader);
    /// </code>
    public async Task WrapUntilAsync(Func<Task> action)
    {
        await WrapOneAsync(async () => await Task.WhenAny([action()]));
    }
    
    public void PostWrap();
    
    public void PreWrap() { }
}