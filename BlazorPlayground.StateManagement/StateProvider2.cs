using System.Collections.Concurrent;
using System.Threading;
using System.Threading.Tasks;

namespace BlazorPlayground.StateManagement;

public class StateProvider2 {
    private readonly ConcurrentBag<IDependent2> dependents = [];
    private uint version = uint.MinValue;

    public uint Version => version;

    public void IncrementVersion() {
        Interlocked.Increment(ref version);

        Parallel.ForEach(dependents, dependent => dependent.Evaluate());
    }

    public void RegisterDependent(IDependent2 dependent) => dependents.Add(dependent);
}
