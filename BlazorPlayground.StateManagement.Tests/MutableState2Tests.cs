using NSubstitute;
using System.Collections.Generic;
using Xunit;

namespace BlazorPlayground.StateManagement.Tests;

public class MutableState2Tests {
    [Fact]
    public void Update() {
        var stateProvider = new StateProvider2();
        var subject = new MutableState2<int>(stateProvider, 41);
        var version = stateProvider.Version;

        subject.Update(value => value + 1);

        Assert.Equal(42, subject.Value);
        Assert.Equal(version + 1, stateProvider.Version);
    }

    [Fact]
    public void Set() {
        var stateProvider = new StateProvider2();
        var subject = new MutableState2<int>(stateProvider, 41);
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
        var stateProvider = new StateProvider2();
        var subject = new MutableState2<int>(stateProvider, 42);
        var version = stateProvider.Version;

        subject.Set(41);

        Assert.Equal(41, subject.Value);
        Assert.Equal(version + 1, stateProvider.Version);
    }

    [Fact]
    public void Set_Evaluates_Dependents() {
        var stateProvider = new StateProvider2();
        var subject = new MutableState2<int>(stateProvider, 41);
        var dependent = Substitute.For<IDependent2>();
        var version = stateProvider.Version;

        stateProvider.BuildDependencyGraph(dependent, () => _ = subject.Value);

        subject.Set(42);

        dependent.Received(1).Evaluate();
    }

    [Fact]
    public void Set_Within_Transaction() {
        var stateProvider = new StateProvider2();
        var subject = new MutableState2<int>(stateProvider, 41);
        var dependent = Substitute.For<IDependent2>();

        subject.AddDependent(dependent);

        stateProvider.ExecuteTransaction(() => {
            subject.Set(42);

            Assert.Equal(42, subject.Value);
            dependent.DidNotReceive().Evaluate();
        });

        dependent.Received(1).Evaluate();
    }
}
