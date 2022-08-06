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
        public void Parse_Raw_Shape() {
            var result = SvgFileParser.Parse("<svg><line x1=\"100\" y1=\"150\" x2=\"200\" y2=\"250\" stroke=\"black\" /></svg>");

            Assert.True(result.IsSuccess);

            var shape = Assert.IsType<RawShape>(Assert.Single(result.Canvas.Shapes));
            var element = shape.CreateSvgElement();

            Assert.Equal("line", element.Name);
            Assert.Equal("100", element.Attribute("x1")?.Value);
            Assert.Equal("150", element.Attribute("y1")?.Value);
            Assert.Equal("200", element.Attribute("x2")?.Value);
            Assert.Equal("250", element.Attribute("y2")?.Value);
        }
    }
}
