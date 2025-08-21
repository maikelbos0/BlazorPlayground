using Xunit;

namespace BlazorPlayground.StateManagement.Tests;

public class Effect2Tests {
    [Fact]
    public void Constructor() {
        var result = 0;
        var stateProvider = new StateProvider2();
        var subject = new Effect2(stateProvider, () => result = 42);

        Assert.Equal(42, result);
    }

    [Fact]
    public void Evaluate() {
        var result = 0;
        var stateProvider = new StateProvider2();
        var mutableState = new MutableState2<int>(stateProvider, 41);
        var subject = new Effect2(stateProvider, () => result = mutableState.Value);

        mutableState.Set(42);

        Assert.Equal(42, result);
    }

    [Fact]
    public void Evaluate_With_Conditional() {
        var result = 0;
        var stateProvider = new StateProvider2();
        var mutableState = new MutableState2<bool>(stateProvider, false);
        var computedState = new ComputedState2<int>(stateProvider, () => mutableState.Value ? 42 : 0);
        var subject = new Effect2(stateProvider, () => result = mutableState.Value ? computedState.Value : 0);

        mutableState.Set(true);

        Assert.Equal(42, result);
    }

    [Fact]
    public void Does_Not_Evaluate_When_Unrelated_State_Updates() {
        var evaluations = 0;

        var stateProvider = new StateProvider2();
        var mutableState = new MutableState2<int>(stateProvider, 41);
        var subject = new Effect2(stateProvider, () => evaluations++);

        mutableState.Set(42);

        Assert.Equal(1, evaluations);
    }
}
