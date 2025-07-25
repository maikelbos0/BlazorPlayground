using System.Collections.Concurrent;

namespace BlazorPlayground.StateManagement;

public abstract class Dependency {
    private ConcurrentBag<IDependent> dependents = [];

    internal void AddDependent(IDependent dependent) => dependents.Add(dependent);

    protected void EvaluateDependents() {
        foreach (var dependent in dependents) {
            dependent.Evaluate();
        }
    }
}
