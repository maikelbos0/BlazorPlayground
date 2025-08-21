using NSubstitute;
using System;
using Xunit;

namespace BlazorPlayground.StateManagement.Tests;

public class ComputedState2Tests {
    [Fact]
    public void Value_Tracks_Dependency() {
        var stateProvider = Substitute.For<IStateProvider2>();
        var subject = new ComputedState2<int>(stateProvider, () => 42);

        _ = subject.Value;

        stateProvider.Received(1).TrackDependency(subject);
    }

    [Fact]
    public void Value_Builds_Dependency_Graph() {
        var stateProvider = Substitute.For<IStateProvider2>();
        var subject = new ComputedState2<int>(stateProvider, () => 42);

        stateProvider.Version.Returns(1u);

        _ = subject.Value;

        stateProvider.Received(1).BuildDependencyGraph(subject, Arg.Any<Action>());
    }

    [Fact]
    public void Value_Updates_After_Version_Increase() {
        var value = 41;
        var stateProvider = new StateProvider2();
        var subject = new ComputedState2<int>(stateProvider, () => value);

        value = 42;
        stateProvider.IncrementVersion();

        Assert.Equal(42, subject.Value);
    }
}
