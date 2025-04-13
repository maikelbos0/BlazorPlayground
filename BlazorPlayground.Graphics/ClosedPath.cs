namespace BlazorPlayground.Graphics;

public class ClosedPath : DrawableShape, IShapeWithOpacity, IShapeWithFill, IShapeWithStroke, IShapeWithStrokeLinejoin, IAutoSelectedShape, IHasSecondaryAction {
    public override string ElementName => "path";
    public override IReadOnlyList<Anchor> Anchors => Array.AsReadOnly([
        new Anchor<ClosedPath>(s => s.StartPoint, (s, p) => s.StartPoint = p),
        .. IntermediatePoints.Select((_, index) => new Anchor<ClosedPath>(s => s.IntermediatePoints[index], (s, p) => s.IntermediatePoints[index] = p))
    ]);
    public Point StartPoint { get; set; }
    public List<Point> IntermediatePoints { get; set; }

    public ClosedPath() : this(new(0, 0), new(0, 0)) { }

    public ClosedPath(Point startPoint, Point firstIntermediatePoint) {
        StartPoint = startPoint;
        IntermediatePoints = [firstIntermediatePoint];
    }

    public override ShapeAttributeCollection GetAttributes() {
        var pathSegments = new List<string>() {
            $"M {StartPoint.X} {StartPoint.Y}"
        };

        foreach (var intermediatePoint in IntermediatePoints) {
            pathSegments.Add($"L {intermediatePoint.X} {intermediatePoint.Y}");
        }

        return new() {
            { "d", $"{string.Join(", ", pathSegments)}, Z" }
        };
    }

    public override IReadOnlyList<Point> GetSnapPoints() => Array.AsReadOnly([StartPoint, .. IntermediatePoints]);

    protected override Shape CreateClone() => new ClosedPath() {
        StartPoint = StartPoint,
        IntermediatePoints = new(IntermediatePoints)
    };

    public void ExecuteSecondaryAction(Point endPoint) => IntermediatePoints.Add(endPoint);
}
