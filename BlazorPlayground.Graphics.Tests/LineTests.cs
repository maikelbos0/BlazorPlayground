using System.Linq;
using Xunit;

namespace BlazorPlayground.Graphics.Tests {
    public class LineTests {
        [Fact]
        public void GetPoints() {
            var line = new Line(new Point(100, 150), new Point(200, 250));

            var result = line.GetPoints().ToList();

            Assert.Equal(2, result.Count);
            PointAssert.Equal(new Point(100, 150), result[0]);
            PointAssert.Equal(new Point(200, 250), result[1]);
        }
    }
    public class RectangleTests {
        [Fact]
        public void GetPoints() {
            var rectangle = new Rectangle(new Point(100, 150), new Point(200, 250));

            var result = rectangle.GetPoints().ToList();

            Assert.Equal(4, result.Count);
            PointAssert.Equal(new Point(100, 150), result[0]);
            PointAssert.Equal(new Point(100, 250), result[1]);
            PointAssert.Equal(new Point(200, 250), result[2]);
            PointAssert.Equal(new Point(200, 150), result[3]);
        }
    }
}
