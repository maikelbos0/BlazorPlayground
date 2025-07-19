namespace BlazorPlayground.StateManagement;

public abstract class State<T> {
    internal StateProvider StateProvider { get; }
    public abstract T Value { get; }

    internal State(StateProvider stateProvider) {
        StateProvider = stateProvider;
    }

    public static implicit operator T(State<T> state) => state.Value;
}
