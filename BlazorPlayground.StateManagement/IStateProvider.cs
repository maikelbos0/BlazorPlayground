using System;

namespace BlazorPlayground.StateManagement;

public interface IStateProvider {
    ComputedState<T> Computed<T>(Func<T> computation);
    void Effect(Action effect);
    MutableState<T> Mutable<T>(T value);
}
