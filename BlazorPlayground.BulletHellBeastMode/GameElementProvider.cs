﻿using NetTopologySuite.IO.Converters;
using System;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text.Json;
using System.Threading.Tasks;

namespace BlazorPlayground.BulletHellBeastMode;

public class GameElementProvider : IGameElementProvider {
    public const string AssetLocation = "./_content/BlazorPlayground.BulletHellBeastMode/assets";

    private readonly static JsonSerializerOptions jsonSerializerOptions = new() {
        Converters = {
            new GeoJsonConverterFactory()
        }
    };

    private readonly HttpClient httpClient;

    public GameElementProvider(HttpClient httpClient) {
        this.httpClient = httpClient;
    }

    public async Task<TGameElement> CreateFromAsset<TGameElement>(string assetName, Coordinate position) where TGameElement : IGameElement<TGameElement> {
        var asset = await httpClient.GetFromJsonAsync<GameAsset>($"{AssetLocation}/{assetName}.json", jsonSerializerOptions) ?? throw new NullReferenceException();

        return TGameElement.Create(
            position,
            asset.Sections.Select(section => new GameElementSection(section.Geometry, section.FillColor, section.StrokeColor, section.StrokeWidth, section.Opacity)).ToList()
        );
    }
}
