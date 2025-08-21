using System.Collections.Generic;
using System.Threading;

namespace BlazorPlayground.StateManagement;

public class MutableState2<T> : DependencyBase2 {
    private readonly IEqualityComparer<T> equalityComparer;
    private readonly Lock valueLock = new();
    private T value;

    public T Value {
        get {
            stateProvider.TrackDependency(this);

            lock (valueLock) {
                return value;
            }
        }
    }

    public MutableState2(IStateProvider2 stateProvider, T value) : this(stateProvider, value, null) { }

    public MutableState2(IStateProvider2 stateProvider, T value, IEqualityComparer<T>? equalityComparer) : base(stateProvider) {
        this.stateProvider.IncrementVersion();
        this.value = value;
        this.equalityComparer = equalityComparer ?? EqualityComparer<T>.Default;
    }

    public void Update(ValueProvider<T> valueProvider) {
        Set(valueProvider(value));
    }

    public void Set(T value) {
        if (!equalityComparer.Equals(this.value, value)) {
            lock (valueLock) {
                this.value = value;
                stateProvider.IncrementVersion();
            }

            EvaluateDependents();
        }
    }
}
