using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

namespace BlazorPlayground.StateManagement;

public class MutableState<T> : IDependency {
    private readonly HashSet<WeakReference<IDependent>> dependents = new(WeakReferenceEqualityComparer<IDependent>.Instance);
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

    public void Update(Func<T, T> valueProvider) {
        Set(valueProvider(value));
    }

    public void Set(T value) {
        if (!equalityComparer.Equals(this.value, value)) {
            lock (valueLock) {
                this.value = value;
                stateProvider.IncrementVersion();
            }

            var activeDependents = new List<IDependent>(dependents.Count);

            lock (dependentsLock) {
                foreach (var dependent in dependents) {
                    if (dependent.TryGetTarget(out var activeDependent)) {
                        activeDependents.Add(activeDependent);
                    }
                    else {
                        dependents.Remove(dependent);
                    }
                }
            }

            if (!stateProvider.TryRegisterForTransaction(activeDependents)) {
                foreach (var activeDependent in activeDependents.OrderByDescending(activeDependent => activeDependent.Priority)) {
                    activeDependent.Evaluate();
                }
            }
        }
    }

    public void AddDependent(IDependent dependent) {
        lock (dependentsLock) {
            dependents.Add(new WeakReference<IDependent>(dependent));
        }
    }
}
