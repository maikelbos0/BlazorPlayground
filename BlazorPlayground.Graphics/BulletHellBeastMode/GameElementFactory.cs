using NetTopologySuite.Geometries;

namespace BlazorPlayground.Graphics.Geometries;

public class GameElementFactory {
    private const int approximationSegmentCount = 60;
    private const double angleIncrement = 2 * Math.PI / approximationSegmentCount;
    private const double stepIncrement = 1.0 / approximationSegmentCount;
    
    private readonly GeometryFactory geometryFactory;

    public GameElementFactory(GeometryFactory geometryFactory) {
        this.geometryFactory = geometryFactory;
    }

    public BulletHellBeastMode.Color? GetFillColor(DrawableShape shape) {
        if (shape is IShapeWithFill shapeWithFill && shapeWithFill.GetFill() is Color fillColor) {
            var fillOpacity = shapeWithFill.GetFillOpacity() / 100.0;

            return new BulletHellBeastMode.Color(fillColor.Red, fillColor.Green, fillColor.Blue, fillColor.Alpha * fillOpacity);
        }

        return null;
    }

    public BulletHellBeastMode.Color? GetStrokeColor(DrawableShape shape) {
        if (shape is IShapeWithStroke shapeWithStroke && shapeWithStroke.GetStroke() is Color strokeColor) {
            var strokeOpacity = shapeWithStroke.GetStrokeOpacity() / 100.0;

            return new BulletHellBeastMode.Color(strokeColor.Red, strokeColor.Green, strokeColor.Blue, strokeColor.Alpha * strokeOpacity);
        }

        return null;
    }

    public Geometry GetGeometry(DrawableShape shape) 
        => shape switch {
            Rectangle rectangle => GetGeometry(rectangle),
            RegularPolygon regularPolygon => GetGeometry(regularPolygon),
            Circle circle => GetGeometry(circle),
            Ellipse ellipse => GetGeometry(ellipse),
            Line line => GetGeometry(line),
            QuadraticBezier quadraticBezier => GetGeometry(quadraticBezier),
            CubicBezier cubicBezier => GetGeometry(cubicBezier),
            ClosedPath closedPath => GetGeometry(closedPath),
            _ => throw new NotImplementedException()
        };

    private Polygon GetGeometry(Rectangle rectangle)
        => geometryFactory.CreatePolygon([
            GetCoordinate(rectangle.StartPoint.X, rectangle.StartPoint.Y),
            GetCoordinate(rectangle.StartPoint.X, rectangle.EndPoint.Y),
            GetCoordinate(rectangle.EndPoint.X, rectangle.EndPoint.Y),
            GetCoordinate(rectangle.EndPoint.X, rectangle.StartPoint.Y),
            GetCoordinate(rectangle.StartPoint.X, rectangle.StartPoint.Y)
        ]);

    private Polygon GetGeometry(RegularPolygon regularPolygon) {
        var coordinates = regularPolygon.GetPoints().Select(point => GetCoordinate(point.X, point.Y)).ToList();

        coordinates.Add(coordinates.First());

        return geometryFactory.CreatePolygon([.. coordinates]);
    }

    private Polygon GetGeometry(Circle circle) {
        var radius = (circle.CenterPoint - circle.RadiusPoint).Distance;
        var coordinates = new Coordinate[approximationSegmentCount + 1];

        for (var i = 0; i < approximationSegmentCount; i++) {
            var angle = angleIncrement * i;
            var dx = radius * Math.Cos(angle);
            var dy = radius * Math.Sin(angle);
            coordinates[i] = GetCoordinate(circle.CenterPoint.X + dx, circle.CenterPoint.Y + dy);
        }

        coordinates[approximationSegmentCount] = coordinates[0];

        return geometryFactory.CreatePolygon(coordinates);
    }

    private Polygon GetGeometry(Ellipse ellipse) {
        var radiusX = Math.Abs(ellipse.CenterPoint.X - ellipse.RadiusPoint.X);
        var radiusY = Math.Abs(ellipse.CenterPoint.Y - ellipse.RadiusPoint.Y);
        var coordinates = new Coordinate[approximationSegmentCount + 1];

        for (var i = 0; i < approximationSegmentCount; i++) {
            var angle = angleIncrement * i;
            var dx = radiusX * Math.Cos(angle);
            var dy = radiusY * Math.Sin(angle);
            coordinates[i] = GetCoordinate(ellipse.CenterPoint.X + dx, ellipse.CenterPoint.Y + dy);
        }

        coordinates[approximationSegmentCount] = coordinates[0];

        return geometryFactory.CreatePolygon(coordinates);
    }

    private LineString GetGeometry(Line line)
        => geometryFactory.CreateLineString([GetCoordinate(line.StartPoint.X, line.StartPoint.Y), GetCoordinate(line.EndPoint.X, line.EndPoint.Y)]);

    private LineString GetGeometry(QuadraticBezier quadraticBezier) {
        var coordinates = new Coordinate[approximationSegmentCount + 1];

        for (var i = 1; i < approximationSegmentCount; i++) {
            var step = stepIncrement * i;
            var invertedStep = 1.0 - step;
            var intermediatePoint = quadraticBezier.StartPoint * invertedStep * invertedStep
                + quadraticBezier.ControlPoint * step * invertedStep * 2
                + quadraticBezier.EndPoint * step * step;

            coordinates[i] = GetCoordinate(intermediatePoint.X, intermediatePoint.Y);
        }

        coordinates[0] = GetCoordinate(quadraticBezier.StartPoint.X, quadraticBezier.StartPoint.Y);
        coordinates[approximationSegmentCount] = GetCoordinate(quadraticBezier.EndPoint.X, quadraticBezier.EndPoint.Y);

        return geometryFactory.CreateLineString(coordinates);
    }

    private LineString GetGeometry(CubicBezier cubicBezier) {
        var coordinates = new Coordinate[approximationSegmentCount + 1];

        for (var i = 1; i < approximationSegmentCount; i++) {
            var step = stepIncrement * i;
            var invertedStep = 1.0 - step;
            var intermediatePoint = cubicBezier.StartPoint * invertedStep * invertedStep * invertedStep
                + cubicBezier.ControlPoint1 * step * invertedStep * invertedStep * 3
                + cubicBezier.ControlPoint2 * step * step * invertedStep * 3
                + cubicBezier.EndPoint * step * step * step;

            coordinates[i] = GetCoordinate(intermediatePoint.X, intermediatePoint.Y);
        }

        coordinates[0] = GetCoordinate(cubicBezier.StartPoint.X, cubicBezier.StartPoint.Y);
        coordinates[approximationSegmentCount] = GetCoordinate(cubicBezier.EndPoint.X, cubicBezier.EndPoint.Y);

        return geometryFactory.CreateLineString(coordinates);
    }

    private Geometry GetGeometry(ClosedPath closedPath) {
        if (closedPath.IntermediatePoints.Count == 1) {
            return geometryFactory.CreateLineString([GetCoordinate(closedPath.StartPoint.X, closedPath.StartPoint.Y), GetCoordinate(closedPath.IntermediatePoints[0].X, closedPath.IntermediatePoints[0].Y)]);
        }
        else { 
            return geometryFactory.CreatePolygon([
                GetCoordinate(closedPath.StartPoint.X, closedPath.StartPoint.Y),
                .. closedPath.IntermediatePoints.Select(point => GetCoordinate(point.X, point.Y)),
                GetCoordinate(closedPath.StartPoint.X, closedPath.StartPoint.Y)
            ]);
        }
    }

    private Coordinate GetCoordinate(double x, double y)
        => new(geometryFactory.PrecisionModel.MakePrecise(x), geometryFactory.PrecisionModel.MakePrecise(y));
}
