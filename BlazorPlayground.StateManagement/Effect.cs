using System;

namespace BlazorPlayground.StateManagement;

public class Effect : IDependent {
    private readonly Action effect;

    public Effect(StateProvider stateProvider, Action effect) {
        this.effect = effect;

        using var dependencyGraphBuilder = stateProvider.GetDependencyGraphBuilder(this);
        effect();
    }

    public void Evaluate() => effect();
}
