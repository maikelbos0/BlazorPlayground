namespace BlazorPlayground.Graphics {
    public class Rectangle : Shape {
        private readonly static Anchor[] anchors = new[] {
            new Anchor<Rectangle>(s => s.StartPoint, (s, p) => s.StartPoint = p),
            new Anchor<Rectangle>(s => s.EndPoint, (s, p) => s.EndPoint = p)
        };

        public override string ElementName => "rect";
        public override IReadOnlyList<Anchor> Anchors { get; } = Array.AsReadOnly(anchors);
        public Point StartPoint { get; set; }
        public Point EndPoint { get; set; }

        public Rectangle(Point startPoint, Point endPoint) {
            StartPoint = startPoint;
            EndPoint = endPoint;
        }

        public override ShapeAttributeCollection GetAttributes() => new() {
            { "x", Math.Min(StartPoint.X, EndPoint.X) },
            { "y", Math.Min(StartPoint.Y, EndPoint.Y) },
            { "width", Math.Abs(StartPoint.X - EndPoint.X) },
            { "height", Math.Abs(StartPoint.Y - EndPoint.Y) }
        };

        public override Shape Clone() => new Rectangle(StartPoint, EndPoint);
    }
}