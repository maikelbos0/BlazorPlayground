using System;
using System.Collections.Generic;
using System.Threading;

namespace BlazorPlayground.StateManagement;

public class StateProvider2 {
    private readonly ThreadLocal<HashSet<IDependent2>> trackedDependents = new(() => []);

    private uint version = uint.MinValue;

    public uint Version => version;

    public MutableState2<T> Mutable<T>(T value)
        => new(this, value);

    public MutableState2<T> Mutable<T>(T value, IEqualityComparer<T> equalityComparer)
        => new(this, value, equalityComparer);

    public ComputedState2<T> Computed<T>(Func<T> computation)
        => new(this, computation);

    public void Effect(Action effect)
        => _ = new Effect2(this, effect);

    public uint IncrementVersion() => Interlocked.Increment(ref version);

    public void BuildDependencyGraph(IDependent2 dependent, Action action) {
        trackedDependents.Value!.Add(dependent);

        action();

        trackedDependents.Value.Remove(dependent);
    }

    public void TrackDependency(DependencyBase2 dependency) {
        foreach (var dependent in trackedDependents.Value!) {
            dependency.AddDependent(dependent);
        }
    }

}
