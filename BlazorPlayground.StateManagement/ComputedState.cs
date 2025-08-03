using System;
using System.Diagnostics.CodeAnalysis;
using System.Threading;

namespace BlazorPlayground.StateManagement;

public class ComputedState<T> : DependencyBase, IDependent {
    private readonly StateProvider stateProvider;
    private readonly Func<T> computation;
    private readonly Lock valueLock = new();
    private T value;

    public T Value {
        get {
            stateProvider.TrackDependency(this);
            return value;
        }
    }
    
    public ComputedState(StateProvider stateProvider, Func<T> computation) {
        this.stateProvider = stateProvider;
        this.computation = computation;

        BuildDependencyGraph();
    }

    [MemberNotNull(nameof(value))]
    private void BuildDependencyGraph() {
        stateProvider.BuildDependencyGraph(this, () => value = computation());
    }

    public void Evaluate() {
        lock (valueLock) {
            value = computation();
        }
        EvaluateDependents();
    }
}
