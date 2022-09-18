using NSubstitute;
using Xunit;

namespace BlazorPlayground.Graphics.Tests {
    public class ShapeWithFillTests {
        [Theory]
        [InlineData(DrawSettings.MinimumOpacity - 1, DrawSettings.MinimumOpacity)]
        [InlineData(DrawSettings.MinimumOpacity, DrawSettings.MinimumOpacity)]
        [InlineData(45, 45)]
        [InlineData(DrawSettings.MaximumOpacity, DrawSettings.MaximumOpacity)]
        [InlineData(DrawSettings.MaximumOpacity + 1, DrawSettings.MaximumOpacity)]
        public void SetFillOpacity(int opacity, int expectedOpacity) {
            var shape = Substitute.For<IShapeWithFill>();

            shape.SetFillOpacity(opacity);

            Assert.Equal(expectedOpacity, shape.GetFillOpacity());
        }
    }
}
