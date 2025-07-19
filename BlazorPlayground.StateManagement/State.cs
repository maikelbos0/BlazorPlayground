namespace BlazorPlayground.StateManagement;

public abstract class State {
    internal StateProvider StateProvider { get; }

    internal State(StateProvider stateProvider) {
        StateProvider = stateProvider;
    }

}

public abstract class State<T> : State {
    public abstract T Value { get; }

    internal State(StateProvider stateProvider) : base(stateProvider) { }

    public static implicit operator T(State<T> state) => state.Value;
}
