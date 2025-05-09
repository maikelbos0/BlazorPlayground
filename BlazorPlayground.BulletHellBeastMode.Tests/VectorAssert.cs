using Xunit;

namespace BlazorPlayground.BulletHellBeastMode.Tests;

public class VectorAssert {
    public static void Equal<T>(Vector<T> expected, Vector<T> actual) {
        Assert.Equal(expected.X, actual.X, 1);
        Assert.Equal(expected.Y, actual.Y, 1);
    }
}
