using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Rendering;
using Microsoft.JSInterop;

namespace BlazorPlayground.BulletHellBeastMode;

public class Game : ComponentBase, IAsyncDisposable {
    public const int Width = 1000;
    public const int Height = 1000;
    public const int FrameRate = 1000;
    public const string ModuleLocation = "./_content/BlazorPlayground.BulletHellBeastMode/game.0.js";
    public const string AssetsLocation = "./_content/BlazorPlayground.BulletHellBeastMode/assets/";

    private ElementReference? canvasReference;
    private IJSObjectReference? moduleReference;
    private DotNetObjectReference<Game>? dotNetObjectReference;
    private readonly Dictionary<Guid, GameElement> gameElements = [];

    [Inject] internal IJSRuntime JSRuntime { get; set; } = null!;

    [Inject] internal HttpClient HttpClient { get; set; } = null!;

    protected override void BuildRenderTree(RenderTreeBuilder builder) {
        builder.OpenElement(0, "canvas");
        builder.AddElementReferenceCapture(1, (elementReference) => canvasReference = elementReference);
        builder.CloseElement();
    }

    protected override async Task OnAfterRenderAsync(bool firstRender) {
        if (firstRender) {
            moduleReference = await JSRuntime.InvokeAsync<IJSObjectReference>("import", ModuleLocation);
            await moduleReference.InvokeVoidAsync("initialize", canvasReference, Width, Height, FrameRate);

            var basicShip = GameElementSerializer.Deserialize(await HttpClient.GetStringAsync(AssetsLocation + "basic-ship.json"));
            await AddGameElement(Guid.NewGuid(), basicShip);
        }
    }

    private async Task AddGameElement(Guid id, GameElement gameElement) {
        if (moduleReference == null) {
            throw new InvalidOperationException();
        }

        gameElements.Add(id, gameElement);
        await moduleReference.InvokeVoidAsync("addGameElement", id, gameElement.ForCanvas());
    }

    private async Task RemoveGameElement(Guid id) {
        if (moduleReference == null) {
            throw new InvalidOperationException();
        }

        gameElements.Remove(id);
        await moduleReference.InvokeVoidAsync("removeGameElement", id);
    }

    public async ValueTask DisposeAsync() {
        if (moduleReference != null) {
            await RemoveGameElement(gameElements.Keys.Single());
            await moduleReference.DisposeAsync();
        }

        dotNetObjectReference?.Dispose();
        GC.SuppressFinalize(this);
    }
}
