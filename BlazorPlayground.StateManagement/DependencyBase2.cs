using System.Collections.Generic;
using System.Threading;

namespace BlazorPlayground.StateManagement;

public class DependencyBase2 {
    private readonly HashSet<IDependent2> dependents = [];
    private readonly Lock dependentsLock = new();
    protected readonly StateProvider2 stateProvider;

    public DependencyBase2(StateProvider2 stateProvider) {
        this.stateProvider = stateProvider;
    }

    public void AddDependent(IDependent2 dependent) {
        lock (dependentsLock) {
            dependents.Add(dependent);
        }
    }

    protected void EvaluateDependents() {
        // TODO transaction
        //if (!stateProvider.TryRegisterForTransaction(dependents)) {
        foreach (var dependent in dependents) {
            dependent.Evaluate();
        }
        //}
    }
}
