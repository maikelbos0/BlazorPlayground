namespace BlazorPlayground.Graphics {
    public class RegularPolygon : IShape {
        public Point CenterPoint { get; }
        public Point RadiusPoint { get; }
        public int Sides { get; }

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

        public RenderType GetSeriesType() => RenderType.Polygon;
    }
}