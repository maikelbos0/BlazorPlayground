using System;
using System.Collections.Generic;
using System.Threading;

namespace BlazorPlayground.StateManagement;

public class ComputedState<T> : IDependentDependency {
    private readonly HashSet<WeakReference<IDependency>> dependencies = new(WeakReferenceEqualityComparer<IDependency>.Instance);
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
        var activeDependencies = new List<IDependency>(dependencies.Count);

        lock (dependenciesLock) {
            foreach (var dependency in dependencies) {
                if (dependency.TryGetTarget(out var activeDependency)) {
                    activeDependencies.Add(activeDependency);
                }
                else {
                    dependencies.Remove(dependency);
                }
            }
        }

        foreach (var activeDependency in activeDependencies) {
            activeDependency.AddDependent(dependent);
        }
    }

    public void AddDependency(IDependency dependency) {
        lock (dependenciesLock) {
            dependencies.Add(new WeakReference<IDependency>(dependency));
        }
    }
}
