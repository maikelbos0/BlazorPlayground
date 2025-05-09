using System;
using System.Collections.Generic;

namespace BlazorPlayground.BulletHellBeastMode;

public class Ship : IGameElement<Ship> {
    const int DirectionalSpeed = 10;
    public const double MaximumSpeed = 1000;

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
