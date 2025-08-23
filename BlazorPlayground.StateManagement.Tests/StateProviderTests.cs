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
    public void Mutable_With_EqualityComparer() {
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

        var effectHandler = subject.Effect(() => result = mutableState.Value);

        Assert.Equal(42, result);
        Assert.Equal(DependentPriority.Medium, effectHandler.Priority);
    }

    [Fact]
    public void Effect_With_Priority() {
        var result = 41;
        var subject = new StateProvider();
        var mutableState = subject.Mutable(42);

        var effectHandler = subject.Effect(() => result = mutableState.Value, DependentPriority.Highest);

        Assert.Equal(42, result);
        Assert.Equal(DependentPriority.Highest, effectHandler.Priority);
    }

    [Fact]
    public void IncrementVersion() {
        var subject = new StateProvider();
        var version = subject.Version;

        var result = subject.IncrementVersion();

        Assert.Equal(version + 1, subject.Version);
        Assert.Equal(subject.Version, result);
    }

    [Fact]
    public void BuildDependencyGraph_TrackDependency_Dependent() {
        var subject = new StateProvider();
        var dependent = Substitute.For<IDependent>();
        var dependency = Substitute.For<IDependency>();

        subject.BuildDependencyGraph(dependent, () => {
            subject.TrackDependency(dependency);
        });

        dependency.Received(1).AddDependent(dependent);
    }

    [Fact]
    public void BuildDependencyGraph_TrackDependency_DependentDependency() {
        var subject = new StateProvider();
        var dependentDependency = Substitute.For<IDependentDependency>();
        var dependency = Substitute.For<IDependency>();

        subject.BuildDependencyGraph(dependentDependency, () => {
            subject.TrackDependency(dependency);
        });

        dependentDependency.Received(1).AddDependency(dependency);
    }

    [Fact]
    public void ExecuteTransaction() {
        var subject = new StateProvider();
        var dependent1 = Substitute.For<IDependent>();
        var dependent2 = Substitute.For<IDependent>();

        subject.ExecuteTransaction(() => {
            Assert.True(subject.TryRegisterForTransaction([dependent1, dependent2]));
        });

        dependent1.Received(1).Evaluate();
        dependent2.Received(1).Evaluate();
    }

    [Fact]
    public void ExecuteTransaction_Nested_Only_Executes_Once() {
        var subject = new StateProvider();
        var dependent1 = Substitute.For<IDependent>();
        var dependent2 = Substitute.For<IDependent>();

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