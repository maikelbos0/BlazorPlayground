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
}
