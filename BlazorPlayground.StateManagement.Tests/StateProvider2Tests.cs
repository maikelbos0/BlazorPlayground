using NSubstitute;
using System.Collections.Generic;
using Xunit;

namespace BlazorPlayground.StateManagement.Tests;

public class StateProvider2Tests {
    private class DependentDependency : DependentDependencyBase2 {

    }

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

    [Fact]
    public void BuildDependencyGraph_TrackDependency() {
        var subject = new StateProvider2();
        var dependent = Substitute.For<IDependent2>();
        var dependency = Substitute.For<IDependency2>();

        subject.BuildDependencyGraph(dependent, () => {
            subject.TrackDependency(dependency);
        });

        dependency.Received(1).AddDependent(dependent);
    }

    [Fact]
    public void BuildDependencyGraph_TrackDependency_Via_DependentDependency() {
        var subject = new StateProvider2();
        var dependent = Substitute.For<IDependent2>();
        var dependentDependency = Substitute.For<DependentDependencyBase2>();
        var dependency = Substitute.For<IDependency2>();

        subject.BuildDependencyGraph(dependentDependency, () => {
            subject.TrackDependency(dependency);
        });

        subject.BuildDependencyGraph(dependent, () => {
            subject.TrackDependency(dependentDependency);
        });

        dependency.Received(1).AddDependent(dependent);
    }

    [Fact]
    public void ExecuteTransaction() {
        var subject = new StateProvider2();
        var dependent1 = Substitute.For<IDependent2>();
        var dependent2 = Substitute.For<IDependent2>();

        subject.ExecuteTransaction(() => {
            Assert.True(subject.TryRegisterForTransaction([dependent1, dependent2]));
        });

        dependent1.Received(1).Evaluate();
        dependent2.Received(1).Evaluate();
    }

    [Fact]
    public void ExecuteTransaction_Nested_Only_Executes_Once() {
        var subject = new StateProvider2();
        var dependent1 = Substitute.For<IDependent2>();
        var dependent2 = Substitute.For<IDependent2>();

        subject.ExecuteTransaction(() => {
            subject.ExecuteTransaction(() => {
                Assert.True(subject.TryRegisterForTransaction([dependent1, dependent2]));
            });

            subject.ExecuteTransaction(() => {
                Assert.True(subject.TryRegisterForTransaction([dependent1, dependent2]));
            });
        });

        dependent1.Received(1).Evaluate();
        dependent2.Received(1).Evaluate();
    }
}