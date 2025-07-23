using System.Collections.Concurrent;

namespace BlazorPlayground.StateManagement;

public class MutableState<T> : IDependency {
    private readonly IDependency dependency;
    private readonly StateProvider stateProvider;
    private T value;

    public T Value {
        get {
            stateProvider.TrackDependency(this);

            return value;
        }
    }

    ConcurrentBag<IDependent> IDependency.Dependents { get; } = [];

    internal MutableState(StateProvider stateProvider, T value) {
        dependency = this;
        this.stateProvider = stateProvider;
        this.value = value;
    }

    public void Update(ValueProvider<T> valueProvider) {
        Set(valueProvider(value));
    }

    public void Set(T value) {
        this.value = value;

        foreach (var dependent in dependency.Dependents) {
            dependent.Evaluate();
        }
    }
}
