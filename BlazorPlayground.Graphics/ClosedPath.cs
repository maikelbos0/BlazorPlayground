using NetTopologySuite.Geometries;

namespace BlazorPlayground.Graphics;

public class ClosedPath : DrawableShape, IShapeWithOpacity, IShapeWithFill, IShapeWithStroke, IShapeWithStrokeLinejoin, IAutoSelectedShape, IHasSecondaryAction {
    public override string ElementName => "path";
    public override IReadOnlyList<Anchor> Anchors => Array.AsReadOnly(
        Points.Select((_, index) => new Anchor<ClosedPath>(s => s.Points[index], (s, p) => s.Points[index] = p)).ToArray()
    );
    public List<Point> Points { get; set; }

    public ClosedPath() : this(new(0, 0), new(0, 0)) { }

    public ClosedPath(Point firstPoint, Point secondPoint) {
        Points = [firstPoint, secondPoint];
    }

    public override ShapeAttributeCollection GetAttributes() {
        if (Points.Count == 0) {
            return [];
        }

        var pathSegments = new List<string>() {
            $"M {Points[0].X} {Points[0].Y}"
        };

        foreach (var point in Points.Skip(1)) {
            pathSegments.Add($"L {point.X} {point.Y}");
        }

        return new() {
            { "d", $"{string.Join(", ", pathSegments)}, Z" }
        };
    }

    public override IReadOnlyList<Point> GetSnapPoints() => Points.AsReadOnly();

    protected override Shape CreateClone() => new ClosedPath() {
        Points = new(Points)
    };

    public void ExecuteSecondaryAction(Point endPoint) => Points.Add(endPoint);

    public override Geometry GetGeometry(GeometryFactory geometryFactory, Point origin) {
        if (Points.Count == 0) {
            return geometryFactory.CreateEmpty(Dimension.Point);
        }
        else if (Points.Count == 1) {
            return geometryFactory.CreatePoint(geometryFactory.GetCoordinate(Points[0].X, Points[0].Y, origin));
        }
        else if (Points.Count == 2) {
            return geometryFactory.CreateLineString([
                geometryFactory.GetCoordinate(Points[0].X, Points[0].Y, origin),
                geometryFactory.GetCoordinate(Points[1].X, Points[1].Y, origin)
            ]);
        }
        else {
            var coordinates = new Coordinate[Points.Count + 1];

            for (var i = 0; i < Points.Count; i++) {
                coordinates[i] = geometryFactory.GetCoordinate(Points[i].X, Points[i].Y, origin);
            }

            coordinates[^1] = geometryFactory.GetCoordinate(Points[0].X, Points[0].Y, origin);

            return geometryFactory.CreatePolygon(coordinates);
        }
    }

    public override BoundingBox GetBoundingBox() {
        return new(
            Points.Min(point => point.X),
            Points.Max(point => point.X),
            Points.Min(point => point.Y),
            Points.Max(point => point.Y)
        );
    }
}
