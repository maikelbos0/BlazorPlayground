using Microsoft.AspNetCore.Components;
using System.Reflection;

namespace BlazorPlayground.StateManagement.Components;

public abstract class StateManagedComponentBase : ComponentBase, IHandleEvent, IDependent2 {
    private static FieldInfo renderFragmentInfo = typeof(ComponentBase).GetField("_renderFragment", BindingFlags.Instance | BindingFlags.NonPublic)
        ?? throw new InvalidOperationException("The render fragment field cannot be found.");

    private bool hasBuiltDependencyGraph = false;
    private bool isEvaluating = false;

    [Inject]
    public required IStateProvider2 StateProvider { get; set; }

    public StateManagedComponentBase() {
        var originalRenderFragment = (RenderFragment)(renderFragmentInfo.GetValue(this) ?? throw new InvalidOperationException("The render fragment field cannot be read."));
        RenderFragment renderFragment = builder => {
            if (hasBuiltDependencyGraph) {
                originalRenderFragment(builder);
            }
            else {
                var stateProvider = StateProvider ?? throw new InvalidOperationException($"Cannot invoke {nameof(renderFragment)} before {nameof(StateManagedComponentBase)} has dependencies set.");
                stateProvider.BuildDependencyGraph(this, () => originalRenderFragment(builder));
                hasBuiltDependencyGraph = true;
            }
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
