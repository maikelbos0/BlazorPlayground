using NSubstitute;
using Xunit;

namespace BlazorPlayground.Graphics.Tests {
    public class ShapeWithStrokeTests {
        [Theory]
        [InlineData(-1, DrawSettings.MinimumStrokeWidth)]
        [InlineData(DrawSettings.MinimumStrokeWidth - 1, DrawSettings.MinimumStrokeWidth)]
        [InlineData(DrawSettings.MinimumStrokeWidth, DrawSettings.MinimumStrokeWidth)]
        [InlineData(DrawSettings.MinimumStrokeWidth + 1, DrawSettings.MinimumStrokeWidth + 1)]
        public void StrokeWidth(int strokeWidth, int expectedStrokeWidth) {
            var shape = Substitute.For<IShapeWithStroke>();

            shape.SetStrokeWidth(strokeWidth);

            Assert.Equal(expectedStrokeWidth, shape.GetStrokeWidth());
        }
    }
}
