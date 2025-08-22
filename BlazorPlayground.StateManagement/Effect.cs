using System;

namespace BlazorPlayground.StateManagement;

public class Effect : IDependent {
    private readonly IStateProvider stateProvider;
    private readonly Action effect;

    public Effect(IStateProvider stateProvider, Action effect) {
        this.stateProvider = stateProvider;
        this.effect = effect;
        Evaluate();
    }

    public void Evaluate() => stateProvider.BuildDependencyGraph(this, effect);
}
