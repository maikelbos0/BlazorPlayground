using Xunit;

namespace BlazorPlayground.StateManagement.Tests;

public class ComputedStateTests {
    [Fact]
    public void Constructor() {
        var stateProvider = new StateProvider();
        var subject = new ComputedState<int>(stateProvider, () => 42);

        Assert.Equal(42, subject.Value);
    }

    [Fact]
    public void Evaluate() {
        var value = 41;
        var stateProvider = new StateProvider();
        var subject = new ComputedState<int>(stateProvider, () => value);

        value = 42;
        subject.Evaluate();

        Assert.Equal(42, subject.Value);
    }

    [Fact]
    public void Evaluate_With_Conditional() {
        Assert.Fail("TODO: tracking");
        //var stateProvider = new StateProvider();
        //var mutableState1 = new MutableState<bool>(stateProvider, false);
        //var mutableState2 = new MutableState<int>(stateProvider, 41);
        //var subject = new ComputedState<int>(stateProvider, () => mutableState1.Value ? mutableState2.Value : 0);

        //mutableState1.Set(true);
        //mutableState2.Set(42);

        //Assert.Equal(42, subject.Value);
    }
}
