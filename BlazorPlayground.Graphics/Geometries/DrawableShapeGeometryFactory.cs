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
                Ellipse ellipse => GetGeometry(ellipse),
                Line line => GetGeometry(line),
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
            var angle = angleIncrement * i;
            var dx = radius * Math.Cos(angle);
            var dy = radius * Math.Sin(angle);
            coordinates[i] = GetCoordinate(circle.CenterPoint.X + dx, circle.CenterPoint.Y + dy);
        }

        coordinates[approximationSegmentCount] = coordinates[0];

        return geometryFactory.CreatePolygon(coordinates);
    }

    private Geometry GetGeometry(Ellipse ellipse) {
        var radiusX = Math.Abs(ellipse.CenterPoint.X - ellipse.RadiusPoint.X);
        var radiusY = Math.Abs(ellipse.CenterPoint.Y - ellipse.RadiusPoint.Y);
        var coordinates = new Coordinate[approximationSegmentCount + 1];

        for (int i = 0; i < approximationSegmentCount; i++) {
            var angle = angleIncrement * i;
            var dx = radiusX * Math.Cos(angle);
            var dy = radiusY * Math.Sin(angle);
            coordinates[i] = GetCoordinate(ellipse.CenterPoint.X + dx, ellipse.CenterPoint.Y + dy);
        }

        coordinates[approximationSegmentCount] = coordinates[0];

        return geometryFactory.CreatePolygon(coordinates);
    }

    private Geometry GetGeometry(Line line)
        => geometryFactory.CreateLineString([GetCoordinate(line.StartPoint.X, line.StartPoint.Y), GetCoordinate(line.EndPoint.X, line.EndPoint.Y)]);

    private Coordinate GetCoordinate(double x, double y)
        => new(geometryFactory.PrecisionModel.MakePrecise(x), geometryFactory.PrecisionModel.MakePrecise(y));
}
