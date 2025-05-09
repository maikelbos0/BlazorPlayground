namespace BlazorPlayground.BulletHellBeastMode;

public static class VelocityExtensions {
    public static Velocity Accelerate(this Velocity velocity, Acceleration acceleration, double elapsedSeconds)
        => new Velocity(velocity.X + acceleration.X * elapsedSeconds, velocity.Y + acceleration.Y * elapsedSeconds);

    public static Acceleration GetAcceleration(this Velocity velocity, double elapsedSeconds)
        => new Acceleration(velocity.X / elapsedSeconds, velocity.Y / elapsedSeconds);
}
