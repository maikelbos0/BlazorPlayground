namespace BlazorPlayground.StateManagement;

public interface IDependentDependency2 : IDependency2 {
    void AddDependency(IDependency2 dependency);
}
