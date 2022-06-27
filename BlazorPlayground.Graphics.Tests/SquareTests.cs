using System.Linq;
using Xunit;

namespace BlazorPlayground.Graphics.Tests {
    public class SquareTests {
        [Fact]
        public void GetPoints() {
            var square = new Square(new Point(100, 100), new Point(50, 50));

            var result = square.GetPoints().ToList();

            Assert.Equal(5, result.Count);
            AssertEqual(new Point(150, 50), result[0]);
            AssertEqual(new Point(150, 150), result[1]);
            AssertEqual(new Point(50, 150), result[2]);
            AssertEqual(new Point(50, 50), result[3]);
            AssertEqual(new Point(150, 50), result[4]);
        }

        private static void AssertEqual(Point expected, Point actual) {
            Assert.Equal(expected.X, actual.X, 3);
            Assert.Equal(expected.Y, actual.Y, 3);
        }
    }
}
