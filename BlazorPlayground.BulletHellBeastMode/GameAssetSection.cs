using NetTopologySuite.Geometries;

namespace BlazorPlayground.BulletHellBeastMode;

public record GameAssetSection(Geometry Geometry, Color FillColor, Color StrokeColor, int StrokeWidth, double Opacity);