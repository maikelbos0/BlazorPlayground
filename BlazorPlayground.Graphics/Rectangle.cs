namespace BlazorPlayground.Graphics {
    public class Rectangle : Shape {
        public override ShapeRenderType RenderType => ShapeRenderType.Polygon;
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

        public override IEnumerable<Anchor> GetAnchors() {
            yield return new Anchor<Rectangle>(this, s => s.StartPoint, (s, p) => s.StartPoint = p);
            yield return new Anchor<Rectangle>(this, s => s.EndPoint, (s, p) => s.EndPoint = p);
        }

        public override Shape Clone() => new Rectangle(StartPoint, EndPoint);
    }
}