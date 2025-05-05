using Xunit;

namespace BlazorPlayground.BulletHellBeastMode.Tests;

public class CoordinateAssert {
    public static void Equal(Coordinate expected, Coordinate? actual) {
        Assert.NotNull(actual);
        Assert.Equal(expected.X, actual.Value.X, 1);
        Assert.Equal(expected.Y, actual.Value.Y, 1);
    }
}
