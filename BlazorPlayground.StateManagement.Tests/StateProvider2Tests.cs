using NSubstitute;
using Xunit;

namespace BlazorPlayground.StateManagement.Tests;

public class StateProvider2Tests { 
    [Fact]
    public void IncrementVersion() {
        var subject = new StateProvider2();
        var version = subject.Version;

        subject.IncrementVersion();
        
        Assert.Equal(version + 1, subject.Version);
    }

    [Fact]
    public void RegisterEagerDependent() {
        var subject = new StateProvider2();
        var eagerDependent = Substitute.For<IEagerDependent2>();

        subject.RegisterEagerDependent(eagerDependent);
        subject.IncrementVersion();

        // TODO more precise determining of dependents
        eagerDependent.Received().Evaluate();
    }
}