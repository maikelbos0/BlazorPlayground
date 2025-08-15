using System.Collections.Concurrent;
using System.Linq;
using System.Threading;

namespace BlazorPlayground.StateManagement;

public abstract class DependencyRootBase : IDependency {
    protected readonly IStateProvider stateProvider;
    private readonly ConcurrentDictionary<IDependent, uint> dependents = new();
    private uint order = uint.MinValue;

    protected DependencyRootBase(IStateProvider stateProvider) {
        this.stateProvider = stateProvider;
    }

    public void AddDependent(IDependent dependent) => dependents.AddOrUpdate(dependent, _ => Interlocked.Increment(ref order), (_, _) => Interlocked.Increment(ref order));

    protected void EvaluateDependents() {
        if (!stateProvider.TryRegisterForTransaction(dependents)) {
            foreach (var dependent in dependents.OrderBy(x => x.Value)) {
                dependent.Key.Evaluate();
            }
        }
    }
}
