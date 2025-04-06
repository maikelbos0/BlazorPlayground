using NetTopologySuite.Geometries;
using NetTopologySuite.Geometries.Utilities;

namespace BlazorPlayground.Graphics.Geometries;

public class DrawableShapeGeometry {
    public Geometry GetGeometry(IEnumerable<DrawableShape> shapes) {
        var geometries = new List<Geometry>();

        foreach (var  shape in shapes) {
            
        }

        return GeometryCombiner.Combine(geometries);
    }
}
