using NetTopologySuite.Geometries;
using NetTopologySuite.Geometries.Utilities;

namespace BlazorPlayground.Graphics.Geometries;

public class DrawableShapeGeometryFactory {
    public Geometry GetGeometry(IEnumerable<DrawableShape> shapes) {
        var geometries = new List<Geometry>();

        foreach (var shape in shapes) {
            geometries.Add(shape switch {
                Rectangle rectangle => GetGeometry(rectangle),
                _ => throw new NotImplementedException()
            });
        }

        return GeometryCombiner.Combine(geometries);
    }

    private Geometry GetGeometry(Rectangle rectangle)
        => GeometryFactory.Default.CreatePolygon([
            new(rectangle.StartPoint.X, rectangle.StartPoint.Y),
            new(rectangle.StartPoint.X, rectangle.EndPoint.Y),
            new(rectangle.EndPoint.X, rectangle.EndPoint.Y),
            new(rectangle.EndPoint.X, rectangle.StartPoint.Y),
            new(rectangle.StartPoint.X, rectangle.StartPoint.Y)
        ]);
}
