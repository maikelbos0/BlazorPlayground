using System;
using System.Collections.Generic;
using System.Threading;

namespace BlazorPlayground.StateManagement;

public class StateProvider : IDisposable, IStateProvider {
    private readonly ThreadLocal<HashSet<IDependent>> trackedDependents = new(() => []);
    private readonly ThreadLocal<HashSet<IDependentDependency>> trackedDependentDependencies = new(() => []);
    private readonly ThreadLocal<HashSet<IDependent>?> transactionDependents = new();
    private bool isDisposed = false;
    private uint version = uint.MinValue;

    public uint Version => version;

    public MutableState<T> Mutable<T>(T value)
        => new(this, value);

    public MutableState<T> Mutable<T>(T value, IEqualityComparer<T> equalityComparer)
        => new(this, value, equalityComparer);

    public ComputedState<T> Computed<T>(Func<T> computation)
        => new(this, computation);

    public void Effect(Action effect)
        => _ = new EffectHandler(this, effect);

    public uint IncrementVersion() => Interlocked.Increment(ref version);

    public void BuildDependencyGraph(IDependent dependent, Action action) {
        trackedDependents.Value!.Add(dependent);

        action();

        trackedDependents.Value.Remove(dependent);
    }

    public void BuildDependencyGraph(IDependentDependency dependentDependency, Action action) {
        trackedDependentDependencies.Value!.Add(dependentDependency);

        action();

        trackedDependentDependencies.Value.Remove(dependentDependency);
    }

    public void TrackDependency(IDependency dependency) {
        foreach (var dependentDependency in trackedDependentDependencies.Value!) {
            dependentDependency.AddDependency(dependency);
        }

        foreach (var dependent in trackedDependents.Value!) {
            dependency.AddDependent(dependent);
        }
    }

    public bool TryRegisterForTransaction(IEnumerable<IDependent> dependents) {
        if (transactionDependents.Value == null) {
            return false;
        }

        foreach (var dependent in dependents) {
            transactionDependents.Value.Add(dependent);
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
            foreach (var dependent in transactionDependents.Value) {
                dependent.Evaluate();
            }

            transactionDependents.Value = null;
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
                transactionDependents.Dispose();
            }
        }
    }
}
