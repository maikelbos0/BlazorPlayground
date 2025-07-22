using NSubstitute;
using Xunit;

namespace BlazorPlayground.StateManagement.Tests;

public class StateProviderTests {
    [Fact]
    public void State() {
        var subject = new StateProvider();

        var result = subject.State(42);

        Assert.Equal(42, result.Value);
    }

    [Fact]
    public void StartBuildingDependencyGraph() {
        var subject = new StateProvider();
        var firstDependent = Substitute.For<IDependent>();
        var secondDependent = Substitute.For<IDependent>();

        subject.StartBuildingDependencyGraph(firstDependent);
        subject.StartBuildingDependencyGraph(secondDependent);

        var trackedDependent = Assert.Single(subject.TrackedDependents);
        Assert.Equal(Environment.CurrentManagedThreadId, trackedDependent.Key);
        Assert.Equivalent(new List<IDependent>() { firstDependent, secondDependent }, trackedDependent.Value);
    }

    [Fact]
    public void TrackDependency() {
        var subject = new StateProvider();
        var dependent = Substitute.For<IDependent>();
        var otherDependent = Substitute.For<IDependent>();
        var dependency = Substitute.For<IDependency>();

        Assert.True(subject.TrackedDependents.TryAdd(Environment.CurrentManagedThreadId, [dependent]));
        Assert.True(subject.TrackedDependents.TryAdd(Environment.CurrentManagedThreadId - 1, [otherDependent]));
        dependency.Dependents.Returns([]);

        subject.TrackDependency(dependency);

        Assert.Equal(dependent, Assert.Single(dependency.Dependents));
    }

    [Fact]
    public void StopBuildingDependencyGraph() {
        var subject = new StateProvider();
        var firstDependent = Substitute.For<IDependent>();
        var secondDependent = Substitute.For<IDependent>();

        Assert.True(subject.TrackedDependents.TryAdd(Environment.CurrentManagedThreadId, [firstDependent, secondDependent]));

        subject.StopBuildingDependencyGraph(firstDependent);
        Assert.Equal(secondDependent, Assert.Single(Assert.Single(subject.TrackedDependents).Value));

        subject.StopBuildingDependencyGraph(secondDependent);
        Assert.Empty(subject.TrackedDependents);
    }
}
