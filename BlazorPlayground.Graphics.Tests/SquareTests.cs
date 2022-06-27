using System.Linq;
using Xunit;

namespace BlazorPlayground.Graphics.Tests {
    public class SquareTests {
        [Fact]
        public void GetPoints() {
            var square = new Square(new Point(100, 100), new Point(50, 50));

            var result = square.GetPoints().ToList();

            Assert.Equal(5, result.Count);
            Assert.Equal(new Point(50, 50), result[0]);
            Assert.Equal(new Point(150, 50), result[1]);
            Assert.Equal(new Point(150, 150), result[2]);
            Assert.Equal(new Point(50, 150), result[3]);
            Assert.Equal(new Point(50, 50), result[4]);
        }
    }
}
