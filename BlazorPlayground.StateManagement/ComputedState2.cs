using System;
using System.Threading;

namespace BlazorPlayground.StateManagement;

public class ComputedState2<T> : DependentDependencyBase2 {
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
}
