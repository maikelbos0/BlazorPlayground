using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Rendering;
using Microsoft.JSInterop;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BlazorPlayground.BulletHellBeastMode;

public class Game : ComponentBase, IAsyncDisposable {
    public const int Width = 1000;
    public const int Height = 1000;
    public const string ModuleLocation = "./_content/BlazorPlayground.BulletHellBeastMode/game.0.js";

    private ElementReference? canvasReference;
    private IJSObjectReference? moduleReference;
    private DotNetObjectReference<Game>? dotNetObjectReference;
    private readonly Dictionary<Guid, GameElement> gameElements = [];

    [Inject] public IJSRuntime JSRuntime { get; set; } = null!;

    [Inject] public IGameElementProvider GameElementProvider { get; set; } = null!;

    protected override void BuildRenderTree(RenderTreeBuilder builder) {
        builder.OpenElement(0, "canvas");
        builder.AddElementReferenceCapture(1, (elementReference) => canvasReference = elementReference);
        builder.CloseElement();
    }

    protected override async Task OnAfterRenderAsync(bool firstRender) {
        if (firstRender) {
            moduleReference = await JSRuntime.InvokeAsync<IJSObjectReference>("import", ModuleLocation);
            dotNetObjectReference = DotNetObjectReference.Create(this);
            await moduleReference.InvokeVoidAsync("initialize", canvasReference, dotNetObjectReference, Width, Height);

            var test = await AddGameElement("basic-ship", new(Width * 0.5, Height * 0.9));
            //await RemoveGameElement(test);
        }
    }

    public async Task<Guid> AddGameElement(string assetName, Coordinate position) {
        if (moduleReference == null) {
            throw new InvalidOperationException();
        }

        var id = Guid.NewGuid();
        var gameElement = await GameElementProvider.CreateFromAsset(assetName, position);

        gameElements.Add(id, gameElement);
        await moduleReference.InvokeVoidAsync("addGameElement", id, gameElement);

        return id;
    }

    public async Task RemoveGameElement(Guid id) {
        if (moduleReference == null) {
            throw new InvalidOperationException();
        }

        gameElements.Remove(id);
        await moduleReference.InvokeVoidAsync("removeGameElement", id);
    }

    [JSInvokable]
    public void ProcessElapsedTime(double elapsedMilliseconds) {
        Console.WriteLine($"Elapsed: {elapsedMilliseconds}");
    }

    public async ValueTask DisposeAsync() {
        if (moduleReference != null) {
            await moduleReference.InvokeVoidAsync("requestCancellation");
            await moduleReference.DisposeAsync();
        }

        dotNetObjectReference?.Dispose();

        GC.SuppressFinalize(this);
    }
}
