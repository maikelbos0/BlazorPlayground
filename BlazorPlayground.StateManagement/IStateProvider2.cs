using System;
using System.Collections.Generic;

namespace BlazorPlayground.StateManagement;

public interface IStateProvider2 {
    uint Version { get; }

    MutableState2<T> Mutable<T>(T value);
    MutableState2<T> Mutable<T>(T value, IEqualityComparer<T> equalityComparer);
    ComputedState2<T> Computed<T>(Func<T> computation);
    void Effect(Action effect);
    uint IncrementVersion();
    void BuildDependencyGraph(IDependent2 dependent, Action action);
    void BuildDependencyGraph(DependentDependencyBase2 dependentDependency, Action action);
    void TrackDependency(IDependency2 dependency);
    bool TryRegisterForTransaction(IEnumerable<IDependent2> dependents);
    void ExecuteTransaction(Action transaction);
}
