using Xunit;

namespace BlazorPlayground.Graphics.Tests {
    public class ColorTests {
        [Theory]
        [InlineData("red", "red", 255, 0, 0, 1)]
        [InlineData("unknown", "unknown", 0, 0, 0, 1)]
        [InlineData("#ffccdd", "#ffccdd", 255, 204, 221, 1)]
        public void CastFromString(string value, string expectedValue, byte expectedRed, byte expectedGreen, byte expectedBlue, double expectedAlpha) {
            Color color = value;

            Assert.Equal(expectedValue, color.ColorValue);
            Assert.Equal(expectedRed, color.Red);
            Assert.Equal(expectedGreen, color.Green);
            Assert.Equal(expectedBlue, color.Blue);
            Assert.Equal(expectedAlpha, color.Alpha);
        }
    }
}
