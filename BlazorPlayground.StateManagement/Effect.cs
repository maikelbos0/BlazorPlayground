using System;

namespace BlazorPlayground.StateManagement;

public class Effect : IDependent {
    private readonly Action effect;

    public Effect(StateProvider stateProvider, Action effect) {
        this.effect = effect;
        stateProvider.BuildDependencyGraph(this, effect);
    }

    public void Evaluate() => effect();
}
