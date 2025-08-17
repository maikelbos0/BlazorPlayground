using System.Threading;

namespace BlazorPlayground.StateManagement;

public class StateProvider2 {
    private uint version = uint.MinValue;

    public uint Version => version;

    public uint IncrementVersion() => Interlocked.Increment(ref version);
}
