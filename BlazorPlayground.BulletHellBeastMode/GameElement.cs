using System.Collections.Generic;

namespace BlazorPlayground.BulletHellBeastMode;

public class GameElement {
    public required Coordinate Position { get; set; }
    public required List<GameElementSection> Sections { get; set; }
}
