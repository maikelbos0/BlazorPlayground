namespace BlazorPlayground.Graphics {
    public class Line : Shape {
        private readonly static Anchor[] anchors = new[] {
            new Anchor<Line>(s => s.StartPoint, (s, p) => s.StartPoint = p),
            new Anchor<Line>(s => s.EndPoint, (s, p) => s.EndPoint = p)
        };

        public override string ElementName => "line";
        public override IReadOnlyList<Anchor> Anchors { get; } = Array.AsReadOnly(anchors);
        public Point StartPoint { get; set; }
        public Point EndPoint { get; set; }

        public Line(Point startPoint, Point endPoint) {
            StartPoint = startPoint;
            EndPoint = endPoint;
        }

        public override IEnumerable<ShapeAttribute> GetAttributes() {
            yield return new ShapeAttribute("x1", StartPoint.X.ToString());
            yield return new ShapeAttribute("y1", StartPoint.Y.ToString());
            yield return new ShapeAttribute("x2", EndPoint.X.ToString());
            yield return new ShapeAttribute("y2", EndPoint.Y.ToString());
        }

        public override Shape Clone() => new Line(StartPoint, EndPoint);
    }
}