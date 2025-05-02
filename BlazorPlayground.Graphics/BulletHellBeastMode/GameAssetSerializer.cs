using BlazorPlayground.BulletHellBeastMode;
using NetTopologySuite.IO.Converters;
using System.Text.Json;

namespace BlazorPlayground.Graphics.BulletHellBeastMode;

public static  class GameAssetSerializer {
    private readonly static JsonSerializerOptions jsonSerializerOptions = new() {
        Converters = {
            new GeoJsonConverterFactory()
        }
    };

    public static string Serialize(GameAsset gameAsset) {
        return JsonSerializer.Serialize(gameAsset, jsonSerializerOptions);
    }
}
