using System.Collections.Generic;

namespace BlazorPlayground.StateManagement;

public class MutableState2<T> : DependencyBase2 {
    private readonly IEqualityComparer<T> equalityComparer;
    private T value;

    public T Value {
        get {
            stateProvider.TrackDependency(this);
            return value;
        }
    }

    public MutableState2(StateProvider2 stateProvider, T value) : this(stateProvider, value, null) { }

    public MutableState2(StateProvider2 stateProvider, T value, IEqualityComparer<T>? equalityComparer) : base(stateProvider) {
        this.value = value;
        this.stateProvider.IncrementVersion();
        this.equalityComparer = equalityComparer ?? EqualityComparer<T>.Default;
    }

    public void Update(ValueProvider<T> valueProvider) {
        Set(valueProvider(value));
    }

    public void Set(T value) {
        if (!equalityComparer.Equals(this.value, value)) {
            this.value = value;
            stateProvider.IncrementVersion();
            EvaluateDependents();
        }
    }
}
