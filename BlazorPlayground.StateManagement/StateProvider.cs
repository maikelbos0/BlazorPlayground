using System;
using System.Collections.Concurrent;
using System.Collections.Generic;

namespace BlazorPlayground.StateManagement;

public class StateProvider : IStateProvider {
    private readonly ConcurrentDictionary<int, HashSet<IDependent>> trackedDependents = new();

    public MutableState<T> Mutable<T>(T value)
        => new(this, value);

    public ComputedState<T> Computed<T>(Func<T> computation)
        => new(this, computation);

    public void Effect(Action effect)
        => _ = new Effect(this, effect);

    public void BuildDependencyGraph(IDependent dependent, Action action) {
        trackedDependents.AddOrUpdate(
            Environment.CurrentManagedThreadId,
            _ => [dependent],
            (_, dependents) => {
                dependents.Add(dependent);
                return dependents;
            }
        );

        action();

        if (trackedDependents.TryGetValue(Environment.CurrentManagedThreadId, out var dependents)) {
            dependents.Remove(dependent);

            if (dependents.Count == 0) {
                trackedDependents.Remove(Environment.CurrentManagedThreadId, out _);
            }
        }
    }

    public IDependencyGraphBuilder GetDependencyGraphBuilder(IDependent dependent)
        => new DependencyGraphBuilder(trackedDependents, dependent);

    public void TrackDependency(DependencyBase dependency) {
        if (trackedDependents.TryGetValue(Environment.CurrentManagedThreadId, out var dependents)) {
            foreach (var dependent in dependents) {
                dependency.AddDependent(dependent);
            }
        }
    }
}
