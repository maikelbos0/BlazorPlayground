using System;
using System.Collections.Generic;
using System.Threading;

namespace BlazorPlayground.StateManagement;

public class ComputedState2<T> : IDependentDependency2 {
    private readonly HashSet<IDependency2> dependencies = [];
    private readonly Lock dependenciesLock = new();
    private readonly IStateProvider2 stateProvider;
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

    public ComputedState2(IStateProvider2 stateProvider, Func<T> computation) {
        this.stateProvider = stateProvider;
        this.computation = computation;
    }

    public void AddDependent(IDependent2 dependent) {
        foreach (var dependency in dependencies) {
            dependency.AddDependent(dependent);
        }
    }

    public void AddDependency(IDependency2 dependency) {
        lock (dependenciesLock) {
            dependencies.Add(dependency);
        }
    }
}
