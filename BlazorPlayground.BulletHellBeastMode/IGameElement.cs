using System;
using System.Collections.Generic;

namespace BlazorPlayground.BulletHellBeastMode;

public interface IGameElement {
    Guid Id { get; }
    Coordinate Position { get; }
    List<GameElementSection> Sections { get; }

    bool ProcessElapsedTime(double elapsedSeconds);
}
