namespace BlazorPlayground.Graphics {
    public class Line : IShape {
        public Point StartPoint { get; set; }
        public Point EndPoint { get; set; }

        public Line(Point startPoint, Point endPoint) {
            StartPoint = startPoint;
            EndPoint = endPoint;
        }

        public IEnumerable<Point> GetPoints() {
            yield return StartPoint;
            yield return EndPoint;
        }

        public RenderType GetSeriesType() => RenderType.Polyline;
    }
}