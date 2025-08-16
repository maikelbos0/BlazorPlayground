using Xunit;

namespace BlazorPlayground.StateManagement.Tests;

public class EffectTests {
    [Fact]
    public void Constructor() {
        var result = 0;
        var stateProvider = new StateProvider();
        var subject = new Effect(stateProvider, () => result = 42);

        Assert.Equal(42, result);
    }

    [Fact]
    public void Evaluate() {
        var value = 41;
        var result = 0;
        var stateProvider = new StateProvider();
        var subject = new Effect(stateProvider, () => result = value);

        value = 42;
        subject.Evaluate();

        Assert.Equal(42, result);
    }

    [Fact]
    public void Evaluate_With_Conditional() {
        var result = 0;
        var stateProvider = new StateProvider();
        var mutableState1 = new MutableState<bool>(stateProvider, false);
        var mutableState2 = new MutableState<int>(stateProvider, 41);
        var subject = new Effect(stateProvider, () => result = mutableState1.Value ? mutableState2.Value : 0);

        mutableState1.Set(true);
        mutableState2.Set(42);

        Assert.Equal(42, result);
    }
}
