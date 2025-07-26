using Microsoft.AspNetCore.Components;
using NSubstitute;
using System;
using System.Threading.Tasks;
using Xunit;

namespace BlazorPlayground.StateManagement.Components.Tests;

public class StateManagedLayoutComponentBaseTests {
    private class StateManagedLayoutComponent : StateManagedLayoutComponentBase {
        public new void OnParametersSet() => base.OnParametersSet();

        public new void OnAfterRender(bool firstRender) => base.OnAfterRender(firstRender);
    }

    [Fact]
    public async Task HandleEventAsync() {
        var stateProvider = Substitute.For<IStateProvider>();
        var result = 0;
        var workItem = new EventCallbackWorkItem((Action<int>)(value => result = value));

        using var subject = new StateManagedLayoutComponent() {
            StateProvider = stateProvider
        };

        await ((IHandleEvent)subject).HandleEventAsync(workItem, 42);

        Assert.Equal(42, result);
    }

    [Fact]
    public void OnParametersSet_Gets_DependencyGraphBuilder() {
        var stateProvider = Substitute.For<IStateProvider>();
        var dependencyGraphBuilder = Substitute.For<IDependencyGraphBuilder>();
        stateProvider.GetDependencyGraphBuilder(Arg.Any<IDependent>()).Returns(dependencyGraphBuilder);

        using var subject = new StateManagedLayoutComponent() {
            StateProvider = stateProvider
        };

        subject.OnParametersSet();
        
        stateProvider.Received().GetDependencyGraphBuilder(subject);
    }

    [Fact]
    public void OnAfterRender_FirstRender_Disposes_DependencyGraphBuilder() {
        var stateProvider = Substitute.For<IStateProvider>();
        var dependencyGraphBuilder = Substitute.For<IDependencyGraphBuilder>();
        stateProvider.GetDependencyGraphBuilder(Arg.Any<IDependent>()).Returns(dependencyGraphBuilder);

        using var subject = new StateManagedLayoutComponent() {
            StateProvider = stateProvider
        };

        subject.OnParametersSet();
        subject.OnAfterRender(true);

        dependencyGraphBuilder.Received().Dispose();
    }

    [Fact]
    public void OnAfterRender_Not_FirstRender_Does_Not_Disposes_DependencyGraphBuilder() {
        var stateProvider = Substitute.For<IStateProvider>();
        var dependencyGraphBuilder = Substitute.For<IDependencyGraphBuilder>();
        stateProvider.GetDependencyGraphBuilder(Arg.Any<IDependent>()).Returns(dependencyGraphBuilder);

        using var subject = new StateManagedLayoutComponent() {
            StateProvider = stateProvider
        };

        subject.OnParametersSet();
        subject.OnAfterRender(false);

        dependencyGraphBuilder.DidNotReceive().Dispose();
    }

    [Fact]
    public void Dispose_Disposes_DependencyGraphBuilder() {
        var stateProvider = Substitute.For<IStateProvider>();
        var dependencyGraphBuilder = Substitute.For<IDependencyGraphBuilder>();
        stateProvider.GetDependencyGraphBuilder(Arg.Any<IDependent>()).Returns(dependencyGraphBuilder);

        using (var subject = new StateManagedLayoutComponent() {
            StateProvider = stateProvider
        }) {
            subject.OnParametersSet();
        }

        dependencyGraphBuilder.Received().Dispose();
    }
}
