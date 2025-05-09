using System;
using System.Collections.Generic;

namespace BlazorPlayground.BulletHellBeastMode;

public interface IGameElement {
    Guid Id { get; }
    Coordinate Position { get; set; }
    List<GameElementSection> Sections { get; set; }

    bool ProcessElapsedTime(double elapsedSeconds);
}

public interface IGameElement<T> : IGameElement where T : IGameElement<T> {
    static abstract T Create(Coordinate position, List<GameElementSection> sections);
}
