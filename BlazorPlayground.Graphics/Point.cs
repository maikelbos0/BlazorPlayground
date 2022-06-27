namespace BlazorPlayground.Graphics {
    public record Point(double X, double Y) {
        public static Point operator +(Point p1, Point p2) => new Point(p1.X + p2.X, p1.Y + p2.Y);
        public static Point operator -(Point p1, Point p2) => new Point(p1.X - p2.X, p1.Y - p2.Y);
    }
}
