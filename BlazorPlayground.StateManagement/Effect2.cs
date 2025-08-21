using System;

namespace BlazorPlayground.StateManagement;

public class Effect2 : IDependent2 {
    private readonly IStateProvider2 stateProvider;
    private readonly Action effect;

    public Effect2(IStateProvider2 stateProvider, Action effect) {
        this.stateProvider = stateProvider;
        this.effect = effect;
        Evaluate();
    }

    public void Evaluate() => stateProvider.BuildDependencyGraph(this, effect);
}
