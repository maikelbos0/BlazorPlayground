﻿using Microsoft.AspNetCore.Components;
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
    private readonly Dictionary<string, Direction> keyMap = new() {
        { "a", Direction.Left },
        { "d", Direction.Right },
        { "w", Direction.Up },
        { "s", Direction.Down }
    };

    private ElementReference? canvasReference;
    private IJSObjectReference? moduleReference;
    private DotNetObjectReference<Game>? dotNetObjectReference;
    private Ship? ship;
    private readonly Dictionary<Guid, IGameElement> gameElements = [];

    [Inject]
    public IJSRuntime JSRuntime { get; set; } = null!;

    [Inject]
    public IGameElementProvider GameElementProvider { get; set; } = null!;

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
            ship = await GameElementProvider.CreateShip(new(Width * 0.5, Height * 0.9));
            await AddGameElement(ship);
        }
    }

    private async Task AddGameElement(IGameElement gameElement) {
        if (moduleReference == null) {
            throw new InvalidOperationException();
        }

        gameElements.Add(gameElement.Id, gameElement);
        await moduleReference.InvokeVoidAsync("setGameElement", gameElement);
    }

    public async Task RemoveGameElement(Guid id) {
        if (moduleReference == null) {
            throw new InvalidOperationException();
        }

        gameElements.Remove(id);
        await moduleReference.InvokeVoidAsync("removeGameElement", id);
    }

    [JSInvokable]
    public async Task ProcessElapsedTime(double elapsedSeconds) {
        if (moduleReference == null) {
            throw new InvalidOperationException();
        }

        var movedGameElements = new List<IGameElement>();

        foreach (var gameElement in gameElements.Values) {
            if (gameElement.ProcessElapsedTime(elapsedSeconds)) {
                movedGameElements.Add(gameElement);
            }
        }

        if (movedGameElements.Count > 0) {
            await moduleReference.InvokeVoidAsync("setGameElements", movedGameElements);
        }
    }

    [JSInvokable]
    public void SetTargetPosition(double x, double y) {
        if (ship == null) {
            throw new InvalidOperationException();
        }

        ship.TargetPosition = new(x, y);
    }

    [JSInvokable]
    public void ResetTargetPosition() {
        if (ship == null) {
            throw new InvalidOperationException();
        }

        ship.TargetPosition = null;
    }

    [JSInvokable]
    public void AddDirection(string key) {
        if (ship == null) {
            throw new InvalidOperationException();
        }

        if (keyMap.TryGetValue(key, out var direction)) {
            ship.Direction |= direction;
        }
    }

    [JSInvokable]
    public void RemoveDirection(string key) {
        if (ship == null) {
            throw new InvalidOperationException();
        }

        if (keyMap.TryGetValue(key, out var direction)) {
            ship.Direction &= ~direction;
        }
    }

    public async ValueTask DisposeAsync() {
        if (moduleReference != null) {
            await moduleReference.InvokeVoidAsync("terminate");
            await moduleReference.DisposeAsync();
        }

        dotNetObjectReference?.Dispose();

        GC.SuppressFinalize(this);
    }
}
