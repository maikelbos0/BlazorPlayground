using System.Xml.Linq;

namespace BlazorPlayground.Graphics {
    public static class SvgFileParser {
        public static SvgFileParseResult Parse(string contents) {
            try {
                var graphicsElement = XElement.Parse(contents);
                var shapes = new List<Shape>();

                if (graphicsElement.Name.ToString().ToLower() != "svg") {
                    return new SvgFileParseResult("The provided file is not a valid svg file.");
                }

                foreach (var element in graphicsElement.Elements()) {
                    shapes.Add(new RawShape(element));
                }

                return new SvgFileParseResult(shapes);
            }
            catch {
                return new SvgFileParseResult("The provided file is not a valid svg file.");
            }
        }
    }
}
