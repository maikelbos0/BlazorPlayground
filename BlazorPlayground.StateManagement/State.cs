namespace BlazorPlayground.StateManagement;

public class State<T> {
    public State(T value) {
        Value = value;
    }

    public T Value { get; private set; }

    public void Set(T value) {
        Value = value;
    }

    public void Update(ValueProvider<T> valueProvider) {
        Value = valueProvider(Value);
    }
}

public delegate T ValueProvider<T>(T currentValue);
