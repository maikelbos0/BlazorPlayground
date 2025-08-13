namespace BlazorPlayground.StateManagement;

public abstract class DependencyBase {
    public abstract void AddDependent(IDependent dependent);
}
