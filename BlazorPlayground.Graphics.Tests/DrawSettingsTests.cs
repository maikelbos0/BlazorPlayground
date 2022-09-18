using Xunit;

namespace BlazorPlayground.Graphics.Tests {
    public class DrawSettingsTests {
        [Theory]
        [InlineData(DrawSettings.MinimumOpacity - 1, DrawSettings.MinimumOpacity)]
        [InlineData(DrawSettings.MinimumOpacity, DrawSettings.MinimumOpacity)]
        [InlineData(45, 45)]
        [InlineData(DrawSettings.MaximumOpacity, DrawSettings.MaximumOpacity)]
        [InlineData(DrawSettings.MaximumOpacity + 1, DrawSettings.MaximumOpacity)]
        public void Opacity(int opacity, int expectedOpacity) {
            var settings = new DrawSettings() {
                Opacity = opacity
            };

            Assert.Equal(expectedOpacity, settings.Opacity);
        }

        [Theory]
        [InlineData(DrawSettings.MinimumOpacity - 1, DrawSettings.MinimumOpacity)]
        [InlineData(DrawSettings.MinimumOpacity, DrawSettings.MinimumOpacity)]
        [InlineData(45, 45)]
        [InlineData(DrawSettings.MaximumOpacity, DrawSettings.MaximumOpacity)]
        [InlineData(DrawSettings.MaximumOpacity + 1, DrawSettings.MaximumOpacity)]
        public void FillOpacity(int opacity, int expectedFillOpacity) {
            var settings = new DrawSettings() {
                FillOpacity = opacity
            };

            Assert.Equal(expectedFillOpacity, settings.FillOpacity);
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
        [InlineData(DrawSettings.MinimumOpacity - 1, DrawSettings.MinimumOpacity)]
        [InlineData(DrawSettings.MinimumOpacity, DrawSettings.MinimumOpacity)]
        [InlineData(45, 45)]
        [InlineData(DrawSettings.MaximumOpacity, DrawSettings.MaximumOpacity)]
        [InlineData(DrawSettings.MaximumOpacity + 1, DrawSettings.MaximumOpacity)]
        public void StrokeOpacity(int opacity, int expectedStrokeOpacity) {
            var settings = new DrawSettings() {
                StrokeOpacity = opacity
            };

            Assert.Equal(expectedStrokeOpacity, settings.StrokeOpacity);
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

        // TODO add stroke/fill opacity
    }
}
