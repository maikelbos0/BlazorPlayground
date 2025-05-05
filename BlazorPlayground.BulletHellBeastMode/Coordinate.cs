namespace BlazorPlayground.BulletHellBeastMode;

public record Coordinate(double X, double Y) {
    public static Coordinate operator +(Coordinate a, Coordinate b) => new Coordinate(a.X + b.X, a.Y + b.Y);
};
