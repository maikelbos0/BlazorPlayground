using Xunit;

namespace BlazorPlayground.Graphics.Tests {
    public class ColorTests {
        [Theory]
        [InlineData(" red ", " red ", 255, 0, 0, 1)]
        [InlineData("red", "red", 255, 0, 0, 1)]
        [InlineData("#ffccdd", "#ffccdd", 255, 204, 221, 1)]
        [InlineData("#fcd", "#fcd", 255, 204, 221, 1)]
        [InlineData("rgb(1, 1, 1)", "rgb(1, 1, 1)", 1, 1, 1, 1)]
        [InlineData("rgb(255, 0, 0)", "rgb(255, 0, 0)", 255, 0, 0, 1)]
        [InlineData("rgb(249, 0, 0)", "rgb(249, 0, 0)", 249, 0, 0, 1)]
        [InlineData("rgb(199, 0, 0)", "rgb(199, 0, 0)", 199, 0, 0, 1)]
        [InlineData("rgb(0, 255, 0)", "rgb(0, 255, 0)", 0, 255, 0, 1)]
        [InlineData("rgb(0, 249, 0)", "rgb(0, 249, 0)", 0, 249, 0, 1)]
        [InlineData("rgb(0, 199, 0)", "rgb(0, 199, 0)", 0, 199, 0, 1)]
        [InlineData("rgb(0, 0, 255)", "rgb(0, 0, 255)", 0, 0, 255, 1)]
        [InlineData("rgb(0, 0, 249)", "rgb(0, 0, 249)", 0, 0, 249, 1)]
        [InlineData("rgb(0, 0, 199)", "rgb(0, 0, 199)", 0, 0, 199, 1)]
        [InlineData("unknown", "unknown", 0, 0, 0, 1)]
        [InlineData("#fffff", "#fffff", 0, 0, 0, 1)]
        [InlineData("rgb(256, 0, 0)", "rgb(256, 0, 0)", 0, 0, 0, 1)]
        [InlineData("rgb(300, 0, 0)", "rgb(300, 0, 0)", 0, 0, 0, 1)]
        [InlineData("rgb(0, 256, 0)", "rgb(0, 256, 0)", 0, 0, 0, 1)]
        [InlineData("rgb(0, 300, 0)", "rgb(0, 300, 0)", 0, 0, 0, 1)]
        [InlineData("rgb(0, 0, 256)", "rgb(0, 0, 256)", 0, 0, 0, 1)]
        [InlineData("rgb(0, 0, 300)", "rgb(0, 0, 300)", 0, 0, 0, 1)]
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
