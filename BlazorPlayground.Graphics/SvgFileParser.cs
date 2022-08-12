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
            var shape = CreateShape(element);

            if (shape != null && SetAnchors(shape, element)) {
                shape.Fill = ParsePaintServer(element.Attribute("fill")?.Value);
                shape.Stroke = ParsePaintServer(element.Attribute("stroke")?.Value);
                shape.StrokeWidth = ParseDimension(element.Attribute("stroke-width")?.Value);
                shape.StrokeLinecap = ParseEnum(element.Attribute("stroke-linecap")?.Value, Linecap.Butt);
                shape.StrokeLinejoin = ParseEnum(element.Attribute("stroke-linejoin")?.Value, Linejoin.Miter);

                return shape;
            }

            return new RawShape(element);
        }

        private static Shape? CreateShape(XElement element) {
            var shapeTypeName = element.Attribute("data-shape-type")?.Value;
            var shapeType = Type.GetType($"BlazorPlayground.Graphics.{shapeTypeName}");

            if (shapeType == null) {
                return null;
            }

            return (Shape?)Activator.CreateInstance(shapeType, true);
        }

        private static bool SetAnchors(Shape shape, XElement element) {
            for (var i = 0; i < shape.Anchors.Count; i++) {
                var coordinates = element.Attribute($"data-shape-anchor-{i}")?.Value?.Split(',');

                if (coordinates == null
                        || coordinates.Length != 2
                        || !double.TryParse(coordinates[0], NumberStyles.Any, CultureInfo.InvariantCulture, out var x)
                        || !double.TryParse(coordinates[1], NumberStyles.Any, CultureInfo.InvariantCulture, out var y)) {
                    return false;
                }

                shape.Anchors[i].Set(shape, new Point(x, y));
            }

            return true;
        }

        private static IPaintServer ParsePaintServer(string? paintServer) {
            if (paintServer == PaintServer.None.ToString() || string.IsNullOrWhiteSpace(paintServer)) {
                return PaintServer.None;
            }
            else {
                return PaintManager.ParseColor(paintServer);
            }
        }

        private static int ParseDimension(string? dimensionValue) {
            if (dimensionValue != null && int.TryParse(dimensionValue, out var dimension) && dimension >= 1) {
                return dimension;
            }

            return 1;
        }

        private static TEnum ParseEnum<TEnum>(string? enumValue, TEnum defaultValue) where TEnum : struct, Enum {
            if (enumValue != null && Enum.TryParse<TEnum>(enumValue, true, out var value)) {
                return value;
            };

            return defaultValue;
        }
    }
}
