namespace BlazorPlayground.StateManagement;

public interface IDependency {
    void AddDependent(IDependent dependent);
}
