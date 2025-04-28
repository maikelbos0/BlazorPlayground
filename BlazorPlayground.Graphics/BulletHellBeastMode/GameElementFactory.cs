using BlazorPlayground.BulletHellBeastMode;
using NetTopologySuite.Geometries;

namespace BlazorPlayground.Graphics.BulletHellBeastMode;

public class GameElementFactory {
    public const double DefaultOpacity = 1.0;
    public const int DefaultStrokeWidth = 0;

    public readonly static BlazorPlayground.BulletHellBeastMode.Color DefaultColor = new(0, 0, 0, 0);

    private readonly GeometryFactory geometryFactory;

    public GameElementFactory(GeometryFactory geometryFactory) {
        this.geometryFactory = geometryFactory;
    }

    public GameElement GetGameElement(IEnumerable<DrawableShape> shapes) {
        var origin = GetOrigin(shapes);

        return new GameElement() {
            Position = new(0, 0),
            Sections = shapes.Select(shape => new GameElementSection() {
                Geometry = shape.GetGeometry(geometryFactory, origin),
                FillColor = GetFillColor(shape),
                StrokeColor = GetStrokeColor(shape),
                StrokeWidth = GetStrokeWidth(shape),
                Opacity = GetOpacity(shape)
            }).ToList()
        };
    }

    public static Point GetOrigin(IEnumerable<DrawableShape> shapes) {
        var boundingBoxes = shapes.Select(shape => shape.GetBoundingBox()).ToList();

        if (boundingBoxes.Count > 0) {
            return new(
                (boundingBoxes.Max(boundingBox => boundingBox.MaxX) + boundingBoxes.Min(boundingBox => boundingBox.MinX)) / 2,
                (boundingBoxes.Max(boundingBox => boundingBox.MaxY) + boundingBoxes.Min(boundingBox => boundingBox.MinY)) / 2
            );
        }
        else {
            return new(0, 0);
        }
    }

    private static BlazorPlayground.BulletHellBeastMode.Color GetFillColor(DrawableShape shape) {
        if (shape is IShapeWithFill shapeWithFill && shapeWithFill.GetFill() is Color fillColor) {
            var fillOpacity = shapeWithFill.GetFillOpacity() / 100.0;

            return new BlazorPlayground.BulletHellBeastMode.Color(fillColor.Red, fillColor.Green, fillColor.Blue, fillColor.Alpha * fillOpacity);
        }

        return DefaultColor;
    }

    private static BlazorPlayground.BulletHellBeastMode.Color GetStrokeColor(DrawableShape shape) {
        if (shape is IShapeWithStroke shapeWithStroke && shapeWithStroke.GetStroke() is Color strokeColor) {
            var strokeOpacity = shapeWithStroke.GetStrokeOpacity() / 100.0;

            return new BlazorPlayground.BulletHellBeastMode.Color(strokeColor.Red, strokeColor.Green, strokeColor.Blue, strokeColor.Alpha * strokeOpacity);
        }

        return DefaultColor;
    }

    private static int GetStrokeWidth(DrawableShape shape) {
        if (shape is IShapeWithStroke shapeWithStroke) {
            return shapeWithStroke.GetStrokeWidth();
        }

        return DefaultStrokeWidth;
    }

    private static double GetOpacity(DrawableShape shape) {
        if (shape is IShapeWithOpacity shapeWithOpacity) {
            return shapeWithOpacity.GetOpacity() / 100.0;
        }

        return DefaultOpacity;
    }
}
