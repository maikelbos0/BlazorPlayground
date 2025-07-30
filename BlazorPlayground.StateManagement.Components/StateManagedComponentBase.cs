using Microsoft.AspNetCore.Components;
using System.Reflection;

namespace BlazorPlayground.StateManagement.Components;

public abstract class StateManagedComponentBase : ComponentBase, IHandleEvent, IDependent {
    private static FieldInfo renderFragmentInfo = typeof(ComponentBase).GetField("_renderFragment", BindingFlags.Instance | BindingFlags.NonPublic)
        ?? throw new InvalidOperationException("The render fragment field cannot be found.");

    private bool isEvaluating = false;

    [Inject]
    public required IStateProvider StateProvider { get; set; }

    public StateManagedComponentBase() {
        var originalRenderFragment = (RenderFragment)(renderFragmentInfo.GetValue(this) ?? throw new InvalidOperationException("The render fragment field cannot be read."));
        RenderFragment renderFragment = renderFragment = builder => {
            var stateProvider = StateProvider ?? throw new InvalidOperationException($"Cannot invoke {nameof(renderFragment)} before {nameof(StateManagedComponentBase)} has dependencies set.");
            using var dependencyGraphBuilder = stateProvider.GetDependencyGraphBuilder(this);
            originalRenderFragment(builder);
        };

        renderFragmentInfo.SetValue(this, renderFragment);
    }

    protected sealed override bool ShouldRender() => isEvaluating;

    public void Evaluate() {
        isEvaluating = true;
        StateHasChanged();
        isEvaluating = false;
    }

    Task IHandleEvent.HandleEventAsync(EventCallbackWorkItem item, object? arg)
        => item.InvokeAsync(arg);
}
