using NetTopologySuite.Geometries;
using NetTopologySuite.Geometries.Utilities;

namespace BlazorPlayground.Graphics.Geometries;

public class DrawableShapeGeometryFactory {
    private const int approximationSegmentCount = 60;
    private const double angleIncrement = 2 * Math.PI / approximationSegmentCount;
    
    private readonly GeometryFactory geometryFactory;

    public DrawableShapeGeometryFactory(GeometryFactory geometryFactory) {
        this.geometryFactory = geometryFactory;
    }

    public Geometry GetGeometry(IEnumerable<DrawableShape> shapes) {
        var geometries = new List<Geometry>();

        foreach (var shape in shapes) {
            geometries.Add(shape switch {
                Rectangle rectangle => GetGeometry(rectangle),
                RegularPolygon regularPolygon => GetGeometry(regularPolygon),
                Circle circle => GetGeometry(circle),
                _ => throw new NotImplementedException()
            });
        }

        return GeometryCombiner.Combine(geometries);
    }

    private Geometry GetGeometry(Rectangle rectangle)
        => geometryFactory.CreatePolygon([
            GetCoordinate(rectangle.StartPoint.X, rectangle.StartPoint.Y),
            GetCoordinate(rectangle.StartPoint.X, rectangle.EndPoint.Y),
            GetCoordinate(rectangle.EndPoint.X, rectangle.EndPoint.Y),
            GetCoordinate(rectangle.EndPoint.X, rectangle.StartPoint.Y),
            GetCoordinate(rectangle.StartPoint.X, rectangle.StartPoint.Y)
        ]);

    private Geometry GetGeometry(RegularPolygon regularPolygon) {
        var coordinates = regularPolygon.GetPoints().Select(point => GetCoordinate(point.X, point.Y)).ToList();

        coordinates.Add(coordinates.First());

        return geometryFactory.CreatePolygon([.. coordinates]);
    }

    private Geometry GetGeometry(Circle circle) {
        var radius = (circle.CenterPoint - circle.RadiusPoint).Distance;
        var coordinates = new Coordinate[approximationSegmentCount + 1];

        for (int i = 0; i < approximationSegmentCount; i++) {
            double angle = angleIncrement * i;
            double dx = radius * Math.Cos(angle);
            double dy = radius * Math.Sin(angle);
            coordinates[i] = GetCoordinate(circle.CenterPoint.X + dx, circle.CenterPoint.Y + dy);
        }

        coordinates[approximationSegmentCount] = coordinates[0];

        return geometryFactory.CreatePolygon(coordinates);
    }

    private Coordinate GetCoordinate(double x, double y)
        => new(geometryFactory.PrecisionModel.MakePrecise(x), geometryFactory.PrecisionModel.MakePrecise(y));
}
