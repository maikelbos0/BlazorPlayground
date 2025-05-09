namespace BlazorPlayground.BulletHellBeastMode;

public static class CoordinateExtensions {
    public static Coordinate Move(this Coordinate coordinate, Velocity velocity, double elapsedSeconds) 
        => new Coordinate(coordinate.X + velocity.X * elapsedSeconds, coordinate.Y + velocity.Y * elapsedSeconds);
}
