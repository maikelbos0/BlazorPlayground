using Xunit;

namespace BlazorPlayground.BulletHellBeastMode.Tests;

public static class VectorAssert {
    public static void Equal<T>(Vector<T> expected, Vector<T> actual) {
        Assert.Multiple(
            () => Assert.Equal(expected.X, actual.X, 0.1),
            () => Assert.Equal(expected.Y, actual.Y, 0.1)
        );
    }
}
