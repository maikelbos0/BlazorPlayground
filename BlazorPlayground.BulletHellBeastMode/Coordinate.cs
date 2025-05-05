using System;

namespace BlazorPlayground.BulletHellBeastMode;

public readonly record struct Coordinate(double X, double Y) {
    public static Coordinate operator +(Coordinate a, Coordinate b) => new(a.X + b.X, a.Y + b.Y);
    public static Coordinate operator -(Coordinate a, Coordinate b) => new(a.X - b.X, a.Y - b.Y);

    public bool HasMagnitude => X != 0 || Y != 0;

    public double Magnitude => Math.Sqrt(X * X + Y * Y);

    public Coordinate LimitMagnitude(double maximumMagnitude) {
        var magnitude = Magnitude;
        var factor = 1.0;

        if (magnitude > maximumMagnitude) {
            factor = maximumMagnitude / magnitude;
        }

        return new(X * factor, Y * factor);
    }
};
