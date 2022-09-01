using Xunit;

namespace BlazorPlayground.Graphics.Tests {
    public class DrawSettingsTests {
        [Theory]
        [InlineData(DrawSettings.MinimumOpacity - 0.01, DrawSettings.MinimumOpacity)]
        [InlineData(DrawSettings.MinimumOpacity, DrawSettings.MinimumOpacity)]
        [InlineData(0.5, 0.5)]
        [InlineData(DrawSettings.MaximumOpacity, DrawSettings.MaximumOpacity)]
        [InlineData(DrawSettings.MaximumOpacity + 0.01, DrawSettings.MaximumOpacity)]
        public void Opacity(double opacity, double expectedOpacity) {
            var settings = new DrawSettings() {
                Opacity = opacity
            };

            Assert.Equal(expectedOpacity, settings.Opacity);
        }

        [Theory]
        [InlineData(-1, DrawSettings.MinimumStrokeWidth)]
        [InlineData(DrawSettings.MinimumStrokeWidth - 1, DrawSettings.MinimumStrokeWidth)]
        [InlineData(DrawSettings.MinimumStrokeWidth, DrawSettings.MinimumStrokeWidth)]
        [InlineData(DrawSettings.MinimumStrokeWidth + 1, DrawSettings.MinimumStrokeWidth + 1)]
        public void StrokeWidth(int strokeWidth, int expectedStrokeWidth) {
            var settings = new DrawSettings() {
                StrokeWidth = strokeWidth
            };

            Assert.Equal(expectedStrokeWidth, settings.StrokeWidth);
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
