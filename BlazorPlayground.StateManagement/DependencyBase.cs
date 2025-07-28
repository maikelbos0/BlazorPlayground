using System.Collections.Concurrent;

namespace BlazorPlayground.StateManagement;

public abstract class DependencyBase {
    private ConcurrentDictionary<IDependent, bool> dependents = new();

    public void AddDependent(IDependent dependent) => dependents.TryAdd(dependent, false);

    protected void EvaluateDependents() {
        foreach (var dependent in dependents.Keys) {
            dependent.Evaluate();
        }
    }
}
