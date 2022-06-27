namespace BlazorPlayground.Graphics {
    public class Square {
        public const int Points = 4;

        public Point CenterPoint { get; }
        public Point RadiusPoint { get; }

        public Square(Point centerPoint, Point radiusPoint) {
            CenterPoint = centerPoint;
            RadiusPoint = radiusPoint;
        }

        public IEnumerable<Point> GetPoints() {
            var vector = RadiusPoint - CenterPoint;
            var radius = Math.Sqrt(Math.Pow(vector.X, 2) + Math.Pow(vector.Y, 2));
            var startingAngle = Math.Atan2(vector.Y, vector.X);
            var pointAngle = Math.PI / Points * 2;

            for (var i = 0; i <= Points; i++) {
                var angle = startingAngle + pointAngle * i;

                yield return CenterPoint + new Point(radius * Math.Cos(angle), radius * Math.Sin(angle));
            }
        }
    }
}