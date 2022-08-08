using Xunit;

namespace BlazorPlayground.Graphics.Tests {
    public class PointAssert {
        public static void Equal(Point expected, Point? actual) {
            Assert.NotNull(actual);
            Assert.Equal(expected.X, actual!.X, 1);
            Assert.Equal(expected.Y, actual!.Y, 1);
        }
    }
}
