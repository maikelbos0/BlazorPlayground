using NetTopologySuite.Geometries;
using System.Text.Json.Serialization;

namespace BlazorPlayground.BulletHellBeastMode;

public record GameElementSection([property: JsonIgnore] Geometry Geometry, string FillColor, string StrokeColor, int StrokeWidth, double Opacity) {
    public string Type => Geometry.GeometryType;
    public Coordinate[] Coordinates => Geometry.Coordinates.Select(coordinate => new Coordinate(coordinate.X, coordinate.Y)).ToArray();
}
