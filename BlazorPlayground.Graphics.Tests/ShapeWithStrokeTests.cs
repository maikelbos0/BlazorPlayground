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

        [Theory]
        [InlineData(DrawSettings.MinimumOpacity - 1, DrawSettings.MinimumOpacity)]
        [InlineData(DrawSettings.MinimumOpacity, DrawSettings.MinimumOpacity)]
        [InlineData(45, 45)]
        [InlineData(DrawSettings.MaximumOpacity, DrawSettings.MaximumOpacity)]
        [InlineData(DrawSettings.MaximumOpacity + 1, DrawSettings.MaximumOpacity)]
        public void SetStrokeOpacity(int opacity, int expectedOpacity) {
            var shape = Substitute.For<IShapeWithStroke>();

            shape.SetStrokeOpacity(opacity);

            Assert.Equal(expectedOpacity, shape.GetStrokeOpacity());
        }
    }
}
