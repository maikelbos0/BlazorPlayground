using Xunit;

namespace BlazorPlayground.Graphics.Tests {
    public class ColorTests {
        [Theory]
        [InlineData(255, 255, 255, 1, "#FFFFFF")]
        [InlineData(0, 0, 0, 1, "#000000")]
        [InlineData(255, 255, 255, 0.5, "rgba(255, 255, 255, 0.5)")]
        [InlineData(0, 0, 0, 0.5, "rgba(0, 0, 0, 0.5)")]
        public void ToStringMethod(byte red, byte green, byte blue, double alpha, string expectedString) {
            var color = new Color(red, green, blue, alpha);

            Assert.Equal(expectedString, color.ToString());
        }
    }
}
