using System;
using System.Threading;

namespace BlazorPlayground.StateManagement;

public class ComputedState<T> : State<T>, IDependent {
    private readonly Func<T> computation;
    private readonly Lock valueLock = new();
    private T value;

    public override T Value => value;

    internal ComputedState(StateProvider stateProvider, Func<T> computation) : base(stateProvider) {
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
