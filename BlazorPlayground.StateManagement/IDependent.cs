namespace BlazorPlayground.StateManagement;

public interface IDependent {
    DependentPriority Priority { get; }

    void Evaluate();
}
