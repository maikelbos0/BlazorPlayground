using System.Linq;
using Xunit;

namespace BlazorPlayground.Graphics.Tests {
    public class RegularPolygonTests {
        [Fact]
        public void GetPoints_Square_0_Degrees() {
            var square = new RegularPolygon(new Point(100, 100), new Point(50, 50), 4);

            var result = square.GetPoints().ToList();

            Assert.Equal(4, result.Count);
            PointAssert.Equal(new Point(50, 50), result[0]);
            PointAssert.Equal(new Point(150, 50), result[1]);
            PointAssert.Equal(new Point(150, 150), result[2]);
            PointAssert.Equal(new Point(50, 150), result[3]);
        }

        [Fact]
        public void GetPoints_Square_45_Degrees() {
            var square = new RegularPolygon(new Point(100, 100), new Point(50, 100), 4);

            var result = square.GetPoints().ToList();

            Assert.Equal(4, result.Count);
            PointAssert.Equal(new Point(50, 100), result[0]);
            PointAssert.Equal(new Point(100, 50), result[1]);
            PointAssert.Equal(new Point(150, 100), result[2]);
            PointAssert.Equal(new Point(100, 150), result[3]);
        }

        [Fact]
        public void GetPoints_Square_22_Degrees() {
            var square = new RegularPolygon(new Point(100, 100), new Point(50, 120), 4);

            var result = square.GetPoints().ToList();

            Assert.Equal(4, result.Count);
            PointAssert.Equal(new Point(50, 120), result[0]);
            PointAssert.Equal(new Point(80, 50), result[1]);
            PointAssert.Equal(new Point(150, 80), result[2]);
            PointAssert.Equal(new Point(120, 150), result[3]);
        }

        [Fact]
        public void GetPoints_Square_112_Degrees() {
            var square = new RegularPolygon(new Point(100, 100), new Point(150, 120), 4);

            var result = square.GetPoints().ToList();

            Assert.Equal(4, result.Count);
            PointAssert.Equal(new Point(150, 120), result[0]);
            PointAssert.Equal(new Point(80, 150), result[1]);
            PointAssert.Equal(new Point(50, 80), result[2]);
            PointAssert.Equal(new Point(120, 50), result[3]);
        }

        [Fact]
        public void GetPoints_Triangle() {
            var triangle = new RegularPolygon(new Point(100, 100), new Point(100, 50), 3);

            var result = triangle.GetPoints().ToList();

            Assert.Equal(3, result.Count);

            PointAssert.Equal(new Point(100, 50), result[0]);
            PointAssert.Equal(new Point(143.3, 125), result[1]);
            PointAssert.Equal(new Point(56.7, 125), result[2]);
        }

        [Fact]
        public void GetPoints_Hexagon() {
            var hexagon = new RegularPolygon(new Point(100, 100), new Point(100, 50), 6);

            var result = hexagon.GetPoints().ToList();

            Assert.Equal(6, result.Count);

            PointAssert.Equal(new Point(100, 50), result[0]);
            PointAssert.Equal(new Point(143.3, 75), result[1]);
            PointAssert.Equal(new Point(143.3, 125), result[2]);
            PointAssert.Equal(new Point(100, 150), result[3]);
            PointAssert.Equal(new Point(56.7, 125), result[4]);
            PointAssert.Equal(new Point(56.7, 75), result[5]);
        }

        [Fact]
        public void Anchors_Get() {
            var polygon = new RegularPolygon(new Point(100, 150), new Point(200, 250), 3);

            var result = polygon.Anchors;

            Assert.Equal(2, result.Count);
            PointAssert.Equal(new Point(100, 150), result[0].Get(polygon));
            PointAssert.Equal(new Point(200, 250), result[1].Get(polygon));
        }

        [Fact]
        public void Anchors_Set() {
            var polygon = new RegularPolygon(new Point(100, 150), new Point(200, 250), 3);

            var result = polygon.Anchors;

            Assert.Equal(2, result.Count);
            result[0].Set(polygon, new Point(110, 160));
            result[1].Set(polygon, new Point(210, 260));
            PointAssert.Equal(new Point(110, 160), polygon.CenterPoint);
            PointAssert.Equal(new Point(210, 260), polygon.RadiusPoint);
        }
    }
}
