using Xunit;

namespace BlazorPlayground.Graphics.Tests {
    public class PaintServerAssert {
        public static void Equal(Color expected, IPaintServer actual) {
            var actualColor = Assert.IsType<Color>(actual);

            Assert.Equal(expected.Red, actualColor.Red);
            Assert.Equal(expected.Green, actualColor.Green);
            Assert.Equal(expected.Blue, actualColor.Blue);
            Assert.Equal(expected.Alpha, actualColor.Alpha);
        }
    }
}
