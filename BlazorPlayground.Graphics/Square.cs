namespace BlazorPlayground.Graphics {
    public class Square {
        public Point CenterPoint { get; }
        public Point RadiusPoint { get; }

        public Square(Point centerPoint, Point radiusPoint) {
            CenterPoint = centerPoint;
            RadiusPoint = radiusPoint;
        }

        public IEnumerable<Point> GetPoints() {
            var points = 4;
            var radius = Math.Sqrt(Math.Pow(RadiusPoint.X - CenterPoint.X, 2) + Math.Pow(RadiusPoint.Y - CenterPoint.Y, 2));
            var startingAngle = Math.Asin((RadiusPoint.X - CenterPoint.X) / radius);
            var pointAngle = Math.PI / points * 2;

            for (var i = 0; i <= points; i++) {
                var angle = startingAngle + pointAngle * i;

                yield return CenterPoint + new Point(radius * Math.Cos(angle), radius * Math.Sin(angle));
            }
        }
    }
}