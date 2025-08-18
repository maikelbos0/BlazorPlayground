using NSubstitute;
using System.Collections.Generic;
using Xunit;

namespace BlazorPlayground.StateManagement.Tests;

public class StateProvider2Tests {
    [Fact]
    public void Mutable() {
        var subject = new StateProvider2();

        var result = subject.Mutable(42);

        Assert.Equal(42, result.Value);
    }

    [Fact]
    public void Mutable_With_IEqualityComparer() {
        var subject = new StateProvider2();

        var result = subject.Mutable(42, Substitute.For<IEqualityComparer<int>>());

        Assert.Equal(42, result.Value);
    }

    [Fact]
    public void Computed() {
        var subject = new StateProvider2();
        var mutableState = subject.Mutable(42);

        var result = subject.Computed(() => mutableState.Value);

        Assert.Equal(42, result.Value);
    }

    [Fact]
    public void Effect() {
        var result = 41;
        var subject = new StateProvider2();
        var mutableState = subject.Mutable(42);

        subject.Effect(() => result = mutableState.Value);

        Assert.Equal(42, result);
    }

    [Fact]
    public void IncrementVersion() {
        var subject = new StateProvider2();
        var version = subject.Version;

        var result = subject.IncrementVersion();

        Assert.Equal(version + 1, subject.Version);
        Assert.Equal(subject.Version, result);
    }
}