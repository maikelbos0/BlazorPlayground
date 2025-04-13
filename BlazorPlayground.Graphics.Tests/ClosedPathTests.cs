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

        closedPath.IntermediatePoints.Add(new(200, 50));

        var result = closedPath.GetSnapPoints();

        Assert.Equal(3, result.Count);
        PointAssert.Contains(result, new Point(100, 150));
        PointAssert.Contains(result, new Point(200, 250));
        PointAssert.Contains(result, new Point(200, 50));
    }

    [Fact]
    public void GetAttributes() {
        var closedPath = new ClosedPath(new Point(100, 150), new Point(200, 250));

        closedPath.IntermediatePoints.Add(new(200, 50));

        var attributes = closedPath.GetAttributes().ToList();

        var attribute = Assert.Single(attributes);
        Assert.Equal("d", attribute.Key);
        Assert.Equal("M 100 150, L 200 250, L 200 50, Z", attribute.Value);
    }

    [Fact]
    public void Anchors_Get() {
        var closedPath = new ClosedPath(new Point(100, 150), new Point(200, 250));

        closedPath.IntermediatePoints.Add(new(200, 50));

        var result = closedPath.Anchors;

        Assert.Equal(3, result.Count);
        PointAssert.Equal(new Point(100, 150), result[0].Get(closedPath));
        PointAssert.Equal(new Point(200, 250), result[1].Get(closedPath));
        PointAssert.Equal(new Point(200, 50), result[2].Get(closedPath));
    }

    [Fact]
    public void Anchors_Set() {
        var closedPath = new ClosedPath(new Point(100, 150), new Point(200, 250));

        closedPath.IntermediatePoints.Add(new(200, 50));

        var result = closedPath.Anchors;

        Assert.Equal(3, result.Count);
        result[0].Set(closedPath, new Point(110, 160));
        result[1].Set(closedPath, new Point(210, 260));
        result[2].Set(closedPath, new Point(210, 60));
        PointAssert.Equal(new Point(110, 160), closedPath.StartPoint);
        PointAssert.Equal(new Point(210, 260), closedPath.IntermediatePoints[0]);
        PointAssert.Equal(new Point(210, 60), closedPath.IntermediatePoints[1]);
    }

    [Fact]
    public void Clone() {
        var closedPath = new ClosedPath(new Point(100, 150), new Point(200, 250));

        closedPath.IntermediatePoints.Add(new(200, 50));

        var result = closedPath.Clone();

        var resultClosedPath = Assert.IsType<ClosedPath>(result);

        Assert.NotSame(closedPath, resultClosedPath);
        PointAssert.Equal(new Point(100, 150), resultClosedPath.StartPoint);
        Assert.NotSame(closedPath.IntermediatePoints, resultClosedPath.IntermediatePoints);
        Assert.Equal(closedPath.IntermediatePoints, resultClosedPath.IntermediatePoints);
    }

    [Fact]
    public void ExecuteSecondaryActionAddsEndPointAsIntermediatePoint() {
        var closedPath = new ClosedPath(new Point(100, 150), new Point(200, 250));

        closedPath.ExecuteSecondaryAction(new(200, 50));

        Assert.Equal(2, closedPath.IntermediatePoints.Count);
        PointAssert.Equal(new Point(200, 50), closedPath.IntermediatePoints[^1]);
    }
}
