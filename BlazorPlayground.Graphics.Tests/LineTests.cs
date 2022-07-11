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

        [Fact]
        public void GetAnchors_Get() {
            var line = new Line(new Point(100, 150), new Point(200, 250));

            var result = line.GetAnchors().ToList();
            
            Assert.Equal(2, result.Count);
            PointAssert.Equal(new Point(100, 150), result[0].Get());
            PointAssert.Equal(new Point(200, 250), result[1].Get());
        }

        [Fact]
        public void GetAnchors_Set() {
            var line = new Line(new Point(100, 150), new Point(200, 250));

            var result = line.GetAnchors().ToList();
            
            Assert.Equal(2, result.Count);
            result[0].Set(new Point(110, 160));
            result[1].Set(new Point(210, 260));
            PointAssert.Equal(new Point(110, 160), line.StartPoint);
            PointAssert.Equal(new Point(210, 260), line.EndPoint);
        }
    }
}
