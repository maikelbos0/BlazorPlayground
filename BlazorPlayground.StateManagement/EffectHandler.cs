using System;

namespace BlazorPlayground.StateManagement;

public class EffectHandler : IDependent {
    private readonly IStateProvider stateProvider;
    private readonly Action effect;

    public EffectHandler(IStateProvider stateProvider, Action effect) {
        this.stateProvider = stateProvider;
        this.effect = effect;
        Evaluate();
    }

    public void Evaluate() => stateProvider.BuildDependencyGraph(this, effect);
}
