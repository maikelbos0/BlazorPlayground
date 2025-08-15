using NSubstitute;
using System.Collections.Generic;
using Xunit;

namespace BlazorPlayground.StateManagement.Tests;

public class MutableStateTests {
    [Fact]
    public void Update() {
        var stateProvider = new StateProvider();
        var subject = new MutableState<int>(stateProvider, 41);
        var dependent = Substitute.For<IDependent>();

        subject.AddDependent(dependent);

        subject.Update(value => value + 1);

        Assert.Equal(42, subject.Value);
        dependent.Received(1).Evaluate();
    }

    [Fact]
    public void Set() {
        var stateProvider = new StateProvider();
        var subject = new MutableState<int>(stateProvider, 41);
        var dependent = Substitute.For<IDependent>();

        subject.AddDependent(dependent);

        subject.Set(41);

        Assert.Equal(41, subject.Value);
        dependent.DidNotReceive().Evaluate();

        subject.Set(42);

        Assert.Equal(42, subject.Value);
        dependent.Received(1).Evaluate();
    }

    [Fact]
    public void Set_With_EqualityComparer() {
        var equalityComparer = Substitute.For<IEqualityComparer<int>>();
        equalityComparer.Equals(Arg.Any<int>(), Arg.Any<int>()).Returns(false);
        var stateProvider = new StateProvider();
        var subject = new MutableState<int>(stateProvider, 42);
        var dependent = Substitute.For<IDependent>();

        subject.AddDependent(dependent);

        subject.Set(41);

        Assert.Equal(41, subject.Value);
        dependent.Received(1).Evaluate();
    }

    [Fact]
    public void Set_Within_Transaction() {
        var stateProvider = new StateProvider();
        var subject = new MutableState<int>(stateProvider, 41);
        var dependent = Substitute.For<IDependent>();

        subject.AddDependent(dependent);

        stateProvider.ExecuteTransaction(() => {
            subject.Set(42);

            Assert.Equal(42, subject.Value);
            dependent.DidNotReceive().Evaluate();
        });

        dependent.Received(1).Evaluate();
    }
}
