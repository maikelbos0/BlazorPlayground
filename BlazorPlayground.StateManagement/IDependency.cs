using System.Collections.Concurrent;

namespace BlazorPlayground.StateManagement;

internal interface IDependency {
    ConcurrentBag<IDependent> Dependents { get; }
}
