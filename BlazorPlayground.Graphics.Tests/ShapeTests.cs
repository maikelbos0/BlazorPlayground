using Xunit;

namespace BlazorPlayground.Graphics.Tests {
    public class ShapeTests {
        [Fact]
        public void Transform() {
            var shape = new Line(new Point(100, 150), new Point(200, 250));

            shape.Transform(new Point(10, 20), false, 0);

            PointAssert.Equal(new Point(110, 170), shape.StartPoint);
            PointAssert.Equal(new Point(210, 270), shape.EndPoint);
        }

        [Theory]
        [InlineData(110, 105, 200, 300, 450, 550)]
        [InlineData(90, 95, 200, 300, 450, 550)]
        [InlineData(60, 55, 150, 250, 400, 500)]
        [InlineData(40, 45, 150, 250, 400, 500)]
        public void Transform_With_SnapToGrid(double deltaX, double deltaY, double expectedStartX, double expectedStartY, double expectedEndX, double expectedEndY) {
            var shape = new Line(new Point(101, 201), new Point(351, 451));

            shape.Transform(new Point(deltaX, deltaY), true, 100);

            PointAssert.Equal(new Point(expectedStartX, expectedStartY), shape.StartPoint);
            PointAssert.Equal(new Point(expectedEndX, expectedEndY), shape.EndPoint);
        }
    }
}
