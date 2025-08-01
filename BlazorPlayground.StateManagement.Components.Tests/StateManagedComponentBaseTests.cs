using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Rendering;
using NSubstitute;
using System;
using System.Reflection;
using System.Threading.Tasks;
using Xunit;

namespace BlazorPlayground.StateManagement.Components.Tests;

public class StateManagedComponentBaseTests {
    private class StateManagedComponent : StateManagedComponentBase {
        public new void StateHasChanged() => base.StateHasChanged();

        public new bool ShouldRender() => base.ShouldRender();
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

    [Theory]
    [InlineData(true, true)]
    [InlineData(false, false)]
    public void ShouldRender_Returns_Correct_Value(bool isEvaluating, bool expectedShouldRender) {
        var stateProvider = Substitute.For<IStateProvider>();
        var subject = new StateManagedComponent() {
            StateProvider = stateProvider
        };

        var isEvaluatingInfo = typeof(StateManagedComponentBase).GetField("isEvaluating", BindingFlags.Instance | BindingFlags.NonPublic);

        Assert.NotNull(isEvaluatingInfo);
        isEvaluatingInfo.SetValue(subject, isEvaluating);

        Assert.Equal(expectedShouldRender, subject.ShouldRender());
    }

    [Fact]
    public void RenderFragment_Builds_DependencyGraph() {
        var stateProvider = Substitute.For<IStateProvider>();
        var dependencyGraphBuilder = Substitute.For<IDependencyGraphBuilder>();
        stateProvider.GetDependencyGraphBuilder(Arg.Any<IDependent>()).Returns(dependencyGraphBuilder);

        var subject = new StateManagedComponent() {
            StateProvider = stateProvider
        };

        var renderFragmentInfo = typeof(ComponentBase).GetField("_renderFragment", BindingFlags.Instance | BindingFlags.NonPublic);

        Assert.NotNull(renderFragmentInfo);

        var renderFragment = Assert.IsType<RenderFragment>(renderFragmentInfo.GetValue(subject));

        Assert.NotNull(renderFragment);
        renderFragment(new RenderTreeBuilder());

        stateProvider.Received(1).GetDependencyGraphBuilder(subject);
        dependencyGraphBuilder.Received(1).Dispose();
    }
}
