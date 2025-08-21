using System.Collections.Generic;
using System.Threading;

namespace BlazorPlayground.StateManagement;

public class DependentDependencyBase2 : IDependency2 {
    private readonly HashSet<IDependency2> dependencies = [];
    private readonly Lock dependenciesLock = new();

    public void AddDependent(IDependent2 dependent) {
        foreach (var dependency in dependencies) {
            dependency.AddDependent(dependent);
        }
    }

    public void AddDependency(IDependency2 dependency) {
        lock (dependenciesLock) {
            dependencies.Add(dependency);
        }
    }
}
