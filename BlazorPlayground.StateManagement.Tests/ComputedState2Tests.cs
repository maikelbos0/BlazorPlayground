using Xunit;

namespace BlazorPlayground.StateManagement.Tests;

public class ComputedState2Tests {
    [Fact]
    public void Value_After_Version_Increase() {
        var value = 41;
        var stateProvider = new StateProvider2();
        var subject = new ComputedState2<int>(stateProvider, () => value);

        value = 42;
        stateProvider.IncrementVersion();

        Assert.Equal(42, subject.Value);
    }

    [Fact]
    public void Value_With_Conditional() {
        var stateProvider = new StateProvider2();
        var mutableState1 = new MutableState2<bool>(stateProvider, false);
        var mutableState2 = new MutableState2<int>(stateProvider, 41);
        var subject = new ComputedState2<int>(stateProvider, () => mutableState1.Value ? mutableState2.Value : 0);

        mutableState1.Set(true);
        mutableState2.Set(42);

        Assert.Equal(42, subject.Value);
    }
}
