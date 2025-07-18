namespace BlazorPlayground.StateManagement;

public class MutableState<T> : State<T> {
    public MutableState(T value) : base(value) { }

    public void Set(T value) {
        Value = value;
    }

    public void Update(ValueProvider<T> valueProvider) {
        Value = valueProvider(Value);
    }

    public static implicit operator MutableState<T>(T value) => new(value);
}
