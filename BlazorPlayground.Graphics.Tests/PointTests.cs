using Xunit;

namespace BlazorPlayground.Graphics.Tests {
    public class PointTests {
        [Fact]
        public void Add() {
            var result = new Point(100, 150) + new Point(200, 250);

            PointAssert.Equal(new Point(300, 400), result);
        }

        [Fact]
        public void Subtract() {
            var result = new Point(100, 150) - new Point(200, 100);

            PointAssert.Equal(new Point(-100, 50), result);
        }
    }
}
