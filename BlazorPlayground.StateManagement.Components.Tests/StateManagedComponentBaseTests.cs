using Microsoft.AspNetCore.Components;
using NSubstitute;
using System;
using System.Threading.Tasks;
using Xunit;

namespace BlazorPlayground.StateManagement.Components.Tests;

public class StateManagedComponentBaseTests {
    private class StateManagedComponent : StateManagedComponentBase {
        public new void OnInitialized() => base.OnInitialized();

        public new void OnAfterRender(bool firstRender) => base.OnAfterRender(firstRender);
    }

    [Fact]
    public async Task HandleEventAsync() {
        var stateProvider = Substitute.For<IStateProvider>();
        var result = 0;
        var workItem = new EventCallbackWorkItem((Action<int>)(value => result = value));

        using var subject = new StateManagedComponent() {
            StateProvider = stateProvider
        };

        await ((IHandleEvent)subject).HandleEventAsync(workItem, 42);

        Assert.Equal(42, result);
    }

    [Fact]
    public void OnInitialized_Gets_DependencyGraphBuilder() {
        var stateProvider = Substitute.For<IStateProvider>();
        var dependencyGraphBuilder = Substitute.For<IDependencyGraphBuilder>();
        stateProvider.GetDependencyGraphBuilder(Arg.Any<IDependent>()).Returns(dependencyGraphBuilder);

        using var subject = new StateManagedComponent() {
            StateProvider = stateProvider
        };

        subject.OnInitialized();
        
        stateProvider.Received(1).GetDependencyGraphBuilder(subject);
    }

    [Fact]
    public void OnAfterRender_FirstRender_Disposes_DependencyGraphBuilder() {
        var stateProvider = Substitute.For<IStateProvider>();
        var dependencyGraphBuilder = Substitute.For<IDependencyGraphBuilder>();
        stateProvider.GetDependencyGraphBuilder(Arg.Any<IDependent>()).Returns(dependencyGraphBuilder);

        using var subject = new StateManagedComponent() {
            StateProvider = stateProvider
        };

        subject.OnInitialized();
        subject.OnAfterRender(true);

        dependencyGraphBuilder.Received(1).Dispose();
    }

    [Fact]
    public void OnAfterRender_Not_FirstRender_Does_Not_Disposes_DependencyGraphBuilder() {
        var stateProvider = Substitute.For<IStateProvider>();
        var dependencyGraphBuilder = Substitute.For<IDependencyGraphBuilder>();
        stateProvider.GetDependencyGraphBuilder(Arg.Any<IDependent>()).Returns(dependencyGraphBuilder);

        using var subject = new StateManagedComponent() {
            StateProvider = stateProvider
        };

        subject.OnInitialized();
        subject.OnAfterRender(false);

        dependencyGraphBuilder.DidNotReceive().Dispose();
    }

    [Fact]
    public void Dispose_Disposes_DependencyGraphBuilder() {
        var stateProvider = Substitute.For<IStateProvider>();
        var dependencyGraphBuilder = Substitute.For<IDependencyGraphBuilder>();
        stateProvider.GetDependencyGraphBuilder(Arg.Any<IDependent>()).Returns(dependencyGraphBuilder);

        using (var subject = new StateManagedComponent() {
            StateProvider = stateProvider
        }) {
            subject.OnInitialized();
        }

        dependencyGraphBuilder.Received(1).Dispose();
    }
}
