using System;

namespace BlazorPlayground.StateManagement;

public class Effect : IDependent {
    private readonly Action effect;

    internal Effect(StateProvider stateProvider, Action effect) {
        this.effect = effect;

        stateProvider.StartBuildingDependencyGraph(this);
        effect();
        stateProvider.StopBuildingDependencyGraph(this);
    }

    public void Evaluate() => effect();
}
