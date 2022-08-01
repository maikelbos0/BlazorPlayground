using Xunit;

namespace BlazorPlayground.Graphics.Tests {
    public class PaintManagerTests {
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
        public void ParseColor(string value, byte expectedRed, byte expectedGreen, byte expectedBlue, double expectedAlpha) {
            var result = PaintManager.ParseColor(value);

            Assert.Equal(new Color(expectedRed, expectedGreen, expectedBlue, expectedAlpha), result);
        }
        
        [Fact]
        public void ParseColorWhenSettingColorValue() {
            var manager = new PaintManager() {
                ColorValue = "red"
            };

            Assert.Equal("red", manager.ColorValue);
            PaintServerAssert.Equal(new Color(255, 0, 0, 1), manager.Color);
        }

        [Fact]
        public void PaintServerNone() {
            var manager = new PaintManager() { 
                Mode = PaintMode.None 
            };

            Assert.Equal(PaintServer.None, manager.Server);
        }

        [Fact]
        public void PaintServerColor() {
            var manager = new PaintManager() {
                Mode = PaintMode.Color,
                ColorValue = "red"
            };

            Assert.Equal(manager.Color, manager.Server);
        }
    }
}
