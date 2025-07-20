using System;
using System.Collections.Concurrent;
using System.Collections.Generic;

namespace BlazorPlayground.StateManagement;

public class StateProvider {
    internal ConcurrentDictionary<IDependent, int> TrackedDependents { get; } = new();

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

    internal void StartBuildingDependencyGraph(IDependent dependent) {
        _ = TrackedDependents.TryAdd(dependent, Environment.CurrentManagedThreadId);
    }

    internal void AddDependency(IDependency dependency) {
        // TODO optimize
        foreach (var dependent in TrackedDependents) {
            if (dependent.Value == Environment.CurrentManagedThreadId) {
                dependency.Dependents.Add(dependent.Key);
            }
        }
    }

    internal void StopBuildingDependencyGraph(IDependent dependent) {
        TrackedDependents.Remove(dependent, out _);
    }
}
