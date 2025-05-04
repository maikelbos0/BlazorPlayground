using System;
using System.Collections.Generic;

namespace BlazorPlayground.BulletHellBeastMode;

public class Ship : IGameElement<Ship> {
    public Guid Id { get; private set; }
    public required Coordinate Position { get; set; }
    public required List<GameElementSection> Sections { get; set; }


    public static Ship Create(Coordinate position, List<GameElementSection> sections) => new() {
        Id = Guid.NewGuid(),
        Position = position,
        Sections = sections
    };
}
