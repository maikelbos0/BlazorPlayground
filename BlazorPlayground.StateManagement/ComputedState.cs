using System;
using System.Threading;

namespace BlazorPlayground.StateManagement;

public class ComputedState<T> : DependencyBranchBase, IDependent {
    private readonly StateProvider stateProvider;
    private readonly Func<T> computation;
    private readonly Lock valueLock = new();
    private T value = default!;

    public T Value {
        get {
            stateProvider.TrackDependency(this);
            return value;
        }
    }
    
    public ComputedState(StateProvider stateProvider, Func<T> computation) {
        this.stateProvider = stateProvider;
        this.computation = computation;
        stateProvider.BuildDependencyGraph(this, () => value = computation());
    }

    public void Evaluate() {
        lock (valueLock) {
            value = computation();
        }
    }
}
