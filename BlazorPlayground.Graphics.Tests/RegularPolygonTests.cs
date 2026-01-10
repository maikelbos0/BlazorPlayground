using NetTopologySuite.Geometries;
using System.Linq;
using Xunit;

namespace BlazorPlayground.Graphics.Tests;

public class RegularPolygonTests {
    [Fact]
    public void ElementName() {
        var polygon = new RegularPolygon(new Point(100, 100), new Point(50, 50));

        Assert.Equal("polygon", polygon.ElementName);
    }

    [Fact]
    public void GetSnapPoints() {
        var square = new RegularPolygon(new Point(100, 100), new Point(50, 50)) {
            Sides = 4
        };

        var result = square.GetSnapPoints();

        Assert.Equal(5, result.Count);
        Assert.Contains(new Point(50, 50), result);
        Assert.Contains(new Point(150, 50), result);
        Assert.Contains(new Point(150, 150), result);
        Assert.Contains(new Point(50, 150), result);
        Assert.Contains(new Point(100, 100), result);
    }

    [Fact]
    public void GetPoints_Square_0_Degrees() {
        var square = new RegularPolygon(new Point(100, 100), new Point(50, 50)) {
            Sides = 4
        };

        var result = square.GetPoints().ToList();

        Assert.Equal(4, result.Count);
        Assert.Equal(new Point(50, 50), result[0]);
        Assert.Equal(new Point(150, 50), result[1]);
        Assert.Equal(new Point(150, 150), result[2]);
        Assert.Equal(new Point(50, 150), result[3]);
    }

    [Fact]
    public void GetPoints_Square_45_Degrees() {
        var square = new RegularPolygon(new Point(100, 100), new Point(50, 100)) {
            Sides = 4
        };

        var result = square.GetPoints().ToList();

        Assert.Equal(4, result.Count);
        Assert.Equal(new Point(50, 100), result[0]);
        Assert.Equal(new Point(100, 50), result[1]);
        Assert.Equal(new Point(150, 100), result[2]);
        Assert.Equal(new Point(100, 150), result[3]);
    }

    [Fact]
    public void GetPoints_Square_22_Degrees() {
        var square = new RegularPolygon(new Point(100, 100), new Point(50, 120)) {
            Sides = 4
        };

        var result = square.GetPoints().ToList();

        Assert.Equal(4, result.Count);
        Assert.Equal(new Point(50, 120), result[0]);
        Assert.Equal(new Point(80, 50), result[1]);
        Assert.Equal(new Point(150, 80), result[2]);
        Assert.Equal(new Point(120, 150), result[3]);
    }

    [Fact]
    public void GetPoints_Square_112_Degrees() {
        var square = new RegularPolygon(new Point(100, 100), new Point(150, 120)) {
            Sides = 4
        };

        var result = square.GetPoints().ToList();

        Assert.Equal(4, result.Count);
        Assert.Equal(new Point(150, 120), result[0]);
        Assert.Equal(new Point(80, 150), result[1]);
        Assert.Equal(new Point(50, 80), result[2]);
        Assert.Equal(new Point(120, 50), result[3]);
    }

    [Fact]
    public void GetPoints_Triangle() {
        var triangle = new RegularPolygon(new Point(100, 100), new Point(100, 50)) {
            Sides = 3
        };

        var result = triangle.GetPoints().ToList();

        Assert.Equal(3, result.Count);

        Assert.Equal(new Point(100, 50), result[0]);
        Assert.Equal(new Point(143.301, 125), result[1]);
        Assert.Equal(new Point(56.699, 125), result[2]);
    }

    [Fact]
    public void GetPoints_Hexagon() {
        var hexagon = new RegularPolygon(new Point(100, 100), new Point(100, 50)) {
            Sides = 6
        };

        var result = hexagon.GetPoints().ToList();

        Assert.Equal(6, result.Count);

        Assert.Equal(new Point(100, 50), result[0]);
        Assert.Equal(new Point(143.301, 75), result[1]);
        Assert.Equal(new Point(143.301, 125), result[2]);
        Assert.Equal(new Point(100, 150), result[3]);
        Assert.Equal(new Point(56.699, 125), result[4]);
        Assert.Equal(new Point(56.699, 75), result[5]);
    }

    [Fact]
    public void GetAttributes() {
        var polygon = new RegularPolygon(new Point(100, 100), new Point(50, 50)) {
            Sides = 4
        };

        var attributes = polygon.GetAttributes();

        var attribute = Assert.Single(attributes);
        Assert.Equal("points", attribute.Key);
        Assert.Equal("50,50 150,50 150,150 50,150", attribute.Value);
    }

    [Fact]
    public void Anchors_Get() {
        var polygon = new RegularPolygon(new Point(100, 150), new Point(200, 250));

        var result = polygon.Anchors;

        Assert.Equal(2, result.Count);
        Assert.Equal(new Point(100, 150), result[0].Get(polygon));
        Assert.Equal(new Point(200, 250), result[1].Get(polygon));
    }

    [Fact]
    public void Anchors_Set() {
        var polygon = new RegularPolygon(new Point(100, 150), new Point(200, 250));

        var result = polygon.Anchors;

        Assert.Equal(2, result.Count);
        result[0].Set(polygon, new Point(110, 160));
        result[1].Set(polygon, new Point(210, 260));
        Assert.Equal(new Point(110, 160), polygon.CenterPoint);
        Assert.Equal(new Point(210, 260), polygon.RadiusPoint);
    }

    [Fact]
    public void Clone() {
        var polygon = new RegularPolygon(new Point(100, 150), new Point(200, 250));

        var result = polygon.Clone();

        var resultPolygon = Assert.IsType<RegularPolygon>(result);

        Assert.NotSame(polygon, resultPolygon);
        Assert.Equal(new Point(100, 150), resultPolygon.CenterPoint);
        Assert.Equal(new Point(200, 250), resultPolygon.RadiusPoint);
    }

    [Fact]
    public void GetGeometry() {
        var geometryFactory = new GeometryFactory(new PrecisionModel(10));
        var subject = new RegularPolygon(new(50, 50), new(50, 100)) {
            Sides = 3
        };

        var result = subject.GetGeometry(geometryFactory, new(-100, -100));

        Assert.Equal(geometryFactory.CreatePolygon([new(150, 200), new(106.7, 125), new(193.3, 125), new(150, 200)]), result);
    }

    [Fact]
    public void GetBoundingBox() {
        var subject = new RegularPolygon(new(30, 50), new(50, 100));

        var result = subject.GetBoundingBox();

        BoundingBoxAssert.Equal(new(-23.9, 83.9, -3.9, 103.9), result);
    }
}
