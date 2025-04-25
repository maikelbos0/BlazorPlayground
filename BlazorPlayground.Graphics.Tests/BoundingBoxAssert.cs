using Xunit;

namespace BlazorPlayground.Graphics.Tests;

public class BoundingBoxAssert {
    public static void Equal(BoundingBox expected, BoundingBox? actual) {
        Assert.NotNull(actual);
        Assert.Equal(expected.MinX, actual.MinX, 1);
        Assert.Equal(expected.MaxX, actual.MaxX, 1);
        Assert.Equal(expected.MinY, actual.MinY, 1);
        Assert.Equal(expected.MaxY, actual.MaxY, 1);
    }
}
