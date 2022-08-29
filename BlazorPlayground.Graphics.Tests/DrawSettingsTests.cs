using Xunit;

namespace BlazorPlayground.Graphics.Tests {
    public class DrawSettingsTests {
        [Theory]
        [InlineData(-1, DrawSettings.MinimumStrokeWidth)]
        [InlineData(DrawSettings.MinimumStrokeWidth - 1, DrawSettings.MinimumStrokeWidth)]
        [InlineData(DrawSettings.MinimumStrokeWidth, DrawSettings.MinimumStrokeWidth)]
        [InlineData(DrawSettings.MinimumStrokeWidth + 1, DrawSettings.MinimumStrokeWidth + 1)]
        public void StrokeWidth(int strokeWidth, int expectedStrokeWidth) {
            var shape = new Line(new Point(100, 150), new Point(200, 250)) {
                StrokeWidth = strokeWidth
            };

            Assert.Equal(expectedStrokeWidth, shape.StrokeWidth);
        }

        [Theory]
        [InlineData(-1, DrawSettings.MinimumSides)]
        [InlineData(DrawSettings.MinimumSides - 1, DrawSettings.MinimumSides)]
        [InlineData(DrawSettings.MinimumSides, DrawSettings.MinimumSides)]
        [InlineData(DrawSettings.MinimumSides + 1, DrawSettings.MinimumSides + 1)]
        public void Sides(int sides, int expectedSides) {
            var settings = new DrawSettings() {
                Sides = sides
            };

            Assert.Equal(expectedSides, settings.Sides);
        }
    }
}
