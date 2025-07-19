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
}
