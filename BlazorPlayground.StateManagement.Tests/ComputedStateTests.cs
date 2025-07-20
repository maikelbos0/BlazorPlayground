using Xunit;

namespace BlazorPlayground.StateManagement.Tests;

public class ComputedStateTests {
    [Fact]
    public void Constructor() {
        var stateProvider = new StateProvider();
        var state = new MutableState<int>(stateProvider, 42);
        var subject = new ComputedState<int>(stateProvider, () => state.Value);

        Assert.Equal(42, subject.Value);
    }

    [Fact]
    public void Evaluate() {
        var stateProvider = new StateProvider();
        var state = new MutableState<int>(stateProvider, 41);
        var subject = new ComputedState<int>(stateProvider, () => state.Value);

        state.Set(42);

        subject.Evaluate();

        Assert.Equal(42, subject.Value);
    }
}
