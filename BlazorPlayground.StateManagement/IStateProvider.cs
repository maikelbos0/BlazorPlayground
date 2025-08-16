using System;
using System.Collections.Generic;

namespace BlazorPlayground.StateManagement;

public interface IStateProvider {
    ComputedState<T> Computed<T>(Func<T> computation);
    void Effect(Action effect);
    MutableState<T> Mutable<T>(T value);
    MutableState<T> Mutable<T>(T value, IEqualityComparer<T> equalityComparer);
    void BuildDependencyGraph(IDependent dependent, Action action);
    void TrackDependency(IDependency dependency);
    bool TryRegisterForTransaction(IEnumerable<KeyValuePair<IDependent, nuint>> dependents);
    void ExecuteTransaction(Action transaction);
}
