using System;
using System.Collections.Generic;

namespace BlazorPlayground.BulletHellBeastMode;

public class Ship : IGameElement<Ship> {
    public Guid Id { get; private set; }
    public required Coordinate Position { get; set; }
    public required List<GameElementSection> Sections { get; set; }
    public Direction Direction { get; set; } = Direction.None;

    public static Ship Create(Coordinate position, List<GameElementSection> sections) => new() {
        Id = Guid.NewGuid(),
        Position = position,
        Sections = sections
    };

    public bool Move(double elapsedTime) {
        var x = 0.0;
        var y = 0.0;

        if (Direction.HasFlag(Direction.Left)) {
            x -= 10;
        }
        
        if (Direction.HasFlag(Direction.Right)) {
            x += 10;
        }
        
        if (Direction.HasFlag(Direction.Up)) {
            y -= 10;
        }
        
        if (Direction.HasFlag(Direction.Down)) {
            y += 10;
        }

        if (x != 0 || y != 0) {
            Position += new Coordinate(x, y);
            return true;
        }

        return false;
    }
}
