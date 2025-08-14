using System.Collections.Concurrent;

namespace BlazorPlayground.StateManagement;

public abstract class DependencyBranchBase : DependencyBase {
public abstract class DependencyBranchBase : IDependency {
    private readonly ConcurrentBag<IDependency> dependencies = [];

    public void AddDependent(IDependent dependent) {
        foreach (var dependency in dependencies) {
            dependency.AddDependent(dependent);
        }
    }

    public void AddDependency(IDependency dependency) => dependencies.Add(dependency);
}
