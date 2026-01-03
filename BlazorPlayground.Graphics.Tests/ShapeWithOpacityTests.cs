using NSubstitute;
using Xunit;

namespace BlazorPlayground.Graphics.Tests {
    public class ShapeWithOpacityTests {
        [Theory]
        [InlineData(DrawSettings.MinimumOpacity - 1, DrawSettings.MinimumOpacity)]
        [InlineData(DrawSettings.MinimumOpacity, DrawSettings.MinimumOpacity)]
        [InlineData(45, 45)]
        [InlineData(DrawSettings.MaximumOpacity, DrawSettings.MaximumOpacity)]
        [InlineData(DrawSettings.MaximumOpacity + 1, DrawSettings.MaximumOpacity)]
        public void Opacity(int opacity, int expectedOpacity) {
            var shape = Substitute.For<IShapeWithOpacity>();

            shape.Opacity = opacity;

            Assert.Equal(expectedOpacity, shape.Opacity);
        }
    }
}
