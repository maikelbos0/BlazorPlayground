using Microsoft.AspNetCore.Components;
using NSubstitute;
using System;
using System.Threading.Tasks;
using Xunit;

namespace BlazorPlayground.StateManagement.Components.Tests;

public class StateManagedComponentBaseTests {
    private class StateManagedComponent : StateManagedComponentBase {
        public new void StateHasChanged() => base.StateHasChanged();
    }

    [Fact]
    public async Task HandleEventAsync() {
        var stateProvider = Substitute.For<IStateProvider>();
        var result = 0;
        var workItem = new EventCallbackWorkItem((Action<int>)(value => result = value));

        var subject = new StateManagedComponent() {
            StateProvider = stateProvider
        };

        await ((IHandleEvent)subject).HandleEventAsync(workItem, 42);

        Assert.Equal(42, result);
    }

    [Fact]
    public void StateHasChanged_Builds_DependencyGraph() {
        var stateProvider = Substitute.For<IStateProvider>();
        var dependencyGraphBuilder = Substitute.For<IDependencyGraphBuilder>();
        stateProvider.GetDependencyGraphBuilder(Arg.Any<IDependent>()).Returns(dependencyGraphBuilder);

        var subject = new StateManagedComponent() {
            StateProvider = stateProvider
        };

        subject.StateHasChanged();

        stateProvider.Received(1).GetDependencyGraphBuilder(subject);
        dependencyGraphBuilder.Received(1).Dispose();
    }
}
