using System.Globalization;
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
                    shapes.Add(Parse(element));
                }

                return new SvgFileParseResult(new Canvas() {
                    Shapes = shapes
                });
            }
            catch {
                return new SvgFileParseResult("The provided file is not a valid svg file.");
            }
        }

        internal static Shape Parse(XElement element) {
            var shapeTypeName = element.Attribute("data-shape-type")?.Value;

            if (shapeTypeName == null) {
                return new RawShape(element);
            }

            var shapeType = Type.GetType($"BlazorPlayground.Graphics.{shapeTypeName}");

            if (shapeType == null) {
                return new RawShape(element);
            }

            var shape = (Shape?)Activator.CreateInstance(shapeType, true);

            if (shape == null) {
                return new RawShape(element);
            }

            for (var i = 0; i < shape.Anchors.Count; i++) {
                var coordinates = element.Attribute($"data-shape-anchor-{i}")?.Value?.Split(',');

                if (coordinates == null
                        || coordinates.Length != 2 
                        || !double.TryParse(coordinates[0], NumberStyles.Any, CultureInfo.InvariantCulture, out var x) 
                        || !double.TryParse(coordinates[1], NumberStyles.Any, CultureInfo.InvariantCulture, out var y)) {
                    return new RawShape(element);
                }

                shape.Anchors[i].Set(shape, new Point(x, y));
            }

            return shape;
        }
    }
}
