using NSubstitute;
using System.Collections.Generic;
using Xunit;

namespace BlazorPlayground.StateManagement.Tests;

public class StateProviderTests {
    [Fact]
    public void Mutable() {
        var subject = new StateProvider();

        var result = subject.Mutable(42);

        Assert.Equal(42, result.Value);
    }

    [Fact]
    public void Mutable_With_IEqualityComparer() {
        var subject = new StateProvider();

        var result = subject.Mutable(42, Substitute.For<IEqualityComparer<int>>());

        Assert.Equal(42, result.Value);
    }

    [Fact]
    public void Computed() {
        var subject = new StateProvider();
        var mutableState = subject.Mutable(42);

        var result = subject.Computed(() => mutableState.Value);

        Assert.Equal(42, result.Value);
    }

    [Fact]
    public void Effect() {
        var result = 41;
        var subject = new StateProvider();
        var mutableState = subject.Mutable(42);

        subject.Effect(() => result = mutableState.Value);

        Assert.Equal(42, result);
    }
}
