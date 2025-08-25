using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

namespace BlazorPlayground.StateManagement;

public class StateProvider : IDisposable, IStateProvider {
    private readonly ThreadLocal<HashSet<IDependent>> trackedDependents = new(() => []);
    private readonly ThreadLocal<HashSet<IDependentDependency>> trackedDependentDependencies = new(() => []);
    private readonly ThreadLocal<HashSet<IDependent>?> transactionDependents = new();
    private uint version = uint.MinValue;

    public uint Version => version;

    public MutableState<T> Mutable<T>(T value)
        => new(this, value);

    public MutableState<T> Mutable<T>(T value, IEqualityComparer<T> equalityComparer)
        => new(this, value, equalityComparer);

    public ComputedState<T> Computed<T>(Func<T> computation)
        => new(this, computation);

    public EffectHandler Effect(Action effect)
        => _ = new EffectHandler(this, effect);

    public EffectHandler Effect(Action effect, DependentPriority priority)
        => _ = new EffectHandler(this, effect, priority);

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
            foreach (var transactionDependent in transactionDependents.Value.OrderByDescending(transactionDependent => transactionDependent.Priority)) {
                transactionDependent.Evaluate();
            }

            transactionDependents.Value = null;
        }
    }

    public void Dispose() {
        trackedDependents.Dispose();
        transactionDependents.Dispose();
        GC.SuppressFinalize(this);
    }
}
