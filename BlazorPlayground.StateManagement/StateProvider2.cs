using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Threading;

namespace BlazorPlayground.StateManagement;

public class StateProvider2 {
    private readonly ConcurrentBag<IDependent2> dependents = [];
    private readonly ThreadLocal<HashSet<IDependent2>> trackedDependents = new(() => []);

    private uint version = uint.MinValue;

    public uint Version => version;

    public uint IncrementVersion() => Interlocked.Increment(ref version);

    public void RegisterDependent(IDependent2 dependent) => dependents.Add(dependent);

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
