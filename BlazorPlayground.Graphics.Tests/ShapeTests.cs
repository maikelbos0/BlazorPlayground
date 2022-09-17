using Xunit;

namespace BlazorPlayground.Graphics.Tests {
    public class ShapeTests {
        [Theory]
        [InlineData(-1, DrawSettings.MinimumStrokeWidth)]
        [InlineData(DrawSettings.MinimumStrokeWidth - 1, DrawSettings.MinimumStrokeWidth)]
        [InlineData(DrawSettings.MinimumStrokeWidth, DrawSettings.MinimumStrokeWidth)]
        [InlineData(DrawSettings.MinimumStrokeWidth + 1, DrawSettings.MinimumStrokeWidth + 1)]
        public void StrokeWidth(int strokeWidth, int expectedStrokeWidth) {
            var shape = new Line(new Point(100, 150), new Point(200, 250)) {
                StrokeWidth = strokeWidth
            };

            Assert.Equal(expectedStrokeWidth, shape.StrokeWidth);
        }

        [Theory]
        [InlineData(-1, DrawSettings.MinimumSides)]
        [InlineData(DrawSettings.MinimumSides - 1, DrawSettings.MinimumSides)]
        [InlineData(DrawSettings.MinimumSides, DrawSettings.MinimumSides)]
        [InlineData(DrawSettings.MinimumSides + 1, DrawSettings.MinimumSides + 1)]
        public void Sides(int sides, int expectedSides) {
            var shape = new Line(new Point(100, 150), new Point(200, 250)) {
                Sides = sides
            };

            Assert.Equal(expectedSides, shape.Sides);
        }

        [Fact]
        public void Definition() {
            var shape = new Line(new Point(100, 150), new Point(200, 250));

            Assert.Equal(ShapeDefinition.Get(typeof(Line)), shape.Definition);
        }

        [Fact]
        public void Transform() {
            var shape = new Line(new Point(100, 150), new Point(200, 250));

            shape.Transform(new Point(10, 20), false, 50, false, new[] { new Point(50, 50) });

            PointAssert.Equal(new Point(110, 170), shape.StartPoint);
            PointAssert.Equal(new Point(210, 270), shape.EndPoint);
        }

        [Fact]
        public void Transform_With_SnapToGrid() {
            var shape = new Line(new Point(105, 205), new Point(155, 255));

            shape.Transform(new Point(50, 50), true, 50, false, new[] { new Point(151, 251) });

            PointAssert.Equal(new Point(150, 250), shape.StartPoint);
            PointAssert.Equal(new Point(200, 300), shape.EndPoint);
        }

        [Fact]
        public void Transform_With_SnapToShapes() {
            var shape = new Line(new Point(105, 205), new Point(155, 255));

            shape.Transform(new Point(50, 50), false, 50, true, new[] { new Point(151, 251) });

            PointAssert.Equal(new Point(151, 251), shape.StartPoint);
            PointAssert.Equal(new Point(201, 301), shape.EndPoint);
        }

        [Fact]
        public void Transform_With_SnapToGrid_And_SnapToShapes_Grid() {
            var shape = new Line(new Point(105, 205), new Point(155, 255));

            shape.Transform(new Point(50, 50), true, 50, true, new[] { new Point(149, 249) });

            PointAssert.Equal(new Point(150, 250), shape.StartPoint);
            PointAssert.Equal(new Point(200, 300), shape.EndPoint);
        }

        [Fact]
        public void Transform_With_SnapToGrid_And_SnapToShapes_Points() {
            var shape = new Line(new Point(105, 205), new Point(155, 255));

            shape.Transform(new Point(50, 50), true, 50, true, new[] { new Point(151, 251) });

            PointAssert.Equal(new Point(151, 251), shape.StartPoint);
            PointAssert.Equal(new Point(201, 301), shape.EndPoint);
        }

        [Fact]
        public void Clone_SetFill() {
            var polygon = new RegularPolygon(new Point(100, 150), new Point(200, 250));

            polygon.SetFill(new Color(255, 0, 255, 1));

            var result = polygon.Clone();

            PaintServerAssert.Equal(new Color(255, 0, 255, 1), Assert.IsAssignableFrom<IShapeWithFill>(result).GetFill());
        }

        [Fact]
        public void Clone() {
            var polygon = new RegularPolygon(new Point(100, 150), new Point(200, 250)) {
                Stroke = new Color(0, 255, 0, 1),
                Sides = 5,
                StrokeLinecap = Linecap.Round,
                StrokeLinejoin = Linejoin.Round,
                StrokeWidth = 10
            };

            var result = polygon.Clone();

            Assert.NotSame(polygon, result);

            // TODO move to separate test per property
            PaintServerAssert.Equal(new Color(0, 255, 0, 1), result.Stroke);
            Assert.Equal(5, result.Sides);
            Assert.Equal(Linecap.Round, result.StrokeLinecap);
            Assert.Equal(Linejoin.Round, result.StrokeLinejoin);
            Assert.Equal(10, result.StrokeWidth);
        }
    }
}
