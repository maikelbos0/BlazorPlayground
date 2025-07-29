using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Rendering;
using System.Diagnostics;

namespace BlazorPlayground.StateManagement.Components;

public abstract class StateManagedComponentBase : IComponent, IHandleEvent, IHandleAfterRender, IDependent, IDisposable {
    private readonly RenderFragment renderFragment;
    private (IComponentRenderMode? mode, bool cached) renderMode;
    private RenderHandle renderHandle;
    private bool initialized;
    private bool hasNeverRendered = true;
    private bool hasPendingQueuedRender;
    private bool hasCalledOnAfterRender;
    private IDependencyGraphBuilder? dependencyGraphBuilder;
    private bool isDisposed = false;

    [Inject]
    public required IStateProvider StateProvider { get; set; }

    /// <summary>
    /// Constructs an instance of <see cref="StateManagedComponentBase"/>.
    /// </summary>
    public StateManagedComponentBase() {
        renderFragment = builder => {
            hasPendingQueuedRender = false;
            hasNeverRendered = false;
            BuildRenderTree(builder);
        };
    }

    /// <inheritdoc cref="ComponentBase.RendererInfo"/>
    protected RendererInfo RendererInfo => renderHandle.RendererInfo;

    /// <inheritdoc cref="ComponentBase.Assets"/>
    protected ResourceAssetCollection Assets => renderHandle.Assets;

    /// <inheritdoc cref="ComponentBase.AssignedRenderMode"/>
    protected IComponentRenderMode? AssignedRenderMode {
        get {
            if (!renderMode.cached) {
                renderMode = (renderHandle.RenderMode, true);
            }

            return renderMode.mode;
        }
    }

    /// <inheritdoc cref="ComponentBase.BuildRenderTree(RenderTreeBuilder)"/>
    protected virtual void BuildRenderTree(RenderTreeBuilder builder) { }

    /// <inheritdoc cref="ComponentBase.OnInitialized"/>
    protected virtual void OnInitialized()
        => dependencyGraphBuilder = StateProvider.GetDependencyGraphBuilder(this);

    /// <inheritdoc cref="ComponentBase.OnInitializedAsync"/>
    protected virtual Task OnInitializedAsync()
        => Task.CompletedTask;

    /// <inheritdoc cref="ComponentBase.OnParametersSet"/>
    protected virtual void OnParametersSet() { }

    /// <inheritdoc cref="ComponentBase.OnParametersSetAsync"/>
    protected virtual Task OnParametersSetAsync()
        => Task.CompletedTask;

    /// <inheritdoc cref="ComponentBase.StateHasChanged"/>
    protected void StateHasChanged() {
        if (hasPendingQueuedRender) {
            return;
        }

        if (hasNeverRendered || ShouldRender() || renderHandle.IsRenderingOnMetadataUpdate) {
            hasPendingQueuedRender = true;

            try {
                renderHandle.Render(renderFragment);
            }
            catch {
                hasPendingQueuedRender = false;
                throw;
            }
        }
    }

    /// <inheritdoc cref="ComponentBase.ShouldRender"/>
    protected virtual bool ShouldRender()
        => true;

    /// <inheritdoc cref="ComponentBase.OnAfterRender(bool)"/>
    protected virtual void OnAfterRender(bool firstRender) {
        if (firstRender) {
            dependencyGraphBuilder?.Dispose();
        }
    }

    /// <inheritdoc cref="ComponentBase.OnAfterRenderAsync(bool)"/>
    protected virtual Task OnAfterRenderAsync(bool firstRender)
        => Task.CompletedTask;

    /// <inheritdoc cref="ComponentBase.InvokeAsync(Action)"/>
    protected Task InvokeAsync(Action workItem)
        => renderHandle.Dispatcher.InvokeAsync(workItem);

    /// <inheritdoc cref="ComponentBase.InvokeAsync(Func{Task})"/>
    protected Task InvokeAsync(Func<Task> workItem)
        => renderHandle.Dispatcher.InvokeAsync(workItem);

    /// <inheritdoc cref="ComponentBase.DispatchExceptionAsync(Exception)"/>
    protected Task DispatchExceptionAsync(Exception exception)
        => renderHandle.DispatchExceptionAsync(exception);

    void IComponent.Attach(RenderHandle renderHandle) {
        if (this.renderHandle.IsInitialized) {
            throw new InvalidOperationException($"The render handle is already set. Cannot initialize a {nameof(Microsoft.AspNetCore.Components.ComponentBase)} more than once.");
        }

        this.renderHandle = renderHandle;
    }

    /// <inheritdoc cref="ComponentBase.SetParametersAsync(ParameterView)"/>
    public virtual Task SetParametersAsync(ParameterView parameters) {
        parameters.SetParameterProperties(this);
        if (!initialized) {
            initialized = true;

            return RunInitAndSetParametersAsync();
        }
        else {
            return CallOnParametersSetAsync();
        }
    }

    [DebuggerDisableUserUnhandledExceptions]
    private async Task RunInitAndSetParametersAsync() {
        Task task;

        try {
            OnInitialized();
            task = OnInitializedAsync();
        }
        catch (Exception ex) when (ex is not NavigationException) {
            Debugger.BreakForUserUnhandledException(ex);
            throw;
        }

        if (task.Status != TaskStatus.RanToCompletion && task.Status != TaskStatus.Canceled) {
            try {
                await task;
            }
            catch {
                if (!task.IsCanceled) {
                    throw;
                }
            }
        }

        StateHasChanged();

        await CallOnParametersSetAsync();
    }

    [DebuggerDisableUserUnhandledExceptions]
    private async Task CallOnParametersSetAsync() {
        Task task;

        try {
            OnParametersSet();
            task = OnParametersSetAsync();
        }
        catch (Exception ex) when (ex is not NavigationException) {
            Debugger.BreakForUserUnhandledException(ex);
            throw;
        }

        if (task.Status != TaskStatus.RanToCompletion && task.Status != TaskStatus.Canceled) {
            try {
                await task;
            }
            catch {
                if (task.IsCanceled) {
                    return;
                }

                throw;
            }
        }
    }

    Task IHandleEvent.HandleEventAsync(EventCallbackWorkItem item, object? arg)
        => item.InvokeAsync(arg);

    Task IHandleAfterRender.OnAfterRenderAsync() {
        var firstRender = !hasCalledOnAfterRender;
        hasCalledOnAfterRender = true;

        OnAfterRender(firstRender);

        return OnAfterRenderAsync(firstRender);
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
