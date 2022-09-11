namespace BlazorPlayground.Graphics {
    public record Point(double X, double Y) {
        public static Point operator +(Point p1, Point p2) => new(p1.X + p2.X, p1.Y + p2.Y);
        public static Point operator -(Point p1, Point p2) => new(p1.X - p2.X, p1.Y - p2.Y);
        public static Point operator *(Point p, double d) => new(p.X * d, p.Y * d);
        public static Point operator /(Point p, double d) => new(p.X / d, p.Y / d);

        public double Distance => Math.Sqrt(Math.Pow(X, 2) + Math.Pow(Y, 2));

        public bool IsContainedBy(Point boundaryPoint1, Point boundaryPoint2) {
            var containedX = (boundaryPoint1.X >= X && boundaryPoint2.X <= X) || (boundaryPoint2.X >= X && boundaryPoint1.X <= X);
            var containedY = (boundaryPoint1.Y >= Y && boundaryPoint2.Y <= Y) || (boundaryPoint2.Y >= Y && boundaryPoint1.Y <= Y);

            return containedX && containedY;
        }

        public Point SnapToGrid(int gridSize) {
            var remainderX = X % gridSize;
            var remainderY = Y % gridSize;

            var gridPoints = new[] {
                new Point(-remainderX, -remainderY),
                new Point(-remainderX + gridSize, -remainderY),
                new Point(-remainderX, -remainderY + gridSize),
                new Point(-remainderX + gridSize, -remainderY + gridSize)
            };

            return gridPoints.OrderBy(p => p.Distance).First() + this;
        }

        public Point Snap(bool snapToGrid, int gridSize, bool snapToPoints, IEnumerable<Point> points) {
            var snapDeltas = new List<Point>();

            if (snapToPoints) {
                snapDeltas.AddRange(points.Select(p => p - this));
            }

            if (snapToGrid) {
                var remainderX = X % gridSize;
                var remainderY = Y % gridSize;

                snapDeltas.Add(new Point(-remainderX, -remainderY));
                snapDeltas.Add(new Point(-remainderX + gridSize, -remainderY));
                snapDeltas.Add(new Point(-remainderX, -remainderY + gridSize));
                snapDeltas.Add(new Point(-remainderX + gridSize, -remainderY + gridSize));
            }

            var delta = snapDeltas.OrderBy(p => p.Distance).FirstOrDefault();

            if (delta != null) {
                return this + delta;
            }
            else {
                return this;
            }
        }
    }
}
