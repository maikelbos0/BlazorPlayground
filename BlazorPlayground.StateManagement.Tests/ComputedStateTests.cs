using Xunit;

namespace BlazorPlayground.StateManagement.Tests;

public class ComputedStateTests {
    [Fact]
    public void Constructor() {
        var stateProvider = new StateProvider();
        var mutableState = new MutableState<int>(stateProvider, 42);
        var subject = new ComputedState<int>(stateProvider, () => mutableState.Value);

        Assert.Equal(42, subject.Value);
    }

    [Fact]
    public void Evaluate() {
        var stateProvider = new StateProvider();
        var mutableState = new MutableState<int>(stateProvider, 41);
        var subject = new ComputedState<int>(stateProvider, () => mutableState.Value);
        var dependent = new ComputedState<int>(stateProvider, () => subject.Value);

        mutableState.Set(42);

        // Evaluate should be indirectly called

        Assert.Equal(42, subject.Value);
        Assert.Equal(42, dependent.Value);
    }
}
