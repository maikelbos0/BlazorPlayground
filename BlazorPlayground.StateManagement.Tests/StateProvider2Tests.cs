using NSubstitute;
using Xunit;

namespace BlazorPlayground.StateManagement.Tests;

public class StateProvider2Tests { 
    [Fact]
    public void IncrementVersion() {
        var subject = new StateProvider2();
        var version = subject.Version;

        var result = subject.IncrementVersion();
        
        Assert.Equal(version + 1, subject.Version);
        Assert.Equal(subject.Version, result);
    }

    [Fact]
    public void RegisterDependent() {
        var subject = new StateProvider2();
        var dependent = Substitute.For<IDependent2>();

        subject.RegisterDependent(dependent);
        subject.IncrementVersion();

        // TODO more precise determining of dependents
        dependent.Received(1).Evaluate();
    }
}