using System;
using System.Collections.Generic;

namespace BlazorPlayground.BulletHellBeastMode;

public class Ship : IGameElement<Ship> {
    const int DirectionalSpeed = 10;
    public const double MaximumSpeed = 1000;
    public const double MaximumAcceleration = 2000;
    public const double StoppingDistance = MaximumAcceleration / 2 * (MaximumSpeed * MaximumSpeed / MaximumAcceleration / MaximumAcceleration);

    public Guid Id { get; private set; }
    public required Coordinate Position { get; set; }
    public required List<GameElementSection> Sections { get; set; }
    public Direction Direction { get; set; } = Direction.None;
    public Coordinate? TargetPosition { get; set; }

    public static Ship Create(Coordinate position, List<GameElementSection> sections) => new() {
        Id = Guid.NewGuid(),
        Position = position,
        Sections = sections
    };

    public Velocity GetDirectionalVelocity() {
        var velocity = new Velocity(0, 0);

        if (Direction.HasFlag(Direction.Left)) {
            velocity -= new Velocity(1, 0);
        }

        if (Direction.HasFlag(Direction.Right)) {
            velocity += new Velocity(1, 0);
        }

        if (Direction.HasFlag(Direction.Up)) {
            velocity -= new Velocity(0, 1);
        }

        if (Direction.HasFlag(Direction.Down)) {
            velocity += new Velocity(0, 1);
        }

        if (velocity.HasMagnitude) {
            velocity = velocity.AdjustMagnitude(MaximumSpeed);
        }

        return velocity;
    }

    public Velocity GetVelocityFromTargetPosition() {
        if (TargetPosition == null) {
            return new(0, 0);
        }

        var positionDelta = TargetPosition.Value - Position;
        var distance = positionDelta.Magnitude;

        if (distance == 0) {
            return new Velocity(0, 0);
        }
        else if (distance > StoppingDistance) {
            return new Velocity(positionDelta.X, positionDelta.Y).AdjustMagnitude(MaximumSpeed);
        }
        else {
            var stoppingTime = Math.Sqrt(2 * distance / MaximumAcceleration);

            return new Velocity(positionDelta.X, positionDelta.Y).AdjustMagnitude(distance / stoppingTime);
        }
    }

    public Velocity AdjustVelocityToBounds(Velocity velocity) {
        var stoppedPosition = Position.Move(velocity, velocity.Magnitude / MaximumAcceleration / 2);
        var horizontalVelocityAdjustment = 1.0;
        var verticalVelocityAdjustment = 1.0;

        if (stoppedPosition.X < 0) {
            horizontalVelocityAdjustment = 1 + stoppedPosition.X / (Position.X - stoppedPosition.X);
        }
        else if (stoppedPosition.X > Game.Width) {
            horizontalVelocityAdjustment = 1 - (stoppedPosition.X - Game.Width) / (stoppedPosition.X - Position.X);
        }

        if (stoppedPosition.Y < 0) {
            verticalVelocityAdjustment = 1 + stoppedPosition.Y / (Position.Y - stoppedPosition.Y);
        }
        else if (stoppedPosition.Y > Game.Height) {
            verticalVelocityAdjustment = 1 - (stoppedPosition.Y - Game.Height) / (stoppedPosition.Y - Position.Y);
        }

        return new Velocity(velocity.X * horizontalVelocityAdjustment, velocity.Y * verticalVelocityAdjustment);
    }

    public bool Move(double elapsedTime) {
        var targetPosition = TargetPosition == null
            ? new(0, 0)
            : TargetPosition.Value - Position;

        if (Direction.HasFlag(Direction.Left)) {
            targetPosition -= new Coordinate(DirectionalSpeed, 0);
        }

        if (Direction.HasFlag(Direction.Right)) {
            targetPosition += new Coordinate(DirectionalSpeed, 0);
        }

        if (Direction.HasFlag(Direction.Up)) {
            targetPosition -= new Coordinate(0, DirectionalSpeed);
        }

        if (Direction.HasFlag(Direction.Down)) {
            targetPosition += new Coordinate(0, DirectionalSpeed);
        }

        if (targetPosition.HasMagnitude) {
            Position += targetPosition.LimitMagnitude(10); // TOD move to speed / acceleration
            return true;
        }
        
        return false;
    }
}
