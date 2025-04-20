using NetTopologySuite.Geometries;

namespace BlazorPlayground.BulletHellBeastMode;

public class GameElementSection {
    public required Geometry Geometry { get; set; }
    public Color? StrokeColor { get; set; }
    public Color? FillColor { get; set; }
}
