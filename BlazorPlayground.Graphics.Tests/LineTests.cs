using NetTopologySuite.Geometries;
using System.Linq;
using Xunit;

namespace BlazorPlayground.Graphics.Tests;

public class LineTests {
    [Fact]
    public void ElementName() {
        var line = new Line(new Point(100, 150), new Point(200, 250));

        Assert.Equal("line", line.ElementName);
    }

    [Fact]
    public void GetSnapPoints() {
        var line = new Line(new Point(100, 150), new Point(200, 250));

        var result = line.GetSnapPoints();

        Assert.Equal(2, result.Count);
        Assert.Contains(new Point(100, 150), result);
        Assert.Contains(new Point(200, 250), result);
    }

    [Fact]
    public void GetAttributes() {
        var line = new Line(new Point(100, 150), new Point(200, 250));

        var attributes = line.GetAttributes().ToList();

        Assert.Equal(4, attributes.Count);
        Assert.Equal("100", Assert.Single(attributes, a => a.Key == "x1").Value);
        Assert.Equal("150", Assert.Single(attributes, a => a.Key == "y1").Value);
        Assert.Equal("200", Assert.Single(attributes, a => a.Key == "x2").Value);
        Assert.Equal("250", Assert.Single(attributes, a => a.Key == "y2").Value);
    }

    [Fact]
    public void Anchors_Get() {
        var line = new Line(new Point(100, 150), new Point(200, 250));

        var result = line.Anchors;

        Assert.Equal(2, result.Count);
        Assert.Equal(new Point(100, 150), result[0].Get(line));
        Assert.Equal(new Point(200, 250), result[1].Get(line));
    }

    [Fact]
    public void Anchors_Set() {
        var line = new Line(new Point(100, 150), new Point(200, 250));

        var result = line.Anchors;

        Assert.Equal(2, result.Count);
        result[0].Set(line, new Point(110, 160));
        result[1].Set(line, new Point(210, 260));
        Assert.Equal(new Point(110, 160), line.StartPoint);
        Assert.Equal(new Point(210, 260), line.EndPoint);
    }

    [Fact]
    public void Clone() {
        var line = new Line(new Point(100, 150), new Point(200, 250));

        var result = line.Clone();

        var resultLine = Assert.IsType<Line>(result);

        Assert.NotSame(line, resultLine);
        Assert.Equal(new Point(100, 150), resultLine.StartPoint);
        Assert.Equal(new Point(200, 250), resultLine.EndPoint);
    }

    [Fact]
    public void GetGeometry() {
        var geometryFactory = new GeometryFactory(new PrecisionModel(10));
        var subject = new Line(new(30, 50), new(50, 100));

        var result = subject.GetGeometry(geometryFactory, new(-100, -100));

        Assert.Equal(geometryFactory.CreateLineString([new(130, 150), new(150, 200)]), result);
    }

    [Fact]
    public void GetBoundingBox() {
        var subject = new Line(new(50, 100), new(30, 50));

        var result = subject.GetBoundingBox();

        Assert.Equal(new(new(30, 50), new(50, 100)), result);
    }
}
