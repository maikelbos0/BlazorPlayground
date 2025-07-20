using System.Collections.Concurrent;

namespace BlazorPlayground.StateManagement;

public class MutableState<T> : State<T>, IDependency {
    private readonly IDependency dependency;
    private T value;

    public override T Value {
        get {
            StateProvider.AddDependency(this);

            return value;
        }
    }

    ConcurrentBag<IDependent> IDependency.Dependents { get; } = [];

    public MutableState(StateProvider stateProvider, T value) : base(stateProvider) {
        dependency = this;
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
