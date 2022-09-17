using NSubstitute;
using Xunit;

namespace BlazorPlayground.Graphics.Tests {
    public class ShapeWithSidesTests {
        [Theory]
        [InlineData(-1, DrawSettings.MinimumSides)]
        [InlineData(DrawSettings.MinimumSides - 1, DrawSettings.MinimumSides)]
        [InlineData(DrawSettings.MinimumSides, DrawSettings.MinimumSides)]
        [InlineData(DrawSettings.MinimumSides + 1, DrawSettings.MinimumSides + 1)]
        public void Sides(int sides, int expectedSides) {
            var shape = Substitute.For<IShapeWithSides>();

            shape.SetSides(sides);

            Assert.Equal(expectedSides, shape.GetSides());
        }
    }
}
