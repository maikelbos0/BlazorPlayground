using System;
using System.Collections.Concurrent;
using System.Collections.Generic;

namespace BlazorPlayground.StateManagement;

public class StateProvider {
    internal ConcurrentDictionary<int, List<IDependent>> TrackedDependents { get; } = new();

    internal ConcurrentDictionary<StateKey, State> StateCollection { get; } = new();

    public MutableState<T> State<T>(T value)
        => State(value, null);

    public MutableState<T> State<T>(T value, string? name)
        => (MutableState<T>)StateCollection.AddOrUpdate(
            new StateKey(typeof(T), name),
            _ => new MutableState<T>(this, value),
            (key, currentState) => {
                if (currentState is MutableState<T> mutableState) {
                    mutableState.Set(value);
                    return mutableState;
                }

                throw new InvalidStateTypeException(key, typeof(MutableState<T>), currentState.GetType());
            }
        );

    // TODO add computed state methods and maybe rename State?

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
