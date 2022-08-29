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

            shape.Transform(new Point(10, 20), false, 0);

            PointAssert.Equal(new Point(110, 170), shape.StartPoint);
            PointAssert.Equal(new Point(210, 270), shape.EndPoint);
        }

        [Theory]
        [InlineData(110, 105, 200, 300, 450, 550)]
        [InlineData(90, 95, 200, 300, 450, 550)]
        [InlineData(60, 55, 150, 250, 400, 500)]
        [InlineData(40, 45, 150, 250, 400, 500)]
        public void Transform_With_SnapToGrid(double deltaX, double deltaY, double expectedStartX, double expectedStartY, double expectedEndX, double expectedEndY) {
            var shape = new Line(new Point(101, 201), new Point(351, 451));

            shape.Transform(new Point(deltaX, deltaY), true, 100);

            PointAssert.Equal(new Point(expectedStartX, expectedStartY), shape.StartPoint);
            PointAssert.Equal(new Point(expectedEndX, expectedEndY), shape.EndPoint);
        }

        [Fact]
        public void CreateSvgElement_Line() {
            var shape = new Line(new Point(100, 200), new Point(150, 250)) {
                Fill = new Color(255, 255, 0, 1),
                Stroke = new Color(128, 0, 0, 1),
                StrokeWidth = 5,
                StrokeLinecap = Linecap.Round,
                StrokeLinejoin = Linejoin.Arcs
            };

            var result = shape.CreateSvgElement();

            Assert.Equal(shape.ElementName, result.Name);
            Assert.Equal("100", result.Attribute("x1")?.Value);
            Assert.Equal("200", result.Attribute("y1")?.Value);
            Assert.Equal("150", result.Attribute("x2")?.Value);
            Assert.Equal("250", result.Attribute("y2")?.Value);
            Assert.Equal("#800000", result.Attribute("stroke")?.Value);
            Assert.Equal("5", result.Attribute("stroke-width")?.Value);
            Assert.Equal("round", result.Attribute("stroke-linecap")?.Value);
            Assert.Equal("Line", result.Attribute("data-shape-type")?.Value);
            Assert.Equal("100,200", result.Attribute("data-shape-anchor-0")?.Value);
            Assert.Equal("150,250", result.Attribute("data-shape-anchor-1")?.Value);
        }

        [Fact]
        public void CreateSvgElement_Rectangle() {
            var shape = new Rectangle(new Point(100, 200), new Point(150, 250)) {
                Fill = new Color(255, 255, 0, 1),
                Stroke = new Color(128, 0, 0, 1),
                StrokeWidth = 5,
                StrokeLinejoin = Linejoin.Arcs
            };

            var result = shape.CreateSvgElement();

            Assert.Equal(shape.ElementName, result.Name);
            Assert.Equal("100", result.Attribute("x")?.Value);
            Assert.Equal("200", result.Attribute("y")?.Value);
            Assert.Equal("50", result.Attribute("width")?.Value);
            Assert.Equal("50", result.Attribute("height")?.Value);
            Assert.Equal("#FFFF00", result.Attribute("fill")?.Value);
            Assert.Equal("#800000", result.Attribute("stroke")?.Value);
            Assert.Equal("5", result.Attribute("stroke-width")?.Value);
            Assert.Equal("arcs", result.Attribute("stroke-linejoin")?.Value);
            Assert.Equal("Rectangle", result.Attribute("data-shape-type")?.Value);
            Assert.Equal("100,200", result.Attribute("data-shape-anchor-0")?.Value);
            Assert.Equal("150,250", result.Attribute("data-shape-anchor-1")?.Value);
        }

        [Fact]
        public void Clone() {
            var polygon = new RegularPolygon(new Point(100, 150), new Point(200, 250)) {
                Fill = new Color(255, 0, 255, 1),
                Stroke = new Color(0, 255, 0, 1),
                Sides = 5,
                StrokeLinecap = Linecap.Round,
                StrokeLinejoin = Linejoin.Round,
                StrokeWidth = 10
            };

            var result = polygon.Clone();

            Assert.NotSame(polygon, result);
            PaintServerAssert.Equal(new Color(255, 0, 255, 1), result.Fill);
            PaintServerAssert.Equal(new Color(0, 255, 0, 1), result.Stroke);
            Assert.Equal(5, result.Sides);
            Assert.Equal(Linecap.Round, result.StrokeLinecap);
            Assert.Equal(Linejoin.Round, result.StrokeLinejoin);
            Assert.Equal(10, result.StrokeWidth);
        }
    }
}
