using NetTopologySuite.Geometries;

namespace BlazorPlayground.BulletHellBeastMode;

public class GameElementSection {
    public required Geometry Geometry { get; set; }
    public required Color FillColor { get; set; }
    public required Color StrokeColor { get; set; }
    public required int StrokeWidth { get; set; }
    public required double Opacity { get; set; }
}
