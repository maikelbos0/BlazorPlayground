using System;

namespace BlazorPlayground.StateManagement;

public class Effect2 : IDependent2 {
    private readonly StateProvider2 stateProvider;
    private readonly Action effect;

    public Effect2(StateProvider2 stateProvider, Action effect) {
        this.stateProvider = stateProvider;
        this.effect = effect;
        this.stateProvider.RegisterDependent(this);
        effect();
    }

    public void Evaluate() => effect();
}
