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
    }
}