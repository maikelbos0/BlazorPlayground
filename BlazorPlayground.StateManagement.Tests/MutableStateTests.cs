using NSubstitute;
using Xunit;

namespace BlazorPlayground.StateManagement.Tests;

public class MutableStateTests {
    [Fact]
    public void Update() {
        var stateProvider = new StateProvider();
        var subject = new MutableState<int>(stateProvider, 41);
        var dependent = Substitute.For<IDependent>();

        Assert.True(subject.dependents.TryAdd(dependent, false));

        subject.Update(value => value + 1);

        Assert.Equal(42, subject.Value);
        dependent.Received().Evaluate();
    }

    [Fact]
    public void Set() {
        var stateProvider = new StateProvider();
        var subject = new MutableState<int>(stateProvider, 41);
        var dependent = Substitute.For<IDependent>();

        Assert.True(subject.dependents.TryAdd(dependent, false));

        subject.Set(42);

        Assert.Equal(42, subject.Value);
        dependent.Received().Evaluate();
    }
}
