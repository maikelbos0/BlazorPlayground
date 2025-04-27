using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Rendering;
using Microsoft.JSInterop;

namespace BlazorPlayground.BulletHellBeastMode;

public class Game : ComponentBase, IAsyncDisposable {
    public const int Width = 1000;
    public const int Height = 1000;
    public const int FrameRate = 1000;
    public const string ModuleLocation = "./_content/BlazorPlayground.BulletHellBeastMode/game.0.js";

    private ElementReference? canvasReference;
    private IJSObjectReference? moduleReference;
    private DotNetObjectReference<Game>? dotNetObjectReference;

    [Inject] internal IJSRuntime JSRuntime { get; set; } = null!;

    protected override void BuildRenderTree(RenderTreeBuilder builder) {
        builder.OpenElement(0, "canvas");
        builder.AddElementReferenceCapture(1, (elementReference) => canvasReference = elementReference);
        builder.CloseElement();
    }

    protected override async Task OnAfterRenderAsync(bool firstRender) {
        if (firstRender) {
            moduleReference = await JSRuntime.InvokeAsync<IJSObjectReference>("import", ModuleLocation);
            await moduleReference.InvokeVoidAsync("initialize", canvasReference, Width, Height, FrameRate);
        }
    }

    public async ValueTask DisposeAsync() {
        if (moduleReference != null) {
            await moduleReference.DisposeAsync();
        }

        dotNetObjectReference?.Dispose();
        GC.SuppressFinalize(this);
    }
}
