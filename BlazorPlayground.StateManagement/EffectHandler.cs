using System;

namespace BlazorPlayground.StateManagement;

public class EffectHandler : IDependent {
    private readonly IStateProvider stateProvider;
    private readonly Action effect;

    public DependentPriority Priority { get; }

    public EffectHandler(IStateProvider stateProvider, Action effect) : this(stateProvider, effect, DependentPriority.Medium) { }

    public EffectHandler(IStateProvider stateProvider, Action effect, DependentPriority priority) {
        this.stateProvider = stateProvider;
        this.effect = effect;
        Priority = priority;
        Evaluate();
    }

    public void Evaluate() => stateProvider.BuildDependencyGraph(this, effect);
}
