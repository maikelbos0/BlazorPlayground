using System;
using System.Threading;

namespace BlazorPlayground.StateManagement;

public class ComputedState2<T> {
    private readonly StateProvider2 stateProvider;
    private readonly Func<T> computation;
    private readonly Lock valueLock = new();
    private uint version = uint.MinValue;
    private T value = default!;

    public T Value {
        get {
            lock (valueLock) {
                if (version != stateProvider.Version) {
                    value = computation();
                    version = stateProvider.Version;
                }
            }

            return value;
        }
    }

    public ComputedState2(StateProvider2 stateProvider, Func<T> computation) {
        this.stateProvider = stateProvider;
        this.computation = computation;
    }
}
