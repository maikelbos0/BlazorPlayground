namespace BlazorPlayground.Graphics {
    public record Point(double X, double Y) {
        public static Point operator +(Point p1, Point p2) => new(p1.X + p2.X, p1.Y + p2.Y);
        public static Point operator -(Point p1, Point p2) => new(p1.X - p2.X, p1.Y - p2.Y);

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
    }
}
