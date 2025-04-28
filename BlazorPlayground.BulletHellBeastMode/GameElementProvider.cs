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

    public async Task<GameElement> LoadFromAsset(string assetName)
        => await httpClient.GetFromJsonAsync<GameElement>($"{AssetsLocation}{assetName}.json", jsonSerializerOptions) ?? throw new NullReferenceException();
}
