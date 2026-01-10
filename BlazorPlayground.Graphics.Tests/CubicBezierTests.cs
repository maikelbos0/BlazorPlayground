using NetTopologySuite.Geometries;
using Xunit;

namespace BlazorPlayground.Graphics.Tests;

public class CubicBezierTests {
    [Fact]
    public void ElementName() {
        var bezier = new CubicBezier(new Point(100, 150), new Point(200, 250));

        Assert.Equal("path", bezier.ElementName);
    }

    [Fact]
    public void GetSnapPoints() {
        var bezier = new CubicBezier(new Point(100, 150), new Point(200, 250));

        var result = bezier.GetSnapPoints();

        Assert.Equal(2, result.Count);
        Assert.Contains(new Point(100, 150), result);
        Assert.Contains(new Point(200, 250), result);
    }

    [Fact]
    public void ControlPoints() {
        var bezier = new CubicBezier(new Point(100, 150), new Point(250, 300));

        Assert.Equal(new Point(150, 200), bezier.ControlPoint1);
        Assert.Equal(new Point(200, 250), bezier.ControlPoint2);
    }

    [Fact]
    public void GetAttributes() {
        var bezier = new CubicBezier(new Point(100, 150), new Point(200, 250)) {
            ControlPoint1 = new Point(125, 175),
            ControlPoint2 = new Point(225, 275)
        };

        var attributes = bezier.GetAttributes();

        var attribute = Assert.Single(attributes);
        Assert.Equal("d", attribute.Key);
        Assert.Equal("M 100 150 C 125 175, 225 275, 200 250", attribute.Value);
    }

    [Fact]
    public void Anchors_Get() {
        var bezier = new CubicBezier(new Point(100, 150), new Point(200, 250)) {
            ControlPoint1 = new Point(125, 175),
            ControlPoint2 = new Point(225, 275)
        };

        var result = bezier.Anchors;

        Assert.Equal(4, result.Count);
        Assert.Equal(new Point(100, 150), result[0].Get(bezier));
        Assert.Equal(new Point(125, 175), result[1].Get(bezier));
        Assert.Equal(new Point(225, 275), result[2].Get(bezier));
        Assert.Equal(new Point(200, 250), result[3].Get(bezier));
    }

    [Fact]
    public void Anchors_Set() {
        var bezier = new CubicBezier(new Point(100, 150), new Point(200, 250)) {
            ControlPoint1 = new Point(125, 175),
            ControlPoint2 = new Point(225, 275)
        };

        var result = bezier.Anchors;

        Assert.Equal(4, result.Count);
        result[0].Set(bezier, new Point(110, 160));
        result[1].Set(bezier, new Point(135, 185));
        result[2].Set(bezier, new Point(235, 285));
        result[3].Set(bezier, new Point(210, 260));
        Assert.Equal(new Point(110, 160), bezier.StartPoint);
        Assert.Equal(new Point(135, 185), bezier.ControlPoint1);
        Assert.Equal(new Point(235, 285), bezier.ControlPoint2);
        Assert.Equal(new Point(210, 260), bezier.EndPoint);
    }

    [Fact]
    public void Clone() {
        var bezier = new CubicBezier(new Point(100, 150), new Point(200, 250)) {
            ControlPoint1 = new Point(125, 175),
            ControlPoint2 = new Point(225, 275)
        };

        var result = bezier.Clone();

        var resultBezier = Assert.IsType<CubicBezier>(result);

        Assert.NotSame(bezier, resultBezier);
        Assert.Equal(new Point(100, 150), resultBezier.StartPoint);
        Assert.Equal(new Point(125, 175), resultBezier.ControlPoint1);
        Assert.Equal(new Point(225, 275), resultBezier.ControlPoint2);
        Assert.Equal(new Point(200, 250), resultBezier.EndPoint);
    }

    [Fact]
    public void GetGeometry() {
        var geometryFactory = new GeometryFactory(new PrecisionModel(10));
        var subject = new CubicBezier(new(30, 50), new(50, 100)) {
            ControlPoint1 = new(20, 60),
            ControlPoint2 = new(10, 90)
        };

        var result = subject.GetGeometry(geometryFactory, new(-100, -100));

        Assert.Equal(geometryFactory.CreateLineString([new(130, 150), new(129.5, 150.5), new(129, 151.1), new(128.5, 151.6), new(128, 152.3), new(127.5, 152.9), new(127.1, 153.6), new(126.6, 154.3), new(126.1, 155), new(125.7, 155.7), new(125.2, 156.5), new(124.8, 157.3), new(124.4, 158.1), new(124, 158.9), new(123.6, 159.8), new(123.3, 160.6), new(122.9, 161.5), new(122.6, 162.4), new(122.4, 163.3), new(122.1, 164.2), new(121.9, 165.2), new(121.6, 166.1), new(121.5, 167.1), new(121.3, 168.1), new(121.2, 169), new(121.1, 170), new(121.1, 171), new(121.1, 172), new(121.1, 173), new(121.1, 174), new(121.3, 175), new(121.4, 176), new(121.6, 177), new(121.8, 178), new(122.1, 179), new(122.4, 180), new(122.8, 181), new(123.2, 181.9), new(123.7, 182.9), new(124.2, 183.9), new(124.8, 184.8), new(125.5, 185.8), new(126.2, 186.7), new(126.9, 187.6), new(127.7, 188.5), new(128.6, 189.4), new(129.5, 190.2), new(130.5, 191.1), new(131.6, 191.9), new(132.7, 192.7), new(133.9, 193.5), new(135.2, 194.3), new(136.5, 195), new(138, 195.7), new(139.5, 196.4), new(141, 197.1), new(142.7, 197.7), new(144.4, 198.4), new(146.2, 198.9), new(148, 199.5), new(150, 200)]), result);
    }

    [Fact]
    public void GetBoundingBox() {
        var subject = new CubicBezier(new(50, 100), new(30, 50)) {
            ControlPoint1 = new(20, 60),
            ControlPoint2 = new(10, 90)
        };

        var result = subject.GetBoundingBox();

        BoundingBoxAssert.Equal(new(30, 50, 50, 100), result);

    }
}
