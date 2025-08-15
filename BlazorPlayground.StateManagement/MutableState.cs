using System.Collections.Generic;

namespace BlazorPlayground.StateManagement;

public class MutableState<T> : DependencyRootBase {
    private readonly IEqualityComparer<T> equalityComparer;
    private T value;

    public T Value {
        get {
            stateProvider.TrackDependency(this);
            return value;
        }
    }

    public MutableState(StateProvider stateProvider, T value) : this(stateProvider, value, null) { }

    public MutableState(StateProvider stateProvider, T value, IEqualityComparer<T>? equalityComparer) : base(stateProvider) {
        this.value = value;
        this.equalityComparer = equalityComparer ?? EqualityComparer<T>.Default;
    }

    public void Update(ValueProvider<T> valueProvider) {
        Set(valueProvider(value));
    }

    public void Set(T value) {
        if (!equalityComparer.Equals(this.value, value)) {
            this.value = value;
            EvaluateDependents();
        }
    }
}
