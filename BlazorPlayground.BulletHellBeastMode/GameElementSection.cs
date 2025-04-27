using NetTopologySuite.Geometries;

namespace BlazorPlayground.BulletHellBeastMode;

public class GameElementSection {
    public required Geometry Geometry { get; set; }
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
        var coordinates = Geometry.Coordinates.Select(coordinate => (coordinate.X, coordinate.Y)).ToList();

        return new(type, coordinates, FillColor.ToString(), StrokeColor.ToString(), StrokeWidth, Opacity);
    }
}
