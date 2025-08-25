using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Rendering;
using NSubstitute;
using System;
using System.Reflection;
using System.Threading.Tasks;
using Xunit;

namespace BlazorPlayground.StateManagement.Components.Tests;

public class StateManagedLayoutComponentBaseTests {
    private class StateManagedLayoutComponent : StateManagedLayoutComponentBase {
        public int BuildRenderTreeInvocations { get; private set; }

        public new void StateHasChanged() => base.StateHasChanged();

        public new bool ShouldRender() => base.ShouldRender();

        protected override void BuildRenderTree(RenderTreeBuilder builder) => BuildRenderTreeInvocations++;
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

    [Theory]
    [InlineData(true, true)]
    [InlineData(false, false)]
    public void ShouldRender_Returns_Correct_Value(bool isEvaluating, bool expectedShouldRender) {
        var stateProvider = Substitute.For<IStateProvider>();
        using var subject = new StateManagedLayoutComponent() {
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
        using var subject = new StateManagedLayoutComponent() {
            StateProvider = stateProvider
        };

        stateProvider.When(x => x.BuildDependencyGraph(subject, Arg.Any<Action>())).Do(callInfo => callInfo.ArgAt<Action>(1)());

        var renderFragmentInfo = typeof(ComponentBase).GetField("_renderFragment", BindingFlags.Instance | BindingFlags.NonPublic);

        Assert.NotNull(renderFragmentInfo);

        var renderFragment = Assert.IsType<RenderFragment>(renderFragmentInfo.GetValue(subject));

        Assert.NotNull(renderFragment);
        renderFragment(new RenderTreeBuilder());

        stateProvider.Received(1).BuildDependencyGraph(subject, Arg.Any<Action>());
        Assert.Equal(1, subject.BuildRenderTreeInvocations);
    }

    [Fact]
    public void RenderFragment_Builds_DependencyGraph_Every_Time() {
        var stateProvider = Substitute.For<IStateProvider>();
        using var subject = new StateManagedLayoutComponent() {
            StateProvider = stateProvider
        };

        stateProvider.When(x => x.BuildDependencyGraph(subject, Arg.Any<Action>())).Do(callInfo => callInfo.ArgAt<Action>(1)());

        var renderFragmentInfo = typeof(ComponentBase).GetField("_renderFragment", BindingFlags.Instance | BindingFlags.NonPublic);

        Assert.NotNull(renderFragmentInfo);

        var renderFragment = Assert.IsType<RenderFragment>(renderFragmentInfo.GetValue(subject));

        Assert.NotNull(renderFragment);
        renderFragment(new RenderTreeBuilder());
        renderFragment(new RenderTreeBuilder());

        stateProvider.Received(2).BuildDependencyGraph(subject, Arg.Any<Action>());
        Assert.Equal(2, subject.BuildRenderTreeInvocations);
    }

    [Fact]
    public void Dispose() {
        var stateProvider = new StateProvider();
        var subject = new StateManagedLayoutComponent() {
            StateProvider = stateProvider
        };

        subject.Dispose();

        Assert.True(subject.IsDisposed);
    }
}
