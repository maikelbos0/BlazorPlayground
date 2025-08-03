using System.Collections.Concurrent;

namespace BlazorPlayground.StateManagement;

public abstract class DependencyBase {
    protected internal ConcurrentDictionary<IDependent, bool> dependents = new();

    public void AddDependent(IDependent dependent) => dependents.TryAdd(dependent, false);
}
