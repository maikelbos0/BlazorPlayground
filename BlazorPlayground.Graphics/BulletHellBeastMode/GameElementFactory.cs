using BlazorPlayground.BulletHellBeastMode;
using NetTopologySuite.Geometries;

namespace BlazorPlayground.Graphics.BulletHellBeastMode;

public class GameElementFactory {
    private const int approximationSegmentCount = 60;
    private const double angleIncrement = 2 * Math.PI / approximationSegmentCount;
    private const double stepIncrement = 1.0 / approximationSegmentCount;

    public const double DefaultOpacity = 1.0;
    public const int DefaultStrokeWidth = 0;

    public readonly static BlazorPlayground.BulletHellBeastMode.Color DefaultColor = new(0, 0, 0, 0);

    private readonly GeometryFactory geometryFactory;

    public GameElementFactory(GeometryFactory geometryFactory) {
        this.geometryFactory = geometryFactory;
    }

    public GameElement GetGameElement(IEnumerable<DrawableShape> shapes) {
        var origin = GetOrigin(shapes);

        return new GameElement() {
            Sections = shapes.Select(shape => new GameElementSection() {
                Geometry = GetGeometry(shape, origin),
                FillColor = GetFillColor(shape),
                StrokeColor = GetStrokeColor(shape),
                StrokeWidth = GetStrokeWidth(shape),
                Opacity = GetOpacity(shape)
            }).ToList()
        };
    }

    public static Point GetOrigin(IEnumerable<DrawableShape> shapes) {
        var boundingBoxes = new List<(double MinX, double MaxX, double MinY, double MaxY)>();

        foreach (var shape in shapes) {
            boundingBoxes.Add(shape switch {
                Rectangle rectangle => GetBoundingBox(rectangle),
                RegularPolygon regularPolygon => GetBoundingBox(regularPolygon),
                Circle circle => GetBoundingBox(circle),
                Ellipse ellipse => GetBoundingBox(ellipse),
                Line line => GetBoundingBox(line),
                QuadraticBezier quadraticBezier => GetBoundingBox(quadraticBezier),
                CubicBezier cubicBezier => GetBoundingBox(cubicBezier),
                ClosedPath closedPath => GetBoundingBox(closedPath),
                _ => throw new NotImplementedException()
            });
        }

        if (boundingBoxes.Count > 0) {
            return new(
                (boundingBoxes.Max(boundingBox => boundingBox.MaxX) + boundingBoxes.Min(boundingBox => boundingBox.MinX)) / 2,
                (boundingBoxes.Max(boundingBox => boundingBox.MaxY) + boundingBoxes.Min(boundingBox => boundingBox.MinY)) / 2
            );
        }
        else {
            return new(0, 0);
        }
    }

    private static (double MinX, double MaxX, double MinY, double MaxY) GetBoundingBox(Rectangle rectangle)
        => (
            Math.Min(rectangle.StartPoint.X, rectangle.EndPoint.X),
            Math.Max(rectangle.StartPoint.X, rectangle.EndPoint.X),
            Math.Min(rectangle.StartPoint.Y, rectangle.EndPoint.Y),
            Math.Max(rectangle.StartPoint.Y, rectangle.EndPoint.Y)
        );

    private static (double MinX, double MaxX, double MinY, double MaxY) GetBoundingBox(RegularPolygon regularPolygon) {
        var radius = (regularPolygon.CenterPoint - regularPolygon.RadiusPoint).Distance;

        return (
            regularPolygon.CenterPoint.X - radius,
            regularPolygon.CenterPoint.X + radius,
            regularPolygon.CenterPoint.Y - radius,
            regularPolygon.CenterPoint.Y + radius
        );
    }

    private static (double MinX, double MaxX, double MinY, double MaxY) GetBoundingBox(Circle circle) {
        var radius = (circle.CenterPoint - circle.RadiusPoint).Distance;

        return (
            circle.CenterPoint.X - radius,
            circle.CenterPoint.X + radius,
            circle.CenterPoint.Y - radius,
            circle.CenterPoint.Y + radius
        );
    }

    private static (double MinX, double MaxX, double MinY, double MaxY) GetBoundingBox(Ellipse ellipse) {
        var radiusX = Math.Abs(ellipse.CenterPoint.X - ellipse.RadiusPoint.X);
        var radiusY = Math.Abs(ellipse.CenterPoint.Y - ellipse.RadiusPoint.Y);

        return (
            ellipse.CenterPoint.X - radiusX,
            ellipse.CenterPoint.X + radiusX,
            ellipse.CenterPoint.Y - radiusY,
            ellipse.CenterPoint.Y + radiusY
        );
    }

    private static (double MinX, double MaxX, double MinY, double MaxY) GetBoundingBox(Line line)
        => (
            Math.Min(line.StartPoint.X, line.EndPoint.X),
            Math.Max(line.StartPoint.X, line.EndPoint.X),
            Math.Min(line.StartPoint.Y, line.EndPoint.Y),
            Math.Max(line.StartPoint.Y, line.EndPoint.Y)
        );

    private static (double MinX, double MaxX, double MinY, double MaxY) GetBoundingBox(QuadraticBezier quadraticBezier)
        => (
            Math.Min(quadraticBezier.StartPoint.X, quadraticBezier.EndPoint.X),
            Math.Max(quadraticBezier.StartPoint.X, quadraticBezier.EndPoint.X),
            Math.Min(quadraticBezier.StartPoint.Y, quadraticBezier.EndPoint.Y),
            Math.Max(quadraticBezier.StartPoint.Y, quadraticBezier.EndPoint.Y)
        );

    private static (double MinX, double MaxX, double MinY, double MaxY) GetBoundingBox(CubicBezier cubicBezier)
        => (
            Math.Min(cubicBezier.StartPoint.X, cubicBezier.EndPoint.X),
            Math.Max(cubicBezier.StartPoint.X, cubicBezier.EndPoint.X),
            Math.Min(cubicBezier.StartPoint.Y, cubicBezier.EndPoint.Y),
            Math.Max(cubicBezier.StartPoint.Y, cubicBezier.EndPoint.Y)
        );

    private static (double MinX, double MaxX, double MinY, double MaxY) GetBoundingBox(ClosedPath closedPath) {
        var allPoints = closedPath.IntermediatePoints.Append(closedPath.StartPoint).ToList();

        return (
                allPoints.Min(point => point.X),
                allPoints.Max(point => point.X),
                allPoints.Min(point => point.Y),
                allPoints.Max(point => point.Y)
            );
    }

    public Geometry GetGeometry(DrawableShape shape, Point origin)
        => shape switch {
            Rectangle rectangle => GetGeometry(rectangle, origin),
            RegularPolygon regularPolygon => GetGeometry(regularPolygon, origin),
            Circle circle => GetGeometry(circle, origin),
            Ellipse ellipse => GetGeometry(ellipse, origin),
            Line line => GetGeometry(line, origin),
            QuadraticBezier quadraticBezier => GetGeometry(quadraticBezier, origin),
            CubicBezier cubicBezier => GetGeometry(cubicBezier, origin),
            ClosedPath closedPath => GetGeometry(closedPath, origin),
            _ => throw new NotImplementedException()
        };

    private Polygon GetGeometry(Rectangle rectangle, Point origin)
        => geometryFactory.CreatePolygon([
            GetCoordinate(rectangle.StartPoint.X, rectangle.StartPoint.Y, origin),
            GetCoordinate(rectangle.StartPoint.X, rectangle.EndPoint.Y, origin),
            GetCoordinate(rectangle.EndPoint.X, rectangle.EndPoint.Y, origin),
            GetCoordinate(rectangle.EndPoint.X, rectangle.StartPoint.Y, origin),
            GetCoordinate(rectangle.StartPoint.X, rectangle.StartPoint.Y, origin)
        ]);

    private Polygon GetGeometry(RegularPolygon regularPolygon, Point origin) {
        var coordinates = new Coordinate[regularPolygon.GetSides() + 1];

        foreach (var (point, i) in regularPolygon.GetPoints().Select((point, i) => (point, i))) {
            coordinates[i] = GetCoordinate(point.X, point.Y, origin);
        }

        coordinates[^1] = coordinates[0];

        return geometryFactory.CreatePolygon([.. coordinates]);
    }

    private Polygon GetGeometry(Circle circle, Point origin) {
        var radius = (circle.CenterPoint - circle.RadiusPoint).Distance;
        var coordinates = new Coordinate[approximationSegmentCount + 1];

        for (var i = 0; i < approximationSegmentCount; i++) {
            var angle = angleIncrement * i;
            var dx = radius * Math.Cos(angle);
            var dy = radius * Math.Sin(angle);
            coordinates[i] = GetCoordinate(circle.CenterPoint.X + dx, circle.CenterPoint.Y + dy, origin);
        }

        coordinates[^1] = coordinates[0];

        return geometryFactory.CreatePolygon(coordinates);
    }

    private Polygon GetGeometry(Ellipse ellipse, Point origin) {
        var radiusX = Math.Abs(ellipse.CenterPoint.X - ellipse.RadiusPoint.X);
        var radiusY = Math.Abs(ellipse.CenterPoint.Y - ellipse.RadiusPoint.Y);
        var coordinates = new Coordinate[approximationSegmentCount + 1];

        for (var i = 0; i < approximationSegmentCount; i++) {
            var angle = angleIncrement * i;
            var dx = radiusX * Math.Cos(angle);
            var dy = radiusY * Math.Sin(angle);
            coordinates[i] = GetCoordinate(ellipse.CenterPoint.X + dx, ellipse.CenterPoint.Y + dy, origin);
        }

        coordinates[^1] = coordinates[0];

        return geometryFactory.CreatePolygon(coordinates);
    }

    private LineString GetGeometry(Line line, Point origin)
        => geometryFactory.CreateLineString([
            GetCoordinate(line.StartPoint.X, line.StartPoint.Y, origin),
            GetCoordinate(line.EndPoint.X, line.EndPoint.Y, origin)
        ]);

    private LineString GetGeometry(QuadraticBezier quadraticBezier, Point origin) {
        var coordinates = new Coordinate[approximationSegmentCount + 1];

        for (var i = 1; i < approximationSegmentCount; i++) {
            var step = stepIncrement * i;
            var invertedStep = 1.0 - step;
            var intermediatePoint = quadraticBezier.StartPoint * invertedStep * invertedStep
                + quadraticBezier.ControlPoint * step * invertedStep * 2
                + quadraticBezier.EndPoint * step * step;

            coordinates[i] = GetCoordinate(intermediatePoint.X, intermediatePoint.Y, origin);
        }

        coordinates[0] = GetCoordinate(quadraticBezier.StartPoint.X, quadraticBezier.StartPoint.Y, origin);
        coordinates[^1] = GetCoordinate(quadraticBezier.EndPoint.X, quadraticBezier.EndPoint.Y, origin);

        return geometryFactory.CreateLineString(coordinates);
    }

    private LineString GetGeometry(CubicBezier cubicBezier, Point origin) {
        var coordinates = new Coordinate[approximationSegmentCount + 1];

        for (var i = 1; i < approximationSegmentCount; i++) {
            var step = stepIncrement * i;
            var invertedStep = 1.0 - step;
            var intermediatePoint = cubicBezier.StartPoint * invertedStep * invertedStep * invertedStep
                + cubicBezier.ControlPoint1 * step * invertedStep * invertedStep * 3
                + cubicBezier.ControlPoint2 * step * step * invertedStep * 3
                + cubicBezier.EndPoint * step * step * step;

            coordinates[i] = GetCoordinate(intermediatePoint.X, intermediatePoint.Y, origin);
        }

        coordinates[0] = GetCoordinate(cubicBezier.StartPoint.X, cubicBezier.StartPoint.Y, origin);
        coordinates[^1] = GetCoordinate(cubicBezier.EndPoint.X, cubicBezier.EndPoint.Y, origin);

        return geometryFactory.CreateLineString(coordinates);
    }

    private Geometry GetGeometry(ClosedPath closedPath, Point origin) {
        if (closedPath.IntermediatePoints.Count == 1) {
            return geometryFactory.CreateLineString([
                GetCoordinate(closedPath.StartPoint.X, closedPath.StartPoint.Y, origin),
                GetCoordinate(closedPath.IntermediatePoints[0].X, closedPath.IntermediatePoints[0].Y, origin)
            ]);
        }
        else {
            var coordinates = new Coordinate[closedPath.IntermediatePoints.Count + 2];

            for (var i = 0; i < closedPath.IntermediatePoints.Count; i++) {
                coordinates[i + 1] = GetCoordinate(closedPath.IntermediatePoints[i].X, closedPath.IntermediatePoints[i].Y, origin);
            }

            coordinates[0] = coordinates[^1] = GetCoordinate(closedPath.StartPoint.X, closedPath.StartPoint.Y, origin);

            return geometryFactory.CreatePolygon(coordinates);
        }
    }

    private Coordinate GetCoordinate(double x, double y, Point origin)
        => new(geometryFactory.PrecisionModel.MakePrecise(x - origin.X), geometryFactory.PrecisionModel.MakePrecise(y - origin.Y));

    public static BlazorPlayground.BulletHellBeastMode.Color GetFillColor(DrawableShape shape) {
        if (shape is IShapeWithFill shapeWithFill && shapeWithFill.GetFill() is Color fillColor) {
            var fillOpacity = shapeWithFill.GetFillOpacity() / 100.0;

            return new BlazorPlayground.BulletHellBeastMode.Color(fillColor.Red, fillColor.Green, fillColor.Blue, fillColor.Alpha * fillOpacity);
        }

        return DefaultColor;
    }

    public static BlazorPlayground.BulletHellBeastMode.Color GetStrokeColor(DrawableShape shape) {
        if (shape is IShapeWithStroke shapeWithStroke && shapeWithStroke.GetStroke() is Color strokeColor) {
            var strokeOpacity = shapeWithStroke.GetStrokeOpacity() / 100.0;

            return new BlazorPlayground.BulletHellBeastMode.Color(strokeColor.Red, strokeColor.Green, strokeColor.Blue, strokeColor.Alpha * strokeOpacity);
        }

        return DefaultColor;
    }

    public static int GetStrokeWidth(DrawableShape shape) {
        if (shape is IShapeWithStroke shapeWithStroke) {
            return shapeWithStroke.GetStrokeWidth();
        }

        return DefaultStrokeWidth;
    }

    public static double GetOpacity(DrawableShape shape) {
        if (shape is IShapeWithOpacity shapeWithOpacity) {
            return shapeWithOpacity.GetOpacity() / 100.0;
        }

        return DefaultOpacity;
    }
}
