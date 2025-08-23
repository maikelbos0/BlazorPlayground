using System;
using System.Collections.Generic;

namespace BlazorPlayground.StateManagement;

public interface IStateProvider {
    uint Version { get; }

    MutableState<T> Mutable<T>(T value);
    MutableState<T> Mutable<T>(T value, IEqualityComparer<T> equalityComparer);
    ComputedState<T> Computed<T>(Func<T> computation);
    EffectHandler Effect(Action effect, DependentPriority priority);
    EffectHandler Effect(Action effect);
    uint IncrementVersion();
    void BuildDependencyGraph(IDependent dependent, Action action);
    void BuildDependencyGraph(IDependentDependency dependentDependency, Action action);
    void TrackDependency(IDependency dependency);
    bool TryRegisterForTransaction(IEnumerable<IDependent> dependents);
    void ExecuteTransaction(Action transaction);
}
