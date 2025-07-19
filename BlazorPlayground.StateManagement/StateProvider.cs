using System.Collections.Concurrent;

namespace BlazorPlayground.StateManagement;

public class StateProvider {
    internal ConcurrentDictionary<StateKey, State> StateCollection { get; } = new();

    public MutableState<T> State<T>(T value)
        => State(value, null);

    public MutableState<T> State<T>(T value, string? name)
        => (MutableState<T>)StateCollection.AddOrUpdate(
            new StateKey(typeof(T), name),
            _ => new MutableState<T>(this, value),
            (key, currentState) => {
                if (currentState is MutableState<T> mutableState) {
                    mutableState.Set(value);
                    return mutableState;
                }

                throw new InvalidStateTypeException(key, typeof(MutableState<T>), currentState.GetType());
            }
        );
}
