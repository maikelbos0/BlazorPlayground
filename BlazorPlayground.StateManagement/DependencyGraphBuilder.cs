using System;
using System.Collections.Concurrent;
using System.Collections.Generic;

namespace BlazorPlayground.StateManagement;

public sealed class DependencyGraphBuilder : IDisposable {
    private readonly ConcurrentDictionary<int, HashSet<IDependent>> trackedDependents;
    private readonly IDependent dependent;

    public DependencyGraphBuilder(ConcurrentDictionary<int, HashSet<IDependent>> trackedDependents, IDependent dependent) {
        this.trackedDependents = trackedDependents;
        this.dependent = dependent;

        trackedDependents.AddOrUpdate(
            Environment.CurrentManagedThreadId,
            _ => [dependent],
            (_, dependents) => {
                dependents.Add(dependent);
                return dependents;
            }
        );
    }

    public void Dispose() {
        if (trackedDependents.TryGetValue(Environment.CurrentManagedThreadId, out var dependents)) {
            dependents.Remove(dependent);

            if (dependents.Count == 0) {
                trackedDependents.Remove(Environment.CurrentManagedThreadId, out _);
            }
        }
    }
}
