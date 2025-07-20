using System;

namespace BlazorPlayground.StateManagement;

public class ComputedState<T> : State<T> {
    private readonly Func<T> computation;
    private readonly object valueLock = new();
    private T value;

    public override T Value => value;

    public ComputedState(StateProvider stateProvider, Func<T> computation) : base(stateProvider) {
        this.computation = computation;
        value = computation();
    }

    public void Evaluate() {
        lock (valueLock) {
            value = computation();
        }
    }
}
