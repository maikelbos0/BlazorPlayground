using System;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.Linq;
using System.Xml.Linq;

namespace BlazorPlayground.Graphics;

public static class SvgFileParser {
    public static bool TryParse(string contents, [NotNullWhen(true)] out Canvas? canvas) {
        try {
            var graphicsElement = XElement.Parse(contents);

            if (graphicsElement.Name.ToString().Equals("svg", StringComparison.OrdinalIgnoreCase)) {

                canvas = new Canvas() {
                    Shapes = [.. graphicsElement.Elements().Select(Parse)],
                    Width = ParseDimension(graphicsElement.Attribute("width")?.Value, Canvas.MinimumWidth, Canvas.DefaultWidth),
                    Height = ParseDimension(graphicsElement.Attribute("height")?.Value, Canvas.MinimumHeight, Canvas.DefaultHeight)
                };
                return true;
            }
        }
        catch { }

        canvas = null;
        return false;
    }

    internal static Shape Parse(XElement element) {
        if (TryCreateShape(element, out var shape) && TrySetAnchors(shape, element)) {
            (shape as IShapeWithOpacity)?.Opacity = ParseOpacity(element.Attribute("opacity")?.Value);
            (shape as IShapeWithFill)?.Fill = ParsePaintServer(element.Attribute("fill")?.Value);
            (shape as IShapeWithFill)?.FillOpacity = ParseOpacity(element.Attribute("fill-opacity")?.Value);
            (shape as IShapeWithStroke)?.Stroke = ParsePaintServer(element.Attribute("stroke")?.Value);
            (shape as IShapeWithStroke)?.StrokeWidth = ParseDimension(element.Attribute("stroke-width")?.Value, DrawSettings.MinimumStrokeWidth, DrawSettings.DefaultStrokeWidth);
            (shape as IShapeWithStroke)?.StrokeOpacity = ParseOpacity(element.Attribute("stroke-opacity")?.Value);
            (shape as IShapeWithStrokeLinecap)?.StrokeLinecap = ParseEnum(element.Attribute("stroke-linecap")?.Value, DrawSettings.DefaultStrokeLinecap);
            (shape as IShapeWithStrokeLinejoin)?.StrokeLinejoin = ParseEnum(element.Attribute("stroke-linejoin")?.Value, DrawSettings.DefaultStrokeLinejoin);
            (shape as IShapeWithSides)?.Sides = ParseDimension(element.Attribute("data-shape-sides")?.Value, DrawSettings.MinimumSides, DrawSettings.DefaultSides);

            return shape;
        }

        return new RawShape(element);
    }

    private static bool TryCreateShape(XElement element, [NotNullWhen(true)] out Shape? shape) {
        var shapeTypeName = element.Attribute("data-shape-type")?.Value;
        var shapeType = Type.GetType($"BlazorPlayground.Graphics.{shapeTypeName}");

        if (shapeType == null) {
            shape = null;
            return false;
        }

        shape = (Shape?)Activator.CreateInstance(shapeType, true);
        return shape != null;
    }

    private static bool TrySetAnchors(Shape shape, XElement element) {
        int i;

        for (i = 0; i < shape.Anchors.Count; i++) {
            if (!TryParseAnchor(element.Attribute($"data-shape-anchor-{i}")?.Value, out var point)) {
                return false;
            }

            shape.Anchors[i].Set(shape, point);
        }

        if (shape is IExtensibleShape extensibleShape) {
            while (TryParseAnchor(element.Attribute($"data-shape-anchor-{i++}")?.Value, out var point)) {
                extensibleShape.AddPoint(point);
            }
        }

        return true;
    }

    private static bool TryParseAnchor(string? anchorText, [NotNullWhen(true)] out Point? point) {
        var coordinates = anchorText?.Split(',');

        if (coordinates == null
                || coordinates.Length != 2
                || !double.TryParse(coordinates[0], NumberStyles.Any, CultureInfo.InvariantCulture, out var x)
                || !double.TryParse(coordinates[1], NumberStyles.Any, CultureInfo.InvariantCulture, out var y)) {

            point = null;
            return false;
        }

        point = new Point(x, y);
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
        }

        return defaultValue;
    }
}
