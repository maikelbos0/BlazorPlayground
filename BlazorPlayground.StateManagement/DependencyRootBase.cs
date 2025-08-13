using System.Collections.Concurrent;
using System.Linq;
using System.Threading;

namespace BlazorPlayground.StateManagement;

public abstract class DependencyRootBase : DependencyBase {
    private new readonly ConcurrentDictionary<IDependent, int> dependents = new();
    private int order = int.MinValue;

    public override sealed void AddDependent(IDependent dependent) => dependents.AddOrUpdate(dependent, _ => Interlocked.Increment(ref order), (_, _) => Interlocked.Increment(ref order));

    protected void EvaluateDependents() {
        foreach (var dependent in dependents.OrderBy(x => x.Value)) {
            dependent.Key.Evaluate();
        }
    }
}
