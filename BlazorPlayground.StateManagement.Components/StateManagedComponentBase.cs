using Microsoft.AspNetCore.Components;
using System;
using System.Reflection;
using System.Threading.Tasks;

namespace BlazorPlayground.StateManagement.Components;

public abstract class StateManagedComponentBase : ComponentBase, IHandleEvent, IDependent {
    private static readonly FieldInfo renderFragmentInfo = typeof(ComponentBase).GetField("_renderFragment", BindingFlags.Instance | BindingFlags.NonPublic)
        ?? throw new InvalidOperationException("The render fragment field cannot be found.");

    private bool isEvaluating = false;

    [Inject]
    public required IStateProvider StateProvider { get; set; }

    public virtual DependentPriority Priority => DependentPriority.Lowest;
    
    public bool IsDisposed { get; private set; }

    public StateManagedComponentBase() {
        var originalRenderFragment = (RenderFragment)(renderFragmentInfo.GetValue(this) ?? throw new InvalidOperationException("The render fragment field cannot be read."));
        RenderFragment renderFragment = builder => {
            var stateProvider = StateProvider ?? throw new InvalidOperationException($"Cannot invoke {nameof(renderFragment)} before {nameof(StateManagedComponentBase)} has dependencies set.");
            stateProvider.BuildDependencyGraph(this, () => originalRenderFragment(builder));
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

    public virtual void Dispose() {
        IsDisposed = true;
        GC.SuppressFinalize(this);
    }
}
