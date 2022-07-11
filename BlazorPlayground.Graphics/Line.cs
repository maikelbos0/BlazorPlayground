namespace BlazorPlayground.Graphics {
    public class Line : Shape {
        private readonly static Anchor[] anchors = new[] {
            new Anchor<Line>(s => s.StartPoint, (s, p) => s.StartPoint = p),
            new Anchor<Line>(s => s.EndPoint, (s, p) => s.EndPoint = p)
        };

        public override ShapeRenderType RenderType => ShapeRenderType.Polyline;
        public override IReadOnlyList<Anchor> Anchors { get; } = Array.AsReadOnly(anchors);
        public Point StartPoint { get; set; }
        public Point EndPoint { get; set; }

        public Line(Point startPoint, Point endPoint) {
            StartPoint = startPoint;
            EndPoint = endPoint;
        }

        public override IEnumerable<Point> GetPoints() {
            yield return StartPoint;
            yield return EndPoint;
        }

        public override Shape Clone() => new Line(StartPoint, EndPoint);
    }
}