namespace BlazorPlayground.Graphics {
    public class RegularPolygon : DrawableShape, IShapeWithOpacity, IShapeWithFill, IShapeWithStroke, IShapeWithSides, IShapeWithStrokeLinejoin {
        private readonly static Anchor[] anchors = new[] {
            new Anchor<RegularPolygon>(s => s.CenterPoint, (s, p) => s.CenterPoint = p),
            new Anchor<RegularPolygon>(s => s.RadiusPoint, (s, p) => s.RadiusPoint = p)
        };

        public override string ElementName => "polygon";
        public override IReadOnlyList<Anchor> Anchors { get; } = Array.AsReadOnly(anchors);
        public Point CenterPoint { get; set; }
        public Point RadiusPoint { get; set; }

        private RegularPolygon() : this(new Point(0, 0), new Point(0, 0)) { }

        public RegularPolygon(Point centerPoint, Point radiusPoint) {
            CenterPoint = centerPoint;
            RadiusPoint = radiusPoint;
        }

        public override IReadOnlyList<Point> GetSnapPoints() => Array.AsReadOnly(GetPoints().Append(CenterPoint).ToArray());

        public IEnumerable<Point> GetPoints() {
            var vector = RadiusPoint - CenterPoint;
            var radius = Math.Sqrt(Math.Pow(vector.X, 2) + Math.Pow(vector.Y, 2));
            var startingAngle = Math.Atan2(vector.Y, vector.X);
            var pointAngle = Math.PI / this.GetSides() * 2;

            for (var i = 0; i < this.GetSides(); i++) {
                var angle = startingAngle + pointAngle * i;

                yield return CenterPoint + new Point(radius * Math.Cos(angle), radius * Math.Sin(angle));
            }
        }

        public override ShapeAttributeCollection GetAttributes() => new() {
            { "points", string.Join(" ", GetPoints().Select(p => FormattableString.Invariant($"{p.X},{p.Y}"))) }
        };

        protected override Shape CreateClone() => new RegularPolygon(CenterPoint, RadiusPoint);
    }
}