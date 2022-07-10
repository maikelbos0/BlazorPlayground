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
}
