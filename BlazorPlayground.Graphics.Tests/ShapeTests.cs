using Xunit;

namespace BlazorPlayground.Graphics.Tests {
    public class ShapeTests {
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
        public void Clone() {
            var polygon = new RegularPolygon(new Point(100, 150), new Point(200, 250));

            var result = polygon.Clone();

            Assert.NotSame(polygon, result);
        }

        [Fact]
        public void Clone_SetOpacity() {
            var polygon = new RegularPolygon(new Point(100, 150), new Point(200, 250));

            polygon.SetOpacity(50);

            var result = polygon.Clone();

            Assert.Equal(50, Assert.IsAssignableFrom<IShapeWithOpacity>(result).GetOpacity());
        }

        [Fact]
        public void Clone_SetFill() {
            var polygon = new RegularPolygon(new Point(100, 150), new Point(200, 250));

            polygon.SetFill(new Color(255, 0, 255, 1));

            var result = polygon.Clone();

            PaintServerAssert.Equal(new Color(255, 0, 255, 1), Assert.IsAssignableFrom<IShapeWithFill>(result).GetFill());
        }

        [Fact]
        public void Clone_SetFillOpacity() {
            var polygon = new RegularPolygon(new Point(100, 150), new Point(200, 250));

            polygon.SetFillOpacity(50);

            var result = polygon.Clone();

            Assert.Equal(50, Assert.IsAssignableFrom<IShapeWithFill>(result).GetFillOpacity());
        }

        [Fact]
        public void Clone_SetStroke() {
            var polygon = new RegularPolygon(new Point(100, 150), new Point(200, 250));

            polygon.SetStroke(new Color(255, 0, 255, 1));

            var result = polygon.Clone();

            PaintServerAssert.Equal(new Color(255, 0, 255, 1), Assert.IsAssignableFrom<IShapeWithStroke>(result).GetStroke());
        }

        [Fact]
        public void Clone_SetStrokeWidth() {
            var polygon = new RegularPolygon(new Point(100, 150), new Point(200, 250));

            polygon.SetStrokeWidth(10);

            var result = polygon.Clone();

            Assert.Equal(10, Assert.IsAssignableFrom<IShapeWithStroke>(result).GetStrokeWidth());
        }

        [Fact]
        public void Clone_SetStrokeOpacity() {
            var polygon = new RegularPolygon(new Point(100, 150), new Point(200, 250));

            polygon.SetStrokeOpacity(50);

            var result = polygon.Clone();

            Assert.Equal(50, Assert.IsAssignableFrom<IShapeWithStroke>(result).GetStrokeOpacity());
        }

        [Fact]
        public void Clone_SetStrokeLinecap() {
            var line = new Line(new Point(100, 150), new Point(200, 250));

            line.SetStrokeLinecap(Linecap.Square);

            var result = line.Clone();

            Assert.Equal(Linecap.Square, Assert.IsAssignableFrom<IShapeWithStrokeLinecap>(result).GetStrokeLinecap());
        }

        [Fact]
        public void Clone_SetStrokeLinejoin() {
            var rectangle = new Rectangle(new Point(100, 150), new Point(200, 250));

            rectangle.SetStrokeLinejoin(Linejoin.Arcs);

            var result = rectangle.Clone();

            Assert.Equal(Linejoin.Arcs, Assert.IsAssignableFrom<IShapeWithStrokeLinejoin>(result).GetStrokeLinejoin());
        }

        [Fact]
        public void Clone_SetSides() {
            var polygon = new RegularPolygon(new Point(100, 150), new Point(200, 250));

            polygon.SetSides(5);

            var result = polygon.Clone();

            Assert.Equal(5, Assert.IsAssignableFrom<IShapeWithSides>(result).GetSides());
        }
    }
}
