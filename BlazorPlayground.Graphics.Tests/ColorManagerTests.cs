using Xunit;

namespace BlazorPlayground.Graphics.Tests {
    public class ColorManagerTests {
        [Theory]
        [InlineData(" red ", 255, 0, 0, 1)]
        [InlineData("red", 255, 0, 0, 1)]
        [InlineData("#ffccdd", 255, 204, 221, 1)]
        [InlineData("#fcd", 255, 204, 221, 1)]
        [InlineData("rgb(1, 1, 1)", 1, 1, 1, 1)]
        [InlineData("rgb(255, 0, 0)", 255, 0, 0, 1)]
        [InlineData("rgb(249, 0, 0)", 249, 0, 0, 1)]
        [InlineData("rgb(199, 0, 0)", 199, 0, 0, 1)]
        [InlineData("rgb(0, 255, 0)", 0, 255, 0, 1)]
        [InlineData("rgb(0, 249, 0)", 0, 249, 0, 1)]
        [InlineData("rgb(0, 199, 0)", 0, 199, 0, 1)]
        [InlineData("rgb(0, 0, 255)", 0, 0, 255, 1)]
        [InlineData("rgb(0, 0, 249)", 0, 0, 249, 1)]
        [InlineData("rgb(0, 0, 199)", 0, 0, 199, 1)]
        [InlineData("rgba(1, 1, 1, 1)", 1, 1, 1, 1)]
        [InlineData("rgba(255, 0, 0, 0.5)", 255, 0, 0, 0.5)]
        [InlineData("rgba(249, 0, 0, 0.5)", 249, 0, 0, 0.5)]
        [InlineData("rgba(199, 0, 0, 0.5)", 199, 0, 0, 0.5)]
        [InlineData("rgba(0, 255, 0, 0.5)", 0, 255, 0, 0.5)]
        [InlineData("rgba(0, 249, 0, 0.5)", 0, 249, 0, 0.5)]
        [InlineData("rgba(0, 199, 0, 0.5)", 0, 199, 0, 0.5)]
        [InlineData("rgba(0, 0, 255, 0.5)", 0, 0, 255, 0.5)]
        [InlineData("rgba(0, 0, 249, 0.5)", 0, 0, 249, 0.5)]
        [InlineData("rgba(0, 0, 199, 0.5)", 0, 0, 199, 0.5)]
        [InlineData("rgba(1, 1, 1, 0.00)", 1, 1, 1, 0)]
        [InlineData("rgba(1, 1, 1, .00)", 1, 1, 1, 0)]
        [InlineData("rgba(1, 1, 1, 0.01)", 1, 1, 1, 0.01)]
        [InlineData("rgba(1, 1, 1, .01)", 1, 1, 1, 0.01)]
        [InlineData("rgba(1, 1, 1, 0.99)", 1, 1, 1, 0.99)]
        [InlineData("rgba(1, 1, 1, 1.00)", 1, 1, 1, 1)]
        [InlineData("unknown", 0, 0, 0, 1)]
        [InlineData("#fffff", 0, 0, 0, 1)]
        [InlineData("rgb(256, 0, 0)", 0, 0, 0, 1)]
        [InlineData("rgb(300, 0, 0)", 0, 0, 0, 1)]
        [InlineData("rgb(0, 256, 0)", 0, 0, 0, 1)]
        [InlineData("rgb(0, 300, 0)", 0, 0, 0, 1)]
        [InlineData("rgb(0, 0, 256)", 0, 0, 0, 1)]
        [InlineData("rgb(0, 0, 300)", 0, 0, 0, 1)]
        [InlineData("rgba(256, 0, 0, 0.5)", 0, 0, 0, 1)]
        [InlineData("rgba(300, 0, 0, 0.5)", 0, 0, 0, 1)]
        [InlineData("rgba(0, 256, 0, 0.5)", 0, 0, 0, 1)]
        [InlineData("rgba(0, 300, 0, 0.5)", 0, 0, 0, 1)]
        [InlineData("rgba(0, 0, 256, 0.5)", 0, 0, 0, 1)]
        [InlineData("rgba(0, 0, 300, 0.5)", 0, 0, 0, 1)]
        [InlineData("rgba(1, 1, 1, 1.01)", 0, 0, 0, 1)]
        [InlineData("rgba(1, 1, 1, 2)", 0, 0, 0, 1)]
        public void Parse(string value, byte expectedRed, byte expectedGreen, byte expectedBlue, double expectedAlpha) {
            var result = ColorManager.Parse(value);

            Assert.Equal(expectedRed, result.Red);
            Assert.Equal(expectedGreen, result.Green);
            Assert.Equal(expectedBlue, result.Blue);
            Assert.Equal(expectedAlpha, result.Alpha);
        }

        [Fact]
        public void ParseWhenConstructing() {
            var colorManager = new ColorManager("red");

            Assert.Equal("red", colorManager.ColorValue);
            Assert.Equal(255, colorManager.Color.Red);
            Assert.Equal(0, colorManager.Color.Green);
            Assert.Equal(0, colorManager.Color.Blue);
            Assert.Equal(1, colorManager.Color.Alpha);
        }

        [Fact]
        public void ParseWhenSetting() {
            var colorManager = new ColorManager("rgba(1, 1, 1, 0.5)");

            colorManager.ColorValue = "red";

            Assert.Equal("red", colorManager.ColorValue);
            Assert.Equal(255, colorManager.Color.Red);
            Assert.Equal(0, colorManager.Color.Green);
            Assert.Equal(0, colorManager.Color.Blue);
            Assert.Equal(1, colorManager.Color.Alpha);
        }
    }
}
