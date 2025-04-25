using NetTopologySuite.Geometries;

namespace BlazorPlayground.Graphics;

public static class GeometryFactoryExtensions {
    public static Coordinate GetCoordinate(this GeometryFactory geometryFactory, double x, double y, Point origin)
        => new(geometryFactory.PrecisionModel.MakePrecise(x - origin.X), geometryFactory.PrecisionModel.MakePrecise(y - origin.Y));

}
