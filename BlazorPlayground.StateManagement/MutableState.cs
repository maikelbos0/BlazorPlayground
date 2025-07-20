using System.Collections.Concurrent;

namespace BlazorPlayground.StateManagement;

public class MutableState<T> : State<T>, IDependency {
    private T value;

    public override T Value {
        get {
            StateProvider.AddDependency(this);

            return value;
        }
    }

    public ConcurrentBag<IDependent> Dependents { get; } = [];

    public MutableState(StateProvider stateProvider, T value) : base(stateProvider) {
        this.value = value;
    }

    public void Update(ValueProvider<T> valueProvider) {
        Set(valueProvider(value));
    }

    public void Set(T value) {
        this.value = value;

        foreach (var dependent in Dependents) {
            dependent.Evaluate();
        }
    }
}
