namespace BlazorPlayground.Graphics {
    public class Line : Shape {
        public override ShapeRenderType RenderType => ShapeRenderType.Polyline;
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

        public override IEnumerable<Anchor> GetAnchors() {
            yield return new Anchor<Line>(this, s => s.StartPoint, (s, p) => s.StartPoint = p);
            yield return new Anchor<Line>(this, s => s.EndPoint, (s, p) => s.EndPoint = p);
        }

        public override Shape Clone() => new Line(StartPoint, EndPoint);
    }
}