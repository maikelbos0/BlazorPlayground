using NetTopologySuite.Geometries;
using Xunit;

namespace BlazorPlayground.Graphics.Tests;

public class QuadraticBezierTests {
    [Fact]
    public void ElementName() {
        var bezier = new QuadraticBezier(new Point(100, 150), new Point(200, 250));

        Assert.Equal("path", bezier.ElementName);
    }

    [Fact]
    public void GetSnapPoints() {
        var bezier = new QuadraticBezier(new Point(100, 150), new Point(200, 250));

        var result = bezier.GetSnapPoints();

        Assert.Equal(2, result.Count);
        Assert.Contains(new Point(100, 150), result);
        Assert.Contains(new Point(200, 250), result);
    }

    [Fact]
    public void ControlPoint() {
        var bezier = new QuadraticBezier(new Point(100, 150), new Point(200, 250));

        Assert.Equal(new Point(150, 200), bezier.ControlPoint);
    }

    [Fact]
    public void GetAttributes() {
        var bezier = new QuadraticBezier(new Point(100, 150), new Point(200, 250)) {
            ControlPoint = new Point(125, 175)
        };

        var attributes = bezier.GetAttributes();

        var attribute = Assert.Single(attributes);
        Assert.Equal("d", attribute.Key);
        Assert.Equal("M 100 150 Q 125 175, 200 250", attribute.Value);
    }

    [Fact]
    public void Anchors_Get() {
        var bezier = new QuadraticBezier(new Point(100, 150), new Point(200, 250)) {
            ControlPoint = new Point(125, 175)
        };

        var result = bezier.Anchors;

        Assert.Equal(3, result.Count);
        Assert.Equal(new Point(100, 150), result[0].Get(bezier));
        Assert.Equal(new Point(125, 175), result[1].Get(bezier));
        Assert.Equal(new Point(200, 250), result[2].Get(bezier));
    }

    [Fact]
    public void Anchors_Set() {
        var bezier = new QuadraticBezier(new Point(100, 150), new Point(200, 250)) {
            ControlPoint = new Point(125, 175)
        };

        var result = bezier.Anchors;

        Assert.Equal(3, result.Count);
        result[0].Set(bezier, new Point(110, 160));
        result[1].Set(bezier, new Point(135, 185));
        result[2].Set(bezier, new Point(210, 260));
        Assert.Equal(new Point(110, 160), bezier.StartPoint);
        Assert.Equal(new Point(135, 185), bezier.ControlPoint);
        Assert.Equal(new Point(210, 260), bezier.EndPoint);
    }

    [Fact]
    public void Clone() {
        var bezier = new QuadraticBezier(new Point(100, 150), new Point(200, 250)) {
            ControlPoint = new Point(125, 175)
        };

        var result = bezier.Clone();

        var resultBezier = Assert.IsType<QuadraticBezier>(result);

        Assert.NotSame(bezier, resultBezier);
        Assert.Equal(new Point(100, 150), resultBezier.StartPoint);
        Assert.Equal(new Point(125, 175), resultBezier.ControlPoint);
        Assert.Equal(new Point(200, 250), resultBezier.EndPoint);
    }

    [Fact]
    public void GetGeometry() {
        var geometryFactory = new GeometryFactory(new PrecisionModel(10));
        var subject = new QuadraticBezier(new(30, 50), new(50, 100)) {
            ControlPoint = new(20, 70)
        };

        var result = subject.GetGeometry(geometryFactory, new(-100, -100));

        Assert.Equal(geometryFactory.CreateLineString([new(130, 150), new(129.7, 150.7), new(129.4, 151.3), new(129.1, 152), new(128.8, 152.7), new(128.6, 153.4), new(128.4, 154.1), new(128.2, 154.8), new(128, 155.5), new(127.9, 156.2), new(127.8, 156.9), new(127.7, 157.7), new(127.6, 158.4), new(127.5, 159.1), new(127.5, 159.9), new(127.5, 160.6), new(127.5, 161.4), new(127.5, 162.1), new(127.6, 162.9), new(127.7, 163.7), new(127.8, 164.4), new(127.9, 165.2), new(128, 166), new(128.2, 166.8), new(128.4, 167.6), new(128.6, 168.4), new(128.8, 169.2), new(129.1, 170), new(129.4, 170.8), new(129.7, 171.7), new(130, 172.5), new(130.3, 173.3), new(130.7, 174.2), new(131.1, 175), new(131.5, 175.9), new(131.9, 176.7), new(132.4, 177.6), new(132.9, 178.5), new(133.4, 179.3), new(133.9, 180.2), new(134.4, 181.1), new(135, 182), new(135.6, 182.9), new(136.2, 183.8), new(136.8, 184.7), new(137.5, 185.6), new(138.2, 186.5), new(138.9, 187.5), new(139.6, 188.4), new(140.3, 189.3), new(141.1, 190.3), new(141.9, 191.2), new(142.7, 192.2), new(143.5, 193.1), new(144.4, 194.1), new(145.3, 195.1), new(146.2, 196), new(147.1, 197), new(148, 198), new(149, 199), new(150, 200)]), result);
    }
    
    [Fact]
    public void GetBoundingBox() {
        var subject = new QuadraticBezier(new(50, 100), new(30, 50)) {
            ControlPoint = new(20, 70)
        };

        var result = subject.GetBoundingBox();

        BoundingBoxAssert.Equal(new(30, 50, 50, 100), result);
    }
}
