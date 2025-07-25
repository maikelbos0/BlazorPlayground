using Xunit;

namespace BlazorPlayground.StateManagement.Tests;

public class EffectTests {
    [Fact]
    public void Constructor() {
        var result = 41;
        var stateProvider = new StateProvider();
        var mutableState = new MutableState<int>(stateProvider, 42);
        var subject = new Effect(stateProvider, () => result = mutableState.Value);

        Assert.Equal(42, result);
    }

    [Fact]
    public void Evaluate() {
        var result = 41;
        var stateProvider = new StateProvider();
        var mutableState = new MutableState<int>(stateProvider, 41);
        var subject = new Effect(stateProvider, () => result = mutableState.Value);

        mutableState.Set(42);

        subject.Evaluate();

        Assert.Equal(42, result);
    }
}
