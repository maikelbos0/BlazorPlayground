namespace BlazorPlayground.Graphics {
    public class Rectangle : Shape {
        private readonly static Anchor[] anchors = new[] {
            new Anchor<Rectangle>(s => s.StartPoint, (s, p) => s.StartPoint = p),
            new Anchor<Rectangle>(s => s.EndPoint, (s, p) => s.EndPoint = p)
        };

        public override ShapeRenderType RenderType => ShapeRenderType.Polygon;
        public override IReadOnlyList<Anchor> Anchors { get; } = Array.AsReadOnly(anchors);
        public Point StartPoint { get; set; }
        public Point EndPoint { get; set; }

        public Rectangle(Point startPoint, Point endPoint) {
            StartPoint = startPoint;
            EndPoint = endPoint;
        }

        public override IEnumerable<Point> GetPoints() {
            yield return StartPoint;
            yield return new Point(StartPoint.X, EndPoint.Y);
            yield return EndPoint;
            yield return new Point(EndPoint.X, StartPoint.Y);
        }

        public override Shape Clone() => new Rectangle(StartPoint, EndPoint);
    }
}