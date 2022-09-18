using System.Xml.Linq;
using Xunit;

namespace BlazorPlayground.Graphics.Tests {
    public class SvgFileParserTests {
        [Fact]
        public void Parse_Invalid_Xml() {
            var result = SvgFileParser.Parse("<>");

            Assert.False(result.IsSuccess);
            Assert.Equal("The provided file is not a valid svg file.", result.ErrorMessage);
        }

        [Fact]
        public void Parse_Invalid_Svg() {
            var result = SvgFileParser.Parse("<test></test>");

            Assert.False(result.IsSuccess);
            Assert.Equal("The provided file is not a valid svg file.", result.ErrorMessage);
        }

        [Fact]
        public void Parse_Valid_Svg() {
            var result = SvgFileParser.Parse("<svg><g><line x1=\"100\" y1=\"150\" x2=\"200\" y2=\"250\" stroke=\"black\" /></g><line x1=\"200\" y1=\"250\" x2=\"300\" y2=\"350\" stroke=\"black\" /></svg>");

            Assert.True(result.IsSuccess);

            Assert.Equal(2, result.Canvas.Shapes.Count);
        }

        [Fact]
        public void Parse_Null_Canvas_Height() {
            var result = SvgFileParser.Parse("<svg></svg>");

            Assert.Equal(Canvas.DefaultHeight, result.Canvas.Height);
        }

        [Theory]
        [InlineData("-1", Canvas.DefaultHeight)]
        [InlineData("0", Canvas.DefaultHeight)]
        [InlineData("foo", Canvas.DefaultHeight)]
        [InlineData("600", 600)]
        public void Parse_Canvas_Height(string height, int expectedHeight) {
            var result = SvgFileParser.Parse($"<svg width=\"500\" height=\"{height}\"></svg>");

            Assert.Equal(expectedHeight, result.Canvas.Height);
        }

        [Fact]
        public void Parse_Null_Canvas_Width() {
            var result = SvgFileParser.Parse("<svg></svg>");

            Assert.Equal(Canvas.DefaultWidth, result.Canvas.Width);
        }

        [Theory]
        [InlineData("-1", Canvas.DefaultWidth)]
        [InlineData("0", Canvas.DefaultWidth)]
        [InlineData("foo", Canvas.DefaultWidth)]
        [InlineData("600", 600)]
        public void Parse_Canvas_Width(string width, int expectedWidth) {
            var result = SvgFileParser.Parse($"<svg width=\"{width}\" height=\"500\"></svg>");

            Assert.Equal(expectedWidth, result.Canvas.Width);
        }

        [Fact]
        public void Parse_Raw_Shape() {
            var result = SvgFileParser.Parse(XElement.Parse("<line x1=\"100\" y1=\"150\" x2=\"200\" y2=\"250\" stroke=\"black\" />"));

            var shape = Assert.IsType<RawShape>(result);
            var element = shape.CreateSvgElement();

            Assert.Equal("line", element.Name);
            Assert.Equal("100", element.Attribute("x1")?.Value);
            Assert.Equal("150", element.Attribute("y1")?.Value);
            Assert.Equal("200", element.Attribute("x2")?.Value);
            Assert.Equal("250", element.Attribute("y2")?.Value);
        }

        [Fact]
        public void Parse_Incomplete_Shape() {
            var result = SvgFileParser.Parse(XElement.Parse("<line x1=\"100\" y1=\"150\" x2=\"200\" y2=\"250\" stroke=\"black\" data-shape-type=\"Line\" />"));

            var shape = Assert.IsType<RawShape>(result);
            var element = shape.CreateSvgElement();

            Assert.Equal("line", element.Name);
            Assert.Equal("100", element.Attribute("x1")?.Value);
            Assert.Equal("150", element.Attribute("y1")?.Value);
            Assert.Equal("200", element.Attribute("x2")?.Value);
            Assert.Equal("250", element.Attribute("y2")?.Value);
        }

        [Fact]
        public void Parse_Shape_With_Invalid_Coordinates() {
            var result = SvgFileParser.Parse(XElement.Parse("<line x1=\"100\" y1=\"150\" x2=\"200\" y2=\"250\" stroke=\"black\" data-shape-type=\"Line\" data-shape-anchor-0=\"100,150\" data-shape-anchor-1=\"200,a\" />"));

            var shape = Assert.IsType<RawShape>(result);
            var element = shape.CreateSvgElement();

            Assert.Equal("line", element.Name);
            Assert.Equal("100", element.Attribute("x1")?.Value);
            Assert.Equal("150", element.Attribute("y1")?.Value);
            Assert.Equal("200", element.Attribute("x2")?.Value);
            Assert.Equal("250", element.Attribute("y2")?.Value);
        }

        [Fact]
        public void Parse_Line() {
            var result = SvgFileParser.Parse(XElement.Parse("<line stroke=\"#000000\" stroke-width=\"1\" stroke-linecap=\"butt\" x1=\"50\" y1=\"50\" x2=\"150\" y2=\"100\" data-shape-type=\"Line\" data-shape-anchor-0=\"50,50\" data-shape-anchor-1=\"150,100\"/>"));

            var line = Assert.IsType<Line>(result);

            PointAssert.Equal(new Point(50, 50), line.StartPoint);
            PointAssert.Equal(new Point(150, 100), line.EndPoint);
        }

        [Fact]
        public void Parse_Rectangle() {
            var result = SvgFileParser.Parse(XElement.Parse("<rect fill=\"none\" stroke=\"#000000\" stroke-width=\"1\" stroke-linejoin=\"miter\" x=\"250\" y=\"150\" width=\"100\" height=\"150\" data-shape-type=\"Rectangle\" data-shape-anchor-0=\"250,150\" data-shape-anchor-1=\"350,300\"/>"));

            var rectangle = Assert.IsType<Rectangle>(result);

            PointAssert.Equal(new Point(250, 150), rectangle.StartPoint);
            PointAssert.Equal(new Point(350, 300), rectangle.EndPoint);
        }

        [Fact]
        public void Parse_Circle() {
            var result = SvgFileParser.Parse(XElement.Parse("<circle fill=\"none\" stroke=\"#000000\" stroke-width=\"1\" cx=\"100\" cy=\"200\" r=\"50\" data-shape-type=\"Circle\" data-shape-anchor-0=\"100,200\" data-shape-anchor-1=\"150,212.5\"/>"));

            var circle = Assert.IsType<Circle>(result);

            PointAssert.Equal(new Point(100, 200), circle.CenterPoint);
            PointAssert.Equal(new Point(150, 212.5), circle.RadiusPoint);
        }

        [Fact]
        public void Parse_Ellipse() {
            var result = SvgFileParser.Parse(XElement.Parse("<ellipse fill=\"none\" stroke=\"#000000\" stroke-width=\"1\" cx=\"250\" cy=\"150\" rx=\"100\" ry=\"50\" data-shape-type=\"Ellipse\" data-shape-anchor-0=\"250,150\" data-shape-anchor-1=\"350,200\"/>"));

            var ellipse = Assert.IsType<Ellipse>(result);

            PointAssert.Equal(new Point(250, 150), ellipse.CenterPoint);
            PointAssert.Equal(new Point(350, 200), ellipse.RadiusPoint);
        }

        [Fact]
        public void Parse_RegularPolygon() {
            var result = SvgFileParser.Parse(XElement.Parse("<polygon fill=\"none\" stroke=\"#000000\" stroke-width=\"1\" stroke-linejoin=\"miter\" points=\"600,300 438.39745962155615,193.30127018922198 611.6025403784438,106.69872981077803\" data-shape-type=\"RegularPolygon\" data-shape-anchor-0=\"550,200\" data-shape-anchor-1=\"600,300\" data-shape-sides=\"5\"/>"));

            var rectangle = Assert.IsType<RegularPolygon>(result);

            PointAssert.Equal(new Point(550, 200), rectangle.CenterPoint);
            PointAssert.Equal(new Point(600, 300), rectangle.RadiusPoint);
        }

        [Fact]
        public void Parse_QuadraticBezier() {
            var result = SvgFileParser.Parse(XElement.Parse("<path fill=\"none\" stroke=\"#000000\" stroke-width=\"1\" stroke-linecap=\"butt\" d=\"M 50 50 Q 200 100, 100 350\" data-shape-type=\"QuadraticBezier\" data-shape-anchor-0=\"50,50\" data-shape-anchor-1=\"200,100\" data-shape-anchor-2=\"100,350\"/>"));

            var rectangle = Assert.IsType<QuadraticBezier>(result);

            PointAssert.Equal(new Point(50, 50), rectangle.StartPoint);
            PointAssert.Equal(new Point(200, 100), rectangle.ControlPoint);
            PointAssert.Equal(new Point(100, 350), rectangle.EndPoint);
        }

        [Fact]
        public void Parse_CubicBezier() {
            var result = SvgFileParser.Parse(XElement.Parse("<path fill=\"none\" stroke=\"#000000\" stroke-width=\"1\" stroke-linecap=\"butt\" d=\"M 200 50 C 200 200, 300 200, 250 350\" data-shape-type=\"CubicBezier\" data-shape-anchor-0=\"200,50\" data-shape-anchor-1=\"200,200\" data-shape-anchor-2=\"300,200\" data-shape-anchor-3=\"250,350\"/>"));

            var rectangle = Assert.IsType<CubicBezier>(result);

            PointAssert.Equal(new Point(200, 50), rectangle.StartPoint);
            PointAssert.Equal(new Point(200, 200), rectangle.ControlPoint1);
            PointAssert.Equal(new Point(300, 200), rectangle.ControlPoint2);
            PointAssert.Equal(new Point(250, 350), rectangle.EndPoint);
        }

        [Theory]
        [InlineData("0", DrawSettings.MinimumOpacity)]
        [InlineData("-0.01", DrawSettings.DefaultOpacity)]
        [InlineData("foo", DrawSettings.DefaultOpacity)]
        [InlineData("1", DrawSettings.MaximumOpacity)]
        [InlineData("1.01", DrawSettings.DefaultOpacity)]
        [InlineData("0.45", 45)]
        public void Parse_Opacity(string opacity, int expectedOpacity) {
            var result = SvgFileParser.Parse(XElement.Parse($"<ellipse opacity=\"{opacity}\" fill=\"none\" stroke=\"#ffff00\" stroke-width=\"1\" cx=\"250\" cy=\"150\" rx=\"100\" ry=\"50\" data-shape-type=\"Ellipse\" data-shape-anchor-0=\"250,150\" data-shape-anchor-1=\"350,200\"/>"));

            Assert.Equal(expectedOpacity, Assert.IsAssignableFrom<IShapeWithOpacity>(result).GetOpacity());
        }

        [Fact]
        public void Parse_Fill_None() {
            var result = SvgFileParser.Parse(XElement.Parse("<ellipse fill=\"none\" stroke=\"#000000\" stroke-width=\"1\" cx=\"250\" cy=\"150\" rx=\"100\" ry=\"50\" data-shape-type=\"Ellipse\" data-shape-anchor-0=\"250,150\" data-shape-anchor-1=\"350,200\"/>"));

            Assert.Equal(PaintServer.None, Assert.IsAssignableFrom<IShapeWithFill>(result).GetFill());
        }

        [Fact]
        public void Parse_Fill_Color() {
            var result = SvgFileParser.Parse(XElement.Parse("<ellipse fill=\"#ffff00\" stroke=\"#000000\" stroke-width=\"1\" cx=\"250\" cy=\"150\" rx=\"100\" ry=\"50\" data-shape-type=\"Ellipse\" data-shape-anchor-0=\"250,150\" data-shape-anchor-1=\"350,200\"/>"));

            PaintServerAssert.Equal(new Color(255, 255, 0, 1), Assert.IsAssignableFrom<IShapeWithFill>(result).GetFill());
        }

        [Fact]
        public void Parse_Fill_Default() {
            var result = SvgFileParser.Parse(XElement.Parse("<ellipse stroke=\"#000000\" stroke-width=\"1\" cx=\"250\" cy=\"150\" rx=\"100\" ry=\"50\" data-shape-type=\"Ellipse\" data-shape-anchor-0=\"250,150\" data-shape-anchor-1=\"350,200\"/>"));

            Assert.Equal(PaintServer.None, Assert.IsAssignableFrom<IShapeWithFill>(result).GetFill());
        }

        [Theory]
        [InlineData("0", DrawSettings.MinimumOpacity)]
        [InlineData("-0.01", DrawSettings.DefaultOpacity)]
        [InlineData("foo", DrawSettings.DefaultOpacity)]
        [InlineData("1", DrawSettings.MaximumOpacity)]
        [InlineData("1.01", DrawSettings.DefaultOpacity)]
        [InlineData("0.45", 45)]
        public void Parse_FillOpacity(string fillOpacity, int expectedFillOpacity) {
            var result = SvgFileParser.Parse(XElement.Parse($"<ellipse opacity=\"1\" fill=\"#ffff00\" fill-opacity=\"{fillOpacity}\" stroke=\"#ffff00\" stroke-width=\"1\" cx=\"250\" cy=\"150\" rx=\"100\" ry=\"50\" data-shape-type=\"Ellipse\" data-shape-anchor-0=\"250,150\" data-shape-anchor-1=\"350,200\"/>"));

            Assert.Equal(expectedFillOpacity, Assert.IsAssignableFrom<IShapeWithFill>(result).GetFillOpacity());
        }

        [Fact]
        public void Parse_Stroke_None() {
            var result = SvgFileParser.Parse(XElement.Parse("<ellipse fill=\"none\" stroke=\"none\" stroke-width=\"1\" cx=\"250\" cy=\"150\" rx=\"100\" ry=\"50\" data-shape-type=\"Ellipse\" data-shape-anchor-0=\"250,150\" data-shape-anchor-1=\"350,200\"/>"));

            Assert.Equal(PaintServer.None, Assert.IsAssignableFrom<IShapeWithStroke>(result).GetStroke());
        }

        [Fact]
        public void Parse_Stroke_Color() {
            var result = SvgFileParser.Parse(XElement.Parse("<ellipse fill=\"none\" stroke=\"#ffff00\" stroke-width=\"1\" cx=\"250\" cy=\"150\" rx=\"100\" ry=\"50\" data-shape-type=\"Ellipse\" data-shape-anchor-0=\"250,150\" data-shape-anchor-1=\"350,200\"/>"));

            PaintServerAssert.Equal(new Color(255, 255, 0, 1), Assert.IsAssignableFrom<IShapeWithStroke>(result).GetStroke());
        }

        [Fact]
        public void Parse_Stroke_Default() {
            var result = SvgFileParser.Parse(XElement.Parse("<ellipse stroke=\"none\" stroke-width=\"1\" cx=\"250\" cy=\"150\" rx=\"100\" ry=\"50\" data-shape-type=\"Ellipse\" data-shape-anchor-0=\"250,150\" data-shape-anchor-1=\"350,200\"/>"));

            Assert.Equal(PaintServer.None, Assert.IsAssignableFrom<IShapeWithStroke>(result).GetStroke());
        }

        [Fact]
        public void Parse_Null_StrokeWidth() {
            var result = SvgFileParser.Parse(XElement.Parse("<ellipse fill=\"none\" stroke=\"none\" cx=\"250\" cy=\"150\" rx=\"100\" ry=\"50\" data-shape-type=\"Ellipse\" data-shape-anchor-0=\"250,150\" data-shape-anchor-1=\"350,200\"/>"));

            Assert.Equal(DrawSettings.DefaultStrokeWidth, Assert.IsAssignableFrom<IShapeWithStroke>(result).GetStrokeWidth());
        }

        [Theory]
        [InlineData("0", DrawSettings.DefaultStrokeWidth)]
        [InlineData("-1", DrawSettings.DefaultStrokeWidth)]
        [InlineData("foo", DrawSettings.DefaultStrokeWidth)]
        [InlineData("5", 5)]
        public void Parse_StrokeWidth(string strokeWidth, int expectedStrokeWidth) {
            var result = SvgFileParser.Parse(XElement.Parse($"<ellipse fill=\"none\" stroke=\"#ffff00\" stroke-width=\"{strokeWidth}\" cx=\"250\" cy=\"150\" rx=\"100\" ry=\"50\" data-shape-type=\"Ellipse\" data-shape-anchor-0=\"250,150\" data-shape-anchor-1=\"350,200\"/>"));
            
            Assert.Equal(expectedStrokeWidth, Assert.IsAssignableFrom<IShapeWithStroke>(result).GetStrokeWidth());
        }

        [Theory]
        [InlineData("0", DrawSettings.MinimumOpacity)]
        [InlineData("-0.01", DrawSettings.DefaultOpacity)]
        [InlineData("foo", DrawSettings.DefaultOpacity)]
        [InlineData("1", DrawSettings.MaximumOpacity)]
        [InlineData("1.01", DrawSettings.DefaultOpacity)]
        [InlineData("0.45", 45)]
        public void Parse_StrokeOpacity(string strokeOpacity, int expectedStrokeOpacity) {
            var result = SvgFileParser.Parse(XElement.Parse($"<ellipse opacity=\"1\" fill=\"#ffff00\" stroke=\"#ffff00\" stroke-width=\"1\" stroke-opacity=\"{strokeOpacity}\" cx=\"250\" cy=\"150\" rx=\"100\" ry=\"50\" data-shape-type=\"Ellipse\" data-shape-anchor-0=\"250,150\" data-shape-anchor-1=\"350,200\"/>"));

            Assert.Equal(expectedStrokeOpacity, Assert.IsAssignableFrom<IShapeWithStroke>(result).GetStrokeOpacity());
        }

        [Fact]
        public void Parse_Null_StrokeLinecap() {
            var result = SvgFileParser.Parse(XElement.Parse("<line stroke=\"#000000\" stroke-width=\"1\" x1=\"50\" y1=\"50\" x2=\"150\" y2=\"100\" data-shape-type=\"Line\" data-shape-anchor-0=\"50,50\" data-shape-anchor-1=\"150,100\"/>"));

            Assert.Equal(DrawSettings.DefaultStrokeLinecap, Assert.IsAssignableFrom<IShapeWithStrokeLinecap>(result).GetStrokeLinecap());
        }

        [Theory]
        [InlineData("", DrawSettings.DefaultStrokeLinecap)]
        [InlineData("foo", DrawSettings.DefaultStrokeLinecap)]
        [InlineData("butt", Linecap.Butt)]
        [InlineData("square", Linecap.Square)]
        [InlineData("Square", Linecap.Square)]
        [InlineData("round", Linecap.Round)]
        [InlineData("Round", Linecap.Round)]
        public void Parse_StrokeLinecap(string strokeLinecap, Linecap expectedStrokeLinecap) {
            var result = SvgFileParser.Parse(XElement.Parse($"<line stroke=\"#000000\" stroke-width=\"1\" stroke-linecap=\"{strokeLinecap}\" x1=\"50\" y1=\"50\" x2=\"150\" y2=\"100\" data-shape-type=\"Line\" data-shape-anchor-0=\"50,50\" data-shape-anchor-1=\"150,100\"/>"));

            Assert.Equal(expectedStrokeLinecap, Assert.IsAssignableFrom<IShapeWithStrokeLinecap>(result).GetStrokeLinecap());
        }

        [Fact]
        public void Parse_Null_StrokeLinejoin() {
            var result = SvgFileParser.Parse(XElement.Parse("<rect fill=\"none\" stroke=\"#000000\" stroke-width=\"1\" x=\"250\" y=\"150\" width=\"100\" height=\"150\" data-shape-type=\"Rectangle\" data-shape-anchor-0=\"250,150\" data-shape-anchor-1=\"350,300\"/>"));

            Assert.Equal(DrawSettings.DefaultStrokeLinejoin, Assert.IsAssignableFrom<IShapeWithStrokeLinejoin>(result).GetStrokeLinejoin());
        }
        // TODO add stroke/fill opacity
        [Theory]
        [InlineData("", DrawSettings.DefaultStrokeLinejoin)]
        [InlineData("foo", DrawSettings.DefaultStrokeLinejoin)]
        [InlineData("miter", Linejoin.Miter)]
        [InlineData("arcs", Linejoin.Arcs)]
        [InlineData("Arcs", Linejoin.Arcs)]
        [InlineData("bevel", Linejoin.Bevel)]
        [InlineData("Bevel", Linejoin.Bevel)]
        [InlineData("round", Linejoin.Round)]
        [InlineData("Round", Linejoin.Round)]
        public void Parse_StrokeLinejoin(string strokeLinejoin, Linejoin expectedStrokeLinejoin) {
            var result = SvgFileParser.Parse(XElement.Parse($"<rect fill=\"none\" stroke=\"#000000\" stroke-width=\"1\" stroke-linejoin=\"{strokeLinejoin}\" x=\"250\" y=\"150\" width=\"100\" height=\"150\" data-shape-type=\"Rectangle\" data-shape-anchor-0=\"250,150\" data-shape-anchor-1=\"350,300\"/>"));

            Assert.Equal(expectedStrokeLinejoin, Assert.IsAssignableFrom<IShapeWithStrokeLinejoin>(result).GetStrokeLinejoin());
        }

        [Fact]
        public void Parse_Null_Sides() {
            var result = SvgFileParser.Parse(XElement.Parse("<polygon fill=\"none\" stroke=\"#000000\" stroke-width=\"1\" stroke-linejoin=\"miter\" points=\"600,300 438.39745962155615,193.30127018922198 611.6025403784438,106.69872981077803\" data-shape-type=\"RegularPolygon\" data-shape-anchor-0=\"550,200\" data-shape-anchor-1=\"600,300\"/>"));

            Assert.Equal(DrawSettings.DefaultSides, Assert.IsAssignableFrom<IShapeWithSides>(result).GetSides());
        }

        [Theory]
        [InlineData("2", DrawSettings.DefaultSides)]
        [InlineData("-1", DrawSettings.DefaultSides)]
        [InlineData("foo", DrawSettings.DefaultSides)]
        [InlineData("5", 5)]
        public void Parse_Sides(string sides, int expectedSides) {
            var result = SvgFileParser.Parse(XElement.Parse($"<polygon fill=\"none\" stroke=\"#000000\" stroke-width=\"1\" stroke-linejoin=\"miter\" points=\"600,300 438.39745962155615,193.30127018922198 611.6025403784438,106.69872981077803\" data-shape-type=\"RegularPolygon\" data-shape-anchor-0=\"550,200\" data-shape-anchor-1=\"600,300\" data-shape-sides=\"{sides}\"/>"));

            Assert.Equal(expectedSides, Assert.IsAssignableFrom<IShapeWithSides>(result).GetSides());
        }
    }
}
