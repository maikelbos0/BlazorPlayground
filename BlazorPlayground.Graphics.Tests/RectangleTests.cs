using System.Linq;
using Xunit;

namespace BlazorPlayground.Graphics.Tests {
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

        [Fact]
        public void GetAnchors_Get() {
            var rectangle = new Rectangle(new Point(100, 150), new Point(200, 250));

            var result = rectangle.GetAnchors().ToList();

            Assert.Equal(2, result.Count);
            PointAssert.Equal(new Point(100, 150), result[0].Get());
            PointAssert.Equal(new Point(200, 250), result[1].Get());
        }

        [Fact]
        public void GetAnchors_Set() {
            var rectangle = new Rectangle(new Point(100, 150), new Point(200, 250));

            var result = rectangle.GetAnchors().ToList();

            Assert.Equal(2, result.Count);
            result[0].Set(new Point(110, 160));
            result[1].Set(new Point(210, 260));
            PointAssert.Equal(new Point(110, 160), rectangle.StartPoint);
            PointAssert.Equal(new Point(210, 260), rectangle.EndPoint);
        }
    }
}
