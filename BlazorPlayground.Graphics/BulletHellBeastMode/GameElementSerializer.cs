using BlazorPlayground.BulletHellBeastMode;
using NetTopologySuite.IO.Converters;
using System.Text.Json;

namespace BlazorPlayground.Graphics.BulletHellBeastMode;

public static class GameElementSerializer {
    private readonly static JsonSerializerOptions jsonSerializerOptions = new() {
        Converters = {
            new GeoJsonConverterFactory()
        }
    };

    public static string Serialize(GameElement gameElement) {
        return JsonSerializer.Serialize(gameElement, jsonSerializerOptions);
    }
}
