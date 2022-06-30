using System.Linq;
using Xunit;

namespace BlazorPlayground.Graphics.Tests {
    public class RegularPolygonTests {
        [Fact]
        public void GetPoints_Square_0_Degrees() {
            var square = new RegularPolygon(new Point(100, 100), new Point(50, 50), 4);

            var result = square.GetPoints().ToList();

            Assert.Equal(5, result.Count);
            AssertEqual(new Point(50, 50), result[0]);
            AssertEqual(new Point(150, 50), result[1]);
            AssertEqual(new Point(150, 150), result[2]);
            AssertEqual(new Point(50, 150), result[3]);
            AssertEqual(new Point(50, 50), result[4]);
        }

        [Fact]
        public void GetPoints_Square_45_Degrees() {
            var square = new RegularPolygon(new Point(100, 100), new Point(50, 100), 4);

            var result = square.GetPoints().ToList();

            Assert.Equal(5, result.Count);
            AssertEqual(new Point(50, 100), result[0]);
            AssertEqual(new Point(100, 50), result[1]);
            AssertEqual(new Point(150, 100), result[2]);
            AssertEqual(new Point(100, 150), result[3]);
            AssertEqual(new Point(50, 100), result[4]);
        }

        [Fact]
        public void GetPoints_Square_22_Degrees() {
            var square = new RegularPolygon(new Point(100, 100), new Point(50, 120), 4);

            var result = square.GetPoints().ToList();

            Assert.Equal(5, result.Count);
            AssertEqual(new Point(50, 120), result[0]);
            AssertEqual(new Point(80, 50), result[1]);
            AssertEqual(new Point(150, 80), result[2]);
            AssertEqual(new Point(120, 150), result[3]);
            AssertEqual(new Point(50, 120), result[4]);
        }

        [Fact]
        public void GetPoints_Square_112_Degrees() {
            var square = new RegularPolygon(new Point(100, 100), new Point(150, 120), 4);

            var result = square.GetPoints().ToList();

            Assert.Equal(5, result.Count);
            AssertEqual(new Point(150, 120), result[0]);
            AssertEqual(new Point(80, 150), result[1]);
            AssertEqual(new Point(50, 80), result[2]);
            AssertEqual(new Point(120, 50), result[3]);
            AssertEqual(new Point(150, 120), result[4]);
        }

        private static void AssertEqual(Point expected, Point actual) {
            Assert.Equal(expected.X, actual.X, 3);
            Assert.Equal(expected.Y, actual.Y, 3);
        }
    }
}
