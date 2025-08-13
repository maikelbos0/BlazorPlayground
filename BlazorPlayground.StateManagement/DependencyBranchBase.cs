using System.Collections.Concurrent;

namespace BlazorPlayground.StateManagement;

public abstract class DependencyBranchBase : DependencyBase {
    private readonly ConcurrentBag<DependencyBase> dependencies = [];

    public override sealed void AddDependent(IDependent dependent) {
        foreach (var dependency in dependencies) {
            dependency.AddDependent(dependent);
        }
    }

    public void AddDependency(DependencyBase dependency) => dependencies.Add(dependency);
}
