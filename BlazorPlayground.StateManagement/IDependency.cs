using System.Collections.Concurrent;

namespace BlazorPlayground.StateManagement;

public interface IDependency {
    ConcurrentBag<IDependent> Dependents { get; }
}
