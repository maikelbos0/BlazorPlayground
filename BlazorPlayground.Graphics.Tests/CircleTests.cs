using NetTopologySuite.Geometries;
using System.Linq;
using Xunit;

namespace BlazorPlayground.Graphics.Tests;

public class CircleTests {
    [Fact]
    public void ElementName() {
        var circle = new Circle(new Point(100, 150), new Point(200, 250));

        Assert.Equal("circle", circle.ElementName);
    }

    [Fact]
    public void GetSnapPoints() {
        var circle = new Circle(new Point(100, 150), new Point(200, 200));

        var result = circle.GetSnapPoints();

        Assert.Equal(5, result.Count);
        PointAssert.Contains(result, new Point(100, 150));
        PointAssert.Contains(result, new Point(200, 200));
        PointAssert.Contains(result, new Point(0, 100));
        PointAssert.Contains(result, new Point(150, 50));
        PointAssert.Contains(result, new Point(50, 250));
    }

    [Fact]
    public void GetAttributes() {
        var circle = new Circle(new Point(100, 150), new Point(200, 250));

        var attributes = circle.GetAttributes().ToList();

        Assert.Equal(3, attributes.Count);
        Assert.Equal("100", Assert.Single(attributes, a => a.Key == "cx").Value);
        Assert.Equal("150", Assert.Single(attributes, a => a.Key == "cy").Value);
        Assert.Equal("141.4213562373095", Assert.Single(attributes, a => a.Key == "r").Value);
    }

    [Fact]
    public void Anchors_Get() {
        var circle = new Circle(new Point(100, 150), new Point(200, 250));

        var result = circle.Anchors;

        Assert.Equal(2, result.Count);
        PointAssert.Equal(new Point(100, 150), result[0].Get(circle));
        PointAssert.Equal(new Point(200, 250), result[1].Get(circle));
    }

    [Fact]
    public void Anchors_Set() {
        var circle = new Circle(new Point(100, 150), new Point(200, 250));

        var result = circle.Anchors;

        Assert.Equal(2, result.Count);
        result[0].Set(circle, new Point(110, 160));
        result[1].Set(circle, new Point(210, 260));
        PointAssert.Equal(new Point(110, 160), circle.CenterPoint);
        PointAssert.Equal(new Point(210, 260), circle.RadiusPoint);
    }

    [Fact]
    public void Clone() {
        var circle = new Circle(new Point(100, 150), new Point(200, 250));

        var result = circle.Clone();

        var resultCircle = Assert.IsType<Circle>(result);

        Assert.NotSame(circle, result);
        PointAssert.Equal(new Point(100, 150), resultCircle.CenterPoint);
        PointAssert.Equal(new Point(200, 250), resultCircle.RadiusPoint);
    }

    [Fact]
    public void GetGeometry() {
        var geometryFactory = new GeometryFactory(new PrecisionModel(10));
        var subject = new Circle(new(30, 50), new(50, 100));

        var result = subject.GetGeometry(geometryFactory, new(-100, -100));

        Assert.NotNull(result);
        Assert.Equal(geometryFactory.CreatePolygon([new(183.9, 150), new(183.6, 155.6), new(182.7, 161.2), new(181.2, 166.6), new(179.2, 171.9), new(176.6, 176.9), new(173.6, 181.7), new(170, 186), new(166, 190), new(161.7, 193.6), new(156.9, 196.6), new(151.9, 199.2), new(146.6, 201.2), new(141.2, 202.7), new(135.6, 203.6), new(130, 203.9), new(124.4, 203.6), new(118.8, 202.7), new(113.4, 201.2), new(108.1, 199.2), new(103.1, 196.6), new(98.3, 193.6), new(94, 190), new(90, 186), new(86.4, 181.7), new(83.4, 176.9), new(80.8, 171.9), new(78.8, 166.6), new(77.3, 161.2), new(76.4, 155.6), new(76.1, 150), new(76.4, 144.4), new(77.3, 138.8), new(78.8, 133.4), new(80.8, 128.1), new(83.4, 123.1), new(86.4, 118.3), new(90, 114), new(94, 110), new(98.3, 106.4), new(103.1, 103.4), new(108.1, 100.8), new(113.4, 98.8), new(118.8, 97.3), new(124.4, 96.4), new(130, 96.1), new(135.6, 96.4), new(141.2, 97.3), new(146.6, 98.8), new(151.9, 100.8), new(156.9, 103.4), new(161.7, 106.4), new(166.0, 110), new(170, 114), new(173.6, 118.3), new(176.6, 123.1), new(179.2, 128.1), new(181.2, 133.4), new(182.7, 138.8), new(183.6, 144.4), new(183.9, 150)]), result);
    }

    [Fact]
    public void GetBoundingBox() {
        var subject = new Circle(new(30, 50), new(50, 100));

        var result = subject.GetBoundingBox();

        BoundingBoxAssert.Equal(new(-23.9, 83.9, -3.9, 103.9), result);
    }
}
