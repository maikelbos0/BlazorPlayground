using NetTopologySuite.Geometries;
using NetTopologySuite.Geometries.Utilities;

namespace BlazorPlayground.Graphics.Geometries;

public class DrawableShapeGeometryFactory {
    private readonly GeometryFactory geometryFactory;

    public DrawableShapeGeometryFactory(GeometryFactory geometryFactory) {
        this.geometryFactory = geometryFactory;
    }

    public Geometry GetGeometry(IEnumerable<DrawableShape> shapes) {
        var geometries = new List<Geometry>();

        foreach (var shape in shapes) {
            geometries.Add(shape switch {
                Rectangle rectangle => GetGeometry(rectangle),
                RegularPolygon regularPolygon => GetGeometry(regularPolygon),
                _ => throw new NotImplementedException()
            });
        }

        return GeometryCombiner.Combine(geometries);
    }

    private Geometry GetGeometry(Rectangle rectangle)
        => GeometryFactory.Default.CreatePolygon([
            GetCoordinate(rectangle.StartPoint.X, rectangle.StartPoint.Y),
            GetCoordinate(rectangle.StartPoint.X, rectangle.EndPoint.Y),
            GetCoordinate(rectangle.EndPoint.X, rectangle.EndPoint.Y),
            GetCoordinate(rectangle.EndPoint.X, rectangle.StartPoint.Y),
            GetCoordinate(rectangle.StartPoint.X, rectangle.StartPoint.Y)
        ]);

    private Geometry GetGeometry(RegularPolygon regularPolygon) {
        var coordinates = regularPolygon.GetPoints().Select(point => GetCoordinate(point.X, point.Y)).ToList();

        coordinates.Add(coordinates.First());

        return GeometryFactory.Default.CreatePolygon(coordinates.ToArray());
    }

    private Coordinate GetCoordinate(double x, double y)
        => new(geometryFactory.PrecisionModel.MakePrecise(x), geometryFactory.PrecisionModel.MakePrecise(y));
}
