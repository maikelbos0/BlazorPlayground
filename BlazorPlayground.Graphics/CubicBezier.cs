namespace BlazorPlayground.Graphics {
    public class CubicBezier : DrawableShape, IShapeWithOpacity, IShapeWithFill, IShapeWithStroke, IShapeWithStrokeLinecap, IAutoSelectedShape {
        private readonly static Anchor[] anchors = new[] {
            new Anchor<CubicBezier>(s => s.StartPoint, (s, p) => s.StartPoint = p),
            new Anchor<CubicBezier>(s => s.ControlPoint1, (s, p) => s.ControlPoint1 = p),
            new Anchor<CubicBezier>(s => s.ControlPoint2, (s, p) => s.ControlPoint2 = p),
            new Anchor<CubicBezier>(s => s.EndPoint, (s, p) => s.EndPoint = p)
        };

        public override string ElementName => "path";
        public override IReadOnlyList<Anchor> Anchors { get; } = Array.AsReadOnly(anchors);
        public Point StartPoint { get; set; }
        public Point ControlPoint1 { get; set; }
        public Point ControlPoint2 { get; set; }
        public Point EndPoint { get; set; }

        private CubicBezier() : this(new Point(0, 0), new Point(0, 0)) { }

        public CubicBezier(Point startPoint, Point endPoint) {
            StartPoint = startPoint;
            EndPoint = endPoint;

            var step = (EndPoint - startPoint) / 3;

            ControlPoint1 = StartPoint + step;
            ControlPoint2 = StartPoint + step * 2;
        }

        public override IReadOnlyList<Point> GetSnapPoints() => Array.AsReadOnly(new[] { StartPoint, EndPoint });

        public override ShapeAttributeCollection GetAttributes() => new() {
            { "d", FormattableString.Invariant($"M {StartPoint.X} {StartPoint.Y} C {ControlPoint1.X} {ControlPoint1.Y}, {ControlPoint2.X} {ControlPoint2.Y}, {EndPoint.X} {EndPoint.Y}") }
        };

        protected override Shape CreateClone() => new CubicBezier(StartPoint, EndPoint) {
            ControlPoint1 = ControlPoint1,
            ControlPoint2 = ControlPoint2
        };
    }
}