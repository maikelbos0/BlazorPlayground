using System.Collections.Generic;

namespace BlazorPlayground.StateManagement;

public abstract class DependencyRootBase : DependencyBase {
    protected void EvaluateDependents() {
        var dependentsToEvaluate = new List<IDependent>();

        FindDependents(dependents.Keys, dependentsToEvaluate);

        foreach (var dependent in dependentsToEvaluate) {
            dependent.Evaluate();
        }
    }

    private void FindDependents(IEnumerable<IDependent> dependents, List<IDependent> dependentsToEvaluate) {
        foreach (var dependent in dependents) {
            dependentsToEvaluate.Remove(dependent);
            dependentsToEvaluate.Add(dependent);

            if (dependent is DependencyBase dependency) {
                FindDependents(dependency.dependents.Keys, dependentsToEvaluate);
            }
        }
    }
}
