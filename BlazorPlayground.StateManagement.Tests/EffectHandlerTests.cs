using NSubstitute;
using System;
using Xunit;

namespace BlazorPlayground.StateManagement.Tests;

public class EffectHandlerTests {
    [Fact]
    public void Constructor_Evaluates() {
        var result = 0;
        var stateProvider = new StateProvider();
        using var subject = new EffectHandler(stateProvider, () => result = 42);

        Assert.Equal(42, result);
    }

    [Fact]
    public void Evaluate() {
        var value = 41;
        var result = 0;
        var stateProvider = new StateProvider();
        using var subject = new EffectHandler(stateProvider, () => result = value);

        value = 42;
        subject.Evaluate();

        Assert.Equal(42, result);
    }

    [Fact]
    public void Evaluate_Builds_Dependency_Graph() {
        var stateProvider = Substitute.For<IStateProvider>();
        using var subject = new EffectHandler(stateProvider, () => { });

        subject.Evaluate();

        stateProvider.Received(2).BuildDependencyGraph(subject, Arg.Any<Action>());
    }

    [Fact]
    public void Dispose() {
        var stateProvider = new StateProvider();
        var subject = new EffectHandler(stateProvider, () => { });

        subject.Dispose();

        Assert.True(subject.IsDisposed);
    }
}
