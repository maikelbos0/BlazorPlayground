using Xunit;

namespace BlazorPlayground.StateManagement.Tests;

public class MutableStateTests {
    [Fact]
    public void Value() {
        var stateProvider = new StateProvider();
        var subject = new MutableState<int>(stateProvider, 41);
        var computedState = new ComputedState<int>(stateProvider, () => subject.Value);

        Assert.Equal(computedState, Assert.Single(subject.Dependents));
    }

    [Fact]
    public void Update() {
        var subject = new MutableState<int>(new StateProvider(), 41);

        subject.Update(value => value + 1);

        Assert.Equal(42, subject.Value);
    }

    [Fact]
    public void Set() {
        var stateProvider = new StateProvider();
        var subject = new MutableState<int>(stateProvider, 41);
        var computedState = new ComputedState<int>(stateProvider, () => subject.Value);

        subject.Set(42);

        Assert.Equal(42, subject.Value);
        Assert.Equal(42, computedState.Value);
    }
}
