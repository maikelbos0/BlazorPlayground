using BlazorPlayground.Graphics.Geometries;
using NetTopologySuite.Geometries;
using System.Linq;
using Xunit;

namespace BlazorPlayground.Graphics.Tests.Geometries;

public class DrawableShapeGeometryFactoryTests {
    [Fact]
    public void GetGeometryFromEmptyEnumerable() {
        var geometryFactory = new GeometryFactory(new PrecisionModel(1000));
        var subject = new DrawableShapeGeometryFactory(geometryFactory);

        var result = subject.GetGeometry(Enumerable.Empty<DrawableShape>());

        Assert.Null(result);
    }

    [Fact]
    public void GetGeometryFromRectangle() {
        var geometryFactory = new GeometryFactory(new PrecisionModel(1000));
        var subject = new DrawableShapeGeometryFactory(geometryFactory);

        var result = subject.GetGeometry([new Rectangle(new(-10, -10), new(50, -30))]);

        Assert.NotNull(result);
        Assert.Equal(GeometryFactory.Default.CreatePolygon([new(-10, -10), new(-10, -30), new(50, -30), new(50, -10), new(-10, -10)]), result);
    }

    [Fact]
    public void GetGeometryFromRegularPolygon() {
        var geometryFactory = new GeometryFactory(new PrecisionModel(1000));
        var subject = new DrawableShapeGeometryFactory(geometryFactory);
        var regularPolygon = new RegularPolygon(new(50, 50), new(50, 100));

        regularPolygon.SetSides(3);

        var result = subject.GetGeometry([regularPolygon]);

        Assert.NotNull(result);
        Assert.Equal(GeometryFactory.Default.CreatePolygon([new(50, 100), new(6.699, 25), new(93.301, 25), new(50, 100)]), result);
    }
}
