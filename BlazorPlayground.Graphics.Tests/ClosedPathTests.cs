using NetTopologySuite.Geometries;
using System.Linq;
using Xunit;

namespace BlazorPlayground.Graphics.Tests;

public class ClosedPathTests {
    [Fact]
    public void ElementName() {
        var closedPath = new ClosedPath(new Point(100, 150), new Point(200, 250));

        Assert.Equal("path", closedPath.ElementName);
    }

    [Fact]
    public void GetSnapPoints() {
        var closedPath = new ClosedPath(new Point(100, 150), new Point(200, 250));

        closedPath.Points.Add(new(200, 50));

        var result = closedPath.GetSnapPoints();

        Assert.Equal(3, result.Count);
        Assert.Contains(new Point(100, 150), result);
        Assert.Contains(new Point(200, 250), result);
        Assert.Contains(new Point(200, 50), result);
    }

    [Fact]
    public void GetAttributesWithoutPoints() {
        var closedPath = new ClosedPath() {
            Points = []
        };

        var attributes = closedPath.GetAttributes().ToList();

        Assert.Empty(attributes);
    }

    [Fact]
    public void GetAttributes() {
        var closedPath = new ClosedPath(new Point(100, 150), new Point(200, 250)) { 
            Points = {
                new(200, 50)
            }
        };

        var attributes = closedPath.GetAttributes().ToList();

        var attribute = Assert.Single(attributes);
        Assert.Equal("d", attribute.Key);
        Assert.Equal("M 100 150, L 200 250, L 200 50, Z", attribute.Value);
    }

    [Fact]
    public void Anchors_Get() {
        var closedPath = new ClosedPath(new Point(100, 150), new Point(200, 250));

        closedPath.Points.Add(new(200, 50));

        var result = closedPath.Anchors;

        Assert.Equal(3, result.Count);
        Assert.Equal(new Point(100, 150), result[0].Get(closedPath));
        Assert.Equal(new Point(200, 250), result[1].Get(closedPath));
        Assert.Equal(new Point(200, 50), result[2].Get(closedPath));
    }

    [Fact]
    public void Anchors_Set() {
        var closedPath = new ClosedPath(new Point(100, 150), new Point(200, 250));

        closedPath.Points.Add(new(200, 50));

        var result = closedPath.Anchors;

        Assert.Equal(3, result.Count);
        result[0].Set(closedPath, new Point(110, 160));
        result[1].Set(closedPath, new Point(210, 260));
        result[2].Set(closedPath, new Point(210, 60));
        Assert.Equal(new Point(110, 160), closedPath.Points[0]);
        Assert.Equal(new Point(210, 260), closedPath.Points[1]);
        Assert.Equal(new Point(210, 60), closedPath.Points[2]);
    }

    [Fact]
    public void Clone() {
        var closedPath = new ClosedPath(new Point(100, 150), new Point(200, 250));

        closedPath.Points.Add(new(200, 50));

        var result = closedPath.Clone();

        var resultClosedPath = Assert.IsType<ClosedPath>(result);

        Assert.NotSame(closedPath, resultClosedPath);
        Assert.NotSame(closedPath.Points, resultClosedPath.Points);
        Assert.Equal(closedPath.Points, resultClosedPath.Points);
    }

    [Fact]
    public void ExecuteSecondaryActionAddsEndPointAsIntermediatePoint() {
        var closedPath = new ClosedPath(new Point(100, 150), new Point(200, 250));

        closedPath.AddPoint(new(200, 50));

        Assert.Equal(3, closedPath.Points.Count);
        Assert.Equal(new Point(200, 50), closedPath.Points[^1]);
    }

    [Fact]
    public void GetGeometryWithoutPoints() {
        var geometryFactory = new GeometryFactory(new PrecisionModel(10));
        var subject = new ClosedPath() {
            Points = []
        };

        var result = subject.GetGeometry(geometryFactory, new(-100, -100));

        Assert.NotNull(result);
        Assert.Equal(geometryFactory.CreateEmpty(Dimension.Point), result);
    }

    [Fact]
    public void GetGeometryWithOnePoint() {
        var geometryFactory = new GeometryFactory(new PrecisionModel(10));
        var subject = new ClosedPath() {
            Points = [new(30, 50)]
        };

        var result = subject.GetGeometry(geometryFactory, new(-100, -100));

        Assert.NotNull(result);
        Assert.Equal(geometryFactory.CreatePoint(new Coordinate(130, 150)), result);
    }

    [Fact]
    public void GetGeometryWithTwoPoints() {
        var geometryFactory = new GeometryFactory(new PrecisionModel(10));
        var subject = new ClosedPath(new(30, 50), new(50, 100));

        var result = subject.GetGeometry(geometryFactory, new(-100, -100));

        Assert.NotNull(result);
        Assert.Equal(geometryFactory.CreateLineString([new(130, 150), new(150, 200)]), result);
    }

    [Fact]
    public void GetGeometry() {
        var geometryFactory = new GeometryFactory(new PrecisionModel(10));
        var subject = new ClosedPath(new(30, 50), new(50, 100)) {
            Points = {
                new(60, 40),
                new(30, 10)
            }
        };

        var result = subject.GetGeometry(geometryFactory, new(-100, -100));

        Assert.NotNull(result);
        Assert.Equal(geometryFactory.CreatePolygon([new(130, 150), new(150, 200), new(160, 140), new(130, 110), new(130, 150)]), result);
    }

    [Fact]
    public void GetBoundingBox() {
        var subject = new ClosedPath(new(30, 50), new(50, 100)) {
            Points = {
                new(70, 20),
                new(30, 20)
            }
        };

        var result = subject.GetBoundingBox();

        BoundingBoxAssert.Equal(new(30, 70, 20, 100), result);
    }
}
