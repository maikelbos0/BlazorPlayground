using System;

namespace BlazorPlayground.BulletHellBeastMode;

[Flags]
public enum Direction {
    None = 0b0000,
    Left = 0b0001,
    Right = 0b0010,
    Up = 0b0100,
    Down = 0b1000
}
