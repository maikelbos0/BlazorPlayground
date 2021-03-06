namespace BlazorPlayground.Graphics {
    public class RegularPolygon : Shape {
        private readonly static Anchor[] anchors = new[] {
            new Anchor<RegularPolygon>(s => s.CenterPoint, (s, p) => s.CenterPoint = p),
            new Anchor<RegularPolygon>(s => s.RadiusPoint, (s, p) => s.RadiusPoint = p)
        };

        public override string ElementName => "polygon";
        public override IReadOnlyList<Anchor> Anchors { get; } = Array.AsReadOnly(anchors);
        public Point CenterPoint { get; set; }
        public Point RadiusPoint { get; set; }
        public int Sides { get; set; }

        public RegularPolygon(Point centerPoint, Point radiusPoint, int sides) {
            CenterPoint = centerPoint;
            RadiusPoint = radiusPoint;
            Sides = sides;
        }

        public IEnumerable<Point> GetPoints() {
            var vector = RadiusPoint - CenterPoint;
            var radius = Math.Sqrt(Math.Pow(vector.X, 2) + Math.Pow(vector.Y, 2));
            var startingAngle = Math.Atan2(vector.Y, vector.X);
            var pointAngle = Math.PI / Sides * 2;

            for (var i = 0; i < Sides; i++) {
                var angle = startingAngle + pointAngle * i;

                yield return CenterPoint + new Point(radius * Math.Cos(angle), radius * Math.Sin(angle));
            }
        }

        public override ShapeAttributeCollection GetAttributes() => new() {
            { "points", string.Join(" ", GetPoints().Select(p => FormattableString.Invariant($"{p.X},{p.Y}"))) }
        };

        public override Shape Clone() => new RegularPolygon(CenterPoint, RadiusPoint, Sides);
    }
}