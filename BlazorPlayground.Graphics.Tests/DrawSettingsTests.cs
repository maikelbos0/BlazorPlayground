using Xunit;

namespace BlazorPlayground.Graphics.Tests {
    public class DrawSettingsTests {
        [Fact]
        public void StrokeWidth_Minimum() {
            var settings = new DrawSettings() {
                StrokeWidth = 0
            };

            Assert.Equal(1, settings.StrokeWidth);
        }

        [Fact]
        public void Sides_Minimum() {
            var settings = new DrawSettings() {
                Sides = 2
            };

            Assert.Equal(3, settings.Sides);
        }
    }
}
