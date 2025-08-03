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
}
