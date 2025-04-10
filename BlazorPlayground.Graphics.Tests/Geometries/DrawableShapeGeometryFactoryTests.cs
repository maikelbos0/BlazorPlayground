using BlazorPlayground.Graphics.Geometries;
using NetTopologySuite.Geometries;
using System.Linq;
using Xunit;

namespace BlazorPlayground.Graphics.Tests.Geometries;

public class DrawableShapeGeometryFactoryTests {
    [Fact]
    public void GetGeometryFromEmptyEnumerable() {
        var subject = new DrawableShapeGeometryFactory();

        var result = subject.GetGeometry(Enumerable.Empty<DrawableShape>());

        Assert.Null(result);
    }

    [Fact]
    public void GetGeometryFromRectangle() {
        var subject = new DrawableShapeGeometryFactory();

        var result = subject.GetGeometry([new Rectangle(new(-10, -10), new(50, -30))]);

        Assert.NotNull(result);
        Assert.Equal(GeometryFactory.Default.CreatePolygon([new(-10, -10), new(-10, -30), new(50, -30), new(50, -10), new(-10, -10)]), result);
    }
}
