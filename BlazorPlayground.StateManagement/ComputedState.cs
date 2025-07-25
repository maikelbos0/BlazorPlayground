using System;
using System.Threading;

namespace BlazorPlayground.StateManagement;

public class ComputedState<T> : IDependent {
    private readonly Func<T> computation;
    private readonly Lock valueLock = new();
    private T value;

    public T Value => value;

    public ComputedState(StateProvider stateProvider, Func<T> computation) {
        this.computation = computation;

        using var dependencyGraphBuilder = stateProvider.GetDependencyGraphBuilder(this);
        value = computation();
    }

    public void Evaluate() {
        lock (valueLock) {
            value = computation();
        }
    }
}
