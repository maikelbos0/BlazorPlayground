namespace BlazorPlayground.StateManagement;

public abstract class State<T> {
    public abstract T Value { get; }
}
