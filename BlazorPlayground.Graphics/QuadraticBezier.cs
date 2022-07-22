namespace BlazorPlayground.Graphics {
    public class QuadraticBezier : Shape {
        private readonly static Anchor[] anchors = new[] {
            new Anchor<QuadraticBezier>(s => s.StartPoint, (s, p) => s.StartPoint = p),
            new Anchor<QuadraticBezier>(s => s.ControlPoint, (s, p) => s.ControlPoint = p),
            new Anchor<QuadraticBezier>(s => s.EndPoint, (s, p) => s.EndPoint = p)
        };

        public override string ElementName => "path";
        public override IReadOnlyList<Anchor> Anchors { get; } = Array.AsReadOnly(anchors);
        public Point StartPoint { get; set; }
        public Point ControlPoint { get; set; }
        public Point EndPoint { get; set; }

        public QuadraticBezier(Point startPoint, Point endPoint) {
            StartPoint = startPoint;
            ControlPoint = (startPoint + endPoint) / 2;
            EndPoint = endPoint;
        }

        public override ShapeAttributeCollection GetAttributes() => new() {
            { "d", FormattableString.Invariant($"M {StartPoint.X} {StartPoint.Y} Q {ControlPoint.X} {ControlPoint.Y}, {EndPoint.X} {EndPoint.Y}") }
        };

        public override Shape Clone() => new QuadraticBezier(StartPoint, EndPoint) {
            ControlPoint = ControlPoint
        };
    }
}