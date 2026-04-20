namespace SmartiErpWin.Client.Polymorphism.Interfaces;

/// <summary>
/// Base interface to handle synchronisation between view models.
/// </summary>
public interface ISynchronisationAware
{
    private static List<ISynchronisationAware> Instances => SynchronisationInstance.SyncInstances;
    
    public void Register(ISynchronisationAware type)
    {
        // Some types register their instances more than once.
        // Since synchronisation should only work with a single instance of a type,
        // they must be removed here.
        
        var oldInstance = Instances.FirstOrDefault(i => i.GetType() == type.GetType());
        if (oldInstance is not null)
        {
            Instances.Remove(oldInstance);
        }

        Instances.Add(type);
    }

    public void Unregister(ISynchronisationAware type) => _ = Instances.Remove(type);
    
    public async void SynchronizeAll(params object[] args) => await InternalSynchronizeAsync(Instances, args);

    public async void SynchronizeType(Type? type, params object[] args)
    {
        if (type is null)
            return;
        
        Verbose("Preparing to synchronise {0}...", type.Name);

        var toSynchronise = Instances.Where(i => i.GetType() == type)
            .ToList();

        if (toSynchronise.Count == 0)
        {
            Error("Synchronising {0} gracefully failed as no registered instances " +
                  "match this type yet, continuing execution...", type.Name);

            return;
        }
        
        await InternalSynchronizeAsync(toSynchronise, args);
    }

    /// <summary>
    /// Forces synchronisation to an unregistered sync instance if it exists in the DI container.
    /// </summary>
    public async void ForceSynchronize<T>(params object[] args)
        where T: class, ISynchronisationAware
    {
        var type = App.GetService<T>();

        if (type is ISynchronisationAware sync)
        {
            await InternalSynchronizeAsync([sync], args);
        } 
    }
    
    private static async Task InternalSynchronizeAsync(
        IReadOnlyCollection<ISynchronisationAware> instances,
        params object[] args)
    {
        // Read-only to prevent mid-enumerator modifications.
        
        // ERROR: InvalidOperationException – "The collection was modified after the enumerator was created."
        
        // Probable fix: create a deep copy of the instances collection to eliminate any possible reference pass overs.

        var newInstances = instances.ToList();
        
        // Fixed: "Collection was modified; operation may not execute."
        // Explicit enumeration no longer required at this stage.
        
        var enumerator = newInstances.GetEnumerator();
        
        while (enumerator.MoveNext())
        {
            var obj = enumerator.Current;
            
            Verbose("Synchronising {0} with op args ({FD})", obj.GetType().Name, args);
            await obj.OnSynchronisationRequestedAsync(args);
        }
        
        enumerator.Dispose();
    }
    
    Task OnSynchronisationRequestedAsync(params object[] args);
    
    void SynchroniseTypes(List<Type> types, params object[] args)
    {
        types.ForEach(t => SynchronizeType(t, args));
    }
    
    void Synchronise<TV>(params object[] args)
    {
        SynchronizeType(typeof(TV), args);
    }
    
    void Synchronise<TV1, TV2>(params object[] args)
    {
        SynchroniseTypes([typeof(TV1), typeof(TV2)], args);
    }
    
    void Synchronise<TV1, TV2, TV3>(params object[] args)
    {
        SynchroniseTypes([typeof(TV1), typeof(TV2), typeof(TV3)], args);
    }
    
    void Synchronise<TV1, TV2, TV3, TV4>(params object[] args)
    {
        SynchroniseTypes([typeof(TV1), typeof(TV2), typeof(TV3), typeof(TV4)], args);
    }
    
    void Synchronise<TV1, TV2, TV3, TV4, TV5>(params object[] args)
    {
        SynchroniseTypes([typeof(TV1), typeof(TV2), typeof(TV3), typeof(TV4), typeof(TV5)], args);
    }
}

/// <summary>
/// File-scoped reference type class to maintain all the registered instances within this assembly.
/// </summary>
file static class SynchronisationInstance
{
    internal static List<ISynchronisationAware> SyncInstances { get; } = [];
}