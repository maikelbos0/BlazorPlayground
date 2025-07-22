using System;
using System.Collections.Concurrent;
using System.Collections.Generic;

namespace BlazorPlayground.StateManagement;

public class StateProvider {
    internal ConcurrentDictionary<int, List<IDependent>> TrackedDependents { get; } = new();

    public MutableState<T> Mutable<T>(T value)
        => new(this, value);

    public ComputedState<T> Computed<T>(Func<T> computation)
        => new(this, computation);

    internal void StartBuildingDependencyGraph(IDependent dependent) {
        TrackedDependents.AddOrUpdate(
            Environment.CurrentManagedThreadId,
            _ => [dependent],
            (_, dependents) => {
                dependents.Add(dependent);
                return dependents;
            }
        );
    }

    internal void TrackDependency(IDependency dependency) {
        if (TrackedDependents.TryGetValue(Environment.CurrentManagedThreadId, out var dependents)) {
            foreach (var dependent in dependents) {
                dependency.Dependents.Add(dependent);
            }
        }
    }

    internal void StopBuildingDependencyGraph(IDependent dependent) {
        if (TrackedDependents.TryGetValue(Environment.CurrentManagedThreadId, out var dependents)) {
            dependents.Remove(dependent);

            if (dependents.Count == 0) {
                TrackedDependents.Remove(Environment.CurrentManagedThreadId, out _);
            }
        }
    }
}
