using System.Collections.Generic;
using System.Linq;
using System.Threading;

namespace BlazorPlayground.StateManagement;

public abstract class DependencyRootBase : IDependency {
    protected readonly StateProvider stateProvider;
    private readonly Dictionary<IDependent, uint> dependents = new();
    private Lock dependentsLock = new();
    private uint order = uint.MinValue;

    protected DependencyRootBase(StateProvider stateProvider) {
        this.stateProvider = stateProvider;
    }

    public void AddDependent(IDependent dependent) {
        lock (dependentsLock) {
            dependents[dependent] = order++;
        }
    }

    protected void EvaluateDependents() {
        if (!stateProvider.TryRegisterForTransaction(dependents)) {
            foreach (var dependent in dependents.OrderBy(x => x.Value)) {
                dependent.Key.Evaluate();
            }
        }
    }
}
