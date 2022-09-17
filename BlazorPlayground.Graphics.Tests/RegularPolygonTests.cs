using System.Linq;
using Xunit;

namespace BlazorPlayground.Graphics.Tests {
    public class RegularPolygonTests {
        [Fact]
        public void ElementName() {
            var polygon = new RegularPolygon(new Point(100, 100), new Point(50, 50));

            Assert.Equal("polygon", polygon.ElementName);
        }

        [Fact]
        public void GetSnapPoints() {
            var square = new RegularPolygon(new Point(100, 100), new Point(50, 50));

            square.SetSides(4);

            var result = square.GetSnapPoints();

            Assert.Equal(5, result.Count);
            PointAssert.Contains(result, new Point(50, 50));
            PointAssert.Contains(result, new Point(150, 50));
            PointAssert.Contains(result, new Point(150, 150));
            PointAssert.Contains(result, new Point(50, 150));
            PointAssert.Contains(result, new Point(100, 100));
        }

        [Fact]
        public void GetPoints_Square_0_Degrees() {
            var square = new RegularPolygon(new Point(100, 100), new Point(50, 50));

            square.SetSides(4);

            var result = square.GetPoints().ToList();

            Assert.Equal(4, result.Count);
            PointAssert.Equal(new Point(50, 50), result[0]);
            PointAssert.Equal(new Point(150, 50), result[1]);
            PointAssert.Equal(new Point(150, 150), result[2]);
            PointAssert.Equal(new Point(50, 150), result[3]);
        }

        [Fact]
        public void GetPoints_Square_45_Degrees() {
            var square = new RegularPolygon(new Point(100, 100), new Point(50, 100));

            square.SetSides(4);

            var result = square.GetPoints().ToList();

            Assert.Equal(4, result.Count);
            PointAssert.Equal(new Point(50, 100), result[0]);
            PointAssert.Equal(new Point(100, 50), result[1]);
            PointAssert.Equal(new Point(150, 100), result[2]);
            PointAssert.Equal(new Point(100, 150), result[3]);
        }

        [Fact]
        public void GetPoints_Square_22_Degrees() {
            var square = new RegularPolygon(new Point(100, 100), new Point(50, 120));

            square.SetSides(4);

            var result = square.GetPoints().ToList();

            Assert.Equal(4, result.Count);
            PointAssert.Equal(new Point(50, 120), result[0]);
            PointAssert.Equal(new Point(80, 50), result[1]);
            PointAssert.Equal(new Point(150, 80), result[2]);
            PointAssert.Equal(new Point(120, 150), result[3]);
        }

        [Fact]
        public void GetPoints_Square_112_Degrees() {
            var square = new RegularPolygon(new Point(100, 100), new Point(150, 120));

            square.SetSides(4);

            var result = square.GetPoints().ToList();

            Assert.Equal(4, result.Count);
            PointAssert.Equal(new Point(150, 120), result[0]);
            PointAssert.Equal(new Point(80, 150), result[1]);
            PointAssert.Equal(new Point(50, 80), result[2]);
            PointAssert.Equal(new Point(120, 50), result[3]);
        }

        [Fact]
        public void GetPoints_Triangle() {
            var triangle = new RegularPolygon(new Point(100, 100), new Point(100, 50));

            triangle.SetSides(3);

            var result = triangle.GetPoints().ToList();

            Assert.Equal(3, result.Count);

            PointAssert.Equal(new Point(100, 50), result[0]);
            PointAssert.Equal(new Point(143.3, 125), result[1]);
            PointAssert.Equal(new Point(56.7, 125), result[2]);
        }

        [Fact]
        public void GetPoints_Hexagon() {
            var hexagon = new RegularPolygon(new Point(100, 100), new Point(100, 50));

            hexagon.SetSides(6);

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
        public void GetAttributes() {
            var polygon = new RegularPolygon(new Point(100, 100), new Point(50, 50));

            polygon.SetSides(4);

            var attributes = polygon.GetAttributes();

            var attribute = Assert.Single(attributes);
            Assert.Equal("points", attribute.Key);
            Assert.Equal("50,49.99999999999999 150,49.99999999999999 150,150 50,150", attribute.Value);
        }

        [Fact]
        public void Anchors_Get() {
            var polygon = new RegularPolygon(new Point(100, 150), new Point(200, 250));

            var result = polygon.Anchors;

            Assert.Equal(2, result.Count);
            PointAssert.Equal(new Point(100, 150), result[0].Get(polygon));
            PointAssert.Equal(new Point(200, 250), result[1].Get(polygon));
        }

        [Fact]
        public void Anchors_Set() {
            var polygon = new RegularPolygon(new Point(100, 150), new Point(200, 250));

            var result = polygon.Anchors;

            Assert.Equal(2, result.Count);
            result[0].Set(polygon, new Point(110, 160));
            result[1].Set(polygon, new Point(210, 260));
            PointAssert.Equal(new Point(110, 160), polygon.CenterPoint);
            PointAssert.Equal(new Point(210, 260), polygon.RadiusPoint);
        }

        [Fact]
        public void Clone() {
            var polygon = new RegularPolygon(new Point(100, 150), new Point(200, 250));

            var result = polygon.Clone();

            var resultPolygon = Assert.IsType<RegularPolygon>(result);

            Assert.NotSame(polygon, resultPolygon);
            PointAssert.Equal(new Point(100, 150), resultPolygon.CenterPoint);
            PointAssert.Equal(new Point(200, 250), resultPolygon.RadiusPoint);
        }
    }
}
