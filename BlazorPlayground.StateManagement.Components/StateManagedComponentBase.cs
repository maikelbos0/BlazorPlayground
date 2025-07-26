using Microsoft.AspNetCore.Components;

namespace BlazorPlayground.StateManagement.Components;

public abstract class StateManagedComponentBase : ComponentBase, IHandleEvent, IDependent, IDisposable {
    private IDependencyGraphBuilder? dependencyGraphBuilder;
    private bool isDisposed = false;

    [Inject]
    public required IStateProvider StateProvider { get; set; } 

    Task IHandleEvent.HandleEventAsync(EventCallbackWorkItem item, object? arg)
        => item.InvokeAsync(arg);

    protected override void OnInitialized()
        => dependencyGraphBuilder = StateProvider.GetDependencyGraphBuilder(this);

    protected override void OnAfterRender(bool firstRender) {
        if (firstRender) {
            dependencyGraphBuilder?.Dispose();
        }
    }

    public void Evaluate() => StateHasChanged();

    public void Dispose() {
        Dispose(true);
        GC.SuppressFinalize(this);
    }

    protected virtual void Dispose(bool disposing) {
        if (!isDisposed) {
            isDisposed = true;

            if (disposing) {
                dependencyGraphBuilder?.Dispose();
            }
        }
    }
}
