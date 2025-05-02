using NetTopologySuite.Geometries;

namespace BlazorPlayground.BulletHellBeastMode;

public record GameAssetSection(Geometry Geometry, string FillColor, string StrokeColor, int StrokeWidth, double Opacity);