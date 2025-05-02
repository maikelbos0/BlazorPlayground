using NetTopologySuite.IO.Converters;
using System.Net.Http.Json;
using System.Text.Json;

namespace BlazorPlayground.BulletHellBeastMode;

public class GameElementProvider : IGameElementProvider {
    public const string AssetsLocation = "./_content/BlazorPlayground.BulletHellBeastMode/assets/";

    private readonly static JsonSerializerOptions jsonSerializerOptions = new() {
        Converters = {
            new GeoJsonConverterFactory()
        }
    };

    private readonly HttpClient httpClient;

    public GameElementProvider(HttpClient httpClient) {
        this.httpClient = httpClient;
    }

    public async Task<GameElement> CreateFromAsset(string assetName) {
        var asset = await httpClient.GetFromJsonAsync<GameAsset>($"{AssetsLocation}{assetName}.json", jsonSerializerOptions) ?? throw new NullReferenceException();

        return new GameElement() {
            Position = new(0, 0),
            Sections = asset.Sections.Select(section => new GameElementSection(section.Geometry, section.FillColor, section.StrokeColor, section.StrokeWidth, section.Opacity)).ToList()
        };
    }
}
