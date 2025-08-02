using System;
using System.Collections.Generic;
using System.Threading;

namespace BlazorPlayground.StateManagement;

public class StateProvider : IStateProvider, IDisposable {
    private readonly ThreadLocal<HashSet<IDependent>> trackedDependents = new(() => []);
    private bool isDisposed = false;

    public MutableState<T> Mutable<T>(T value)
        => new(this, value);

    public ComputedState<T> Computed<T>(Func<T> computation)
        => new(this, computation);

    public void Effect(Action effect)
        => _ = new Effect(this, effect);

    public void BuildDependencyGraph(IDependent dependent, Action action) {
        trackedDependents.Value!.Add(dependent);

        action();

        trackedDependents.Value.Remove(dependent);
    }

    public void TrackDependency(DependencyBase dependency) {
        foreach (var dependent in trackedDependents.Value!) {
            dependency.AddDependent(dependent);
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
}
