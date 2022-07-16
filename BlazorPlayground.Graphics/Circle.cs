namespace BlazorPlayground.Graphics {
    public class Circle : Shape {

        private readonly static Anchor[] anchors = new[] {
            new Anchor<Circle>(s => s.CenterPoint, (s, p) => s.CenterPoint = p),
            new Anchor<Circle>(s => s.RadiusPoint, (s, p) => s.RadiusPoint = p)
        };

        public override string ElementName => "circle";
        public override IReadOnlyList<Anchor> Anchors { get; } = Array.AsReadOnly(anchors);
        public Point CenterPoint { get; set; }
        public Point RadiusPoint { get; set; }

        public Circle(Point centerPoint, Point radiusPoint) {
            CenterPoint = centerPoint;
            RadiusPoint = radiusPoint;
        }

        public override IEnumerable<ShapeAttribute> GetAttributes() {
            yield return new ShapeAttribute("cx", CenterPoint.X);
            yield return new ShapeAttribute("cy", CenterPoint.Y);
            yield return new ShapeAttribute("r", (RadiusPoint - CenterPoint).Distance);
        }

        public override Shape Clone() => new Circle(CenterPoint, RadiusPoint);
    }
}
