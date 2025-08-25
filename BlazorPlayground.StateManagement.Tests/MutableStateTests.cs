using NSubstitute;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace BlazorPlayground.StateManagement.Tests;

public class MutableStateTests {
    [Fact]
    public void Value_Tracks_Dependency() {
        var stateProvider = Substitute.For<IStateProvider>();
        var subject = new MutableState<int>(stateProvider, 42);

        _ = subject.Value;

        stateProvider.Received(1).TrackDependency(subject);
    }

    [Fact]
    public void Update() {
        var stateProvider = new StateProvider();
        var subject = new MutableState<int>(stateProvider, 41);
        var version = stateProvider.Version;

        subject.Update(value => value + 1);

        Assert.Equal(42, subject.Value);
        Assert.Equal(version + 1, stateProvider.Version);
    }

    [Fact]
    public void Set() {
        var stateProvider = new StateProvider();
        var subject = new MutableState<int>(stateProvider, 41);
        var version = stateProvider.Version;

        subject.Set(41);

        Assert.Equal(41, subject.Value);
        Assert.Equal(version, stateProvider.Version);

        subject.Set(42);

        Assert.Equal(42, subject.Value);
        Assert.Equal(version + 1, stateProvider.Version);
    }

    [Fact]
    public void Set_With_EqualityComparer() {
        var equalityComparer = Substitute.For<IEqualityComparer<int>>();
        equalityComparer.Equals(Arg.Any<int>(), Arg.Any<int>()).Returns(false);
        var stateProvider = new StateProvider();
        var subject = new MutableState<int>(stateProvider, 42);
        var version = stateProvider.Version;

        subject.Set(41);

        Assert.Equal(41, subject.Value);
        Assert.Equal(version + 1, stateProvider.Version);
    }

    [Fact]
    public void Set_Evaluates_Active_Dependents() {
        var stateProvider = new StateProvider();
        var subject = new MutableState<int>(stateProvider, 41);
        var dependent1 = Substitute.For<IDependent>();
        var dependent2 = Substitute.For<IDependent>();
        var dependent3 = Substitute.For<IDependent>();
        var version = stateProvider.Version;

        dependent1.Priority.Returns(DependentPriority.Low);
        dependent2.Priority.Returns(DependentPriority.High);
        dependent3.IsDisposed.Returns(true);

        stateProvider.BuildDependencyGraph(dependent1, () => _ = subject.Value);
        stateProvider.BuildDependencyGraph(dependent2, () => _ = subject.Value);
        stateProvider.BuildDependencyGraph(dependent3, () => _ = subject.Value);

        subject.Set(42);

        Received.InOrder(() => {
            dependent2.Evaluate();
            dependent1.Evaluate();
        });

        dependent3.DidNotReceive().Evaluate();
    }

    [Fact]
    public void Set_Within_Transaction_Registers_Active_Dependents() {
        var stateProvider = Substitute.For<IStateProvider>();
        var subject = new MutableState<int>(stateProvider, 41);
        var dependent1 = Substitute.For<IDependent>();
        var dependent2 = Substitute.For<IDependent>();
        var dependent3 = Substitute.For<IDependent>();

        dependent3.IsDisposed.Returns(true);

        stateProvider.TryRegisterForTransaction(Arg.Any<IEnumerable<IDependent>>()).Returns(true);

        subject.AddDependent(dependent1);
        subject.AddDependent(dependent2);
        subject.AddDependent(dependent3);

        subject.Set(42);

        stateProvider.Received(1).TryRegisterForTransaction(Arg.Is<IEnumerable<IDependent>>(dependents => dependents.Count() == 2 && dependents.Contains(dependent1) && dependents.Contains(dependent2)));

        dependent1.DidNotReceive().Evaluate();
        dependent2.DidNotReceive().Evaluate();
        dependent3.DidNotReceive().Evaluate();
    }
}
