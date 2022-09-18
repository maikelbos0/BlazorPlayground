using Xunit;

namespace BlazorPlayground.Graphics.Tests {
    public class DrawableShapeTests {
        [Fact]
        public void CreateSvgElement() {
            var shape = new Line(new Point(100, 200), new Point(150, 250));

            var result = shape.CreateSvgElement();

            Assert.Equal(shape.ElementName, result.Name);
            Assert.Equal("100", result.Attribute("x1")?.Value);
            Assert.Equal("200", result.Attribute("y1")?.Value);
            Assert.Equal("150", result.Attribute("x2")?.Value);
            Assert.Equal("250", result.Attribute("y2")?.Value);
            Assert.Equal("Line", result.Attribute("data-shape-type")?.Value);
            Assert.Equal("100,200", result.Attribute("data-shape-anchor-0")?.Value);
            Assert.Equal("150,250", result.Attribute("data-shape-anchor-1")?.Value);
        }

        [Fact]
        public void CreateSvgElement_Opacity() {
            var shape = new Rectangle(new Point(100, 200), new Point(150, 250));

            shape.SetOpacity(50);

            var result = shape.CreateSvgElement();

            Assert.Equal("0.5", result.Attribute("opacity")?.Value);
        }

        [Fact]
        public void CreateSvgElement_Fill() {
            var shape = new Rectangle(new Point(100, 200), new Point(150, 250));

            shape.SetFill(new Color(255, 255, 0, 1));

            var result = shape.CreateSvgElement();

            Assert.Equal("#FFFF00", result.Attribute("fill")?.Value);
        }

        [Fact]
        public void CreateSvgElement_FillOpacity() {
            var shape = new Rectangle(new Point(100, 200), new Point(150, 250));

            shape.SetFillOpacity(50);

            var result = shape.CreateSvgElement();

            Assert.Equal("0.5", result.Attribute("fill-opacity")?.Value);
        }

        [Fact]
        public void CreateSvgElement_Stroke() {
            var shape = new Rectangle(new Point(100, 200), new Point(150, 250));

            shape.SetStroke(new Color(255, 255, 0, 1));

            var result = shape.CreateSvgElement();

            Assert.Equal("#FFFF00", result.Attribute("stroke")?.Value);
        }

        [Fact]
        public void CreateSvgElement_StrokeWidth() {
            var shape = new Rectangle(new Point(100, 200), new Point(150, 250));

            shape.SetStrokeWidth(10);

            var result = shape.CreateSvgElement();

            Assert.Equal("10", result.Attribute("stroke-width")?.Value);
        }

        [Fact]
        public void CreateSvgElement_StrokeOpacity() {
            var shape = new Rectangle(new Point(100, 200), new Point(150, 250));

            shape.SetStrokeOpacity(50);

            var result = shape.CreateSvgElement();

            Assert.Equal("0.5", result.Attribute("stroke-opacity")?.Value);
        }

        [Fact]
        public void CreateSvgElement_StrokeLinecap() {
            var shape = new Line(new Point(100, 200), new Point(150, 250));

            shape.SetStrokeLinecap(Linecap.Square);

            var result = shape.CreateSvgElement();

            Assert.Equal("square", result.Attribute("stroke-linecap")?.Value);
        }

        [Fact]
        public void CreateSvgElement_StrokeLinejoin() {
            var shape = new Rectangle(new Point(100, 200), new Point(150, 250));

            shape.SetStrokeLinejoin(Linejoin.Arcs);

            var result = shape.CreateSvgElement();

            Assert.Equal("arcs", result.Attribute("stroke-linejoin")?.Value);
        }
    }
}
