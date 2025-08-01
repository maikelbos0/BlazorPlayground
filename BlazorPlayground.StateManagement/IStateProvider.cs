using System;

namespace BlazorPlayground.StateManagement;

public interface IStateProvider {
    void BuildDependencyGraph(IDependent dependent, Action action);
    ComputedState<T> Computed<T>(Func<T> computation);
    void Effect(Action effect);
    IDependencyGraphBuilder GetDependencyGraphBuilder(IDependent dependent);
    MutableState<T> Mutable<T>(T value);
}
