using System;
using System.Threading;

namespace BlazorPlayground.StateManagement;

public class ComputedState<T> : IDependent {
    private readonly Func<T> computation;
    private readonly Lock valueLock = new();
    private T value;

    public T Value => value;

    internal ComputedState(StateProvider stateProvider, Func<T> computation) {
        this.computation = computation;

        stateProvider.StartBuildingDependencyGraph(this);
        value = computation();
        stateProvider.StopBuildingDependencyGraph(this);
    }

    void IDependent.Evaluate() {
        lock (valueLock) {
            value = computation();
        }
    }
}
