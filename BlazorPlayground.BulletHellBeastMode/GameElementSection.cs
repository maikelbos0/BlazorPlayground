using NetTopologySuite.Geometries;
using NetTopologySuite.IO;
using System.Text.Json.Serialization;

namespace BlazorPlayground.BulletHellBeastMode;

public class GameElementSection {
    private readonly static WKTWriter wktWriter = new();
    private readonly static WKTReader wktReader = new();

    [JsonIgnore]
    public Geometry Geometry { get; set; } = Polygon.Empty;
    [JsonRequired]
    public string GeometryText {
        get => wktWriter.Write(Geometry);
        set => Geometry = wktReader.Read(value);
    }
    public required Color FillColor { get; set; }
    public required Color StrokeColor { get; set; }
    public required int StrokeWidth { get; set; }
    public required double Opacity { get; set; }

    public CanvasGameElementSection ForCanvas() {
        var type = Geometry switch {
            Polygon => "polygon",
            LineString => "linestring",
            _ => throw new NotImplementedException()
        };
        var coordinates = Geometry.Coordinates.Select(coordinate => new Coordinate(coordinate.X, coordinate.Y)).ToList();

        return new(type, coordinates, FillColor.ToString(), StrokeColor.ToString(), StrokeWidth, Opacity);
    }
}
