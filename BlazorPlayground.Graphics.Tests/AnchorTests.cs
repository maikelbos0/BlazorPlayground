using Xunit;

namespace BlazorPlayground.Graphics.Tests {
    public class AnchorTests {
        [Fact]
        public void Get() {
            var line = new Line(new Point(100, 150), new Point(200, 250));
            var anchor = new Anchor<Line>(line => line.StartPoint, (line, point) => line.StartPoint = point);

            PointAssert.Equal(new Point(100, 150), anchor.Get(line));
        }

        [Fact]
        public void Set() {
            var line = new Line(new Point(100, 150), new Point(200, 250));
            var anchor = new Anchor<Line>(line => line.StartPoint, (line, point) => line.StartPoint = point);

            anchor.Set(line, new Point(110, 160));

            PointAssert.Equal(new Point(110, 160), line.StartPoint);
        }

        [Fact]
        public void Move() {
            var line = new Line(new Point(100, 150), new Point(200, 250));
            var anchor = new Anchor<Line>(line => line.StartPoint, (line, point) => line.StartPoint = point);

            anchor.Move(line, new Point(10, 20));

            PointAssert.Equal(new Point(110, 170), line.StartPoint);
        }
    }
}
