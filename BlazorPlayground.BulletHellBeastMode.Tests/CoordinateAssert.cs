using BlazorPlayground.BulletHellBeastMode;
using Xunit;

namespace BlazorPlayground.Graphics.Tests;

public class CoordinateAssert {
    public static void Equal(Coordinate expected, Coordinate? actual) {
        Assert.NotNull(actual);
        Assert.Equal(expected.X, actual.X, 1);
        Assert.Equal(expected.Y, actual.Y, 1);
    }
}
