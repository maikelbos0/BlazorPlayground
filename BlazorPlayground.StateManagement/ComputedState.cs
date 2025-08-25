using System;
using System.Collections.Generic;
using System.Threading;

namespace BlazorPlayground.StateManagement;

public class ComputedState<T> : IDependentDependency {
    private readonly HashSet<IDependency> dependencies = [];
    private readonly Lock dependenciesLock = new();
    private readonly IStateProvider stateProvider;
    private readonly Func<T> computation;
    private readonly Lock valueLock = new();
    private uint version = uint.MinValue;
    private T value = default!;

    public T Value {
        get {
            stateProvider.TrackDependency(this);

            var currentVersion = version;
            var expectedVersion = stateProvider.Version;

            if (currentVersion != expectedVersion) {
                stateProvider.BuildDependencyGraph(this, () => {
                    var newValue = computation();

                    lock (valueLock) {
                        value = newValue;
                        version = expectedVersion;
                    }
                });
            }

            return value;
        }
    }

    public ComputedState(IStateProvider stateProvider, Func<T> computation) {
        this.stateProvider = stateProvider;
        this.computation = computation;
    }

    public void AddDependent(IDependent dependent) {
        lock (dependenciesLock) {

            foreach (var dependency in dependencies) {
                dependency.AddDependent(dependent);
            }
        }
    }

    public void AddDependency(IDependency dependency) {
        lock (dependenciesLock) {
            dependencies.Add(dependency);
        }
    }
}
