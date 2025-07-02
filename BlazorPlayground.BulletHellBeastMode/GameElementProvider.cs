using NetTopologySuite.IO.Converters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text.Json;
using System.Threading.Tasks;

namespace BlazorPlayground.BulletHellBeastMode;

public class GameElementProvider : IGameElementProvider {
    public const string AssetLocation = "./_content/BlazorPlayground.BulletHellBeastMode/assets";
    public const string ShipAssetName = "basic-ship";

    private readonly static JsonSerializerOptions jsonSerializerOptions = new() {
        Converters = {
            new GeoJsonConverterFactory()
        }
    };

    private readonly HttpClient httpClient;

    public GameElementProvider(HttpClient httpClient) {
        this.httpClient = httpClient;
    }

    public async Task<Ship> CreateShip(Coordinate position)
        => new Ship(position, await GetGameElementSections(ShipAssetName));

    private async Task<List<GameElementSection>> GetGameElementSections(string assetName) {
        var asset = await httpClient.GetFromJsonAsync<GameAsset>($"{AssetLocation}/{assetName}.json", jsonSerializerOptions) ?? throw new NullReferenceException();

        return asset.Sections.Select(section => new GameElementSection(section.Geometry, section.FillColor, section.StrokeColor, section.StrokeWidth, section.Opacity)).ToList();
    }
}
