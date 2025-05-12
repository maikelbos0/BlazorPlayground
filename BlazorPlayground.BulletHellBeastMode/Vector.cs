using System;
using System.Text.Json.Serialization;

namespace BlazorPlayground.BulletHellBeastMode;

public readonly record struct Vector<T>(double X, double Y) {
    public static Vector<T> operator +(Vector<T> a, Vector<T> b) => new(a.X + b.X, a.Y + b.Y);
    public static Vector<T> operator -(Vector<T> a, Vector<T> b) => new(a.X - b.X, a.Y - b.Y);
    public static Vector<T> operator *(Vector<T> a, double b) => new(a.X * b, a.Y * b);
    public static Vector<T> operator /(Vector<T> a, double b) => new(a.X / b, a.Y / b);

    [JsonIgnore]
    public bool HasMagnitude => X != 0 || Y != 0;

    [JsonIgnore]
    public double Magnitude => Math.Sqrt(X * X + Y * Y);

    public Vector<T> LimitMagnitude(double maximumMagnitude) {
        var magnitude = Magnitude;
        var factor = 1.0;

        if (magnitude > maximumMagnitude) {
            factor = maximumMagnitude / magnitude;
        }

        return this * factor;
    }

    public Vector<T> AdjustMagnitude(double newMagnitude) {
        var magnitude = Magnitude;
        var factor = newMagnitude / magnitude;

        return this * factor;
    }
}

public struct CoordinateType { }

public struct VelocityType { }

public struct AccelerationType { }
