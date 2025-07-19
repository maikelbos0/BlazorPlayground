namespace BlazorPlayground.StateManagement;

public class MutableState<T> : State<T> {
    private T value;

    public override T Value => value;

    public MutableState(StateProvider stateProvider, T value) : base(stateProvider) {
        this.value = value;
    }

    public void Set(T value) {
        this.value = value;
    }

    public void Update(ValueProvider<T> valueProvider) {
        value = valueProvider(value);
    }
}
