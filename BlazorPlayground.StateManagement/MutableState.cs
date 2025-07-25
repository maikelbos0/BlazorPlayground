namespace BlazorPlayground.StateManagement;

public class MutableState<T> : Dependency {
    private readonly StateProvider stateProvider;
    private T value;

    public T Value {
        get {
            stateProvider.TrackDependency(this);
            return value;
        }
    }

    public MutableState(StateProvider stateProvider, T value) {
        this.stateProvider = stateProvider;
        this.value = value;
    }

    public void Update(ValueProvider<T> valueProvider) {
        Set(valueProvider(value));
    }

    public void Set(T value) {
        this.value = value;
        EvaluateDependents();
    }
}
