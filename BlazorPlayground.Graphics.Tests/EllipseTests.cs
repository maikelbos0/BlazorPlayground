using NetTopologySuite.Geometries;
using System.Linq;
using Xunit;

namespace BlazorPlayground.Graphics.Tests;

public class EllipseTests {
    [Fact]
    public void ElementName() {
        var ellipse = new Ellipse(new Point(100, 150), new Point(200, 250));

        Assert.Equal("ellipse", ellipse.ElementName);
    }

    [Fact]
    public void GetAttributes() {
        var ellipse = new Ellipse(new Point(100, 150), new Point(50, 75));

        var attributes = ellipse.GetAttributes().ToList();

        Assert.Equal(4, attributes.Count);
        Assert.Equal("100", Assert.Single(attributes, a => a.Key == "cx").Value);
        Assert.Equal("150", Assert.Single(attributes, a => a.Key == "cy").Value);
        Assert.Equal("50", Assert.Single(attributes, a => a.Key == "rx").Value);
        Assert.Equal("75", Assert.Single(attributes, a => a.Key == "ry").Value);
    }

    [Fact]
    public void GetSnapPoints() {
        var ellipse = new Ellipse(new Point(100, 150), new Point(200, 200));

        var result = ellipse.GetSnapPoints();

        Assert.Equal(5, result.Count);
        Assert.Contains(new Point(100, 150), result);
        Assert.Contains(new Point(200, 150), result);
        Assert.Contains(new Point(0, 150), result);
        Assert.Contains(new Point(100, 200), result);
        Assert.Contains(new Point(100, 100), result);
    }

    [Fact]
    public void Anchors_Get() {
        var ellipse = new Ellipse(new Point(100, 150), new Point(200, 250));

        var result = ellipse.Anchors;

        Assert.Equal(2, result.Count);
        Assert.Equal(new Point(100, 150), result[0].Get(ellipse));
        Assert.Equal(new Point(200, 250), result[1].Get(ellipse));
    }

    [Fact]
    public void Anchors_Set() {
        var ellipse = new Ellipse(new Point(100, 150), new Point(200, 250));

        var result = ellipse.Anchors;

        Assert.Equal(2, result.Count);
        result[0].Set(ellipse, new Point(110, 160));
        result[1].Set(ellipse, new Point(210, 260));
        Assert.Equal(new Point(110, 160), ellipse.CenterPoint);
        Assert.Equal(new Point(210, 260), ellipse.RadiusPoint);
    }

    [Fact]
    public void Clone() {
        var ellipse = new Ellipse(new Point(100, 150), new Point(200, 250));

        var result = ellipse.Clone();

        var resultEllipse = Assert.IsType<Ellipse>(result);

        Assert.NotSame(ellipse, resultEllipse);
        Assert.Equal(new Point(100, 150), resultEllipse.CenterPoint);
        Assert.Equal(new Point(200, 250), resultEllipse.RadiusPoint);
    }


    [Fact]
    public void GetGeometry() {
        var geometryFactory = new GeometryFactory(new PrecisionModel(10));
        var subject = new Ellipse(new(30, 50), new(50, 100));

        var result = subject.GetGeometry(geometryFactory, new(-100, -100));

        Assert.NotNull(result);
        Assert.Equal(geometryFactory.CreatePolygon([new(150, 150), new(149.9, 155.2), new(149.6, 160.4), new(149, 165.5), new(148.3, 170.3), new(147.3, 175), new(146.2, 179.4), new(144.9, 183.5), new(143.4, 187.2), new(141.8, 190.5), new(140, 193.3), new(138.1, 195.7), new(136.2, 197.6), new(134.2, 198.9), new(132.1, 199.7), new(130, 200), new(127.9, 199.7), new(125.8, 198.9), new(123.8, 197.6), new(121.9, 195.7), new(120, 193.3), new(118.2, 190.5), new(116.6, 187.2), new(115.1, 183.5), new(113.8, 179.4), new(112.7, 175), new(111.7, 170.3), new(111, 165.5), new(110.4, 160.4), new(110.1, 155.2), new(110, 150), new(110.1, 144.8), new(110.4, 139.6), new(111, 134.5), new(111.7, 129.7), new(112.7, 125), new(113.8, 120.6), new(115.1, 116.5), new(116.6, 112.8), new(118.2, 109.5), new(120, 106.7), new(121.9, 104.3), new(123.8, 102.4), new(125.8, 101.1), new(127.9, 100.3), new(130, 100), new(132.1, 100.3), new(134.2, 101.1), new(136.2, 102.4), new(138.1, 104.3), new(140, 106.7), new(141.8, 109.5), new(143.4, 112.8), new(144.9, 116.5), new(146.2, 120.6), new(147.3, 125), new(148.3, 129.7), new(149, 134.5), new(149.6, 139.6), new(149.9, 144.8), new(150, 150)]), result);
    }

    [Fact]
    public void GetBoundingBox() {
        var subject = new Ellipse(new(30, 50), new(50, 100));

        var result = subject.GetBoundingBox();

        BoundingBoxAssert.Equal(new(10, 50, 0, 100), result);
    }
}
