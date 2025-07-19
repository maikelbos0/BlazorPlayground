namespace BlazorPlayground.StateManagement;

public class MutableState<T> : State<T> {
    private T value;

    public override T Value => value;

    public MutableState(T value) {
        this.value = value;
    }

    public void Set(T value) {
        this.value = value;
    }

    public void Update(ValueProvider<T> valueProvider) {
        this.value = valueProvider(value);
    }

    public static implicit operator MutableState<T>(T value) => new(value);
}
