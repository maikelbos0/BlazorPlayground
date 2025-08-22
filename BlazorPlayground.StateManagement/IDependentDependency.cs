namespace BlazorPlayground.StateManagement;

public interface IDependentDependency : IDependency {
    void AddDependency(IDependency dependency);
}
