using Xunit;

namespace BlazorPlayground.Graphics.Tests {
    public class DrawableShapeTests {
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
    }
}
