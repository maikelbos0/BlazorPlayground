using System.Globalization;
using System.Xml.Linq;

namespace BlazorPlayground.Graphics {
    public static class SvgFileParser {
        public static SvgFileParseResult Parse(string contents) {
            try {
                var graphicsElement = XElement.Parse(contents);

                if (graphicsElement.Name.ToString().ToLower() != "svg") {
                    return new SvgFileParseResult("The provided file is not a valid svg file.");
                }

                return new SvgFileParseResult(new Canvas() {
                    Shapes = graphicsElement.Elements().Select(Parse).ToList(),
                    Width = ParseDimension(graphicsElement.Attribute("width")?.Value, Canvas.MinimumWidth, Canvas.DefaultWidth),
                    Height = ParseDimension(graphicsElement.Attribute("height")?.Value, Canvas.MinimumHeight, Canvas.DefaultHeight)
                });
            }
            catch {
                return new SvgFileParseResult("The provided file is not a valid svg file.");
            }
        }

        internal static Shape Parse(XElement element) {
            var shape = CreateShape(element);

            if (shape != null && SetAnchors(shape, element)) {
                (shape as IShapeWithOpacity)?.SetOpacity(ParseOpacity(element.Attribute("opacity")?.Value));
                (shape as IShapeWithFill)?.SetFill(ParsePaintServer(element.Attribute("fill")?.Value));
                (shape as IShapeWithFill)?.SetFillOpacity(ParseOpacity(element.Attribute("fill-opacity")?.Value));
                (shape as IShapeWithStroke)?.SetStroke(ParsePaintServer(element.Attribute("stroke")?.Value));
                (shape as IShapeWithStroke)?.SetStrokeWidth(ParseDimension(element.Attribute("stroke-width")?.Value, DrawSettings.MinimumStrokeWidth, DrawSettings.DefaultStrokeWidth));
                (shape as IShapeWithStroke)?.SetStrokeOpacity(ParseOpacity(element.Attribute("stroke-opacity")?.Value));
                (shape as IShapeWithStrokeLinecap)?.SetStrokeLinecap(ParseEnum(element.Attribute("stroke-linecap")?.Value, DrawSettings.DefaultStrokeLinecap));
                (shape as IShapeWithStrokeLinejoin)?.SetStrokeLinejoin(ParseEnum(element.Attribute("stroke-linejoin")?.Value, DrawSettings.DefaultStrokeLinejoin));
                (shape as IShapeWithSides)?.SetSides(ParseDimension(element.Attribute("data-shape-sides")?.Value, DrawSettings.MinimumSides, DrawSettings.DefaultSides));
                
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

        private static int ParseOpacity(string? opacityValue) {
            if (opacityValue != null && double.TryParse(opacityValue, NumberStyles.Any, CultureInfo.InvariantCulture, out var opacity) && opacity * 100 >= DrawSettings.MinimumOpacity && opacity * 100 <= DrawSettings.MaximumOpacity) {
                return (int)(opacity * 100);
            }

            return DrawSettings.DefaultOpacity;
        }

        private static IPaintServer ParsePaintServer(string? paintServer) {
            if (paintServer == PaintServer.None.ToString() || string.IsNullOrWhiteSpace(paintServer)) {
                return PaintServer.None;
            }
            else {
                return PaintManager.ParseColor(paintServer);
            }
        }

        private static int ParseDimension(string? dimensionValue, int minimumValue, int defaultValue) {
            if (dimensionValue != null && int.TryParse(dimensionValue, out var dimension) && dimension >= minimumValue) {
                return dimension;
            }

            return defaultValue;
        }

        private static TEnum ParseEnum<TEnum>(string? enumValue, TEnum defaultValue) where TEnum : struct, Enum {
            if (enumValue != null && Enum.TryParse<TEnum>(enumValue, true, out var value)) {
                return value;
            };

            return defaultValue;
        }
    }
}
