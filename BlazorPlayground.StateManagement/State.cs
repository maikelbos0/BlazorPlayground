namespace BlazorPlayground.StateManagement;

public class State<T> {
    public State(T value) {
        Value = value;
    }

    public T Value { get; protected set; }

    public static implicit operator T(State<T> state) => state.Value;

    public static implicit operator State<T>(T value) => new(value);
}
