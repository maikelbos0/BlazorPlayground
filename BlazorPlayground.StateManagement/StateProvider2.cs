using System.Collections.Concurrent;
using System.Threading;
using System.Threading.Tasks;

namespace BlazorPlayground.StateManagement;

public class StateProvider2 {
    private readonly ConcurrentBag<IEagerDependent2> eagerDependents = [];
    private uint version = uint.MinValue;

    public uint Version => version;

    public void IncrementVersion() {
        Interlocked.Increment(ref version);

        Parallel.ForEach(eagerDependents, eagerDependent => eagerDependent.Evaluate());
    }

    public void RegisterEagerDependent(IEagerDependent2 eagerDependent) => eagerDependents.Add(eagerDependent);
}
