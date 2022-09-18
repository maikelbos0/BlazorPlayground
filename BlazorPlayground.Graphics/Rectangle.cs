namespace BlazorPlayground.Graphics {
    public class Rectangle : DrawableShape, IShapeWithOpacity, IShapeWithFill, IShapeWithStroke, IShapeWithStrokeLinejoin {
        private readonly static Anchor[] anchors = new[] {
            new Anchor<Rectangle>(s => s.StartPoint, (s, p) => s.StartPoint = p),
            new Anchor<Rectangle>(s => s.EndPoint, (s, p) => s.EndPoint = p)
        };

        public override string ElementName => "rect";
        public override IReadOnlyList<Anchor> Anchors { get; } = Array.AsReadOnly(anchors);
        public Point StartPoint { get; set; }
        public Point EndPoint { get; set; }

        private Rectangle() : this(new Point(0, 0), new Point(0, 0)) { }

        public Rectangle(Point startPoint, Point endPoint) {
            StartPoint = startPoint;
            EndPoint = endPoint;
        }

        public override IReadOnlyList<Point> GetSnapPoints() => Array.AsReadOnly(new[] { StartPoint, EndPoint, new Point(StartPoint.X, EndPoint.Y), new Point(EndPoint.X, StartPoint.Y) });

        public override ShapeAttributeCollection GetAttributes() => new() {
            { "x", Math.Min(StartPoint.X, EndPoint.X) },
            { "y", Math.Min(StartPoint.Y, EndPoint.Y) },
            { "width", Math.Abs(StartPoint.X - EndPoint.X) },
            { "height", Math.Abs(StartPoint.Y - EndPoint.Y) }
        };

        protected override Shape CreateClone() => new Rectangle(StartPoint, EndPoint);
    }
}