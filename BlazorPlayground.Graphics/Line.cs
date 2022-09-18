namespace BlazorPlayground.Graphics {
    public class Line : DrawableShape, IShapeWithOpacity, IShapeWithStroke, IShapeWithStrokeLinecap {
        private readonly static Anchor[] anchors = new[] {
            new Anchor<Line>(s => s.StartPoint, (s, p) => s.StartPoint = p),
            new Anchor<Line>(s => s.EndPoint, (s, p) => s.EndPoint = p)
        };

        public override string ElementName => "line";
        public override IReadOnlyList<Anchor> Anchors { get; } = Array.AsReadOnly(anchors);
        public Point StartPoint { get; set; }
        public Point EndPoint { get; set; }

        private Line() : this(new Point(0, 0), new Point(0, 0)) { }

        public Line(Point startPoint, Point endPoint) {
            StartPoint = startPoint;
            EndPoint = endPoint;
        }

        public override IReadOnlyList<Point> GetSnapPoints() => Array.AsReadOnly(new[] { StartPoint, EndPoint });

        public override ShapeAttributeCollection GetAttributes() => new() {
            { "x1", StartPoint.X },
            { "y1", StartPoint.Y },
            { "x2", EndPoint.X },
            { "y2", EndPoint.Y }
        };

        protected override Shape CreateClone() => new Line(StartPoint, EndPoint);
    }
}