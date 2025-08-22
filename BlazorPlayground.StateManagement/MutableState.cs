using System.Collections.Generic;
using System.Threading;

namespace BlazorPlayground.StateManagement;

public class MutableState<T> : IDependency {
    private readonly HashSet<IDependent> dependents = [];
    private readonly Lock dependentsLock = new();
    private readonly IStateProvider stateProvider;
    private readonly Lock valueLock = new();
    private readonly IEqualityComparer<T> equalityComparer;
    private T value;

    public T Value {
        get {
            stateProvider.TrackDependency(this);

            lock (valueLock) {
                return value;
            }
        }
    }

    public MutableState(IStateProvider stateProvider, T value) : this(stateProvider, value, null) { }

    public MutableState(IStateProvider stateProvider, T value, IEqualityComparer<T>? equalityComparer) {
        this.stateProvider = stateProvider;
        this.equalityComparer = equalityComparer ?? EqualityComparer<T>.Default;
        this.value = value;
        this.stateProvider.IncrementVersion();
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

            if (!stateProvider.TryRegisterForTransaction(dependents)) {
                foreach (var dependent in dependents) {
                    dependent.Evaluate();
                }
            }
        }
    }

    public void AddDependent(IDependent dependent) {
        lock (dependentsLock) {
            dependents.Add(dependent);
        }
    }
}
