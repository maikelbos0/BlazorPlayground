using NSubstitute;
using Xunit;

namespace BlazorPlayground.StateManagement.Tests;

public class StateProviderTests {
    // TODO clean up
    private class TestState<T> : State<T> {
        public override T Value { get; }

        public TestState(StateProvider stateProvider, T value) : base(stateProvider) {
            Value = value;
        }
    }

    [Fact]
    public void StateCreatesNewMutableState() {
        var subject = new StateProvider();

        var result = subject.State(42, "meaning");

        Assert.Equal(42, result.Value);
    }

    [Fact]
    public void StateReturnsAndUpdatesExistingMutableState() {
        var subject = new StateProvider();

        subject.StateCollection.AddOrUpdate(new StateKey(typeof(int), "meaning"), _ => new MutableState<int>(subject, 40), (_, _) => throw new NullReferenceException());

        var result = subject.State(42, "meaning");

        Assert.Equal(42, result.Value);
    }

    [Fact]
    public void StateThrowsExceptionForInvalidExistingState() {
        var subject = new StateProvider();

        subject.StateCollection.AddOrUpdate(new StateKey(typeof(int), "meaning"), _ => new TestState<int>(subject, 40), (_, _) => throw new NullReferenceException());

        var exception = Assert.Throws<InvalidStateTypeException>(() => subject.State(42, "meaning"));

        Assert.Equal(typeof(TestState<int>), exception.FoundType);
        Assert.Equal(typeof(MutableState<int>), exception.ExpectedType);
        Assert.Equal(new StateKey(typeof(int), "meaning"), exception.Key);
    }

    [Fact]
    public void StartBuildingDependencyGraph() {
        var subject = new StateProvider();
        var dependent = Substitute.For<IDependent>();

        subject.StartBuildingDependencyGraph(dependent);

        var trackedDependent = Assert.Single(subject.TrackedDependents);
        Assert.Same(dependent, trackedDependent.Key);
        Assert.Equal(Environment.CurrentManagedThreadId, trackedDependent.Value);
    }

    [Fact]
    public void TrackDependency() {
        var subject = new StateProvider();
        var dependent = Substitute.For<IDependent>();
        var otherDependent = Substitute.For<IDependent>();
        var dependency = Substitute.For<IDependency>();

        Assert.True(subject.TrackedDependents.TryAdd(dependent, Environment.CurrentManagedThreadId));
        Assert.True(subject.TrackedDependents.TryAdd(otherDependent, Environment.CurrentManagedThreadId - 1));
        dependency.Dependents.Returns([]);

        subject.TrackDependency(dependency);

        Assert.Equal(dependent, Assert.Single(dependency.Dependents));
    }

    [Fact]
    public void StopBuildingDependencyGraph() {
        var subject = new StateProvider();
        var dependent = Substitute.For<IDependent>();

        Assert.True(subject.TrackedDependents.TryAdd(dependent, Environment.CurrentManagedThreadId));

        subject.StopBuildingDependencyGraph(dependent);
        Assert.Empty(subject.TrackedDependents);
    }
}
