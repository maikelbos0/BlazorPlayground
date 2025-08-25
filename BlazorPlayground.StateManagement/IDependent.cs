using System;

namespace BlazorPlayground.StateManagement;

public interface IDependent : IDisposable {
    bool IsDisposed { get; }

    DependentPriority Priority { get; }

    void Evaluate();
}
