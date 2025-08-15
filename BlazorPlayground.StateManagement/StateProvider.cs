using System;
using System.Collections.Generic;
using System.Threading;

namespace BlazorPlayground.StateManagement;

public class StateProvider : IStateProvider, IDisposable {
    private readonly ThreadLocal<HashSet<IDependent>> trackedDependents = new(() => []);
    private readonly ThreadLocal<Dictionary<IDependent, int>?> transactionDependents = new();
    private bool isDisposed = false;

    public MutableState<T> Mutable<T>(T value)
        => new(this, value);

    public MutableState<T> Mutable<T>(T value, IEqualityComparer<T> equalityComparer)
        => new(this, value, equalityComparer);

    public ComputedState<T> Computed<T>(Func<T> computation)
        => new(this, computation);

    public void Effect(Action effect)
        => _ = new Effect(this, effect);

    public bool TryRegisterForTransaction(IEnumerable<KeyValuePair<IDependent, int>> dependents) {
        if (transactionDependents.Value == null) {
            return false;
        }

        foreach (var dependent in dependents) {
            if (!transactionDependents.Value.TryAdd(dependent.Key, dependent.Value)) {
                transactionDependents.Value[dependent.Key] += dependent.Value;
            }
        }

        return true;
    }

    public void BuildDependencyGraph(IDependent dependent, Action action) {
        trackedDependents.Value!.Add(dependent);
        
        action();

        trackedDependents.Value.Remove(dependent);
    }

    public void TrackDependency(IDependency dependency) {
        foreach (var dependent in trackedDependents.Value!) {
            dependency.AddDependent(dependent);

            if (dependent is DependencyBranchBase dependencyBranch) {
                dependencyBranch.AddDependency(dependency);
            }
        }
    }

    public void Dispose() {
        Dispose(true);
        GC.SuppressFinalize(this);
    }

    protected virtual void Dispose(bool disposing) {
        if (!isDisposed) {
            isDisposed = true;

            if (disposing) {
                trackedDependents.Dispose();
            }
        }
    }
}
