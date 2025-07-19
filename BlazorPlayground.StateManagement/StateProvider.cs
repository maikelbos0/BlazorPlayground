using System.Collections.Concurrent;

namespace BlazorPlayground.StateManagement;

public class StateProvider {
    // TODO consider visibility of this
    public ConcurrentDictionary<StateKey, object> StateCollection { get; } = new();

    public MutableState<T> State<T>(T value)
        => State(value, null);

    public MutableState<T> State<T>(T value, string? name)
        => (MutableState<T>)StateCollection.AddOrUpdate(
            new StateKey(typeof(T), name),
            _ => new MutableState<T>(value),
            (key, currentState) => {
                if (currentState is MutableState<T> mutableState) {
                    mutableState.Set(value);
                    return mutableState;
                }

                throw new InvalidStateTypeException(key, typeof(MutableState<T>), currentState.GetType());
            }
        );
}
