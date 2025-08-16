using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

namespace BlazorPlayground.StateManagement;

public class StateProvider : IStateProvider, IDisposable {
    private readonly ThreadLocal<HashSet<IDependent>> trackedDependents = new(() => []);
    private readonly ThreadLocal<Dictionary<IDependent, nuint>?> transactionDependents = new();
    private bool isDisposed = false;

    public MutableState<T> Mutable<T>(T value)
        => new(this, value);

    public MutableState<T> Mutable<T>(T value, IEqualityComparer<T> equalityComparer)
        => new(this, value, equalityComparer);

    public ComputedState<T> Computed<T>(Func<T> computation)
        => new(this, computation);

    public void Effect(Action effect)
        => _ = new Effect(this, effect);

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

    public bool TryRegisterForTransaction(IEnumerable<KeyValuePair<IDependent, nuint>> dependents) {
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

    public void ExecuteTransaction(Action transaction) {
        var isNested = true;

        if (transactionDependents.Value == null) {
            transactionDependents.Value = [];
            isNested = false;
        }

        transaction();

        if (!isNested) {
            foreach (var dependent in transactionDependents.Value.OrderBy(x => x.Value)) {
                dependent.Key.Evaluate();
            }

            transactionDependents.Value = null;
        }
    }
}
