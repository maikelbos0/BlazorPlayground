using NetTopologySuite.Geometries;
using System.Linq;
using Xunit;

namespace BlazorPlayground.Graphics.Tests;

public class RectangleTests {
    [Fact]
    public void ElementName() {
        var rectangle = new Rectangle(new Point(100, 150), new Point(200, 250));

        Assert.Equal("rect", rectangle.ElementName);
    }

    [Fact]
    public void GetSnapPoints() {
        var rectangle = new Rectangle(new Point(100, 150), new Point(200, 250));

        var result = rectangle.GetSnapPoints();

        Assert.Equal(4, result.Count);
        PointAssert.Contains(result, new Point(100, 150));
        PointAssert.Contains(result, new Point(200, 150));
        PointAssert.Contains(result, new Point(100, 250));
        PointAssert.Contains(result, new Point(200, 250));
    }

    [Fact]
    public void GetAttributes() {
        var rectangle = new Rectangle(new Point(180, 210), new Point(100, 150));

        var attributes = rectangle.GetAttributes().ToList();

        Assert.Equal(4, attributes.Count);
        Assert.Equal("100", Assert.Single(attributes, a => a.Key == "x").Value);
        Assert.Equal("150", Assert.Single(attributes, a => a.Key == "y").Value);
        Assert.Equal("80", Assert.Single(attributes, a => a.Key == "width").Value);
        Assert.Equal("60", Assert.Single(attributes, a => a.Key == "height").Value);
    }

    [Fact]
    public void Anchors_Get() {
        var rectangle = new Rectangle(new Point(100, 150), new Point(200, 250));

        var result = rectangle.Anchors;

        Assert.Equal(2, result.Count);
        PointAssert.Equal(new Point(100, 150), result[0].Get(rectangle));
        PointAssert.Equal(new Point(200, 250), result[1].Get(rectangle));
    }

    [Fact]
    public void Anchors_Set() {
        var rectangle = new Rectangle(new Point(100, 150), new Point(200, 250));

        var result = rectangle.Anchors;

        Assert.Equal(2, result.Count);
        result[0].Set(rectangle, new Point(110, 160));
        result[1].Set(rectangle, new Point(210, 260));
        PointAssert.Equal(new Point(110, 160), rectangle.StartPoint);
        PointAssert.Equal(new Point(210, 260), rectangle.EndPoint);
    }

    [Fact]
    public void Clone() {
        var rectangle = new Rectangle(new Point(100, 150), new Point(200, 250));

        var result = rectangle.Clone();

        var resultRectangle = Assert.IsType<Rectangle>(result);

        Assert.NotSame(rectangle, resultRectangle);
        PointAssert.Equal(new Point(100, 150), resultRectangle.StartPoint);
        PointAssert.Equal(new Point(200, 250), resultRectangle.EndPoint);
    }

    [Fact]
    public void GetGeometry() {
        var geometryFactory = new GeometryFactory(new PrecisionModel(10));
        var subject = new Rectangle(new(30, 50), new(50, 100));

        var result = subject.GetGeometry(geometryFactory, new(-100, -100));

        Assert.Equal(geometryFactory.CreatePolygon([new(130, 150), new(130, 200), new(150, 200), new(150, 150), new(130, 150)]), result);
    }

    [Fact]
    public void GetBoundingBox() {
        var subject = new Rectangle(new(50, 100), new(30, 50));

        var result = subject.GetBoundingBox();

        BoundingBoxAssert.Equal(new(30, 50, 50, 100), result);
    }
}
