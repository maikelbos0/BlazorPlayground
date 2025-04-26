using NetTopologySuite.IO.Converters;
using System.Text.Json;

namespace BlazorPlayground.BulletHellBeastMode;

public static class GameElementSerializer {
    private readonly static JsonSerializerOptions jsonSerializerOptions = new() {
        Converters = {
            new GeoJsonConverterFactory()
        }
    };

    public static GameElement Deserialize(string serializedGameElement) {
        return JsonSerializer.Deserialize<GameElement>(serializedGameElement, jsonSerializerOptions) 
            ?? throw new NullReferenceException();
    }

    public static string Serialize(GameElement gameElement) {
        return JsonSerializer.Serialize(gameElement, jsonSerializerOptions);
    }
}
