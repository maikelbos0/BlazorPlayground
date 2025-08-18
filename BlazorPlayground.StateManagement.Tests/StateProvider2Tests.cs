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
}