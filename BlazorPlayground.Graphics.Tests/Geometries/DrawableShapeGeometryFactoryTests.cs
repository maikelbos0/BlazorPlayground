using BlazorPlayground.Graphics.Geometries;
using NetTopologySuite.Geometries;
using System;
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

    [Fact]
    public void GetGeometryFromCircle() {
        var geometryFactory = new GeometryFactory(new PrecisionModel(1000));
        var subject = new DrawableShapeGeometryFactory(geometryFactory);
        var circle = new Circle(new(30, 50), new(50, 100));

        var result = subject.GetGeometry([circle]);

        Assert.NotNull(result);
        Assert.Equal(GeometryFactory.Default.CreatePolygon([new(83.852, 50), new(83.557, 55.629), new(82.675, 61.196), new(81.216, 66.641), new(79.196, 71.903), new(76.637, 76.926), new(73.567, 81.653), new(70.02, 86.034), new(66.034, 90.02), new(61.653, 93.567), new(56.926, 96.637), new(51.903, 99.196), new(46.641, 101.216), new(41.196, 102.675), new(35.629, 103.557), new(30, 103.852), new(24.371, 103.557), new(18.804, 102.675), new(13.359, 101.216), new(8.097, 99.196), new(3.074, 96.637), new(-1.653, 93.567), new(-6.034, 90.02), new(-10.02, 86.034), new(-13.567, 81.653), new(-16.637, 76.926), new(-19.196, 71.903), new(-21.216, 66.641), new(-22.675, 61.196), new(-23.557, 55.629), new(-23.852, 50), new(-23.557, 44.371), new(-22.675, 38.804), new(-21.216, 33.359), new(-19.196, 28.097), new(-16.637, 23.074), new(-13.567, 18.347), new(-10.02, 13.966), new(-6.034, 9.98), new(-1.653, 6.433), new(3.074, 3.363), new(8.097, 0.804), new(13.359, -1.216), new(18.804, -2.675), new(24.371, -3.557), new(30, -3.852), new(35.629, -3.557), new(41.196, -2.675), new(46.641, -1.216), new(51.903, 0.804), new(56.926, 3.363), new(61.653, 6.433), new(66.034, 9.98), new(70.02, 13.966), new(73.567, 18.347), new(76.637, 23.074), new(79.196, 28.097), new(81.216, 33.359), new(82.675, 38.804), new(83.557, 44.371), new(83.852, 50)]), result);
    }
}
