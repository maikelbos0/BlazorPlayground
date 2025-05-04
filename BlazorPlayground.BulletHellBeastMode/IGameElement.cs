using System.Collections.Generic;

namespace BlazorPlayground.BulletHellBeastMode;

public interface IGameElement {
    Coordinate Position { get; set; }
    List<GameElementSection> Sections { get; set; }
}

public interface IGameElement<T> : IGameElement where T : IGameElement<T> {
    static abstract T Create(Coordinate position, List<GameElementSection> sections);
}
