using BlazorPlayground.BulletHellBeastMode;
using NetTopologySuite.Geometries;
using Coordinate = BlazorPlayground.BulletHellBeastMode.Vector<BlazorPlayground.BulletHellBeastMode.CoordinateType>;

namespace BlazorPlayground.Graphics.BulletHellBeastMode;

public class GameAssetFactory {
    public const double DefaultOpacity = 1.0;
    public const int DefaultStrokeWidth = 0;

    public readonly static string DefaultColor = "rgba(0, 0, 0, 0)";

    private readonly GeometryFactory geometryFactory;

    public GameAssetFactory(GeometryFactory geometryFactory) {
        this.geometryFactory = geometryFactory;
    }

    public GameAsset GetGameAsset(IEnumerable<DrawableShape> shapes) {
        var origin = GetOrigin(shapes);

        return new GameAsset(shapes.Select(shape => new GameAssetSection(
                shape.GetGeometry(geometryFactory, origin),
                GetFillColor(shape),
                GetStrokeColor(shape),
                GetStrokeWidth(shape),
                GetOpacity(shape)
            )).ToList());
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

    private static string GetFillColor(DrawableShape shape) {
        if (shape is IShapeWithFill shapeWithFill && shapeWithFill.Fill is Color fillColor) {
            var fillOpacity = shapeWithFill.FillOpacity / 100.0;

            return new Color(fillColor.Red, fillColor.Green, fillColor.Blue, fillColor.Alpha * fillOpacity).ToString();
        }

        return DefaultColor;
    }

    private static string GetStrokeColor(DrawableShape shape) {
        if (shape is IShapeWithStroke shapeWithStroke && shapeWithStroke.GetStroke() is Color strokeColor) {
            var strokeOpacity = shapeWithStroke.GetStrokeOpacity() / 100.0;

            return new Color(strokeColor.Red, strokeColor.Green, strokeColor.Blue, strokeColor.Alpha * strokeOpacity).ToString();
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

    public GameElementPath GetGameElementPath(Point origin, IEnumerable<DrawableShape> shapes) {
        var sections = shapes
            .Select(shape => shape.GetGeometry(geometryFactory, origin))
            .OfType<LineString>()
            .Select(lineString => lineString.Coordinates.Select(coordinate => new Coordinate(coordinate.X, coordinate.Y)).ToList())
            .ToList();
        var coordinates = new List<Coordinate>();

        if (sections.Count > 0) {
            coordinates.AddRange(sections[0]);
            sections.Remove(sections[0]);

            while (sections.Count != 0) {
                var next = sections
                    .SelectMany(section => SectionConnector.All.Select(connector => new {
                        Section = section,
                        Connector = connector
                    }))
                    .OrderBy(candidate => candidate.Connector.GetMagnitude(coordinates, candidate.Section))
                    .First();

                next.Connector.Add(coordinates, next.Section);
                sections.Remove(next.Section);
            }
        }

        return new GameElementPath(coordinates);
    }
}
